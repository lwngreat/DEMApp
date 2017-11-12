using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DEMApp.UC
{
    /// <summary>
    /// RibbonComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class RibbonComboBox : UserControl
    {
        private List<LinearColors> colors;

        public RibbonComboBox()
        {
            InitializeComponent();
            this.Loaded += RibbonComboBox_Loaded;
        }
        public void setColors(List<LinearColors> c)
        {
            colors = c;
            this.combColor.ItemsSource = colors;
        }

        void RibbonComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            colors = new List<LinearColors>();
            List<Color> c = new List<Color> { Color.FromRgb(255, 255, 255), Color.FromRgb(204, 204, 204), Color.FromRgb(153, 153, 153), Color.FromRgb(102, 102, 102), Color.FromRgb(51, 51, 51), Color.FromRgb(0, 0, 0) };
            List<Color> c1 = new List<Color> { Color.FromRgb(177, 242, 213), Color.FromRgb(250, 250, 179), Color.FromRgb(15, 145, 52), Color.FromRgb(252, 186, 3), Color.FromRgb(112, 36, 8), Color.FromRgb(255, 255, 255) };
            List<Color> c2 = new List<Color> { Color.FromRgb(177, 242, 213), Color.FromRgb(250, 250, 179), Color.FromRgb(15, 145, 52), Color.FromRgb(252, 186, 3), Color.FromRgb(112, 36, 8), Color.FromRgb(255, 255, 255) };
            c.Reverse();
            colors.Add(new LinearColors(c));
            colors.Add(new LinearColors(c1));
            var lc2 = new LinearColors(c2);
            lc2.classfied = true;
            colors.Add(lc2);
            this.combColor.ItemsSource = colors;
            this.combColor.SelectedIndex = 0;

        }

    }
    /// <summary>
    /// 色带模型，必须加入6个
    /// </summary>
    public class LinearColors
    {
        private List<Color> colors;
        public bool classfied = false;
        public Visibility classfiedV
        {
            get
            {
                if (classfied)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        public LinearColors(List<Color> c)
        {
            colors = c;
        }
        public Color color1
        {
            get { return colors[0]; }
        }
        public Color color2
        {
            get { return colors[1]; }
        }
        public Color color3
        {
            get { return colors[2]; }
        }
        public Color color4
        {
            get { return colors[3]; }
        }
        public Color color5
        {
            get { return colors[4]; }
        }
        public Color color6
        {
            get { return colors[5]; }
        }

        public Color getColor(int m)
        {
            if (m < 0 || m > 255) return Colors.Black;
            if (m == 255) return color6;
            var d = m / 51;
            try
            {
               
                Color c1 = colors[d];
                Color c2 = colors[d + 1];
                if (!classfied)
                {
                    var r = (byte)Math.Round((m - d * 51) * (c2.R - c1.R) / 51.0 + c1.R);
                    var g = (byte)Math.Round((m - d * 51) * (c2.G - c1.G) / 51.0 + c1.G);
                    var b = (byte)Math.Round((m - d * 51) * (c2.B - c1.B) / 51.0 + c1.B);
                    return Color.FromRgb(r, g, b);
                }
                else
                {
                    return c1;
                }
                
                
            }
            catch (Exception ex)
            {
                return Colors.Black;
            }
            
        }
    }
}
