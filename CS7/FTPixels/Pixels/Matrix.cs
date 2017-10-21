using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Single;

namespace test20161116
{
    class Program
    {
        static void Main(string[] args)
        {

            //var filename = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\001.btn";
            //var filename2 = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\003.btn";
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();


            List<double> result = new List<double>();

            for (;;)
                for (int i = 1; i < 101; i += 1)
                {
                    var filename = $"J:\\PC-211_share\\BTN\\20150914_100枚RAW画像＿チャート\\L50\\{i:000}.btn";

                    var buf = (PixFloat)(new PixShort(filename, 7872, 4348));
                    var buf2 = new PixMatFloat(buf.pixel, 7872, 4348);

                    sw.Restart();

                    //buf += (buf * buf);
                    buf *= buf;

                    sw.Stop();
                    Console.WriteLine(sw.Elapsed.TotalMilliseconds / 1000);

                    sw.Restart();

                    buf2 *= buf2;
                    //buf2 += (buf2 * buf2);

                    sw.Stop();
                    Console.WriteLine(sw.Elapsed.TotalMilliseconds / 1000);
                }

            //for(;;)
            //for (int i = 1; i < 101; i += 1)
            //{
            //    sw.Restart();

            //    var filename = $"J:\\PC-211_share\\BTN\\20150914_100枚RAW画像＿チャート\\L50\\{i:000}.btn";
            //    var buf = (PixFloat)(new PixShort(filename, 7872, 4348));

            //        buf += (buf * buf);
            //        buf += (buf * buf);

            //        GC.Collect();
            //        GC.WaitForPendingFinalizers();
            //        GC.Collect();

            //        sw.Stop();
            //    Console.WriteLine(sw.Elapsed.TotalMilliseconds / 1000);
            //}



            //var ave = new PixFloat(new float[7872 * 4348], 7872, 4348);
            //var dev = new PixFloat(new float[7872 * 4348], 7872, 4348);
            //var ave2 = new PixFloatVec(new float[7872 * 4348], 7872, 4348);
            //var dev2 = new PixFloatVec(new float[7872 * 4348], 7872, 4348);
            //for (int i = 1; i < 101; i+=2)
            //{
            //    sw.Restart();
            //    var filename = $"J:\\PC-211_share\\BTN\\20150914_100枚RAW画像＿チャート\\L50\\{i:000}.btn";
            //    var buf = (PixFloat)(new PixShort(filename, 7872, 4348));

            //    buf += (buf * buf);

            //    sw.Stop();
            //    Console.WriteLine(sw.Elapsed.TotalMilliseconds / 1000);


            //    /*********/

            //    sw.Restart();
            //    var filename2 = $"J:\\PC-211_share\\BTN\\20150914_100枚RAW画像＿チャート\\L50\\{i+1:000}.btn";
            //    var buf2 = (PixFloatVec)(new PixShort(filename2, 7872, 4348));

            //    buf2 += (buf2 * buf2);

            //    sw.Stop();
            //    Console.WriteLine(" " + sw.Elapsed.TotalMilliseconds / 1000);

            //    //result.Add(sw.Elapsed.TotalMilliseconds / 1000);
            //}

            //var ave = new PixFloat(new float[7872 * 4348], 7872, 4348);
            //var dev = new PixFloat(new float[7872 * 4348], 7872, 4348);
            //for (int i = 1; i < 101; i++)
            //{
            //    sw.Restart();
            //    var filename = $"J:\\PC-211_share\\BTN\\20150914_100枚RAW画像＿チャート\\L50\\{i:000}.btn";

            //    var buf = (PixFloat)(new PixShort(filename, 7872, 4348));
            //    ave += buf;
            //    dev += (buf * buf);
            //    //ave.Add(buf);
            //    //p.Div(100);

            //    buf = null;
            //    GC.Collect();

            //    sw.Stop();
            //    Console.WriteLine(sw.Elapsed.TotalMilliseconds / 1000);
            //    result.Add(sw.Elapsed.TotalMilliseconds / 1000);
            //}

            //var ave2 = new PixFloatVec(new Vector4[7872 * 4348 / 4], 7872, 4348);
            //var dev2 = new PixFloatVec(new Vector4[7872 * 4348 / 4], 7872, 4348);
            //for (int i = 1; i < 101; i++)
            //{
            //    sw.Restart();
            //    var filename = $"J:\\PC-211_share\\BTN\\20150914_100枚RAW画像＿チャート\\L50\\{i:000}.btn";


            //    var buf = (PixFloatVec)(new PixShort(filename, 7872, 4348));
            //    ave2 += buf;
            //    dev2 += (buf * buf);

            //    buf = null;
            //    //GC.Collect();

            //    sw.Stop();
            //    Console.WriteLine(sw.Elapsed.TotalMilliseconds / 1000);
            //    result.Add(sw.Elapsed.TotalMilliseconds / 1000);
            //}


            //Console.WriteLine($"{result.Max()}, {result.Min()}, {result.Average()}");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Console.WriteLine("end");
            Console.ReadKey();
        }

    }

    //GCで処理落ちさせないようにするには、使いまわししなきゃならない
    // -> 面倒くさい
    // ストリームから直で配列に入れる命令の方が良い

        //1. 使いまわし用（直書）の命令作る = new しない！
        //2. 串刺し演算用の命令作る
       

    public class PixShort
    {
        public short[] pixel;
        public int Width;
        public int Height;

        public PixShort(string filename, int width, int height)
        {
            Width = width;
            Height = height;
            pixel = new short[width * height];
            unsafe
            {
                byte[] src = File.ReadAllBytes(filename);
                fixed (byte* p = &src[0])
                {
                    Marshal.Copy(new IntPtr(p), pixel, 0, width * height);
                }
            }
        }

        public static explicit operator PixFloat(PixShort a)
        {
            float[] dst = new float[a.Width * a.Height];
            Parallel.For(0, a.Width * a.Height, (i) =>
            {
                dst[i] = (float)a.pixel[i];
            });
            return new PixFloat(dst, a.Width, a.Height);
        }
        public static explicit operator PixFloatVec(PixShort a)
        {
            Vector4[] dst = new Vector4[a.Width * a.Height / 4];
            Parallel.For(0, a.Width * a.Height / 4, (i) =>
            {
                dst[i] = new Vector4((float)a.pixel[i * 4], (float)a.pixel[i * 4 + 1], (float)a.pixel[i * 4 + 2], (float)a.pixel[i * 4 + 3]);
            });
            return new PixFloatVec(dst, a.Width, a.Height);
        }

    }

    public class PixFloat
    {
        public float[] pixel;
        public int Width;
        public int Height;

        public PixFloat(string filename, int width, int height)
        {
            Width = width;
            Height = height;
            pixel = new float[width * height];
            unsafe
            {
                byte[] src = File.ReadAllBytes(filename);
                fixed (byte* p = &src[0])
                {
                    Marshal.Copy(new IntPtr(p), pixel, 0, width * height);
                }
            }
        }

        public PixFloat(float[] src, int width, int height)
        {
            Width = width;
            Height = height;
            pixel = src;
        }

        public PixFloat(int width, int height)
        {
            Width = width;
            Height = height;
            pixel = new float[width * height];
        }

        public static PixFloat operator + (PixFloat x, PixFloat y)
        {
            float[] dst = new float[x.Width * x.Height];
            Parallel.For(0, x.Width * x.Height, (i) =>
            {
                dst[i] = x.pixel[i] + y.pixel[i];
            });

            return new PixFloat(dst, x.Width, x.Height);
        }

        public static PixFloat operator *(PixFloat x, PixFloat y)
        {
            float[] dst = new float[x.Width * x.Height];
            Parallel.For(0, x.Width * x.Height, (i) =>
            {
                dst[i] = x.pixel[i] * y.pixel[i];
            });

            return new PixFloat(dst, x.Width, x.Height);
        }

        public void Add(PixFloat x)
        {
            Parallel.For(0, Width * Height, (i) =>
            {
                pixel[i] += x.pixel[i];
            });
        }

        public void Div(PixFloat x)
        {
            Parallel.For(0, Width * Height, (i) =>
            {
                pixel[i] /= x.pixel[i];
            });
        }
        public void Div(float x)
        {
            Parallel.For(0, Width * Height, (i) =>
            {
                pixel[i] /= x;
            });
        }

    }

    public class PixFloatB
    {
        public float[] pixel;
        public int Width;
        public int Height;

        public PixFloatB(string filename, int width, int height)
        {
            Width = width;
            Height = height;
            pixel = new float[width * height];
            unsafe
            {
                byte[] src = File.ReadAllBytes(filename);
                fixed (byte* p = &src[0])
                {
                    Marshal.Copy(new IntPtr(p), pixel, 0, width * height);
                }
            }
        }

        public PixFloatB(float[] src, int width, int height)
        {
            Width = width;
            Height = height;
            pixel = src;
        }

        public PixFloatB(int width, int height)
        {
            Width = width;
            Height = height;
            pixel = new float[width * height];
        }

        public static PixFloatB operator +(PixFloatB x, PixFloatB y)
        {
            var dst = new PixFloatB(x.Width, x.Height);
            Parallel.For(0, x.Width * x.Height, (i) =>
            {
                dst.pixel[i] = x.pixel[i] + y.pixel[i];
            });

            return dst;
        }

        public static PixFloatB operator *(PixFloatB x, PixFloatB y)
        {
            var dst = new PixFloatB(x.Width, x.Height);
            Parallel.For(0, x.Width * x.Height, (i) =>
            {
                dst.pixel[i] = x.pixel[i] * y.pixel[i];
            });

            return dst;
        }

    }

    public class PixFloatVec
    {
        public Vector4[] pixel;
        //public float[] pixel;
        public int Width;
        public int Height;

        public PixFloatVec(string filename, int width, int height)
        {
            Width = width;
            Height = height;
            pixel = new Vector4[width * height / 4];
            unsafe
            {
                byte[] src = File.ReadAllBytes(filename);

            }
        }

        public PixFloatVec(float[] src, int width, int height)
        {
            Width = width;
            Height = height;
            pixel = new Vector4[width * height / 4];
            Parallel.For(0, width * height / 4, (i) =>
            {
                pixel[i] = new Vector4(src[i * 4], src[i * 4 + 1], src[i * 4 + 2], src[i * 4 + 3]);
            });
        }
        public PixFloatVec(Vector4[] src, int width, int height)
        {
            Width = width;
            Height = height;
            pixel = src;
        }

        public static PixFloatVec operator +(PixFloatVec x, PixFloatVec y)
        {
            Vector4[] dst = new Vector4[x.Width * x.Height / 4];
            Parallel.For(0, x.Width * x.Height / 4, (i) =>
            {
                dst[i] = x.pixel[i] + y.pixel[i];
            });

            return new PixFloatVec(dst, x.Width, x.Height);
        }

        public static PixFloatVec operator *(PixFloatVec x, PixFloatVec y)
        {
            Vector4[] dst = new Vector4[x.Width * x.Height / 4];
            Parallel.For(0, x.Width * x.Height / 4, (i) =>
            {
                dst[i] = x.pixel[i] * y.pixel[i];
            });

            return new PixFloatVec(dst, x.Width, x.Height);
        }

    }


    public class PixMatFloat
    {
        Matrix<float> pixel;
        public int Width;
        public int Height;
        public PixMatFloat(float[] src, int width, int height)
        {
            Width = width;
            Height = height;

            float[,] buf = new float[width, height];

            for(int y=0;y< height;y++)
                for(int x = 0; x < height; x++)
                {
                    buf[x, y] = src[x + y * width];
                }

            pixel = MathNet.Numerics.LinearAlgebra.Single.DenseMatrix.OfArray(buf);
        }

        public PixMatFloat(Matrix<float> src, int width, int height)
        {
            Width = width;
            Height = height;
            pixel = src;
        }

        public static PixMatFloat operator +(PixMatFloat x, PixMatFloat y)
        {
            return new PixMatFloat(x.pixel.Add(y.pixel), x.Width ,y.Height);
        }

        public static PixMatFloat operator *(PixMatFloat x, PixMatFloat y)
        {
            return new PixMatFloat(x.pixel.PointwiseMultiply(y.pixel), x.Width, y.Height);
        }

        //x.pixel.Append(y.pixel) 横に足してる
    }

}