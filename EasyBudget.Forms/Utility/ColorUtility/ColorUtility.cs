using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EasyBudget.Forms.Utility.ColorUtility
{
    public class ColorUtility
    {
        public ColorUtility()
        {
            this.AccentColors = new List<AppColor>();
        }

        public IList<AppColor> PrimaryColors { get; set; }

        public IList<AppColor> AccentColors { get; set; }

        public delegate IList<AppColor> GetColorSetDelegate();

        public static IList<AppColor> GetPrimaryColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#EF5350"), "Red - 500", "#EF5350", GetRedColors),
                new AppColor(Color.FromHex("#E91E63"), "Pink - 500", "#E91E63", GetPinkColors),
                new AppColor(Color.FromHex("#9C27B0"), "Purple - 500", "#9C27B0", GetPurpleColors),
                new AppColor(Color.FromHex("#673AB7"), "Deep Purple - 500", "#673AB7", GetDeepPurpleColors),
                new AppColor(Color.FromHex("#3F51B5"), "Indigo - 500", "#3F51B5", GetIndigoColors),
                new AppColor(Color.FromHex("#2196F3"), "Blue - 500", "#2196F3", GetBlueColors),
                new AppColor(Color.FromHex("#03A9F4"), "Light Blue - 500", "#03A9F4", GetLightBlueColors),
                new AppColor(Color.FromHex("#00BCD4"), "Cyan - 500", "#00BCD4", GetCyanColors),
                new AppColor(Color.FromHex("#009688"), "Teal - 500", "#009688", GetTealColors),
                new AppColor(Color.FromHex("#4CAF50"), "Green - 500", "#4CAF50", GetGreenColors),
                new AppColor(Color.FromHex("#8BC34A"), "Light Green - 500", "#8BC34A", GetLightGreenColors),
                new AppColor(Color.FromHex("#CDDC39"), "Lime - 500", "#CDDC39", GetLimeColors),
                new AppColor(Color.FromHex("#FFEB3B"), "Yellow - 500", "#FFEB3B", GetYellowColors),
                new AppColor(Color.FromHex("#FFC107"), "Amber - 500", "#FFC107", GetAmberColors),
                new AppColor(Color.FromHex("#FF9800"), "Orange - 500", "#FF9800", GetOrangeColors),
                new AppColor(Color.FromHex("#FF5722"), "Deep Orange - 500", "#FF5722", GetDeepOrangeColors),
                new AppColor(Color.FromHex("#795548"), "Brown - 500", "#795548", GetBrownColors),
                new AppColor(Color.FromHex("#9E9E9E"), "Grey - 500", "#9E9E9E", GetGreyColors),
                new AppColor(Color.FromHex("#607D8B"), "Blue Grey - 500", "#607D8B", GetBlueGreyColors),
                new AppColor(Color.FromHex("#000000"), "Black", "#000000", GetBlackWhiteColors),
                new AppColor(Color.FromHex("#ffffff"), "White", "#ffffff", GetBlackWhiteColors),
            };
            return _colors;
        }

        public static IList<AppColor> GetRedColors()
        {

            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#EF5350"), "Red - 500", "#EF5350"),
                new AppColor(Color.FromHex("#F44336"), "50", "#F44336"),
                new AppColor(Color.FromHex("#FFEBEE"), "100", "#FFEBEE"),
                new AppColor(Color.FromHex("#FFCDD2"), "200", "#FFCDD2"),
                new AppColor(Color.FromHex("#EF9A9A"), "300", "#EF9A9A"),
                new AppColor(Color.FromHex("#E57373"), "400", "#E57373"),
                new AppColor(Color.FromHex("#EF5350"), "500", "#EF5350"),
                new AppColor(Color.FromHex("#F44336"), "600", "#F44336"),
                new AppColor(Color.FromHex("#E53935"), "700", "#E53935"),
                new AppColor(Color.FromHex("#D32F2F"), "800", "#D32F2F"),
                new AppColor(Color.FromHex("#C62828"), "900", "#C62828"),
                new AppColor(Color.FromHex("#B71C1C"), "A100", "#B71C1C"),
                new AppColor(Color.FromHex("#FF8A80"), "A200", "#FF8A80"),
                new AppColor(Color.FromHex("#FF5252"), "A400", "#FF5252"),
                new AppColor(Color.FromHex("#FF1744"), "A700", "#FF1744"),
            };
            return _colors;
        }

        public static IList<AppColor> GetPinkColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#E91E63"), "Pink - 500", "#E91E63"),
                new AppColor(Color.FromHex("#FCE4EC"), "50", "#FCE4EC"),
                new AppColor(Color.FromHex("#F8BBD0"), "100", "#F8BBD0"),
                new AppColor(Color.FromHex("#F48FB1"), "200", "#F48FB1"),
                new AppColor(Color.FromHex("#F06292"), "300", "#F06292"),
                new AppColor(Color.FromHex("#EC407A"), "400", "#EC407A"),
                new AppColor(Color.FromHex("#E91E63"), "500", "#E91E63"),
                new AppColor(Color.FromHex("#D81B60"), "600", "#D81B60"),
                new AppColor(Color.FromHex("#C2185B"), "700", "#C2185B"),
                new AppColor(Color.FromHex("#AD1457"), "800", "#AD1457"),
                new AppColor(Color.FromHex("#880E4F"), "900", "#880E4F"),
                new AppColor(Color.FromHex("#FF80AB"), "A100", "#FF80AB"),
                new AppColor(Color.FromHex("#FF4081"), "A200", "#FF4081"),
                new AppColor(Color.FromHex("#F50057"), "A400", "#F50057"),
                new AppColor(Color.FromHex("#C51162"), "A700", "#C51162"),
            };
            return _colors;
        }

        public static IList<AppColor> GetPurpleColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#9C27B0"), "Purple - 500", "#9C27B0"),
                new AppColor(Color.FromHex("#F3E5F5"), "50", "#F3E5F5"),
                new AppColor(Color.FromHex("#E1BEE7"), "100", "#E1BEE7"),
                new AppColor(Color.FromHex("#CE93D8"), "200", "#CE93D8"),
                new AppColor(Color.FromHex("#BA68C8"), "300", "#BA68C8"),
                new AppColor(Color.FromHex("#AB47BC"), "400", "#AB47BC"),
                new AppColor(Color.FromHex("#9C27B0"), "500", "#9C27B0"),
                new AppColor(Color.FromHex("#8E24AA"), "600", "#8E24AA"),
                new AppColor(Color.FromHex("#7B1FA2"), "700", "#7B1FA2"),
                new AppColor(Color.FromHex("#6A1B9A"), "800", "#6A1B9A"),
                new AppColor(Color.FromHex("#4A148C"), "900", "#4A148C"),
                new AppColor(Color.FromHex("#EA80FC"), "A100", "#EA80FC"),
                new AppColor(Color.FromHex("#E040FB"), "A200", "#E040FB"),
                new AppColor(Color.FromHex("#D500F9"), "A400", "#D500F9"),
                new AppColor(Color.FromHex("#AA00FF"), "A700", "#AA00FF"),
            };
            return _colors;
        }

        public static IList<AppColor> GetDeepPurpleColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#673AB7"), "Deep Purple - 500", "#673AB7"),
                new AppColor(Color.FromHex("#EDE7F6"), "50", "#EDE7F6"),
                new AppColor(Color.FromHex("#D1C4E9"), "100", "#D1C4E9"),
                new AppColor(Color.FromHex("#B39DDB"), "200", "#B39DDB"),
                new AppColor(Color.FromHex("#9575CD"), "300", "#9575CD"),
                new AppColor(Color.FromHex("#7E57C2"), "400", "#7E57C2"),
                new AppColor(Color.FromHex("#673AB7"), "500", "#673AB7"),
                new AppColor(Color.FromHex("#5E35B1"), "600", "#5E35B1"),
                new AppColor(Color.FromHex("#512DA8"), "700", "#512DA8"),
                new AppColor(Color.FromHex("#4527A0"), "800", "#4527A0"),
                new AppColor(Color.FromHex("#311B92"), "900", "#311B92"),
                new AppColor(Color.FromHex("#B388FF"), "A100", "#B388FF"),
                new AppColor(Color.FromHex("#7C4DFF"), "A200", "#7C4DFF"),
                new AppColor(Color.FromHex("#651FFF"), "A400", "#651FFF"),
                new AppColor(Color.FromHex("#6200EA"), "A700", "#6200EA"),
            };

            return _colors;
        }

        public static IList<AppColor> GetIndigoColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#3F51B5"), "Indigo - 500", "#3F51B5"),
                new AppColor(Color.FromHex("#E8EAF6"), "50", "#E8EAF6"),
                new AppColor(Color.FromHex("#C5CAE9"), "100", "#C5CAE9"),
                new AppColor(Color.FromHex("#9FA8DA"), "200", "#9FA8DA"),
                new AppColor(Color.FromHex("#7986CB"), "300", "#7986CB"),
                new AppColor(Color.FromHex("#5C6BC0"), "400", "#5C6BC0"),
                new AppColor(Color.FromHex("#3F51B5"), "500", "#3F51B5"),
                new AppColor(Color.FromHex("#3949AB"), "600", "#3949AB"),
                new AppColor(Color.FromHex("#303F9F"), "700", "#303F9F"),
                new AppColor(Color.FromHex("#283593"), "800", "#283593"),
                new AppColor(Color.FromHex("#1A237E"), "900", "#1A237E"),
                new AppColor(Color.FromHex("#8C9EFF"), "A100", "#8C9EFF"),
                new AppColor(Color.FromHex("#536DFE"), "A200", "#536DFE"),
                new AppColor(Color.FromHex("#3D5AFE"), "A400", "#3D5AFE"),
                new AppColor(Color.FromHex("#304FFE"), "A700", "#304FFE"),
            };

            return _colors;
        }

        public static IList<AppColor> GetBlueColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#2196F3"), "Blue - 500", "#2196F3"),
                new AppColor(Color.FromHex("#E3F2FD"), "50", "#E3F2FD"),
                new AppColor(Color.FromHex("#BBDEFB"), "100", "#BBDEFB"),
                new AppColor(Color.FromHex("#90CAF9"), "200", "#90CAF9"),
                new AppColor(Color.FromHex("#64B5F6"), "300", "#64B5F6"),
                new AppColor(Color.FromHex("#42A5F5"), "400", "#42A5F5"),
                new AppColor(Color.FromHex("#2196F3"), "500", "#2196F3"),
                new AppColor(Color.FromHex("#1E88E5"), "600", "#1E88E5"),
                new AppColor(Color.FromHex("#1976D2"), "700", "#1976D2"),
                new AppColor(Color.FromHex("#1565C0"), "800", "#1565C0"),
                new AppColor(Color.FromHex("#0D47A1"), "900", "#0D47A1"),
                new AppColor(Color.FromHex("#82B1FF"), "A100", "#82B1FF"),
                new AppColor(Color.FromHex("#448AFF"), "A200", "#448AFF"),
                new AppColor(Color.FromHex("#2979FF"), "A400", "#2979FF"),
                new AppColor(Color.FromHex("#2962FF"), "A700", "#2962FF"),
            };

            return _colors;
        }

        public static IList<AppColor> GetLightBlueColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#03A9F4"), "Light Blue - 500", "#03A9F4"),
                new AppColor(Color.FromHex("#E1F5FE"), "50", "#E1F5FE"),
                new AppColor(Color.FromHex("#B3E5FC"), "100", "#B3E5FC"),
                new AppColor(Color.FromHex("#81D4FA"), "200", "#81D4FA"),
                new AppColor(Color.FromHex("#4FC3F7"), "300", "#4FC3F7"),
                new AppColor(Color.FromHex("#29B6F6"), "400", "#29B6F6"),
                new AppColor(Color.FromHex("#03A9F4"), "500", "#03A9F4"),
                new AppColor(Color.FromHex("#039BE5"), "600", "#039BE5"),
                new AppColor(Color.FromHex("#0288D1"), "700", "#0288D1"),
                new AppColor(Color.FromHex("#0277BD"), "800", "#0277BD"),
                new AppColor(Color.FromHex("#01579B"), "900", "#01579B"),
                new AppColor(Color.FromHex("#01579B"), "A100", "#01579B"),
                new AppColor(Color.FromHex("#40C4FF"), "A200", "#40C4FF"),
                new AppColor(Color.FromHex("#00B0FF"), "A400", "#00B0FF"),
                new AppColor(Color.FromHex("#0091EA"), "A700", "#0091EA"),
            };

            return _colors;
        }

        public static IList<AppColor> GetCyanColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#00BCD4"), "Cyan - 500", "#00BCD4"),
                new AppColor(Color.FromHex("#E0F7FA"), "50", "#E0F7FA"),
                new AppColor(Color.FromHex("#B2EBF2"), "100", "#B2EBF2"),
                new AppColor(Color.FromHex("#80DEEA"), "200", "#80DEEA"),
                new AppColor(Color.FromHex("#4DD0E1"), "300", "#4DD0E1"),
                new AppColor(Color.FromHex("#26C6DA"), "400", "#26C6DA"),
                new AppColor(Color.FromHex("#00BCD4"), "500", "#00BCD4"),
                new AppColor(Color.FromHex("#00ACC1"), "600", "#00ACC1"),
                new AppColor(Color.FromHex("#0097A7"), "700", "#0097A7"),
                new AppColor(Color.FromHex("#00838F"), "800", "#00838F"),
                new AppColor(Color.FromHex("#006064"), "900", "#006064"),
                new AppColor(Color.FromHex("#84FFFF"), "A100", "#84FFFF"),
                new AppColor(Color.FromHex("#18FFFF"), "A200", "#18FFFF"),
                new AppColor(Color.FromHex("#00E5FF"), "A400", "#00E5FF"),
                new AppColor(Color.FromHex("#00B8D4"), "A700", "#00B8D4"),
            };
            return _colors;
        }

        public static IList<AppColor> GetTealColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#009688"), "Teal - 500", "#009688"),
                new AppColor(Color.FromHex("#E0F2F1"), "50", "#E0F2F1"),
                new AppColor(Color.FromHex("#B2DFDB"), "100", "#B2DFDB"),
                new AppColor(Color.FromHex("#80CBC4"), "200", "#80CBC4"),
                new AppColor(Color.FromHex("#4DB6AC"), "300", "#4DB6AC"),
                new AppColor(Color.FromHex("#26A69A"), "400", "#26A69A"),
                new AppColor(Color.FromHex("#009688"), "500", "#009688"),
                new AppColor(Color.FromHex("#00897B"), "600", "#00897B"),
                new AppColor(Color.FromHex("#00796B"), "700", "#00796B"),
                new AppColor(Color.FromHex("#00695C"), "800", "#00695C"),
                new AppColor(Color.FromHex("#004D40"), "900", "#004D40"),
                new AppColor(Color.FromHex("#A7FFEB"), "A100", "#A7FFEB"),
                new AppColor(Color.FromHex("#64FFDA"), "A200", "#64FFDA"),
                new AppColor(Color.FromHex("#1DE9B6"), "A400", "#1DE9B6"),
                new AppColor(Color.FromHex("#00BFA5"), "A700", "#00BFA5"),
            };
            return _colors;
        }

        public static IList<AppColor> GetGreenColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#4CAF50"), "Green - 500", "#4CAF50"),
                new AppColor(Color.FromHex("#E8F5E9"), "50", "#E8F5E9"),
                new AppColor(Color.FromHex("#C8E6C9"), "100", "#C8E6C9"),
                new AppColor(Color.FromHex("#A5D6A7"), "200", "#A5D6A7"),
                new AppColor(Color.FromHex("#81C784"), "300", "#81C784"),
                new AppColor(Color.FromHex("#66BB6A"), "400", "#66BB6A"),
                new AppColor(Color.FromHex("#4CAF50"), "500", "#4CAF50"),
                new AppColor(Color.FromHex("#43A047"), "600", "#43A047"),
                new AppColor(Color.FromHex("#388E3C"), "700", "#388E3C"),
                new AppColor(Color.FromHex("#2E7D32"), "800", "#2E7D32"),
                new AppColor(Color.FromHex("#1B5E20"), "900", "#1B5E20"),
                new AppColor(Color.FromHex("#B9F6CA"), "A100", "#B9F6CA"),
                new AppColor(Color.FromHex("#69F0AE"), "A200", "#69F0AE"),
                new AppColor(Color.FromHex("#00E676"), "A400", "#00E676"),
                new AppColor(Color.FromHex("#00C853"), "A700", "#00C853"),
            };
            return _colors;
        }

        public static IList<AppColor> GetLightGreenColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#8BC34A"), "Light Green - 500", "#8BC34A"),
                new AppColor(Color.FromHex("#F1F8E9"), "50", "#F1F8E9"),
                new AppColor(Color.FromHex("#DCEDC8"), "100", "#DCEDC8"),
                new AppColor(Color.FromHex("#C5E1A5"), "200", "#C5E1A5"),
                new AppColor(Color.FromHex("#AED581"), "300", "#AED581"),
                new AppColor(Color.FromHex("#9CCC65"), "400", "#9CCC65"),
                new AppColor(Color.FromHex("#8BC34A"), "500", "#8BC34A"),
                new AppColor(Color.FromHex("#7CB342"), "600", "#7CB342"),
                new AppColor(Color.FromHex("#689F38"), "700", "#689F38"),
                new AppColor(Color.FromHex("#558B2F"), "800", "#558B2F"),
                new AppColor(Color.FromHex("#33691E"), "900", "#33691E"),
                new AppColor(Color.FromHex("#CCFF90"), "A100", "#CCFF90"),
                new AppColor(Color.FromHex("#B2FF59"), "A200", "#B2FF59"),
                new AppColor(Color.FromHex("#76FF03"), "A400", "#76FF03"),
                new AppColor(Color.FromHex("#64DD17"), "A700", "#64DD17"),
            };
            return _colors;
        }

        public static IList<AppColor> GetLimeColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#CDDC39"), "Lime - 500", "#CDDC39"),
                new AppColor(Color.FromHex("#F9FBE7"), "50", "#F9FBE7"),
                new AppColor(Color.FromHex("#F0F4C3"), "100", "#F0F4C3"),
                new AppColor(Color.FromHex("#E6EE9C"), "200", "#E6EE9C"),
                new AppColor(Color.FromHex("#DCE775"), "300", "#DCE775"),
                new AppColor(Color.FromHex("#D4E157"), "400", "#D4E157"),
                new AppColor(Color.FromHex("#CDDC39"), "500", "#CDDC39"),
                new AppColor(Color.FromHex("#C0CA33"), "600", "#C0CA33"),
                new AppColor(Color.FromHex("#AFB42B"), "700", "#AFB42B"),
                new AppColor(Color.FromHex("#9E9D24"), "800", "#9E9D24"),
                new AppColor(Color.FromHex("#827717"), "900", "#827717"),
                new AppColor(Color.FromHex("#F4FF81"), "A100", "#F4FF81"),
                new AppColor(Color.FromHex("#EEFF41"), "A200", "#EEFF41"),
                new AppColor(Color.FromHex("#C6FF00"), "A400", "#C6FF00"),
                new AppColor(Color.FromHex("#AEEA00"), "A700", "#AEEA00"),
            };
            return _colors;
        }

        public static IList<AppColor> GetYellowColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#FFEB3B"), "Yellow - 500", "#FFEB3B"),
                new AppColor(Color.FromHex("#FFFDE7"), "50", "#FFFDE7"),
                new AppColor(Color.FromHex("#FFF9C4"), "100", "#FFF9C4"),
                new AppColor(Color.FromHex("#FFF59D"), "200", "#FFF59D"),
                new AppColor(Color.FromHex("#FFF176"), "300", "#FFF176"),
                new AppColor(Color.FromHex("#FFEE58"), "400", "#FFEE58"),
                new AppColor(Color.FromHex("#FFEB3B"), "500", "#FFEB3B"),
                new AppColor(Color.FromHex("#FDD835"), "600", "#FDD835"),
                new AppColor(Color.FromHex("#FBC02D"), "700", "#FBC02D"),
                new AppColor(Color.FromHex("#F9A825"), "800", "#F9A825"),
                new AppColor(Color.FromHex("#F57F17"), "900", "#F57F17"),
                new AppColor(Color.FromHex("#FFFF8D"), "A100", "#FFFF8D"),
                new AppColor(Color.FromHex("#FFFF00"), "A200", "#FFFF00"),
                new AppColor(Color.FromHex("#FFEA00"), "A400", "#FFEA00"),
                new AppColor(Color.FromHex("#FFD600"), "A700", "#FFD600"),
            };
            return _colors;
        }

        public static IList<AppColor> GetAmberColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#FFC107"), "Amber - 500", "#FFC107"),
                new AppColor(Color.FromHex("#FFF8E1"), "50", "#FFF8E1"),
                new AppColor(Color.FromHex("#FFECB3"), "100", "#FFECB3"),
                new AppColor(Color.FromHex("#FFE082"), "200", "#FFE082"),
                new AppColor(Color.FromHex("#FFD54F"), "300", "#FFD54F"),
                new AppColor(Color.FromHex("#FFCA28"), "400", "#FFCA28"),
                new AppColor(Color.FromHex("#FFC107"), "500", "#FFC107"),
                new AppColor(Color.FromHex("#FFB300"), "600", "#FFB300"),
                new AppColor(Color.FromHex("#FFA000"), "700", "#FFA000"),
                new AppColor(Color.FromHex("#FF8F00"), "800", "#FF8F00"),
                new AppColor(Color.FromHex("#FF6F00"), "900", "#FF6F00"),
                new AppColor(Color.FromHex("#FFE57F"), "A100", "#FFE57F"),
                new AppColor(Color.FromHex("#FFD740"), "A200", "#FFD740"),
                new AppColor(Color.FromHex("#FFC400"), "A400", "#FFC400"),
                new AppColor(Color.FromHex("#FFAB00"), "A700", "#FFAB00"),
            };
            return _colors;
        }

        public static IList<AppColor> GetOrangeColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#FF9800"), "Orange - 500", "#FF9800"),
                new AppColor(Color.FromHex("#FFF3E0"), "50", "#FFF3E0"),
                new AppColor(Color.FromHex("#FFE0B2"), "100", "#FFE0B2"),
                new AppColor(Color.FromHex("#FFCC80"), "200", "#FFCC80"),
                new AppColor(Color.FromHex("#FFB74D"), "300", "#FFB74D"),
                new AppColor(Color.FromHex("#FFA726"), "400", "#FFA726"),
                new AppColor(Color.FromHex("#FF9800"), "500", "#FF9800"),
                new AppColor(Color.FromHex("#FB8C00"), "600", "#FB8C00"),
                new AppColor(Color.FromHex("#F57C00"), "700", "#F57C00"),
                new AppColor(Color.FromHex("#EF6C00"), "800", "#EF6C00"),
                new AppColor(Color.FromHex("#E65100"), "900", "#E65100"),
                new AppColor(Color.FromHex("#FFD180"), "A100", "#FFD180"),
                new AppColor(Color.FromHex("#FFAB40"), "A200", "#FFAB40"),
                new AppColor(Color.FromHex("#FF9100"), "A400", "#FF9100"),
                new AppColor(Color.FromHex("#FF6D00"), "A700", "#FF6D00"),
            };
            return _colors;
        }

        public static IList<AppColor> GetDeepOrangeColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#FF5722"), "Deep Orange - 500", "#FF5722"),
                new AppColor(Color.FromHex("#FBE9E7"), "50", "#FBE9E7"),
                new AppColor(Color.FromHex("#FFCCBC"), "100", "#FFCCBC"),
                new AppColor(Color.FromHex("#FFAB91"), "200", "#FFAB91"),
                new AppColor(Color.FromHex("#FF8A65"), "300", "#FF8A65"),
                new AppColor(Color.FromHex("#FF7043"), "400", "#FF7043"),
                new AppColor(Color.FromHex("#FF5722"), "500", "#FF5722"),
                new AppColor(Color.FromHex("#F4511E"), "600", "#F4511E"),
                new AppColor(Color.FromHex("#E64A19"), "700", "#E64A19"),
                new AppColor(Color.FromHex("#D84315"), "800", "#D84315"),
                new AppColor(Color.FromHex("#BF360C"), "900", "#BF360C"),
                new AppColor(Color.FromHex("#FF9E80"), "A100", "#FF9E80"),
                new AppColor(Color.FromHex("#FF6E40"), "A200", "#FF6E40"),
                new AppColor(Color.FromHex("#FF3D00"), "A400", "#FF3D00"),
                new AppColor(Color.FromHex("#DD2C00"), "A700", "#DD2C00"),
            };
            return _colors;
        }

        public static IList<AppColor> GetBrownColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#795548"), "Brown - 500", "#795548"),
                new AppColor(Color.FromHex("#EFEBE9"), "50", "#EFEBE9"),
                new AppColor(Color.FromHex("#D7CCC8"), "100", "#D7CCC8"),
                new AppColor(Color.FromHex("#BCAAA4"), "200", "#BCAAA4"),
                new AppColor(Color.FromHex("#A1887F"), "300", "#A1887F"),
                new AppColor(Color.FromHex("#8D6E63"), "400", "#8D6E63"),
                new AppColor(Color.FromHex("#795548"), "500", "#795548"),
                new AppColor(Color.FromHex("#6D4C41"), "600", "#6D4C41"),
                new AppColor(Color.FromHex("#5D4037"), "700", "#5D4037"),
                new AppColor(Color.FromHex("#4E342E"), "800", "#4E342E"),
                new AppColor(Color.FromHex("#3E2723"), "900", "#3E2723"),
            };
            return _colors;
        }

        public static IList<AppColor> GetGreyColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#9E9E9E"), "Grey - 500", "#9E9E9E"),
                new AppColor(Color.FromHex("#FAFAFA"), "50", "#FAFAFA"),
                new AppColor(Color.FromHex("#F5F5F5"), "100", "#F5F5F5"),
                new AppColor(Color.FromHex("#EEEEEE"), "200", "#EEEEEE"),
                new AppColor(Color.FromHex("#E0E0E0"), "300", "#E0E0E0"),
                new AppColor(Color.FromHex("#BDBDBD"), "400", "#BDBDBD"),
                new AppColor(Color.FromHex("#9E9E9E"), "500", "#9E9E9E"),
                new AppColor(Color.FromHex("#757575"), "600", "#757575"),
                new AppColor(Color.FromHex("#616161"), "700", "#616161"),
                new AppColor(Color.FromHex("#424242"), "800", "#424242"),
                new AppColor(Color.FromHex("#212121"), "900", "#212121"),
            };
            return _colors;
        }

        public static IList<AppColor> GetBlueGreyColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#607D8B"), "Blue Grey - 500", "#607D8B"),
                new AppColor(Color.FromHex("#ECEFF1"), "50", "#ECEFF1"),
                new AppColor(Color.FromHex("#CFD8DC"), "100", "#CFD8DC"),
                new AppColor(Color.FromHex("#B0BEC5"), "200", "#B0BEC5"),
                new AppColor(Color.FromHex("#90A4AE"), "300", "#90A4AE"),
                new AppColor(Color.FromHex("#78909C"), "400", "#78909C"),
                new AppColor(Color.FromHex("#607D8B"), "500", "#607D8B"),
                new AppColor(Color.FromHex("#546E7A"), "600", "#546E7A"),
                new AppColor(Color.FromHex("#455A64"), "700", "#455A64"),
                new AppColor(Color.FromHex("#37474F"), "800", "#37474F"),
                new AppColor(Color.FromHex("#263238"), "900", "#263238"),
            };
            return _colors;
        }

        public static IList<AppColor> GetBlackWhiteColors()
        {
            List<AppColor> _colors = new List<AppColor>
            {
                new AppColor(Color.FromHex("#000000"), "Black", "#000000"),
                new AppColor(Color.FromHex("#ffffff"), "White", "#ffffff"),
            };
            return _colors;
        }

        public static AppColor FindAppColor(string colorCode)
        {
            AppColor _color = GetAllColors().Any(c => c.Name == colorCode) ? 
                                            GetAllColors().FirstOrDefault(c => c.Name == colorCode) : 
                                            null;

            return _color;
        }

        public static IList<AppColor> GetAllColors()
        {
            List<AppColor> _allColors = new List<AppColor>();
            _allColors.AddRange(GetRedColors());
            _allColors.AddRange(GetBlueColors());
            _allColors.AddRange(GetCyanColors());
            _allColors.AddRange(GetGreyColors());
            _allColors.AddRange(GetLimeColors());
            _allColors.AddRange(GetPinkColors());
            _allColors.AddRange(GetTealColors());
            _allColors.AddRange(GetAmberColors());
            _allColors.AddRange(GetBrownColors());
            _allColors.AddRange(GetGreenColors());
            _allColors.AddRange(GetIndigoColors());
            _allColors.AddRange(GetOrangeColors());
            _allColors.AddRange(GetPurpleColors());
            _allColors.AddRange(GetYellowColors());
            _allColors.AddRange(GetBlueGreyColors());
            _allColors.AddRange(GetLightBlueColors());
            _allColors.AddRange(GetBlackWhiteColors());
            _allColors.AddRange(GetDeepOrangeColors());
            _allColors.AddRange(GetDeepPurpleColors());
            _allColors.AddRange(GetLightGreenColors());
            return _allColors;
        }
    }




}
