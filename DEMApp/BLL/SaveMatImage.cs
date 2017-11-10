using DEMApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DEMApp.BLL
{
    public class SaveMatImage
    {
        MatImage matimage;
        string fileHeader =
  @"ncols         {0}
nrows         {1}
xllcorner     {2}
yllcorner     {3}
cellsize      {4}
NODATA_value  {5}";
        public SaveMatImage(MatImage image)
        {
            matimage = image;
        }
        public void Save(){           
            if (matimage != null) //需要选中检测记录
            {
                string headStr = string.Format(fileHeader, matimage.ncols, matimage.nrows, matimage.xllcorner, matimage.yllcorner, matimage.cellsize, matimage.NODATA_value);
                System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
                saveDlg.Filter = "ASC文件|*.asc";
                if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveDlg.FileName))
                    {
                        sw.WriteLine(headStr);
                        foreach(var r in matimage.data){
                            StringBuilder sb = new StringBuilder();
                            foreach(var c in r){
                                sb.Append(c+" ");
                            }
                            sw.WriteLine(sb.ToString());
                        }
                        sw.Close();
                    }
                    
                }
            }
      }
    }
}
