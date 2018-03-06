using System;
using SkiaSharp;

namespace EasyBudget.Forms.Utility
{
    public sealed class ChartUtility
    {
        public static ChartUtility Instance { get; private set; }

        Random rnd;
        int currentIdx = 0;

        public ChartUtility()
        {
            if (Instance != null)
            {
                throw new Exception("Only one instance of ChartUtility is allowed!");
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

            SKColors.Aquamarine,
            SKColors.BurlyWood,
            SKColors.Cyan,
            SKColors.Olive,
            SKColors.BlueViolet,
            SKColors.ForestGreen,
            SKColors.Goldenrod,
            SKColors.IndianRed,
            SKColors.Khaki,
            SKColor.Parse("#C43DB6"),
            SKColor.Parse("#A04997"),
            SKColor.Parse("#ffb200"),
            SKColor.Parse("#0EF6F9"),
            SKColor.Parse("#F6FF00"),
            SKColor.Parse("#FFC000"),
            SKColor.Parse("#257C19"),
            SKColor.Parse("#76846E"),
            SKColor.Parse("#DABFAF"),
            SKColor.Parse("#A65B69"),
            SKColor.Parse("#97A69D"),
            SKColor.Parse("#B6D7EA"),
            SKColor.Parse("#1E9835"),
            SKColor.Parse("#3CE5E7"),
            SKColor.Parse("#44A5A6"),
            SKColor.Parse("#266489"),
            SKColor.Parse("#68B9C0"),
            SKColor.Parse("#90D585"),
            SKColor.Parse("#F3C151"),
            SKColor.Parse("#F37F64"),
            SKColor.Parse("#424856"),

        };



        public SKColor GetColor()
        {
            //var idx = rnd.Next(0, 22);
            return Colors[currentIdx++];
        }
    }
}
