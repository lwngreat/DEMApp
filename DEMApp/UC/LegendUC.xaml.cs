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
      /// <summary>
      /// 图例数据模型
      /// </summary>
    public class LegendModel
    {
        /// <summary>
        /// 图例名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public int lowValue { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public int highVlaue { get; set; }
        /// <summary>
        /// 色带
        /// </summary>
        public LinearColors colors { get; set; }
    }
   
}
