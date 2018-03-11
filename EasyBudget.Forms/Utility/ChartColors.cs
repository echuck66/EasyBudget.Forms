using System;
using SkiaSharp;

namespace EasyBudget.Forms.Utility
{
    public class ChartColors
    {
        public static ChartColors Instance { get; private set; }

        Random rnd;
        int currentIdx = 0;


        public ChartColors()
        {
            if (Instance != null)
            {
                throw new Exception("Only one instance of ChartColors is allowed!");
            }
            else
            {
                Instance = this;
                RandomGenerator generator = new RandomGenerator();
                this.rnd = generator.GetRandom(null);
            }
        }

        static readonly SKColor[] Colors =
        {

            SKColor.Parse("#9C27B0"),
            SKColor.Parse("#E1BEE7"),
            SKColor.Parse("#673AB7"),
            SKColor.Parse("#D1C4E9"),

            SKColor.Parse("#2196F3"),
            SKColor.Parse("#90CAF9"),
            SKColor.Parse("#009688"),
            SKColor.Parse("#80CBC4"),

            SKColor.Parse("#4CAF50"),
            SKColor.Parse("#A5D6A7"),
            SKColor.Parse("#76FF03"),
            SKColor.Parse("#CCFF90"),

            SKColor.Parse("#FF9800"),
            SKColor.Parse("#FFCC80"),
            SKColor.Parse("#FFC107"),
            SKColor.Parse("#FFE082"),

            SKColor.Parse("#FFEB3B"),
            SKColor.Parse("#FFF176"),
            SKColor.Parse("#FFD600"),
            SKColor.Parse("#FFFF8D"),

            SKColor.Parse("#795548"),
            SKColor.Parse("#BCAAA4"),
            SKColor.Parse("#3E2723"),
            SKColor.Parse("#795548"),

            // Blacks and Greys
            SKColor.Parse("#9E9E9E"),
            SKColor.Parse("#E0E0E0"),
            SKColor.Parse("#757575"),
            SKColor.Parse("#000000"),
            // Reds and Pinks
            SKColor.Parse("#F44336"),
            SKColor.Parse("#FFCDD2"),
            SKColor.Parse("#E91E63"),
            SKColor.Parse("#F8BBD0"),

        };
        



        public SKColor GetColor()
        {
            // If we've exceeded the length of the array, start over:
            if (currentIdx >= Colors.Length - 1)
                currentIdx = 0;

            return Colors[currentIdx++];
        }
    }
}
