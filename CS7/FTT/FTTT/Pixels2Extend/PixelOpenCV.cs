using OpenCvSharp;
using OpenCvSharp.Extensions;
using Pixels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//PresentationCore
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Pixels.Math;

namespace Pixels.Extend
{
    public static class PixelOpenCV
    {
        public static WriteableBitmap ToMono(this Pixel<float> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            if (buf == null) buf = new byte[src.Width * src.Height * 3];
            if (dst == null) dst = new WriteableBitmap(src.Width, src.Height, 96, 96, PixelFormats.Bgr24, null);

            using (Mat matsrc = new Mat(src.Height, src.Width, MatType.CV_8UC3, buf))
            {
                int c = 0;
                for (int y = 0; y < src.Height; y++)
                    for (int x = 0; x < src.Width; x++)
                    {
                        var hoge = src[x, y].ConvertToByte();
                        buf[c++] = hoge;
                        buf[c++] = hoge;
                        buf[c++] = hoge;
                    }
                WriteableBitmapConverter.ToWriteableBitmap(matsrc, dst);
            }
            return dst;
        }
        public static WriteableBitmap ToColor(this Pixel<float> src, ColorConversionCodes cc, byte[] buf = null, WriteableBitmap dst = null)
        {
            byte[] bufraw = null;
            if (buf == null) buf = new byte[src.Width * src.Height * 3];
            if (bufraw == null) bufraw = new byte[src.Width * src.Height];
            if (dst == null) dst = new WriteableBitmap(src.Width, src.Height, 96, 96, PixelFormats.Bgr24, null);

            var matrix = new float[] { 2, 0, 0, 0, 1, 0, 0, 0, 1.8F };

            using (Mat matmatrix = new Mat(3, 3, MatType.CV_32FC1, matrix))
            using (Mat matraw = new Mat(src.Height, src.Width, MatType.CV_8UC1, bufraw))
            using (Mat mat = new Mat(src.Height, src.Width, MatType.CV_8UC3, buf))
            {
                int c = 0;
                for (int y = 0; y < src.Height; y++)
                    for (int x = 0; x < src.Width; x++)
                    {
                        var hoge = src[x, y].ConvertToByte();
                        bufraw[c++] = hoge;
                    }
                Cv2.CvtColor(matraw, mat, cc);
                Cv2.Transform(mat, mat, matmatrix);

                WriteableBitmapConverter.ToWriteableBitmap(mat, dst);
            }
            return dst;
        }
        public static WriteableBitmap ToColorBG(this Pixel<float> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            return ToColor(src, ColorConversionCodes.BayerBG2BGR, buf, dst);
        }
        public static WriteableBitmap ToColorGB(this Pixel<float> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            return ToColor(src, ColorConversionCodes.BayerGB2BGR, buf, dst);
        }
        public static WriteableBitmap ToColorRG(this Pixel<float> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            return ToColor(src, ColorConversionCodes.BayerRG2BGR, buf, dst);
        }
        public static WriteableBitmap ToColorGR(this Pixel<float> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            return ToColor(src, ColorConversionCodes.BayerGR2BGR, buf, dst);
        }
        public static void ShowCV(this WriteableBitmap src)
        {
            using (Mat mat = new Mat(src.PixelHeight, src.PixelWidth, MatType.CV_8UC3))
            {
                src.ToMat(mat);
                Cv2.NamedWindow("image", WindowMode.Normal);
                Cv2.ImShow("image", mat);
                
                Cv2.WaitKey();
                Cv2.DestroyWindow("image");
            }
        }


        public static int Labling(this WriteableBitmap src)
        {
            using (Mat mat = new Mat(src.PixelHeight, src.PixelWidth, MatType.CV_8UC3))
            {
                src.ToMat(mat);

                Mat gray = mat.CvtColor(ColorConversionCodes.BGR2GRAY);

                //Cv2.NamedWindow("image", WindowMode.Normal);
                //Cv2.ImShow("image", mat);
                Mat binary = gray.Threshold(0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);

                //Cv2.NamedWindow("image", WindowMode.Normal);
                //Cv2.ImShow("image", binary);
                //Cv2.WaitKey();
                //Cv2.DestroyWindow("image");
                ConnectedComponents cc = Cv2.ConnectedComponentsEx(binary);

                int c = 0;
                foreach(var i in cc.Blobs)
                {
                    c += i.Area > 3 ? 1 : 0;
                }


                return c;
            }
        }
    }
}
