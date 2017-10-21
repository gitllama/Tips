using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Windows.Media.Imaging;
using Pixels;

namespace PixelsExtend
{
    public static class OpenCVExtensions
    {
        public static BitmapSource DemosaicToRGBW(this PixelDouble pix,double offset,double depth,double depth_w,double Rgain,double Bgain)
        {
            byte[] R = new byte[pix.Size];
            byte[] G = new byte[pix.Size];
            byte[] B = new byte[pix.Size];
            byte[] W = new byte[pix.Size];

            int c11 = 0;
            int c12 = 1;
            int c13 = 2;
            int c21 = pix.Width + 0;
            int c22 = pix.Width + 1;
            int c23 = pix.Width + 2;
            int c31 = 2 * pix.Width + 0;
            int c32 = 2 * pix.Width + 1;
            int c33 = 2 * pix.Width + 2;

            for (int i = 0; i < pix.Size - 2 - 2 * pix.Width; i++)
            {
                double rr = 0;
                double gg = 0;
                double bb = 0;
                double ww = 0;

                int hoge = i % 2 + 2 * ((int)(i / pix.Width) % 2);
                switch (hoge)
                {
                    case 0://G
                        rr = (pix[c12] + pix[c32]) / 2;
                        gg = (pix[c11] + pix[c13] + pix[c31] + pix[c33]) / 4;
                        bb = (pix[c21] + pix[c23]) / 2;
                        ww = pix[c22];
                        break;

                    case 1://R
                        gg = (pix[c12] + pix[c32]) / 2;
                        rr = (pix[c11] + pix[c13] + pix[c31] + pix[c33]) / 4;
                        ww = (pix[c21] + pix[c23]) / 2;
                        bb = pix[c22];
                        break;

                    case 2://B
                        ww = (pix[c12] + pix[c32]) / 2;
                        bb = (pix[c11] + pix[c13] + pix[c31] + pix[c33]) / 4;
                        gg = (pix[c21] + pix[c23]) / 2;
                        rr = pix[c22];
                        break;
                    case 3://W
                        bb = (pix[c12] + pix[c32]) / 2;
                        ww = (pix[c11] + pix[c13] + pix[c31] + pix[c33]) / 4;
                        rr = (pix[c21] + pix[c23]) / 2;
                        gg = pix[c22];
                        break;
                }
                c11++;
                c12++;
                c13++;
                c21++;
                c22++;
                c23++;
                c31++;
                c32++;
                c33++;

                rr = 255 * Rgain * (rr + offset) / depth;
                bb = 255 * Bgain * (bb + offset) / depth;
                gg = 255 * (gg + offset) / depth;
                ww = 255 * depth_w * (ww + offset) / depth;

                R[i] = (byte)(rr > 255 ? 255 : rr < 0 ? 0 : rr);
                G[i] = (byte)(gg > 255 ? 255 : gg < 0 ? 0 : gg);
                B[i] = (byte)(bb > 255 ? 255 : bb < 0 ? 0 : bb);
                W[i] = (byte)(ww > 255 ? 255 : ww < 0 ? 0 : ww);
            }

            using (var Rmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, R))
            using (var Gmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, G))
            using (var Bmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, B))
            using (var Wmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, W))
            {
                var rgbmat = new Mat();
                Cv2.Merge(new Mat[]{Bmat,Gmat,Rmat}, rgbmat);

                var hsvmat = new Mat();
                Cv2.CvtColor(rgbmat, hsvmat, ColorConversionCodes.BGR2HSV);

                var splitmat = hsvmat.Split();

                Cv2.Merge(new Mat[] { splitmat[0], splitmat[1], Wmat }, hsvmat);
                //Cv2.Merge(new Mat[]{Wmat, splitmat[1],splitmat[2]}, hsvmat);

                Cv2.CvtColor(hsvmat, rgbmat, ColorConversionCodes.HSV2BGR);

                var buf = rgbmat.ToBitmapSource();
                buf.Freeze();

                return buf;
            }


        }

        public static List<byte[]> RGBWDemosaicToMat(PixelInt pix)
        {
            byte[] R = new byte[pix.Size];
            byte[] G = new byte[pix.Size];
            byte[] B = new byte[pix.Size];
            byte[] W = new byte[pix.Size];

            int c11 = 0;
            int c12 = 1;
            int c13 = 2;
            int c21 = pix.Width + 0;
            int c22 = pix.Width + 1;
            int c23 = pix.Width + 2;
            int c31 = 2 * pix.Width + 0;
            int c32 = 2 * pix.Width + 1;
            int c33 = 2 * pix.Width + 2;

            for (int i = 0; i < pix.Size - 2 - 2 * pix.Width; i++)
            {
                int rr = 0;
                int gg = 0;
                int bb = 0;
                int ww = 0;

                int hoge = i % 2 + 2 * ((int)(i / pix.Width) % 2);
                switch (hoge)
                {
                    case 0://G
                        rr = (pix[c12] + pix[c32]) / 2;
                        gg = (pix[c11] + pix[c13] + pix[c31] + pix[c33]) / 4;
                        bb = (pix[c21] + pix[c23]) / 2;
                        ww = pix[c22];
                        break;

                    case 1://R
                        gg = (pix[c12] + pix[c32]) / 2;
                        rr = (pix[c11] + pix[c13] + pix[c31] + pix[c33]) / 4;
                        ww = (pix[c21] + pix[c23]) / 2;
                        bb = pix[c22];
                        break;

                    case 2://B
                        ww = (pix[c12] + pix[c32]) / 2;
                        bb = (pix[c11] + pix[c13] + pix[c31] + pix[c33]) / 4;
                        gg = (pix[c21] + pix[c23]) / 2;
                        rr = pix[c22];
                        break;
                    case 3://W
                        bb = (pix[c12] + pix[c32]) / 2;
                        ww = (pix[c11] + pix[c13] + pix[c31] + pix[c33]) / 4;
                        rr = (pix[c21] + pix[c23]) / 2;
                        gg = pix[c22];
                        break;
                }
                c11++;
                c12++;
                c13++;
                c21++;
                c22++;
                c23++;
                c31++;
                c32++;
                c33++;

                rr = (int)(1 * rr * 1);
                bb = (int)(1 * bb * 1);
                gg = (int)(1 * gg * 1);
                ww = (int)(1 * ww * 1);
                R[i] = (byte)(rr > 255 ? 255 : rr < 0 ? 0 : rr);
                G[i] = (byte)(gg > 255 ? 255 : gg < 0 ? 0 : gg);
                B[i] = (byte)(bb > 255 ? 255 : bb < 0 ? 0 : bb);
                W[i] = (byte)(ww > 255 ? 255 : ww < 0 ? 0 : ww);
            }

            return new List<byte[]>{ R,G,B,W };
        }

        public static BitmapSource ToDemosaicRGBG(this PixelInt pix)
        {
            using (var mat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, pix.Limits(typeof(byte)).ToPixelByte().Stagger(1).ToArray()))
            using (var retmat = new Mat())
            {
                Cv2.CvtColor(mat, retmat, ColorConversionCodes.BayerGR2BGR);
                var buf = retmat.ToBitmapSource();
                buf.Freeze();
                return buf;
            }
        }

        public static BitmapSource ToDemosaicRGBW(this PixelInt pix)
        {
            var c = RGBWDemosaicToMat(pix);


            using (var Rmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[0]))
            using (var Gmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[1]))
            using (var Bmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[2]))
            {
                var rgbmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC3);
                //var Retmat = new Mat();
                Cv2.Merge(new Mat[]
                {
                    Bmat,Gmat,Rmat
                }, rgbmat);

                //Cv2.CvtColor(rgbmat, Retmat, ColorConversionCodes.);
                var buf = rgbmat.ToBitmapSource();
                buf.Freeze();

                return buf;
            }
        }
        public static BitmapSource ToDemosaicRGBW2(this PixelInt pix)
        {
            var c = RGBWDemosaicToMat(pix);


            using (var Rmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[0]))
            using (var Gmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[1]))
            using (var Bmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[2]))
            using (var Wmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[3]))
            {
                var rgbmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC3);
                //var Retmat = new Mat();
                Cv2.Merge(new Mat[]
                {
                Bmat,Gmat,Rmat
                }, rgbmat);

                var retmat = new Mat();
                Cv2.CvtColor(rgbmat, retmat, ColorConversionCodes.BGR2HSV_FULL);

                var splitmat = retmat.Split();

                Cv2.Merge(new Mat[]
                {
                    splitmat[0],
                    splitmat[1],
                    Wmat
                }, retmat);

                Cv2.CvtColor(retmat, rgbmat, ColorConversionCodes.HSV2BGR_FULL);

                var buf = rgbmat.ToBitmapSource();
                buf.Freeze();

                return buf;
            }

        }

        public static BitmapSource ToDemosaicRGBW3(this PixelInt pix)
        {
            var c = RGBWDemosaicToMat(pix);


            using (var Rmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[0]))
            using (var Gmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[1]))
            using (var Bmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[2]))
            using (var Wmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[3]))
            {
                var rgbmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC3);
                //var Retmat = new Mat();
                Cv2.Merge(new Mat[]
                {
                    Bmat,Gmat,Rmat
                }, rgbmat);

                var retmat = new Mat();
                Cv2.CvtColor(rgbmat, retmat, ColorConversionCodes.BGR2HSV_FULL);

                var splitmat = retmat.Split();
                Cv2.FastNlMeansDenoising(splitmat[0], splitmat[0],20);
                Cv2.FastNlMeansDenoising(splitmat[1], splitmat[1],20);

                //Cv2(splitmat[0], splitmat[0],10,35);
                //Cv2.PyrMeanShiftFiltering(splitmat[1], splitmat[1], 10, 35);

                //Cv2.FastNlMeansDenoising(Wmat, Wmat);



                Cv2.Merge(new Mat[]
                {
                    splitmat[0],
                    splitmat[1],
                    Wmat
                }, retmat);

                Cv2.CvtColor(retmat, rgbmat, ColorConversionCodes.HSV2BGR_FULL);

                splitmat = rgbmat.Split();
                Cv2.Merge(new Mat[]
                {
                    splitmat[0] / 1.6,
                    splitmat[1] / 1.6,
                    splitmat[2] / 1.6,
                }, rgbmat);

                var buf = rgbmat.ToBitmapSource();
                buf.Freeze();

                return buf;
            }
        }

        public static BitmapSource ToDemosaicRGBW4(this PixelInt pix)
        {
            var c = RGBWDemosaicToMat(pix);


            using (var Rmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[0]))
            using (var Gmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[1]))
            using (var Bmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[2]))
            using (var Wmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, c[3]))
            {
                var rgbmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC3);
                //var Retmat = new Mat();
                Cv2.Merge(new Mat[]
                {
                    Bmat,Gmat,Rmat
                }, rgbmat);

                var retmat = new Mat();

                Cv2.PyrMeanShiftFiltering(rgbmat, rgbmat, 5, 15);
                Cv2.CvtColor(rgbmat, retmat, ColorConversionCodes.BGR2HSV_FULL);

                var splitmat = retmat.Split();

                Cv2.Merge(new Mat[]
                {
                    splitmat[0],
                    splitmat[1],
                    Wmat
                }, retmat);

                Cv2.CvtColor(retmat, rgbmat, ColorConversionCodes.HSV2BGR_FULL);

                var buf = rgbmat.ToBitmapSource();
                buf.Freeze();

                return buf;
            }

        }


        public static BitmapSource ToCvColor(this BitmapSource bitmap, int bayertype, double RGain, double BGain)
        {
            using (var in_mat = BitmapSourceConverter.ToMat(bitmap))
            {
                var rgb_mat = new Mat();
                switch(bayertype)
                {
                    case 1:
                        Cv2.CvtColor(in_mat, rgb_mat, ColorConversionCodes.BayerBG2RGB);
                        break;
                    case 0:
                        Cv2.CvtColor(in_mat, rgb_mat, ColorConversionCodes.BayerGB2BGR);
                        break;
                    case 3:
                        Cv2.CvtColor(in_mat, rgb_mat, ColorConversionCodes.BayerGR2BGR);
                        break;
                    case 2:
                        Cv2.CvtColor(in_mat, rgb_mat, ColorConversionCodes.BayerRG2BGR);
                        break;
                }

                var split_mat = rgb_mat.Split();
                var rgb2_mat = new Mat();

                Cv2.Merge(new Mat[] { split_mat[0] * BGain, split_mat[1], split_mat[2] * RGain }, rgb2_mat);



                //var a = new Mat(a.Rows, a.Cols, MatType.CV_8UC1);
                //var b = new Mat(a.Rows,a.Cols,MatType.CV_8UC3);

                //var b = mat[0];
                //var c = new Mat(a.Rows, a.Cols, MatType.CV_8UC1);


                //Cv2.CvtColor(b, c, ColorConvers);
                //Cv2.CvtColor(mat[0], c, ColorConversionCodes.BayerBG2RGB);

                //pictureBox1.Image = mat[0].ToBitmap();
                //pictureBox2.Image =c.ToBitmap();
                //pictureBox1.Image = b.ToBitmap();
                //pictureBox1.Image = mat[0].ToBitmap();

                return rgb2_mat.ToBitmapSource();
            }
        }

        public static BitmapSource ToCvColor(this BitmapSource bitmap, int bayertype, double RGain, double BGain, double[] Matrix, double Gamma)
        {
            using (var in_mat = BitmapSourceConverter.ToMat(bitmap))
            {
                var rgb_mat = new Mat();
                switch (bayertype)
                {
                    case 1:
                        Cv2.CvtColor(in_mat, rgb_mat, ColorConversionCodes.BayerBG2RGB);
                        break;
                    case 0:
                        Cv2.CvtColor(in_mat, rgb_mat, ColorConversionCodes.BayerGB2BGR);
                        break;
                    case 3:
                        Cv2.CvtColor(in_mat, rgb_mat, ColorConversionCodes.BayerGR2BGR);
                        break;
                    case 2:
                        Cv2.CvtColor(in_mat, rgb_mat, ColorConversionCodes.BayerRG2BGR);
                        break;
                }

                var split_mat = rgb_mat.Split();
                var rgb2_mat = new Mat();

                Cv2.Merge(new Mat[] 
                {
                    split_mat[2] * Matrix[6] + split_mat[1] * Matrix[7] + split_mat[0] * Matrix[8] * BGain,
                    split_mat[2] * Matrix[3] + split_mat[1]* Matrix[4] + split_mat[0] * Matrix[5],
                    split_mat[2] * Matrix[0] * RGain + split_mat[1] * Matrix[1] + split_mat[0] * Matrix[2]

                }, rgb2_mat);

                var rgb3_mat = new Mat();
                byte[] lut = new byte[256];
                double gm = 1.0 / Gamma;
                for (int i = 0; i < 256; i++)
                {
                    lut[i] = (byte)(Math.Pow(1.0 * i / 255, gm) * 255);
                }
                Cv2.LUT(rgb2_mat, lut, rgb3_mat);

                var buf = rgb3_mat.ToBitmapSource();
                buf.Freeze();
                return buf;
            }
        }


        public static BitmapSource ToCVMonoBMP(this int[,] raw, int bitshift, int width,int height)
        {
            byte[] buf = new byte[height * width];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int hoge = raw[x, y] >> bitshift;

                    if (hoge > 255) buf[x + y * width] = 255;
                    else if (hoge < 0) buf[x + y * width] = 0;
                    else buf[x + y * width] = (byte)hoge;
                }

            using (var rawmat = new Mat(height, width, MatType.CV_8UC1, buf))
            {
                return rawmat.ToBitmapSource();
            }
        }

        public static BitmapSource ToCVColorBMP(this int[,] raw, int bitshift, int width, int height, double RGain, double BGain)
        {
            byte[] buf = new byte[height * width];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int hoge = raw[x, y] >> bitshift;

                    if (hoge > 255) buf[x + y * width] = 255;
                    else if (hoge < 0) buf[x + y * width] = 0;
                    else buf[x + y * width] = (byte)hoge;
                }

            using (var rawmat = new Mat(height, width, MatType.CV_8UC1, buf))
            {
                var mat_output = new Mat(height, width, MatType.CV_8UC3);
                Cv2.CvtColor(rawmat, mat_output, ColorConversionCodes.BayerGB2BGR);

                var mat_split = Cv2.Split(mat_output);
                Cv2.Merge(new Mat[]
                {
                    mat_split[0] * BGain,
                    mat_split[1] * 1,
                    mat_split[2] * RGain
                }, mat_output);

                return mat_output.ToBitmapSource();
            }
        }


        public static BitmapSource RawToHDR(this PixelInt pix, int bitshift, int bitwidth, double RGain, double BGain, double Gamma)
        {
            //ToByteArray

            var buf = new byte[pix.Height * pix.Width];
            int Max = (int)Math.Pow(2, bitwidth);

            //make LUT
            var LUT = new byte[Max];
            for (int i = 0; i < Max; i++)
            {
                LUT[i] = (byte)(Math.Pow((double)i / Max, 1.0 / Gamma) * 255.0);
            }


            for (int y = 0; y < pix.Height; y++)
                for (int x = 0; x < pix.Width; x++)
                {
                    //shift
                    double hoge = (double)(pix[x, y] >> bitshift);

                    //gain
                    switch ((x % 2) + 2 * (y % 2))
                    {
                        case 0:
                            //hoge = hoge;
                            break;
                        case 1:
                            hoge = hoge * RGain;
                            break;
                        case 2:
                            hoge = hoge * BGain;
                            break;
                        case 3:
                            //hoge = hoge;
                            break;
                    }

                    //trim
                    if (hoge > Max - 1) hoge = Max - 1;
                    else if (hoge < 0) hoge = 0;

                    //LUT
                    buf[x + y * pix.Width] = LUT[(int)hoge];

                }


            //ToOpenCVMat

            using (var rawmat = new Mat(pix.Height, pix.Width, MatType.CV_8UC1, buf))
            {
                var mat_output = new Mat(pix.Height, pix.Width, MatType.CV_8UC3);
                Cv2.CvtColor(rawmat, mat_output, ColorConversionCodes.BayerGB2BGR);

                return mat_output.ToBitmapSource();
            }
        }
    }

    public class OpenCVAvi
    {
        public static void WriteAviFile(List<string> sourcefiles, string outfilename, double fps)
        {
            WriteAviFile(sourcefiles, outfilename, fps, FourCC.Default);
        }

        public static void WriteAviFileTest(List<string> sourcefiles, string outfilename, double fps)
        {
            Size size;
            using (var mat = new Mat(sourcefiles[0]))
            {
                size = mat.Size();

            }

            VideoWriter outV = null;
            for (int i = 0;i< 30;i++)
            {
                using (var mat1 = new Mat(sourcefiles[i]))
                using (var mat2 = new Mat(sourcefiles[i + 1]))
                using (var mat3 = new Mat(sourcefiles[i + 2]))
                using (var mat4 = new Mat(sourcefiles[i + 3]))
                using (var mat5 = new Mat(sourcefiles[i + 4]))
                using (var mat6 = new Mat(sourcefiles[i + 5]))
                using (var mat7 = new Mat(sourcefiles[i + 6]))
                using (var mat8 = new Mat(sourcefiles[i + 7]))
                using (var mat11 = new Mat())
                using (var mat22 = new Mat())
                using (var mat33 = new Mat())
                using (var mat44 = new Mat())
                using (var mat111 = new Mat())
                using (var mat222= new Mat())
                using (var mat = new Mat())
                {
                    if (i == 0)
                    {
                        outV = new VideoWriter(outfilename, FourCC.Default, fps, mat.Size());
                    }
                    Cv2.AddWeighted(mat1, 0.5, mat2, 0.5, 1, mat11);
                    Cv2.AddWeighted(mat3, 0.5, mat4, 0.5, 1, mat22);
                    Cv2.AddWeighted(mat5, 0.5, mat6, 0.5, 1, mat33);
                    Cv2.AddWeighted(mat7, 0.5, mat8, 0.5, 1, mat44);

                    Cv2.AddWeighted(mat11, 0.5, mat22, 0.5, 1, mat111);
                    Cv2.AddWeighted(mat33, 0.5, mat44, 0.5, 1, mat222);

                    Cv2.AddWeighted(mat111, 0.5, mat222, 0.5, 1, mat);

                    outV.Write(mat);
                }
            }
            outV.Release();
        }


        public static void WriteAviFileTest2(List<string> sourcefiles, string outfilename, double fps)
        {
            //Size size;
            //using (var mat = new Mat(sourcefiles[0]))
            //{
            //    size = mat.Size();

            //}

            //VideoWriter outV = null;
            //for (int i = 0; i < 30; i++)
            //{
            //    var i1 = (PixelInt)PixelModelDependent.Read(sourcefiles[i], System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "..\\PixelModels"));
            //    var i2 = (PixelInt)PixelModelDependent.Read(sourcefiles[i+1], System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "..\\PixelModels"));

            //    using (var mat1 = BitmapSourceConverter.ToMat(i1.Limits(typeof(byte)).ToPixelByte().WriteBitmapSource()))
            //    using (var mat2 = BitmapSourceConverter.ToMat(i2.Limits(typeof(byte)).ToPixelByte().WriteBitmapSource()))
            //    using (var mat3 = new Mat())
            //    using (var mat11 = new Mat())
            //    using (var mat22 = new Mat())
            //    {
            //        Cv2.CvtColor(in_mat, rgb_mat, ColorConversionCodes.BayerBG2RGB);

            //        if (i == 0)
            //        {
            //            outV = new VideoWriter(outfilename, FourCC.Default, fps, mat.Size());
            //        }
            //        Cv2.AddWeighted(mat, 0.5, mat1, 0.5, 1, mat2);

            //        outV.Write(mat2);
            //    }
            //}
            //outV.Release();
        }


        public static void WriteAviFile(List<string> sourcefiles, string outfilename, double fps, FourCC fourcc)
        {
            Size size;
            using (var mat = new Mat(sourcefiles[0]))
            {
                size = mat.Size();

            }

            VideoWriter outV = null;
            foreach (var i in sourcefiles.Select((value, index) => new { value, index }))
            {
                using (var mat = new Mat(i.value))
                {
                    //var mat_2 = new Mat();
                    //Cv2.FastNlMeansDenoisingColored(mat, mat_2);

                    if (i.index == 0)
                    {
                        outV = new VideoWriter(outfilename, fourcc, fps, mat.Size());
                    }
                    outV.Write(mat);
                }
            }
            outV.Release();
        }

        public Func<PixelDouble, Mat> action;

        VideoWriter outV = null;
        public OpenCVAvi(string outputfilename, int width, int height, double fps, FourCC fourcc = FourCC.Default)
        {
            outV = new VideoWriter(outputfilename, fourcc, fps, new Size(width, height));
        }

        public void Add(PixelDouble i)
        {
            outV.Write(action(i));
        } 


    }
}
