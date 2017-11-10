using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DEMApp.Model
{
    public class MatImage
    {

        public int ncols;
        public int nrows;
        public double xllcorner;
        public double yllcorner;
        public double cellsize;
        public int NODATA_value = -9999;
        public List<List<double>> data;
        private double _maxData;
        private double maxData
        {
            get
            {
                if (data != null)
                {
                    if (_maxData == NODATA_value)
                    {
                        _maxData = data.Max(r => r.Max());
                    }
                    return _maxData;
                }
                else
                {
                    return NODATA_value;
                }
                
            }
        }

        private double _minData;
        private double minData
        {
            get
            {
                if (data != null)
                {
                    if (_minData == NODATA_value)
                    {

                        foreach (List<double> r in data)
                        {
                            foreach(var c in r){
                                if (c != NODATA_value)
                                {
                                    if (_minData == NODATA_value)
                                    {
                                        _minData = c;
                                    }
                                    else if (c < _minData)
                                    {
                                        _minData = c;
                                    }
                                }

                            }
                        }
                    }
                    return _minData;
                }
                else
                {
                    return NODATA_value;
                }

            }
        }
        public MatImage(int cols, int rows, int noValue = -9999)
        {
            ncols = cols;
            nrows = rows;
            NODATA_value = noValue;
            _maxData = NODATA_value;
            _minData = NODATA_value;
            data = new List<List<double>>();
            for (int i = 0; i < nrows; i++)
            {
                var aRowList = new List<double>();
                for (int j = 0; j < ncols; j++)
                {
                    aRowList.Add(NODATA_value);
                }
                data.Add(aRowList);
            }
        }

        public MatImage(int cols, int rows,double xcorner,double ycorner,double csize, int noValue = -9999)
        {
            ncols = cols;
            nrows = rows;
            xllcorner = xcorner;
            yllcorner = ycorner;
            cellsize = csize;
            NODATA_value = noValue;
            _maxData = NODATA_value;
            _minData = NODATA_value;
            
            data = new List<List<double>>();
            for (int i = 0; i < nrows; i++)
            {
                var aRowList = new List<double>();
                for (int j = 0; j < ncols; j++)
                {
                    aRowList.Add(NODATA_value);
                }
                data.Add(aRowList);
            }
        }
        //public Image bitmap = new Bitmap(towidth, toheight);
        public ImageSource ToBitMap()
        {
            try
            {
                var bmp = new Bitmap(ncols, nrows);
                for (int i = 0; i < bmp.Height; i++)
                {
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        double dem = data[i][j];
                        int gray = normalizeDEM(dem);
                        System.Drawing.Color newColor = System.Drawing.Color.FromArgb(gray, gray, gray);
                        bmp.SetPixel(j, i, newColor);
                    }
                }
                return BitmapToImageSource(bmp);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private int normalizeDEM(double dem)
        {
            if (dem != NODATA_value&&maxData!=NODATA_value&&maxData>0)
            {
                var v = (int)Math.Round((dem - minData) * 255 * 1.0 / (maxData - minData));
                if (v < 0 || v > 255)
                {
                    throw new Exception("异常值" + v);
                }                     
                return v;
            }
            else
            {
                return 0;
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        private static ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty, 
                BitmapSizeOptions.FromEmptyOptions());  
            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;
        }

    }
}
