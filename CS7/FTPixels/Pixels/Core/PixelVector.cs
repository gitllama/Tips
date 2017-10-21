using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
//using System.Drawing.Imaging;
using System.Threading;

using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

using System.IO.Compression;
using System.Threading.Tasks;

using System.Numerics;

namespace Pixels
{

    /// <summary>
    /// Vector4 Pixelクラス
    /// </summary>
    public partial class PixelVector
    {
        public CancellationTokenSource token;
        protected Vector4[] pixel;
        float[] buf = new float[4];

        public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

        /// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
        //public byte[] ToArray() => pixel.ToArray();

        public float this[int x]
        {
            protected set{ this[x % Width, x / Width] = value; }
            get{ return this[x % Width, x / Width]; }
        }

        public float this[int x, int y]
        {
            protected set
            {
                switch (x / 2 + 2 * (y % 2))
                {
                    case 0:
                        pixel[x / 2].X = value;
                        break;
                    case 1:
                        pixel[x / 2].Y = value;
                        break;
                    case 2:
                        pixel[x / 2].Z = value;
                        break;
                    case 3:
                        pixel[x / 2].W = value;
                        break;
                }
            }
            get
            {
                switch (x / 2 + 2 * (y % 2))
                {
                    case 0:
                        return pixel[x / 2].X;
                    case 1:
                        return pixel[x / 2].Y;
                    case 2:
                        return pixel[x / 2].Z;
                    case 3:
                        return pixel[x / 2].W;
                    default:
                        return 0;
                }
            }
        }


        /* -------------------------------------- */


        public PixelVector(float[] buf, int Width, int Height, CancellationTokenSource tok)
        {
            this.Width = Width;
            this.Height = Height;
            this.Size = Width * Height;

            if (buf.Length != Size) throw new ArgumentOutOfRangeException("new");
            if (int.MaxValue <= Size) throw new OverflowException("Over MaxValue of int");

            this.pixel = buf;//参照コピー（コンストラクタ呼び出し時は値コピー
            token = tok;

            //this.pixel = new double[Size]; <-いらんかったんや
            //buf.CopyTo(pixel, 0);//値コピー
            //initmemo();
        }

        public PixelVector(float[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

        public PixelVector(int Width, int Height) : this(new float[Width * Height], Width, Height, new CancellationTokenSource()) { }


        /*--------------------------------------*/
        //   operator
        /*--------------------------------------*/
        #region operator 四則演算

        public unsafe static PixelInt operator +(PixelByte x, PixelByte y)
        {
            if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");

            int size = x.Size;
            int[] dst = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &dst[0])
            {
                byte* pt_x = p_x;
                byte* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < x.pixel.Length; ++i)
                //for (int i = 0; i < size; ++i)
                {
                    *pt_ret++ = *pt_x++ + *pt_y++;
                }
                x.token.Token.ThrowIfCancellationRequested(); // 600くらい稼げる

                //1800 vs 2000
                //for(int i = 0; i < x.pixel.Length; ++i)
                //{
                //	dst[i] = x[i] + y[i];
                //	x.token.Token.ThrowIfCancellationRequested();
                //}

                //9479
                //foreach (var i in x.pixel.Select((v, index) => new { v, index }))
                //{
                //dst[i.index] = x[i.index] + y[i.index];
                //x.token.Token.ThrowIfCancellationRequested();
                //}

                //8900
                //foreach (var i in x.pixel.Select((v, index) => new { v, index }))
                //{
                //dst[i.index] = i.v + y[i.index];
                //x.token.Token.ThrowIfCancellationRequested();
                //}

            }
            return new PixelInt(dst, x.Width, y.Height, x.token);
        }



        #endregion


    }


}