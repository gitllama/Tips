using OpenCvSharp;
using OpenCvSharp.Extensions;
using Pixels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pixels2Extend
{
    public static class PixelOpenCV
    {
        public static WriteableBitmap ToMono(this Pixel<int> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            if (buf == null) buf = new byte[src.Width * src.Height * 3];
            if (dst == null) dst = new WriteableBitmap(src.Width, src.Height, 96, 96, PixelFormats.Bgr24, null);

            for (int i = 0; i < src.pixel.Length; i++)
            {
                var hoge = (byte)(src.pixel[i] > 255 ? 255 : src.pixel[i] < 0 ? 0 : src.pixel[i]);
                buf[i * 3] = hoge;
                buf[i * 3 + 1] = hoge;
                buf[i * 3 + 2] = hoge;
            }
            using (Mat matsrc = new Mat(src.Height, src.Width, MatType.CV_8UC3, buf))
            {
                WriteableBitmapConverter.ToWriteableBitmap(matsrc, dst);
            }
            return dst;
        }
        public static WriteableBitmap ToColorBG(this Pixel<int> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            return ToColor(src, ColorConversionCodes.BayerBG2BGR, buf, dst);
        }
        public static WriteableBitmap ToColorGB(this Pixel<int> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            return ToColor(src, ColorConversionCodes.BayerGB2BGR, buf, dst);
        }
        public static WriteableBitmap ToColorRG(this Pixel<int> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            return ToColor(src, ColorConversionCodes.BayerRG2BGR, buf, dst);
        }
        public static WriteableBitmap ToColorGR(this Pixel<int> src, byte[] buf = null, WriteableBitmap dst = null)
        {
            return ToColor(src, ColorConversionCodes.BayerGR2BGR, buf, dst);
        }
        public static WriteableBitmap ToColor(this Pixel<int> src, ColorConversionCodes cc, byte[] buf = null, WriteableBitmap dst = null)
        {
            byte[] bufraw = null;
            if (buf == null) buf = new byte[src.Width * src.Height * 3];
            if (bufraw == null) bufraw = new byte[src.Width * src.Height];
            if (dst == null) dst = new WriteableBitmap(src.Width, src.Height, 96, 96, PixelFormats.Bgr24, null);

            var matrix = new float[] { 2, 0, 0, 0, 1, 0, 0, 0, 1.8F };

            for (int i = 0; i < src.pixel.Length; i++)
            {
                var hoge = (byte)(src.pixel[i] > 255 ? 255 : src.pixel[i] < 0 ? 0 : src.pixel[i]);
                bufraw[i] = hoge;
            }
            using (Mat matmatrix = new Mat(3, 3, MatType.CV_32FC1, matrix))
            using (Mat matraw = new Mat(src.Height, src.Width, MatType.CV_8UC1, bufraw))
            using (Mat mat = new Mat(src.Height, src.Width, MatType.CV_8UC3, buf))
            {
                Cv2.CvtColor(matraw, mat, cc);
                Cv2.Transform(mat, mat, matmatrix);

                WriteableBitmapConverter.ToWriteableBitmap(mat, dst);
            }
            return dst;
        }

        public static void Show(WriteableBitmap src)
        {
            using (Mat mat = new Mat())
            {
                //src.ToMat(mat);
                Cv2.ImShow("image", mat);
                Cv2.WaitKey();
                Cv2.DestroyWindow("image");
            }
        }

    }
}
