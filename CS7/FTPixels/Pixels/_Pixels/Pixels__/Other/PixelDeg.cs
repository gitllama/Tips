using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Pixels
{
    /******************************/
    /*  デジタル後段処理クラス    */
    /******************************/

    public static class DegProc
    {

        public static Bitmap Demosaicing(this PixelByte p, double[] gain, double[,] matrix, double gamma, int bayer = 0)
        {
            #region 初期化
            int s = p.Size;
            int h = p.Height;
            int w = p.Width;

            byte[] buf = new byte[s * 3];

            double r = 0;
            double b = 0;
            double g = 0;

            Bitmap bit = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData bmpdat = bit.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            #endregion
            #region byte配列化
            //RGB信号作成（8x8)
            int x = 0;
            int y = 0;
            #region ベイヤ補間/一階調
            double gain_r = gain[0] / 16 / 255;
            double gain_g = gain[1] / 2 / 255;
            double gain_b = gain[2] / 16 / 255;

            Action FucA = () =>
            {
                r = (9 * p[x + 1, y + 2] + 3 * (p[x + 1, y] + p[x + 3, y + 2]) +  p[x + 3, y]) * gain_r;
                g = (p[x + 1, y + 1] + p[x + 2, y + 2] ) * gain_g;
                b = (9 * p[x + 2, y + 1] + 3 * ( p[x, y + 1] + p[x + 2, y + 3]) + p[x, y + 3]) * gain_b;
            };
            Action FucB = () =>
            {
                r = (9 * p[x + 2, y + 2] + 3 * (p[x + 2, y] + p[x, y + 2]) + p[x, y]) * gain_r;
                g = (p[x + 2, y + 1] + p[x + 1, y + 2]) * gain_g;
                b = (9 * p[x + 1, y + 1] + 3 * (p[x + 3, y + 1] + p[x + 1, y + 3]) + p[x + 3, y + 3]) * gain_b;
            };
            Action FucC = () =>
            {
                r = (9 * p[x + 1, y + 1] + 3 * (p[x + 3, y + 1] + p[x + 1, y + 3]) + p[x + 3, y + 3]) * gain_r;
                g = (p[x + 2, y + 1] + p[x + 1, y + 2]) * gain_g;
                b = (9 * p[x + 2, y + 2] + 3 * (p[x + 2, y] + p[x, y + 2]) + p[x, y]) * gain_b;
            };
            Action FucD = () =>
            {
                r = (9 * p[x + 2, y + 1] + 3 *(p[x, y + 1] + p[x + 2, y + 3]) + p[x + 3, y]) * gain_r;
                g = (p[x + 1, y + 1] + p[x + 2, y + 2]) * gain_g;
                b = (9 * p[x + 1, y + 2] + 3 * ( p[x + 1, y] + p[x + 3, y + 2]) + p[x + 3, y]) * gain_b;
            };


            #endregion
            #region マトリックス
            Action FuncMatrix = () =>
            {
                double rr = r;
                double gg = g;
                double bb = b;
                r = matrix[0, 0] * rr + matrix[0, 1] * gg + matrix[0, 2] * bb;
                g = matrix[1, 0] * rr + matrix[1, 1] * gg + matrix[1, 2] * bb;
                b = matrix[2, 0] * rr + matrix[2, 1] * gg + matrix[2, 2] * bb;
            };
            #endregion
            #region ガンマ
            Action FuncGamma = () =>
            {
                r = 255 * Math.Pow(r, gamma);
                g = 255 * Math.Pow(g, gamma);
                b = 255 * Math.Pow(b, gamma);
            };
            #endregion
            #region 配列化
            int col = 0;
            Action FuncToByte = () =>
            {
                //int col = (y * w + x) * 3;
                buf[col + 2] = (byte)(r < 0 ? 0 : (255 < r ? 255 : r));
                buf[col + 1] = (byte)(g < 0 ? 0 : (255 < g ? 255 : g));
                buf[col + 0] = (byte)(b < 0 ? 0 : (255 < b ? 255 : b));
            };
            #endregion
            Action Func1 = FucA;
            Action Func2 = FucB;
            Action Func3 = FucC;
            Action Func4 = FucD;
            switch(bayer)
            {
                case 0:
                    Func1 = FucA;
                    Func2 = FucB;
                    Func3 = FucC;
                    Func4 = FucD;
                    break;
                case 1:
                    Func1 = FucB;
                    Func2 = FucA;
                    Func3 = FucD;
                    Func4 = FucC;
                    break;
                case 2:
                    Func1 = FucC;
                    Func2 = FucD;
                    Func3 = FucA;
                    Func4 = FucB;
                    break;
                case 3:
                    Func1 = FucD;
                    Func2 = FucC;
                    Func3 = FucB;
                    Func4 = FucA;
                    break;
            }


            col = 0;
            for (y = 0; y < h - 4; y++)
            {
                for (x = 0; x < w - 3; x++)
                {
                    Func1();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;

                    x++;

                    Func2();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;
                }
                col += (3 * 2);
                y++;

                for (x = 0; x < w - 3; x++)
                {
                    Func3();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;

                    x++;

                    Func4();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;
                }
                col += (3 * 2);
            }

            #endregion
            #region BMPにマーシャルコピー/アンロック
            for (int j = 0; j < h; ++j)
            {
                IntPtr dst_line = (IntPtr)((Int64)bmpdat.Scan0 + j * bmpdat.Stride);
                Marshal.Copy(buf, j * w * 3 , dst_line, w * 3);
            }
            bit.UnlockBits(bmpdat);
            #endregion

            return bit;
        }

        public static Bitmap DemosaicingNN(this PixelByte p, double[] gain, double[,] matrix, double gamma, int bayer = 0)
        {
            #region 初期化
            int s = p.Size;
            int h = p.Height;
            int w = p.Width;

            byte[] buf = new byte[s * 3];

            double r = 0;
            double b = 0;
            double g = 0;

            Bitmap bit = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData bmpdat = bit.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            #endregion
            #region byte配列化
            //RGB信号作成（8x8)
            int x = 0;
            int y = 0;
            #region ベイヤ補間/一階調
            double gain_r = gain[0] / 255;
            double gain_g = gain[1] / 255;
            double gain_b = gain[2] / 255;

            Action FucA = () =>
            {
                //Gr
                r = (p[x + 1, y]) * gain_r;
                g = (p[x, y] + p[x + 1, y + 1]) / 2 * gain_g;
                b = (p[x, y + 1]) * gain_b;
            };
            Action FucB = () =>
            {
                //R
                r = (p[x, y]) * gain_r;
                g = (p[x+1, y] + p[x, y + 1]) / 2 * gain_g;
                b = (p[x+1, y + 1]) * gain_b;
            };
            Action FucC = () =>
            {
                //B
                r = (p[x+1, y+1]) * gain_r;
                g = (p[x + 1, y] + p[x, y + 1]) / 2 * gain_g;
                b = (p[x, y]) * gain_b;
            };
            Action FucD = () =>
            {
                //Gb
                r = (p[x, y+1]) * gain_r;
                g = (p[x, y] + p[x + 1, y + 1]) / 2 * gain_g;
                b = (p[x+1, y]) * gain_b;
            };


            #endregion
            #region マトリックス
            Action FuncMatrix = () =>
            {
                double rr = r;
                double gg = g;
                double bb = b;
                r = matrix[0, 0] * rr + matrix[0, 1] * gg + matrix[0, 2] * bb;
                g = matrix[1, 0] * rr + matrix[1, 1] * gg + matrix[1, 2] * bb;
                b = matrix[2, 0] * rr + matrix[2, 1] * gg + matrix[2, 2] * bb;
            };
            #endregion
            #region ガンマ
            Action FuncGamma = () =>
            {
                r = 255 * Math.Pow(r, gamma);
                g = 255 * Math.Pow(g, gamma);
                b = 255 * Math.Pow(b, gamma);
            };
            #endregion
            #region 配列化
            int col = 0;
            Action FuncToByte = () =>
            {
                //int col = (y * w + x) * 3;
                buf[col + 2] = (byte)(r < 0 ? 0 : (255 < r ? 255 : r));
                buf[col + 1] = (byte)(g < 0 ? 0 : (255 < g ? 255 : g));
                buf[col + 0] = (byte)(b < 0 ? 0 : (255 < b ? 255 : b));
            };
            #endregion
            Action Func1 = FucA;
            Action Func2 = FucB;
            Action Func3 = FucC;
            Action Func4 = FucD;
            switch (bayer)
            {
                case 0:
                    Func1 = FucA;
                    Func2 = FucB;
                    Func3 = FucC;
                    Func4 = FucD;
                    break;
                case 1:
                    Func1 = FucB;
                    Func2 = FucA;
                    Func3 = FucD;
                    Func4 = FucC;
                    break;
                case 2:
                    Func1 = FucC;
                    Func2 = FucD;
                    Func3 = FucA;
                    Func4 = FucB;
                    break;
                case 3:
                    Func1 = FucD;
                    Func2 = FucC;
                    Func3 = FucB;
                    Func4 = FucA;
                    break;
            }


            col = 0;
            for (y = 0; y < h - 4; y++)
            {
                for (x = 0; x < w - 3; x++)
                {
                    Func1();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;

                    x++;

                    Func2();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;
                }
                col += (3 * 2);
                y++;

                for (x = 0; x < w - 3; x++)
                {
                    Func3();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;

                    x++;

                    Func4();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;
                }
                col += (3 * 2);
            }

            #endregion
            #region BMPにマーシャルコピー/アンロック
            for (int j = 0; j < h; ++j)
            {
                IntPtr dst_line = (IntPtr)((Int64)bmpdat.Scan0 + j * bmpdat.Stride);
                Marshal.Copy(buf, j * w * 3, dst_line, w * 3);
            }
            bit.UnlockBits(bmpdat);
            #endregion

            return bit;
        }

        public static Bitmap DemosaicingTest(this PixelByte p, double[] gain, double[,] matrix, double gamma, int bayer = 0)
        {
            #region 初期化
            int s = p.Size;
            int h = p.Height;
            int w = p.Width;

            byte[] buf = new byte[s * 3];

            double r = 0;
            double b = 0;
            double g = 0;

            Bitmap bit = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData bmpdat = bit.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            #endregion
            #region byte配列化
            //RGB信号作成（8x8)
            int x = 0;
            int y = 0;
            #region ベイヤ補間/一階調
            double gain_r = gain[0] / 9 / 255;
            double gain_g = gain[1] / 18 / 255;
            double gain_b = gain[2] / 9 / 255;

            Action FucA = () =>
            {
                r = ( p[x + 1, y + 0] + p[x + 3, y + 0] + p[x + 5, y + 0]
                    + p[x + 1, y + 2] + p[x + 3, y + 2] + p[x + 5, y + 2]
                    + p[x + 1, y + 4] + p[x + 3, y + 4] + p[x + 5, y + 4]) * gain_r;

                g = ( p[x + 0, y + 0] + p[x + 2, y + 0] + p[x + 4, y + 0]
                    + p[x + 1, y + 1] + p[x + 3, y + 1] + p[x + 5, y + 1]
                    + p[x + 0, y + 2] + p[x + 2, y + 2] + p[x + 4, y + 2]
                    + p[x + 1, y + 3] + p[x + 3, y + 3] + p[x + 5, y + 3]
                    + p[x + 0, y + 4] + p[x + 2, y + 4] + p[x + 4, y + 4]
                    + p[x + 1, y + 5] + p[x + 3, y + 5] + p[x + 5, y + 5]
                    ) * gain_g;

                b = ( p[x + 0, y + 1] + p[x + 2, y + 1] + p[x + 4, y + 1]
                    + p[x + 0, y + 3] + p[x + 2, y + 3] + p[x + 4, y + 3]
                    + p[x + 0, y + 5] + p[x + 2, y + 5] + p[x + 4, y + 5]
                    ) * gain_b;
            };
            Action FucB = () =>
            {
                r = (p[x + 0, y + 0] + p[x + 2, y + 0] + p[x + 4, y + 0]
                    + p[x + 0, y + 2] + p[x + 2, y + 2] + p[x + 4, y + 2]
                    + p[x + 0, y + 4] + p[x + 2, y + 4] + p[x + 4, y + 4]) * gain_r;

                g = ( p[x + 1, y + 0] + p[x + 3, y + 0] + p[x + 5, y + 0]
                    + p[x + 0, y + 1] + p[x + 2, y + 1] + p[x + 4, y + 1]
                    + p[x + 1, y + 2] + p[x + 3, y + 2] + p[x + 5, y + 2]
                    + p[x + 0, y + 3] + p[x + 2, y + 3] + p[x + 4, y + 3]
                    + p[x + 1, y + 4] + p[x + 3, y + 4] + p[x + 5, y + 4]
                    + p[x + 0, y + 5] + p[x + 2, y + 5] + p[x + 4, y + 5]
                    ) * gain_g;

                b = ( p[x + 1, y + 1] + p[x + 3, y + 1] + p[x + 5, y + 1]
                    + p[x + 1, y + 3] + p[x + 3, y + 3] + p[x + 5, y + 3]
                    + p[x + 1, y + 5] + p[x + 3, y + 5] + p[x + 5, y + 5]
                    ) * gain_b;
            };
            Action FucC = () =>
            {
                r = ( p[x + 1, y + 1] + p[x + 3, y + 1] + p[x + 5, y + 1]
                    + p[x + 1, y + 3] + p[x + 3, y + 3] + p[x + 5, y + 3]
                    + p[x + 1, y + 5] + p[x + 3, y + 5] + p[x + 5, y + 5]) * gain_r;

                g = ( p[x + 1, y + 0] + p[x + 3, y + 0] + p[x + 5, y + 0]
                    + p[x + 0, y + 1] + p[x + 2, y + 1] + p[x + 4, y + 1]
                    + p[x + 1, y + 2] + p[x + 3, y + 2] + p[x + 5, y + 2]
                    + p[x + 0, y + 3] + p[x + 2, y + 3] + p[x + 4, y + 3]
                    + p[x + 1, y + 4] + p[x + 3, y + 4] + p[x + 5, y + 4]
                    + p[x + 0, y + 5] + p[x + 2, y + 5] + p[x + 4, y + 5]
                    ) * gain_g;

                b = ( p[x + 0, y + 0] + p[x + 2, y + 0] + p[x + 4, y + 0]
                    + p[x + 0, y + 2] + p[x + 2, y + 2] + p[x + 4, y + 2]
                    + p[x + 0, y + 4] + p[x + 2, y + 4] + p[x + 4, y + 4]
                    ) * gain_b;
            };
            Action FucD = () =>
            {
                r = ( p[x + 0, y + 1] + p[x + 2, y + 1] + p[x + 4, y + 1]
                    + p[x + 0, y + 3] + p[x + 2, y + 3] + p[x + 4, y + 3]
                    + p[x + 0, y + 5] + p[x + 2, y + 5] + p[x + 4, y + 5]) * gain_r;

                g = ( p[x + 0, y + 0] + p[x + 2, y + 0] + p[x + 4, y + 0]
                    + p[x + 1, y + 1] + p[x + 3, y + 1] + p[x + 5, y + 1]
                    + p[x + 0, y + 2] + p[x + 2, y + 2] + p[x + 4, y + 2]
                    + p[x + 1, y + 3] + p[x + 3, y + 3] + p[x + 5, y + 3]
                    + p[x + 0, y + 4] + p[x + 2, y + 4] + p[x + 4, y + 4]
                    + p[x + 1, y + 5] + p[x + 3, y + 5] + p[x + 5, y + 5]
                    ) * gain_g;

                b = ( p[x + 1, y + 0] + p[x + 3, y + 0] + p[x + 5, y + 0]
                    + p[x + 1, y + 2] + p[x + 3, y + 2] + p[x + 5, y + 2]
                    + p[x + 1, y + 4] + p[x + 3, y + 4] + p[x + 5, y + 4]
                    ) * gain_b;
            };


            #endregion
            #region マトリックス
            Action FuncMatrix = () =>
            {
                double rr = r;
                double gg = g;
                double bb = b;
                r = matrix[0, 0] * rr + matrix[0, 1] * gg + matrix[0, 2] * bb;
                g = matrix[1, 0] * rr + matrix[1, 1] * gg + matrix[1, 2] * bb;
                b = matrix[2, 0] * rr + matrix[2, 1] * gg + matrix[2, 2] * bb;
            };
            #endregion
            #region ガンマ
            Action FuncGamma = () =>
            {
                r = 255 * Math.Pow(r, gamma);
                g = 255 * Math.Pow(g, gamma);
                b = 255 * Math.Pow(b, gamma);
            };
            #endregion
            #region 配列化
            int col = 0;
            Action FuncToByte = () =>
            {
                //int col = (y * w + x) * 3;
                buf[col + 2] = (byte)(r < 0 ? 0 : (255 < r ? 255 : r));
                buf[col + 1] = (byte)(g < 0 ? 0 : (255 < g ? 255 : g));
                buf[col + 0] = (byte)(b < 0 ? 0 : (255 < b ? 255 : b));
            };
            #endregion
            Action Func1 = FucA;
            Action Func2 = FucB;
            Action Func3 = FucC;
            Action Func4 = FucD;
            switch (bayer)
            {
                case 0:
                    Func1 = FucA;
                    Func2 = FucB;
                    Func3 = FucC;
                    Func4 = FucD;
                    break;
                case 1:
                    Func1 = FucB;
                    Func2 = FucA;
                    Func3 = FucD;
                    Func4 = FucC;
                    break;
                case 2:
                    Func1 = FucC;
                    Func2 = FucD;
                    Func3 = FucA;
                    Func4 = FucB;
                    break;
                case 3:
                    Func1 = FucD;
                    Func2 = FucC;
                    Func3 = FucB;
                    Func4 = FucA;
                    break;
            }


            col = 0;
            for (y = 0; y < h - 6; y++)
            {
                for (x = 0; x < w - 6; x++)
                {
                    Func1();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;

                    x++;

                    Func2();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;
                }
                col += (3 * 6);
                y++;

                for (x = 0; x < w - 6; x++)
                {
                    Func3();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;

                    x++;

                    Func4();
                    FuncMatrix();
                    FuncGamma();
                    FuncToByte();
                    col += 3;
                }
                col += (3 * 6);
            }

            #endregion
            #region BMPにマーシャルコピー/アンロック
            for (int j = 0; j < h; ++j)
            {
                IntPtr dst_line = (IntPtr)((Int64)bmpdat.Scan0 + j * bmpdat.Stride);
                Marshal.Copy(buf, j * w * 3, dst_line, w * 3);
            }
            bit.UnlockBits(bmpdat);
            #endregion

            return bit;
        }

        public static double[] AutoWhiteBalance(this PixelByte p)
        {
            double Gr = p.TrimBayer(0, 0).Med();
            double Gb = p.TrimBayer(1, 1).Med();
            double R = p.TrimBayer(1, 0).Med();
            double B = p.TrimBayer(0, 1).Med();

            return new double[]{ (Gr+ Gb)/(2 * R) , 1.0, (Gr + Gb) / (2 * B) };
        }

        /*
        public static PixelDouble[] ColorBMP(PixelDouble p)
                {
                    double[] buf_R = new double[p.Size];
                    double[] buf_G = new double[p.Size];
                    double[] buf_B = new double[p.Size];

                    //カラーフィルター（暫定）
                    double[] gain = new double[] { 1.659, 1.0, 2.433 };
                    double[,] matrix = new double[,]  {{1.2, -0.3, 0.05}
                                                    ,{-0.8, 1.12,  -0.04}
                                                    ,{0.06,  -0.52,  1.4}
                                                    };
                    double gamma = 0.454545;

                    double m = 0;
                    for (int y = 0; y < p.Height; y++) //max値算出。端のデータは飛んでるので有効画素内のみで計算
                        for (int x = 0; x < p.Width; x++)
                            if (p[x, y] > m) m = p[x, y];

                    double r = 0;
                    double b = 0;
                    double g = 0;

                    //RGB信号作成（8x8）
                    for (int y = 0; y < p.Height - 3; y++)
                    {
                        for (int x = 0; x < p.Width - 3; x++)
                        {
                            switch ((x + 1) % 2 + ((y) % 2) * 2)
                            {
                                case 0:B
                                    r = (9 * p[x + 2, y + 2]
                                        + 3 * p[x + 2, y]
                                        + 3 * p[x, y + 2]
                                        + p[x, y]
                                        ) / 16 * gain[0];
                                    g = (p[x + 2, y + 1]
                                        + p[x + 1, y + 2]
                                        ) / 2 * gain[1];
                                    b = (9 * p[x + 1, y + 1]
                                        + 3 * p[x + 3, y + 1]
                                        + 3 * p[x + 1, y + 3]
                                        + p[x + 3, y + 3]
                                        ) / 16 * gain[2];
                                    break;
                                case 1:gr
                                    r = (9 * p[x + 1, y + 2]
                                        + 3 * p[x + 1, y]
                                        + 3 * p[x + 3, y + 2]
                                        + p[x + 3, y]
                                        ) / 16 * gain[0];
                                    g = (p[x + 1, y + 1]
                                        + p[x + 2, y + 2]
                                        ) / 2 * gain[1];
                                    b = (9 * p[x + 2, y + 1]
                                        + 3 * p[x, y + 1]
                                        + 3 * p[x + 2, y + 3]
                                        + p[x, y + 3]
                                        ) / 16 * gain[2];
                                    break;
                                case 2:R
                                    r = (9 * p[x + 2, y + 1]
                                        + 3 * p[x, y + 1]
                                        + 3 * p[x + 2, y + 3]
                                        + p[x + 3, y]
                                        ) / 16 * gain[0];
                                    g = (p[x + 1, y + 1]
                                        + p[x + 2, y + 2]
                                        ) / 2 * gain[1];
                                    b = (9 * p[x + 1, y + 2]
                                        + 3 * p[x + 1, y]
                                        + 3 * p[x + 3, y + 2]
                                        + p[x + 3, y]
                                        ) / 16 * gain[2];
                                    break;
                                case 3:
                                    r = (9 * p[x + 1, y + 1]
                                        + 3 * p[x + 3, y + 1]
                                        + 3 * p[x + 1, y + 3]
                                        + p[x + 3, y + 3]
                                        ) / 16 * gain[0];
                                    g = (p[x + 2, y + 1]
                                        + p[x + 1, y + 2]
                                        ) / 2 * gain[1];
                                    b = (9 * p[x + 2, y + 2]
                                        + 3 * p[x + 2, y]
                                        + 3 * p[x, y + 2]
                                        + p[x, y]
                                        ) / 16 * gain[2];
                                    break;
                            }
                            r = r / m; //1階調化
                            g = g / m;
                            b = b / m;

                            double rr = matrix[0, 0] * r + matrix[0, 1] * g + matrix[0, 2] * b;
                            double gg = matrix[1, 1] * r + matrix[1, 1] * g + matrix[1, 2] * b;
                            double bb = matrix[2, 0] * r + matrix[2, 1] * g + matrix[2, 2] * b;

                            r = 255 * (double)Math.Pow(rr, gamma); //ガンマ
                            g = 255 * (double)Math.Pow(gg, gamma);
                            b = 255 * (double)Math.Pow(bb, gamma);

                            buf_R[(y * p.Width + x)] = (r < 0 ? 0 : (255 < r ? 255 : r));
                            buf_G[(y * p.Width + x)] = (g < 0 ? 0 : (255 < g ? 255 : g));
                            buf_B[(y * p.Width + x)] = (b < 0 ? 0 : (255 < b ? 255 : b));
                        }
                    }
                    //計算外黒埋め
                    for (int y = p.Height - 3; y < p.Height; y++)
                        for (int x = p.Width - 3; x < p.Width; x++)
                        {
                            buf_R[(y * p.Width + x)] = 0;
                            buf_G[(y * p.Width + x)] = 0;
                            buf_B[(y * p.Width + x)] = 0;
                        }

                    return new PixelDouble[3] { new PixelDouble(buf_R, p.Width, p.Height), new PixelDouble(buf_G, p.Width, p.Height), new PixelDouble(buf_B, p.Width, p.Height) };
                }

                /// <summary>
                /// カラー画像の作成。補間します
                /// </summary>
                /// <param name="f"></param>
                /// <param name="P"></param>
                public static void SaveBMPColor(string f, PixelDouble p, int bayer, double gain_0, double gain_1, double gain_2, double gain_3)
                {
                    //RGB化
                    double[] rr = new double[p.Size];
                    double[] gg = new double[p.Size];
                    double[] bb = new double[p.Size];
                    double r;
                    double g;
                    double b;


                    double[] gain = new double[4];
                    gain[0] = gain_0;
                    gain[1] = gain_1;
                    gain[2] = gain_2;
                    gain[3] = gain_3;

                    int case_x = 0;
                    int case_y = 0;

                    switch (bayer)
                    {
                        case 0:
                            break;
                        case 1:
                            case_x = 1;
                            case_y = 0;
                            break;
                        case 2:
                            case_x = 0;
                            case_y = 1;
                            break;
                        case 3:
                            case_x = 1;
                            case_y = 1;
                            break;
                    }

                    for (int y = 0; y < p.Height - 3; y++)
                        for (int x = 0; x < p.Width - 3; x++)
                        {
                            r = 0.0;
                            g = 0.0;
                            b = 0.0;
                            switch ((x + 1 + case_x) % 2 + ((y + case_y) % 2) * 2)
                            {
                                case 0:
                                    r = (9 * p[x + 2, y + 2]
                                        + 3 * p[x + 2, y]
                                        + 3 * p[x, y + 2]
                                        + p[x, y]
                                        ) / 16.0 * gain[0];
                                    g = (p[x + 2, y + 1] * gain[2]
                                        + p[x + 1, y + 2] * gain[1]
                                        ) / 2.0;
                                    b = (9 * p[x + 1, y + 1]
                                        + 3 * p[x + 3, y + 1]
                                        + 3 * p[x + 1, y + 3]
                                        + p[x + 3, y + 3]
                                        ) / 16.0 * gain[3];
                                    break;
                                case 1:
                                    r = (9 * p[x + 1, y + 2]
                                        + 3 * p[x + 1, y]
                                        + 3 * p[x + 3, y + 2]
                                        + p[x + 3, y]
                                        ) / 16.0 * gain[0];
                                    g = (p[x + 1, y + 1] * gain[2]
                                        + p[x + 2, y + 2] * gain[1]
                                        ) / 2.0;
                                    b = (9 * p[x + 2, y + 1]
                                        + 3 * p[x, y + 1]
                                        + 3 * p[x + 2, y + 3]
                                        + p[x, y + 3]
                                        ) / 16.0 * gain[3];
                                    break;
                                case 2:
                                    r = (9 * p[x + 2, y + 1]
                                        + 3 * p[x, y + 1]
                                        + 3 * p[x + 2, y + 3]
                                        + p[x + 3, y]
                                        ) / 16.0 * gain[0];
                                    g = (p[x + 1, y + 1] * gain[1]
                                        + p[x + 2, y + 2] * gain[2]
                                        ) / 2.0;
                                    b = (9 * p[x + 1, y + 2]
                                        + 3 * p[x + 1, y]
                                        + 3 * p[x + 3, y + 2]
                                        + p[x + 3, y]
                                        ) / 16.0 * gain[3];
                                    break;
                                case 3:
                                    r = (9 * p[x + 1, y + 1]
                                        + 3 * p[x + 3, y + 1]
                                        + 3 * p[x + 1, y + 3]
                                        + p[x + 3, y + 3]
                                        ) / 16.0 * gain[0];
                                    g = (p[x + 2, y + 1] * gain[2]
                                        + p[x + 1, y + 2] * gain[1]
                                        ) / 2.0;
                                    b = (9 * p[x + 2, y + 2]
                                        + 3 * p[x + 2, y]
                                        + 3 * p[x, y + 2]
                                        + p[x, y]
                                        ) / 16.0 * gain[3];
                                    break;
                            }

                            rr[x + y * p.Width] = r;
                            gg[x + y * p.Width] = g;
                            bb[x + y * p.Width] = b;
                        }

                    DegProc.SaveBMPColor(f, new PixelDouble(rr, p.Width, p.Height), new PixelDouble(gg, p.Width, p.Height), new PixelDouble(bb, p.Width, p.Height));

                }
                /// <summary>
                /// カラー画像の作成。補間処理済RGBレイヤー加算のみ行います
                /// </summary>
                /// <param name="f">ファイル名</param>
                /// <param name="R">赤</param>
                /// <param name="G">緑</param>
                /// <param name="B">青</param>
                public static void SaveBMPColor(string f, PixelDouble R, PixelDouble G, PixelDouble B)
                {
                    using (Bitmap b = new Bitmap(G.Width, G.Height))
                    {
                        BitmapData bmpdat = b.LockBits(new Rectangle(0, 0, G.Width, G.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                        byte[] buf = new byte[G.Size * 4];
                        for (int i = 0; i < G.Size; i++)
                        {
                            buf[i * 4 + 2] = (byte)(R[i] < 0 ? 0 : (255 < R[i] ? 255 : R[i]));  //R
                            buf[i * 4 + 1] = (byte)(G[i] < 0 ? 0 : (255 < G[i] ? 255 : G[i]));  //G
                            buf[i * 4 + 0] = (byte)(B[i] < 0 ? 0 : (255 < B[i] ? 255 : B[i]));  //B
                            buf[i * 4 + 3] = 0;                                                 //α値。LockBits時にFormat32bppArgbを指定した場合
                        }
                        Marshal.Copy(buf, 0, bmpdat.Scan0, buf.Length);
                        b.UnlockBits(bmpdat);
                        b.Save(f, ImageFormat.Bmp);
                    }
                }
                /// <summary>
                /// カラー画像の作成。補間処理済RGBレイヤー加算のみ行います
                /// </summary>
                /// <param name="f">ファイル名</param>
                /// <param name="P">RGB画像配列</param>
                public static void SaveBMPColor(string f, PixelDouble[] P)
                {
                    SaveBMPColor(f, P[0], P[1], P[2]);
                }

                public static double[] AutoWBBayer(PixelDouble p)
                {
                    double[] a = new double[4];

                    for (int y = 0; y < p.Height; y += 2)
                        for (int x = 0; x < p.Width; x += 2)
                        {
                            a[0] += p[x, y];
                            a[1] += p[x + 1, y];
                            a[2] += p[x, y + 1];
                            a[3] += p[x + 1, y + 1];
                        }
                    return a;
                }
                */
    }

}
