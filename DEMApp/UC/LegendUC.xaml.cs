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
    /// LegendUC.xaml 的交互逻辑
    /// </summary>
    public partial class LegendUC : UserControl
    {
        public LegendUC()
        {
            InitializeComponent();
            this.Visibility = System.Windows.Visibility.Hidden;
        }
        public void setValue(LegendModel v)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            mainGrid.DataContext = null;
            mainGrid.DataContext = v;              
        }
    }

    public class LegendModel
    {
        public string name { get; set; }
        public int lowValue { get; set; }
        public int highVlaue { get; set; }
        public LinearColors colors { get; set; }
    }
   
}
