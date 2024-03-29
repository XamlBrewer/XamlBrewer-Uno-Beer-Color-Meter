using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using XamlBrewer.Uno.BeerColorMeter;
using XamlBrewer.Uno.BeerColorMeter.Models;

namespace XamlBrewer_Uno_BeerColorMeter;

public sealed partial class MainPage : Page
{
    IRandomAccessStream? current;

    public MainPage()
    {
        InitializeComponent();
    }

    private async Task PickImage()
    {
        // Create a file picker
        var openPicker = new FileOpenPicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle((Application.Current as App).MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options
        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        openPicker.FileTypeFilter.Add(".bmp");
        openPicker.FileTypeFilter.Add(".jpg");
        openPicker.FileTypeFilter.Add(".jpeg");
        openPicker.FileTypeFilter.Add(".png");

        // Open the picker
        var file = await openPicker.PickSingleFileAsync();
        await OpenFile(file);
    }

    private async Task OpenFile(StorageFile file)
    {
        if (file != null)
        {
            IBuffer buffer = await FileIO.ReadBufferAsync(file);
            current = buffer.AsStream().AsRandomAccessStream();
            FullImage.Source = new BitmapImage(new Uri(file.Path));
        }
    }

    private async void PickButton_Click(object sender, RoutedEventArgs e)
    {
        await PickImage();
    }

    private async void CalculateButton_Click(object sender, RoutedEventArgs e)
    {
        WriteableBitmap destination;

        current ??= Properties.Resources.Beer.AsBuffer().AsStream().AsRandomAccessStream();

        current.Seek(0);
        var bitmapImage = FullImage.Source as BitmapImage;
        if (bitmapImage == null)
        {
            // The legendary 'should not happen'
            destination = new WriteableBitmap((int)FullImage.ActualWidth, (int)FullImage.ActualHeight);
        }
        else
        {
            destination = new WriteableBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
        }

        destination.SetSource(current);

        byte[] sourcePixels = destination.PixelBuffer.ToArray();

        // Calculate average color
        var nbrOfPixels = sourcePixels.Length / 4;
        int avgR = 0, avgG = 0, avgB = 0;
        for (int i = 0; i < sourcePixels.Length; i += 4)
        {
            avgB += sourcePixels[i];
            avgG += sourcePixels[i + 1];
            avgR += sourcePixels[i + 2];
        }

        var color = Color.FromArgb(255, (byte)(avgR / nbrOfPixels), (byte)(avgG / nbrOfPixels), (byte)(avgB / nbrOfPixels));
        Result.Background = new SolidColorBrush(color);

        // Calculate nearest beer color
        double distance = int.MaxValue;
        BeerColor closest = DAL.BeerColors[0];
        foreach (var beerColor in DAL.BeerColors)
        {
            double d = Math.Sqrt(Math.Pow(beerColor.B - color.B, 2)
                               + Math.Pow(beerColor.G - color.G, 2)
                               + Math.Pow(beerColor.R - color.R, 2));
            if (d < distance)
            {
                distance = d;
                closest = beerColor;
            }
        }

        DisplayResult(closest);
    }

    private void BeerColorSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        var closest = DAL.BeerColors.Where(c => c.SRM == e.NewValue).FirstOrDefault();
        if (closest != null)
        {
            DisplayResult(closest);
        }
    }

    private void DisplayResult(BeerColor closest)
    {
        ClosestBeerColor.Background = new SolidColorBrush(Color.FromArgb(255, closest.R, closest.G, closest.B));
        ClosestBeerColorText.Text = $"SRM: {(int)closest.SRM}{Environment.NewLine}EBC: {(int)closest.EBC}{Environment.NewLine}{Environment.NewLine}{closest.ColorName}";

        // Contrasting text color.
        if (closest.EBC < 12)
        {
            ClosestBeerColorText.Foreground = new SolidColorBrush(Colors.Maroon);
        }
        else
        {
            ClosestBeerColorText.Foreground = new SolidColorBrush(Colors.Beige);
        }

        BeerColorSlider.Value = closest.SRM;
    }
}
