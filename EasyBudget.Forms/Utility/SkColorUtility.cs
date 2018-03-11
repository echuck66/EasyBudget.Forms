using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;

namespace EasyBudget.Forms.Utility
{
    public class AppSkColor
    {
        private AppSkColor() { }

        public string Name { private set; get; }

        public string HexCodeString { private set; get; }

        public SKColor SkColor { private set; get; }

        public string RgbDisplay { private set; get; }

        public IList<AppSkColor> AccentColors { private set; get; }

        public IList<AppSkColor> GetAccentColors(AppSkColor primaryColor) 
        {
            return primaryColor.AccentColors;
        }

        //public IList<AppSkColor> GetPrimaryColors() {
            
        //}

        static AppSkColor()
        {
            List<AppSkColor> all = new List<AppSkColor>();
            List<AppSkColor> primaries = new List<AppSkColor>();

            List<SKColor> skColors = GetSkColorList();

            StringBuilder sb = new StringBuilder();
            foreach(SKColor _color in skColors)
            {
                string name = _color.ToString();
                string hex = _color.ToString();

                AppSkColor _skColor = new AppSkColor
                {
                    Name = name,
                    HexCodeString = hex,
                    SkColor = _color,
                    RgbDisplay = string.Format("{0:X2}-{1:X2}-{2:X2}",
                                              (int)(255 * _color.Red),
                                              (int)(255 * _color.Green),
                                              (int)(255 * _color.Blue))
                };
                all.Add(_skColor);
            }

            all.TrimExcess();
            All = all;
        }


        public static IList<AppSkColor> All { private set; get; }

        public static SKColor GetSKColor(string hexCode)
        {
            SKColor _color = SKColors.Transparent;

            SKColor.TryParse(hexCode, out _color);

            return _color;
        }

        public static IList<AppSkColor> PrimaryColors { private set; get; }



        //public static List<AppSkColor> GetSkColorsList()
        //{
            //List<SKColor> _PrimaryColors = new List<SKColor>()
            //{
            //    GetSKColor("#F44336"),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //    GetSKColor(""),
            //};
            //var clr = SKColor.Parse("#F44336");
            //Red
            //500
            //var clr = new AppSkColor() {
            //    Name = "Red",
            //SkColor = SKColor.Parse("#F44336"),
            //              50
            //SKColor.TryParse("#FFEBEE");
            //            100
            //SKColor.TryParse("#FFCDD2");
            //            200
            //SKColor.TryParse("#EF9A9A");
            //            300
            //SKColor.TryParse("#E57373");
            //            400
            //SKColor.TryParse("#EF5350");
            //            500
            //SKColor.TryParse("#F44336");
            //            600
            //SKColor.TryParse("#E53935");
            //            700
            //SKColor.TryParse("#D32F2F");
            //            800
            //SKColor.TryParse("#C62828");
            //            900
            //SKColor.TryParse("#B71C1C");
            //            A100
            //SKColor.TryParse("#FF8A80");
            //            A200
            //SKColor.TryParse("#FF5252");
            //            A400
            //SKColor.TryParse("#FF1744");
            //            A700
            //SKColor.TryParse("#D50000");
            //            Pink
            //500
            //SKColor.TryParse("#E91E63");
            //            50
            //SKColor.TryParse("#FCE4EC");
            //            100
            //SKColor.TryParse("#F8BBD0");
            //            200
            //SKColor.TryParse("#F48FB1");
            //            300
            //SKColor.TryParse("#F06292");
            //            400
            //SKColor.TryParse("#EC407A");
            //            500
            //SKColor.TryParse("#E91E63");
            //            600
            //SKColor.TryParse("#D81B60");
            //            700
            //SKColor.TryParse("#C2185B");
            //            800
            //SKColor.TryParse("#AD1457");
            //            900
            //SKColor.TryParse("#880E4F");
            //            A100
            //SKColor.TryParse("#FF80AB");
            //            A200
            //SKColor.TryParse("#FF4081");
            //            A400
            //SKColor.TryParse("#F50057");
            //            A700
            //SKColor.TryParse("#C51162");
            //        };
            //            //Purple
            //            //500
            //            SKColor.TryParse("#9C27B0");
            //            50
            //SKColor.TryParse("#F3E5F5");
            //            100
            //SKColor.TryParse("#E1BEE7");
            //            200
            //SKColor.TryParse("#CE93D8");
            //            300
            //SKColor.TryParse("#BA68C8");
            //            400
            //SKColor.TryParse("#AB47BC");
            //            500
            //SKColor.TryParse("#9C27B0");
            //            600
            //SKColor.TryParse("#8E24AA");
            //            700
            //SKColor.TryParse("#7B1FA2");
            //            800
            //SKColor.TryParse("#6A1B9A");
            //            900
            //SKColor.TryParse("#4A148C");
            //            A100
            //SKColor.TryParse("#EA80FC");
            //            A200
            //SKColor.TryParse("#E040FB");
            //            A400
            //SKColor.TryParse("#D500F9");
            //            A700
            //SKColor.TryParse("#AA00FF");
            //            //Deep Purple
            //            //500
            //            SKColor.TryParse("#673AB7");
            //            50
            //SKColor.TryParse("#EDE7F6");
            //            100
            //SKColor.TryParse("#D1C4E9");
            //            200
            //SKColor.TryParse("#B39DDB");
            //            300
            //SKColor.TryParse("#9575CD");
            //            400
            //SKColor.TryParse("#7E57C2");
            //            500
            //SKColor.TryParse("#673AB7");
            //            600
            //SKColor.TryParse("#5E35B1");
            //            700
            //SKColor.TryParse("#512DA8");
            //            800
            //SKColor.TryParse("#4527A0");
            //            900
            //SKColor.TryParse("#311B92");
            //            A100
            //SKColor.TryParse("#B388FF");
            //            A200
            //SKColor.TryParse("#7C4DFF");
            //            A400
            //SKColor.TryParse("#651FFF");
            //            A700
            //SKColor.TryParse("#6200EA");
            //            //Indigo
            //            //500
            //            SKColor.TryParse("#3F51B5");
            //            50
            //SKColor.TryParse("#E8EAF6");
            //            100
            //SKColor.TryParse("#C5CAE9");
            //            200
            //SKColor.TryParse("#9FA8DA");
            //            300
            //SKColor.TryParse("#7986CB");
            //            400
            //SKColor.TryParse("#5C6BC0");
            //            500
            //SKColor.TryParse("#3F51B5");
            //            600
            //SKColor.TryParse("#3949AB");
            //            700
            //SKColor.TryParse("#303F9F");
            //            800
            //SKColor.TryParse("#283593");
            //            900
            //SKColor.TryParse("#1A237E");
            //            A100
            //SKColor.TryParse("#8C9EFF");
            //            A200
            //SKColor.TryParse("#536DFE");
            //            A400
            //SKColor.TryParse("#3D5AFE");
            //            A700
            //SKColor.TryParse("#304FFE");
            //            Blue
            //500
            //SKColor.TryParse("#2196F3");
            //            50
            //SKColor.TryParse("#E3F2FD");
            //            100
            //SKColor.TryParse("#BBDEFB");
            //            200
            //SKColor.TryParse("#90CAF9");
            //            300
            //SKColor.TryParse("#64B5F6");
            //            400
            //SKColor.TryParse("#42A5F5");
            //            500
            //SKColor.TryParse("#2196F3");
            //            600
            //SKColor.TryParse("#1E88E5");
            //            700
            //SKColor.TryParse("#1976D2");
            //            800
            //SKColor.TryParse("#1565C0");
            //            900
            //SKColor.TryParse("#0D47A1");
            //            A100
            //SKColor.TryParse("#82B1FF");
            //            A200
            //SKColor.TryParse("#448AFF");
            //            A400
            //SKColor.TryParse("#2979FF");
            //            A700
            //SKColor.TryParse("#2962FF");
            //            //Light Blue
            //            //500
            //            SKColor.TryParse("#03A9F4");
            //            50
            //SKColor.TryParse("#E1F5FE");
            //            100
            //SKColor.TryParse("#B3E5FC");
            //            200
            //SKColor.TryParse("#81D4FA");
            //            300
            //SKColor.TryParse("#4FC3F7");
            //            400
            //SKColor.TryParse("#29B6F6");
            //            500
            //SKColor.TryParse("#03A9F4");
            //            600
            //SKColor.TryParse("#039BE5");
            //            700
            //SKColor.TryParse("#0288D1");
            //            800
            //SKColor.TryParse("#0277BD");
            //            900
            //SKColor.TryParse("#01579B");
            //            A100
            //SKColor.TryParse("#80D8FF");
            //            A200
            //SKColor.TryParse("#40C4FF");
            //            A400
            //SKColor.TryParse("#00B0FF");
            //            A700
            //SKColor.TryParse("#0091EA");
            //            Cyan
            //500
            //SKColor.TryParse("#00BCD4");
            //            50
            //SKColor.TryParse("#E0F7FA");
            //            100
            //SKColor.TryParse("#B2EBF2");
            //            200
            //SKColor.TryParse("#80DEEA");
            //            300
            //SKColor.TryParse("#4DD0E1");
            //            400
            //SKColor.TryParse("#26C6DA");
            //            500
            //SKColor.TryParse("#00BCD4");
            //            600
            //SKColor.TryParse("#00ACC1");
            //            700
            //SKColor.TryParse("#0097A7");
            //            800
            //SKColor.TryParse("#00838F");
            //            900
            //SKColor.TryParse("#006064");
            //            A100
            //SKColor.TryParse("#84FFFF");
            //            A200
            //SKColor.TryParse("#18FFFF");
            //            A400
            //SKColor.TryParse("#00E5FF");
            //            A700
            //SKColor.TryParse("#00B8D4");
            //            //Teal
            //            //500
            //            SKColor.TryParse("#009688");
            //            50
            //SKColor.TryParse("#E0F2F1");
            //            100
            //SKColor.TryParse("#B2DFDB");
            //            200
            //SKColor.TryParse("#80CBC4");
            //            300
            //SKColor.TryParse("#4DB6AC");
            //            400
            //SKColor.TryParse("#26A69A");
            //            500
            //SKColor.TryParse("#009688");
            //            600
            //SKColor.TryParse("#00897B");
            //            700
            //SKColor.TryParse("#00796B");
            //            800
            //SKColor.TryParse("#00695C");
            //            900
            //SKColor.TryParse("#004D40");
            //            A100
            //SKColor.TryParse("#A7FFEB");
            //            A200
            //SKColor.TryParse("#64FFDA");
            //            A400
            //SKColor.TryParse("#1DE9B6");
            //            A700
            //SKColor.TryParse("#00BFA5");
            //            //Green
            //            //500
            //            SKColor.TryParse("#4CAF50");
            //            50
            //SKColor.TryParse("#E8F5E9");
            //            100
            //SKColor.TryParse("#C8E6C9");
            //            200
            //SKColor.TryParse("#A5D6A7");
            //            300
            //SKColor.TryParse("#81C784");
            //            400
            //SKColor.TryParse("#66BB6A");
            //            500
            //SKColor.TryParse("#4CAF50");
            //            600
            //SKColor.TryParse("#43A047");
            //            700
            //SKColor.TryParse("#388E3C");
            //            800
            //SKColor.TryParse("#2E7D32");
            //            900
            //SKColor.TryParse("#1B5E20");
            //            A100
            //SKColor.TryParse("#B9F6CA");
            //            A200
            //SKColor.TryParse("#69F0AE");
            //            A400
            //SKColor.TryParse("#00E676");
            //            A700
            //SKColor.TryParse("#00C853");
            //            //Light Green
            //            //500
            //            SKColor.TryParse("#8BC34A");
            //            50
            //SKColor.TryParse("#F1F8E9");
            //            100
            //SKColor.TryParse("#DCEDC8");
            //            200
            //SKColor.TryParse("#C5E1A5");
            //            300
            //SKColor.TryParse("#AED581");
            //            400
            //SKColor.TryParse("#9CCC65");
            //            500
            //SKColor.TryParse("#8BC34A");
            //            600
            //SKColor.TryParse("#7CB342");
            //            700
            //SKColor.TryParse("#689F38");
            //            800
            //SKColor.TryParse("#558B2F");
            //            900
            //SKColor.TryParse("#33691E");
            //            A100
            //SKColor.TryParse("#CCFF90");
            //            A200
            //SKColor.TryParse("#B2FF59");
            //            A400
            //SKColor.TryParse("#76FF03");
            //            A700
            //SKColor.TryParse("#64DD17");
            //            //Lime
            //            //500
            //            SKColor.TryParse("#CDDC39");
            //            50
            //SKColor.TryParse("#F9FBE7");
            //            100
            //SKColor.TryParse("#F0F4C3");
            //            200
            //SKColor.TryParse("#E6EE9C");
            //            300
            //SKColor.TryParse("#DCE775");
            //            400
            //SKColor.TryParse("#D4E157");
            //            500
            //SKColor.TryParse("#CDDC39");
            //            600
            //SKColor.TryParse("#C0CA33");
            //            700
            //SKColor.TryParse("#AFB42B");
            //            800
            //SKColor.TryParse("#9E9D24");
            //            900
            //SKColor.TryParse("#827717");
            //            A100
            //SKColor.TryParse("#F4FF81");
            //            A200
            //SKColor.TryParse("#EEFF41");
            //            A400
            //SKColor.TryParse("#C6FF00");
            //            A700
            //SKColor.TryParse("#AEEA00");
            //            //Yellow
            //            //500
            //            SKColor.TryParse("#FFEB3B");
            //            50
            //SKColor.TryParse("#FFFDE7");
            //            100
            //SKColor.TryParse("#FFF9C4");
            //            200
            //SKColor.TryParse("#FFF59D");
            //            300
            //SKColor.TryParse("#FFF176");
            //            400
            //SKColor.TryParse("#FFEE58");
            //            500
            //SKColor.TryParse("#FFEB3B");
            //            600
            //SKColor.TryParse("#FDD835");
            //            700
            //SKColor.TryParse("#FBC02D");
            //            800
            //SKColor.TryParse("#F9A825");
            //            900
            //SKColor.TryParse("#F57F17");
            //            A100
            //SKColor.TryParse("#FFFF8D");
            //            A200
            //SKColor.TryParse("#FFFF00");
            //            A400
            //SKColor.TryParse("#FFEA00");
            //            A700
            //SKColor.TryParse("#FFD600");
            //            //Amber
            //            //500
            //            SKColor.TryParse("#FFC107");
            //            50
            //SKColor.TryParse("#FFF8E1");
            //            100
            //SKColor.TryParse("#FFECB3");
            //            200
            //SKColor.TryParse("#FFE082");
            //            300
            //SKColor.TryParse("#FFD54F");
            //            400
            //SKColor.TryParse("#FFCA28");
            //            500
            //SKColor.TryParse("#FFC107");
            //            600
            //SKColor.TryParse("#FFB300");
            //            700
            //SKColor.TryParse("#FFA000");
            //            800
            //SKColor.TryParse("#FF8F00");
            //            900
            //SKColor.TryParse("#FF6F00");
            //            A100
            //SKColor.TryParse("#FFE57F");
            //            A200
            //SKColor.TryParse("#FFD740");
            //            A400
            //SKColor.TryParse("#FFC400");
            //            A700
            //SKColor.TryParse("#FFAB00");
            //            //Orange
            //            //500
            //            SKColor.TryParse("#FF9800");
            //            50
            //SKColor.TryParse("#FFF3E0");
            //            100
            //SKColor.TryParse("#FFE0B2");
            //            200
            //SKColor.TryParse("#FFCC80");
            //            300
            //SKColor.TryParse("#FFB74D");
            //            400
            //SKColor.TryParse("#FFA726");
            //            500
            //SKColor.TryParse("#FF9800");
            //            600
            //SKColor.TryParse("#FB8C00");
            //            700
            //SKColor.TryParse("#F57C00");
            //            800
            //SKColor.TryParse("#EF6C00");
            //            900
            //SKColor.TryParse("#E65100");
            //            A100
            //SKColor.TryParse("#FFD180");
            //            A200
            //SKColor.TryParse("#FFAB40");
            //            A400
            //SKColor.TryParse("#FF9100");
            //            A700
            //SKColor.TryParse("#FF6D00");
            //            Deep Orange
            //500
            //SKColor.TryParse("#FF5722");
            //            50
            //SKColor.TryParse("#FBE9E7");
            //            100
            //SKColor.TryParse("#FFCCBC");
            //            200
            //SKColor.TryParse("#FFAB91");
            //            300
            //SKColor.TryParse("#FF8A65");
            //            400
            //SKColor.TryParse("#FF7043");
            //            500
            //SKColor.TryParse("#FF5722");
            //            600
            //SKColor.TryParse("#F4511E");
            //            700
            //SKColor.TryParse("#E64A19");
            //            800
            //SKColor.TryParse("#D84315");
            //            900
            //SKColor.TryParse("#BF360C");
            //            A100
            //SKColor.TryParse("#FF9E80");
            //            A200
            //SKColor.TryParse("#FF6E40");
            //            A400
            //SKColor.TryParse("#FF3D00");
            //            A700
            //SKColor.TryParse("#DD2C00");
            //            //Brown
            //            //500
            //            SKColor.TryParse("#795548");
            //            50
            //SKColor.TryParse("#EFEBE9");
            //            100
            //SKColor.TryParse("#D7CCC8");
            //            200
            //SKColor.TryParse("#BCAAA4");
            //            300
            //SKColor.TryParse("#A1887F");
            //            400
            //SKColor.TryParse("#8D6E63");
            //            500
            //SKColor.TryParse("#795548");
            //            600
            //SKColor.TryParse("#6D4C41");
            //            700
            //SKColor.TryParse("#5D4037");
            //            800
            //SKColor.TryParse("#4E342E");
            //            900
            //SKColor.TryParse("#3E2723");
            //            //Grey
            //            //500
            //            SKColor.TryParse("#9E9E9E");
            //            50
            //SKColor.TryParse("#FAFAFA");
            //            100
            //SKColor.TryParse("#F5F5F5");
            //            200
            //SKColor.TryParse("#EEEEEE");
            //            300
            //SKColor.TryParse("#E0E0E0");
            //            400
            //SKColor.TryParse("#BDBDBD");
            //            500
            //SKColor.TryParse("#9E9E9E");
            //            600
            //SKColor.TryParse("#757575");
            //            700
            //SKColor.TryParse("#616161");
            //            800
            //SKColor.TryParse("#424242");
            //            900
            //SKColor.TryParse("#212121");
            //            //Blue Grey
            //            //500
            //            SKColor.TryParse("#607D8B");
            //            50
            //SKColor.TryParse("#ECEFF1");
            //            100
            //SKColor.TryParse("#CFD8DC");
            //            200
            //SKColor.TryParse("#B0BEC5");
            //            300
            //SKColor.TryParse("#90A4AE");
            //            400
            //SKColor.TryParse("#78909C");
            //            500
            //SKColor.TryParse("#607D8B");
            //            600
            //SKColor.TryParse("#546E7A");
            //            700
            //SKColor.TryParse("#455A64");
            //            800
            //SKColor.TryParse("#37474F");
            //            900
            //SKColor.TryParse("#263238");
            //    //Black
            //    SKColor.TryParse("#000000");
            //    //White
            //    SKColor.TryParse("#FFFFFF");
            //    }
            //}

        public static List<SKColor> GetSkColorList() 
        {
            List<SKColor> _colors = new List<SKColor>()
            {
                // Red and Pink
                SKColor.Parse("#F44336"),
                SKColor.Parse("#FFCDD2"),
                SKColor.Parse("#E91E63"),
                SKColor.Parse("#F8BBD0"),
                // Purple
                SKColor.Parse("#9C27B0"),
                SKColor.Parse("#E1BEE7"),
                SKColor.Parse("#673AB7"),
                SKColor.Parse("#D1C4E9"),
                // Blue
                SKColor.Parse("#2196F3"),
                SKColor.Parse("#90CAF9"),
                SKColor.Parse("#009688"),
                SKColor.Parse("#80CBC4"),
                // Green
                SKColor.Parse("#4CAF50"),
                SKColor.Parse("#A5D6A7"),
                SKColor.Parse("#76FF03"),
                SKColor.Parse("#CCFF90"),
                // Orange
                SKColor.Parse("#FF9800"),
                SKColor.Parse("#FFCC80"),
                SKColor.Parse("#FFC107"),
                SKColor.Parse("#FFE082"),
                // Yellow
                SKColor.Parse("#FFEB3B"),
                SKColor.Parse("#FFF176"),
                SKColor.Parse("#FFD600"),
                SKColor.Parse("#FFFF8D"),
                // Brown
                SKColor.Parse("#795548"),
                SKColor.Parse("#BCAAA4"),
                SKColor.Parse("#3E2723"),
                SKColor.Parse("#795548"),
                // Black and Gray
                SKColor.Parse("#9E9E9E"),
                SKColor.Parse("#E0E0E0"),
                SKColor.Parse("#757575"),
                SKColor.Parse("#000000"),

            };
            return _colors;
        }
    }

    public class SkColorUtility
    {
        public SkColorUtility()
        {
        }



    }
}
