using DEMApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DEMApp.BLL
{
    public class ReadASCFile
    {
        public delegate void LoadDataFinishedHandler(double per, string msg);
        public static event LoadDataFinishedHandler DataLoadState;
        public static void UpdateLoadState(double per, string msg = "")
        {
            if (DataLoadState != null)
            {
                DataLoadState(per, msg);
            }
        }  
        public static MatImage read(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                try
                {
                    MatImage result = readSixLieHeader(sr);
                    string aLine = sr.ReadLine().Trim();
                    UpdateLoadState(0);
                    for (int lineNum = 0; lineNum<result.nrows; lineNum++)      // !string.IsNullOrEmpty(aLine)
                    {
                        string[] splitLine = aLine.Split('\t', ' ');
                        for (int j = 0; j < result.ncols; j++)
                        {
                            var it = splitLine[j];
                            if (!string.IsNullOrWhiteSpace(it))
                            {
                                result.data[lineNum][j] = Convert.ToDouble(it);
                            }
                        }
                        aLine = sr.ReadLine();
                    }
                    UpdateLoadState(1);
                    return result;
                    
                }
                catch (Exception e)
                {

                    UpdateLoadState(-1);
                    App.Current.Dispatcher.Invoke(new Action(delegate()
                    {

                    }));
                    return null;
                }

            }
        }   
        private static MatImage readSixLieHeader(StreamReader sr)
        {
            try
            {
                string r1 = sr.ReadLine();
                string r2 = sr.ReadLine();
                string r3 = sr.ReadLine();
                string r4 = sr.ReadLine();
                string r5 = sr.ReadLine();
                string r6 = sr.ReadLine();
                string[] sparry;

                sparry    =r1.Split(' ', '\t');
                int cols = Convert.ToInt32(sparry[sparry.Length - 1].Trim());
                sparry = r2.Split(' ', '\t');
                int rows = Convert.ToInt32(sparry[sparry.Length - 1].Trim());       
                sparry = r3.Split(' ', '\t');
                double xllcorner = Convert.ToDouble(sparry[sparry.Length - 1].Trim());
                sparry = r4.Split(' ', '\t');
                double yllcorner = Convert.ToDouble(sparry[sparry.Length - 1].Trim());
                sparry = r5.Split(' ', '\t');
                double cellsize = Convert.ToDouble(sparry[sparry.Length - 1].Trim());
                sparry = r6.Split(' ', '\t');
                int noValue = Convert.ToInt32(sparry[sparry.Length - 1].Trim());

                var mi = new MatImage(cols, rows, noValue);
                mi.xllcorner = xllcorner;
                mi.yllcorner = yllcorner;
                mi.cellsize = cellsize;
                return mi;
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据格式损坏" + ex.Message);
                return null;
            }

        }
    }
}
