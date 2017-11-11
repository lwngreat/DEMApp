using DEMApp.BLL;
using DEMApp.Model;
using DEMApp.UC;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DEMApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath;
        private MatImage matImage;
        private LinearColors curCorlor;
        public MainWindow()
        {
            InitializeComponent();
            ReadASCFile.DataLoadState += ReadASCFile_DataLoadState;
            rcombColor.combColor.SelectionChanged += combColor_SelectionChanged;
        }

        private void combColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            curCorlor = cb.SelectedItem as LinearColors;
        }

        private void ReadASCFile_DataLoadState(double per, string msg)
        {
            if (per < 1)
            {
                bi_busying.IsBusy = true;
            }
            else
            {
                bi_busying.IsBusy = false;
            }
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog oFileDialog = new System.Windows.Forms.OpenFileDialog();
            oFileDialog.Filter = "文本文件(*.asc)|*.asc|所有文件(*.*)|*.*";
            if (oFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = oFileDialog.FileName;
                    matImage = ReadASCFile.read(filePath);
                    imgMain.Source = matImage.ToBitMap(curCorlor);
                }
                catch (Exception ex)
                {

                }

            }
        }


        private MatImage aspectMatImage;
        private void btnAspect_Click(object sender, RoutedEventArgs e)
        {

            if (matImage != null && matImage.data != null)
            {
                SlopeCal slopeCal = new SlopeCal(matImage);
                aspectMatImage = slopeCal.calAspect();
                imgDeputy.Source = aspectMatImage.ToBitMap(curCorlor);
            }
            else
            {

            }
        }

        private MatImage slopeMatImage;
        private void btnSlope_Click(object sender, RoutedEventArgs e)
        {
            if (matImage != null && matImage.data != null)
            {
                var slopeCal = new SlopeCal(matImage);
                slopeMatImage = slopeCal.calSlope();
                imgDeputy.Source = slopeMatImage.ToBitMap(curCorlor);
            }
            else
            {

            }
        }

        private void btnSaveSlope_Click(object sender, RoutedEventArgs e)
        {
            if (slopeMatImage != null)
            {
                SaveMatImage saveImage = new SaveMatImage(slopeMatImage);
                saveImage.Save();
            }
            else
            {
                MessageBox.Show("请先计算坡度");
            }

        }

        private void btnSaveAspect_Click(object sender, RoutedEventArgs e)
        {
            if (aspectMatImage != null)
            {
                SaveMatImage saveImage = new SaveMatImage(aspectMatImage);
                saveImage.Save();
            }
            else
            {
                MessageBox.Show("请先计算坡向");
            }
        }
    }
}
