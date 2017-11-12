using DEMApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEMApp.BLL
{
    public class SlopeCal
    {
        private MatImage oMatImage;
        public SlopeCal(MatImage origionMatImage)
        {
            oMatImage = origionMatImage;  
        }
        /// <summary>
        /// 百分比表示
        /// </summary>
        /// <returns></returns>
        public MatImage calSlope2() {
            MatImage slope = new MatImage(oMatImage.ncols, oMatImage.nrows, oMatImage.xllcorner, oMatImage.yllcorner, oMatImage.cellsize);
            for (int i = 1; i < oMatImage.nrows - 1; i++)
            {
                for (int j = 1; j < oMatImage.ncols - 1; j++)
                {
                    var value = oMatImage.data[i][j];
                    var valueBefor = oMatImage.data[i][j - 1];
                    var valueAfter = oMatImage.data[i][j + 1];
                    var valueUp = oMatImage.data[i - 1][j];
                    var valueDown = oMatImage.data[i + 1][j];
                    if (value != oMatImage.NODATA_value
                        && valueBefor != oMatImage.NODATA_value
                        && valueAfter != oMatImage.NODATA_value
                        && valueUp != oMatImage.NODATA_value
                        && valueDown != oMatImage.NODATA_value
                        && oMatImage.cellsize > 0)
                    {
                        var we = (valueBefor - valueAfter) / (2 * oMatImage.cellsize);
                        var sn = (valueDown - valueUp) / (2 * oMatImage.cellsize);
                        slope.data[i][j] = Math.Sqrt(sn * sn + we * we)*100;       //%
                    }

                }
            }
            return slope;
        
        }
        /// <summary>
        /// 度数法表示
        /// </summary>
        /// <returns></returns>
        public MatImage calSlope()
        {
            MatImage slope = new MatImage(oMatImage.ncols, oMatImage.nrows, oMatImage.xllcorner, oMatImage.yllcorner, oMatImage.cellsize);
            for (int i = 1; i < oMatImage.nrows - 1; i++)
            {
                for (int j = 1; j < oMatImage.ncols - 1; j++)
                {
                    var value = oMatImage.data[i][j];
                    var valueBefor = oMatImage.data[i][j - 1];
                    var valueAfter = oMatImage.data[i][j + 1];
                    var valueUp = oMatImage.data[i - 1][j];
                    var valueDown = oMatImage.data[i + 1][j];
                    if (value != oMatImage.NODATA_value
                        && valueBefor != oMatImage.NODATA_value
                        && valueAfter != oMatImage.NODATA_value
                        && valueUp != oMatImage.NODATA_value
                        && valueDown != oMatImage.NODATA_value
                        && oMatImage.cellsize > 0)
                    {
                        var we = (valueBefor - valueAfter) / (2 * oMatImage.cellsize);
                        var sn = (valueDown - valueUp) / (2 * oMatImage.cellsize);
                        slope.data[i][j] = Math.Atan(Math.Sqrt(sn * sn + we * we)) * 180.0 / Math.PI;
                    }

                }
            }
            return slope;

        }
        public MatImage calAspect()
        {
            MatImage aspect = new MatImage(oMatImage.ncols, oMatImage.nrows, oMatImage.xllcorner, oMatImage.yllcorner, oMatImage.cellsize,-1);
            for (int i = 1; i < oMatImage.nrows - 1; i++)
            {
                for (int j = 1; j < oMatImage.ncols - 1; j++)
                {
                    var value = oMatImage.data[i][j];
                    var valueBefor = oMatImage.data[i][j - 1];
                    var valueAfter = oMatImage.data[i][j + 1];
                    var valueUp = oMatImage.data[i - 1][j];
                    var valueDown = oMatImage.data[i + 1][j];
                    if (value != oMatImage.NODATA_value
                        && valueBefor != oMatImage.NODATA_value
                        && valueAfter != oMatImage.NODATA_value
                        && valueUp != oMatImage.NODATA_value
                        && valueDown != oMatImage.NODATA_value
                        && oMatImage.cellsize > 0)
                    {
                        var we = (valueBefor - valueAfter) / (2 * oMatImage.cellsize);
                        var sn = (valueDown - valueUp) / (2 * oMatImage.cellsize);
                        var v = Math.Atan2(sn, we) * 180.0 / Math.PI;
                        if (we == 0 && sn == 0)
                        {

                            v = aspect.NODATA_value;
                        } 
                        else if (v < 0)
                        { 
                            v += 360.0;
                        }
                        aspect.data[i][j] = v;
                    } 
                }
            }
            return aspect;
        }
    }
}
