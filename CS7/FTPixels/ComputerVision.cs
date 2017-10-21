using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PixelsExtend
{
    public class ComputerVision
    {
        /// <summary>
        /// AdaptiveThreshold 大津より精度高い
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="s"></param>
        /// <returns>true : Pass false:Fail</returns>
        public static bool AdaptiveThreshold(BitmapSource src, out BitmapSource dst)
        {
            using (Mat mat = BitmapSourceConverter.ToMat(src))
            using (Mat matbuf = new Mat())
            {
                //Cv2.CvtColor
                //(
                //    mat,
                //    matbuf,
                //    ColorConversionCodes.BGR2GRAY
                //);

                Cv2.AdaptiveThreshold
                (
                    mat,
                    matbuf,
                    255,
                    AdaptiveThresholdTypes.GaussianC,
                    ThresholdTypes.Binary,
                    9,
                    128
                );

                dst = matbuf.ToBitmapSource();
            }

            return true;
        }

        public static BitmapSource AdaptiveThreshold(BitmapSource src)
        {
            using (Mat mat = BitmapSourceConverter.ToMat(src))
            using (Mat matbuf = new Mat())
            using (Mat matbuf2 = new Mat())
            {
                //グレイスケール化                   
                Cv2.CvtColor
                (
                    mat,
                    matbuf,
                    ColorConversionCodes.BayerBG2GRAY
                );

                //強めのフィルタ処理
                //Cv2.BilateralFilter
                //(
                //    matbuf,
                //    mat,
                //    7,
                //    35,
                //    5
                //);

                Cv2.FastNlMeansDenoising
                (
                    matbuf,
                    mat
                );

                Cv2.AdaptiveThreshold
                (
                    mat,
                    matbuf,
                    255,
                    AdaptiveThresholdTypes.GaussianC,
                    ThresholdTypes.Binary,
                    9,
                    5
                );

                //Cv2.FastNlMeansDenoising
                //(
                //    matbuf,
                //    mat

                //);

                return matbuf.ToBitmapSource();;
            }
        }
        //Sauvola OpenCVSharpはSauvolaもこっそり実装してる


        public static BitmapSource Test(BitmapSource src)
        {
            using (Mat mat = BitmapSourceConverter.ToMat(src))
            using (Mat matbuf = new Mat())
            using (Mat matbuf2 = new Mat())
            {
                //グレイスケール化                   
                Cv2.CvtColor
                (
                    mat,
                    matbuf,
                    ColorConversionCodes.BayerBG2GRAY
                );

                //強めのフィルタ処理
                //Cv2.BilateralFilter
                //(
                //    matbuf,
                //    mat,
                //    7,
                //    35,
                //    5
                //);

                Cv2.FastNlMeansDenoising
                (
                    matbuf,
                    mat
                );

                Cv2.AdaptiveThreshold
                (
                    mat,
                    matbuf,
                    255,
                    AdaptiveThresholdTypes.GaussianC,
                    ThresholdTypes.Binary,
                    9,
                    2
                );

                //Cv2.FastNlMeansDenoising
                //(
                //    matbuf,
                //    mat

                //);

                return matbuf.ToBitmapSource(); ;
            }
        }
    }
}
