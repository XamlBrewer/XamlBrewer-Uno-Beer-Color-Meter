namespace XamlBrewer.Uno.BeerColorMeter;

using System.Collections.Generic;
using XamlBrewer.Uno.BeerColorMeter.Models;

public partial class DAL
{
    public static List<BeerColorGroup> BeerColorGroups
    {
        get
        {
            List<BeerColorGroup> result = new()
            {
                new BeerColorGroup()
                {
                    ColorName = "Straw",
                    MinimumSRM = 0,
                    MaximumSRM = 3
                },
                new BeerColorGroup()
                {
                    ColorName = "Yellow",
                    MinimumSRM = 3,
                    MaximumSRM = 5
                },
                new BeerColorGroup()
                {
                    ColorName = "Gold",
                    MinimumSRM = 5,
                    MaximumSRM = 6
                },
                new BeerColorGroup()
                {
                    ColorName = "Amber",
                    MinimumSRM = 6,
                    MaximumSRM = 9
                },
                new BeerColorGroup()
                {
                    ColorName = "Deep amber / light copper",
                    MinimumSRM = 9,
                    MaximumSRM = 14
                },
                new BeerColorGroup()
                {
                    ColorName = "Copper",
                    MinimumSRM = 14,
                    MaximumSRM = 17
                },
                new BeerColorGroup()
                {
                    ColorName = "Deep copper / light brown",
                    MinimumSRM = 17,
                    MaximumSRM = 19
                },
                new BeerColorGroup()
                {
                    ColorName = "Brown",
                    MinimumSRM = 19,
                    MaximumSRM = 22
                },
                new BeerColorGroup()
                {
                    ColorName = "Dark brown",
                    MinimumSRM = 22,
                    MaximumSRM = 30
                },
                new BeerColorGroup()
                {
                    ColorName = "Very dark brown",
                    MinimumSRM = 30,
                    MaximumSRM = 35
                },
                new BeerColorGroup()
                {
                    ColorName = "Black",
                    MinimumSRM = 35,
                    MaximumSRM = 40
                },
                new BeerColorGroup()
                {
                    ColorName = "Black, opaque",
                    MinimumSRM = 40,
                    MaximumSRM = 300
                }
            };

            return result;
        }
    }
}
