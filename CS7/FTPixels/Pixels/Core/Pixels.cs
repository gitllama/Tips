

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

/*******************************************************************************/
/*   Pixels 3.0                                                                */
/*   2016/11/15                                                                */
/*******************************************************************************/
namespace Pixels
{

    /// <summary>
    /// 符号なし8ビット整数Pixelクラス
    /// </summary>
    public partial class PixelByte
    {
		public CancellationTokenSource token;
		protected byte[] pixel;

		public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

		/// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
		public byte[] ToArray() => pixel;

		public byte this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

		public byte this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }


		/* -------------------------------------- */


		public PixelByte(byte[] buf, int Width, int Height, CancellationTokenSource tok)
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

        public PixelByte(byte[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

		public PixelByte(int Width, int Height) : this(new byte[Width * Height], Width, Height, new CancellationTokenSource()) { }


        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも



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
                byte*	pt_x = p_x;
				byte*	pt_y = p_y;
                int*	pt_ret = p_ret;

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

		public unsafe static PixelInt operator -(PixelByte x, PixelByte y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] dst = new int[size];

			//900
			Parallel.For(0, size, i =>
			{
				dst[i] = x[i] + y[i];
			});
			x.token.Token.ThrowIfCancellationRequested();


			//あまりきれいじゃないけど 以下みたいなのが落としどころ
			for(int j=0;j<2;j++)
			{
				Parallel.For(0, size, i =>
				{
					dst[i] = x[i] + y[i];
				});
				x.token.Token.ThrowIfCancellationRequested();
			}


            return new PixelInt(dst, x.Width, y.Height, x.token);
		}

		#endregion



        /// <summary>
        /// 平均値を求める, 一画素でもDouble介することによる誤差有
        /// </summary>
        /// <returns></returns>
        public double Ave()
        {
            double ave = 0;

            /*
            int s = Size;
            for (int i = 0; i < Size; i++)
                ave += Convert.ToDouble(pixel[i]);
            return ave / Size;
            */

            //ave = pixel.Select<T, double>(i => Convert.ToDouble(i)).Average();

            foreach (byte i in pixel)
                ave += Convert.ToDouble(i);

            return ave / Size;
        }
	
	}


    /// <summary>
    /// 符号なし16ビット整数Pixelクラス
    /// </summary>
    public partial class PixelUShort
    {
		public CancellationTokenSource token;
		protected ushort[] pixel;

		public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

		/// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
		public ushort[] ToArray() => pixel;

		public ushort this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

		public ushort this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }


		/* -------------------------------------- */


		public PixelUShort(ushort[] buf, int Width, int Height, CancellationTokenSource tok)
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

        public PixelUShort(ushort[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

		public PixelUShort(int Width, int Height) : this(new ushort[Width * Height], Width, Height, new CancellationTokenSource()) { }


        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも



		/*--------------------------------------*/
		//   operator
		/*--------------------------------------*/
		#region operator 四則演算

        public unsafe static PixelInt operator +(PixelUShort x, PixelUShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] dst = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
			fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &dst[0])
            {
                ushort*	pt_x = p_x;
				ushort*	pt_y = p_y;
                int*	pt_ret = p_ret;

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

		public unsafe static PixelInt operator -(PixelUShort x, PixelUShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] dst = new int[size];

			//900
			Parallel.For(0, size, i =>
			{
				dst[i] = x[i] + y[i];
			});
			x.token.Token.ThrowIfCancellationRequested();


			//あまりきれいじゃないけど 以下みたいなのが落としどころ
			for(int j=0;j<2;j++)
			{
				Parallel.For(0, size, i =>
				{
					dst[i] = x[i] + y[i];
				});
				x.token.Token.ThrowIfCancellationRequested();
			}


            return new PixelInt(dst, x.Width, y.Height, x.token);
		}

		#endregion



        /// <summary>
        /// 平均値を求める, 一画素でもDouble介することによる誤差有
        /// </summary>
        /// <returns></returns>
        public double Ave()
        {
            double ave = 0;

            /*
            int s = Size;
            for (int i = 0; i < Size; i++)
                ave += Convert.ToDouble(pixel[i]);
            return ave / Size;
            */

            //ave = pixel.Select<T, double>(i => Convert.ToDouble(i)).Average();

            foreach (ushort i in pixel)
                ave += Convert.ToDouble(i);

            return ave / Size;
        }
	
	}


    /// <summary>
    /// 符号なし32ビット整数Pixelクラス
    /// </summary>
    public partial class PixelUInt
    {
		public CancellationTokenSource token;
		protected uint[] pixel;

		public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

		/// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
		public uint[] ToArray() => pixel;

		public uint this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

		public uint this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }


		/* -------------------------------------- */


		public PixelUInt(uint[] buf, int Width, int Height, CancellationTokenSource tok)
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

        public PixelUInt(uint[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

		public PixelUInt(int Width, int Height) : this(new uint[Width * Height], Width, Height, new CancellationTokenSource()) { }


        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも



		/*--------------------------------------*/
		//   operator
		/*--------------------------------------*/
		#region operator 四則演算

        public unsafe static PixelUInt operator +(PixelUInt x, PixelUInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			uint[] dst = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
			fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &dst[0])
            {
                uint*	pt_x = p_x;
				uint*	pt_y = p_y;
                uint*	pt_ret = p_ret;

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
            return new PixelUInt(dst, x.Width, y.Height, x.token);
		}

		public unsafe static PixelUInt operator -(PixelUInt x, PixelUInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			uint[] dst = new uint[size];

			//900
			Parallel.For(0, size, i =>
			{
				dst[i] = x[i] + y[i];
			});
			x.token.Token.ThrowIfCancellationRequested();


			//あまりきれいじゃないけど 以下みたいなのが落としどころ
			for(int j=0;j<2;j++)
			{
				Parallel.For(0, size, i =>
				{
					dst[i] = x[i] + y[i];
				});
				x.token.Token.ThrowIfCancellationRequested();
			}


            return new PixelUInt(dst, x.Width, y.Height, x.token);
		}

		#endregion



        /// <summary>
        /// 平均値を求める, 一画素でもDouble介することによる誤差有
        /// </summary>
        /// <returns></returns>
        public double Ave()
        {
            double ave = 0;

            /*
            int s = Size;
            for (int i = 0; i < Size; i++)
                ave += Convert.ToDouble(pixel[i]);
            return ave / Size;
            */

            //ave = pixel.Select<T, double>(i => Convert.ToDouble(i)).Average();

            foreach (uint i in pixel)
                ave += Convert.ToDouble(i);

            return ave / Size;
        }
	
	}


    /// <summary>
    /// 符号なし64ビット整数Pixelクラス
    /// </summary>
    public partial class PixelULong
    {
		public CancellationTokenSource token;
		protected ulong[] pixel;

		public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

		/// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
		public ulong[] ToArray() => pixel;

		public ulong this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

		public ulong this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }


		/* -------------------------------------- */


		public PixelULong(ulong[] buf, int Width, int Height, CancellationTokenSource tok)
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

        public PixelULong(ulong[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

		public PixelULong(int Width, int Height) : this(new ulong[Width * Height], Width, Height, new CancellationTokenSource()) { }


        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも



		/*--------------------------------------*/
		//   operator
		/*--------------------------------------*/
		#region operator 四則演算

        public unsafe static PixelULong operator +(PixelULong x, PixelULong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			ulong[] dst = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
			fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &dst[0])
            {
                ulong*	pt_x = p_x;
				ulong*	pt_y = p_y;
                ulong*	pt_ret = p_ret;

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
            return new PixelULong(dst, x.Width, y.Height, x.token);
		}

		public unsafe static PixelULong operator -(PixelULong x, PixelULong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			ulong[] dst = new ulong[size];

			//900
			Parallel.For(0, size, i =>
			{
				dst[i] = x[i] + y[i];
			});
			x.token.Token.ThrowIfCancellationRequested();


			//あまりきれいじゃないけど 以下みたいなのが落としどころ
			for(int j=0;j<2;j++)
			{
				Parallel.For(0, size, i =>
				{
					dst[i] = x[i] + y[i];
				});
				x.token.Token.ThrowIfCancellationRequested();
			}


            return new PixelULong(dst, x.Width, y.Height, x.token);
		}

		#endregion



        /// <summary>
        /// 平均値を求める, 一画素でもDouble介することによる誤差有
        /// </summary>
        /// <returns></returns>
        public double Ave()
        {
            double ave = 0;

            /*
            int s = Size;
            for (int i = 0; i < Size; i++)
                ave += Convert.ToDouble(pixel[i]);
            return ave / Size;
            */

            //ave = pixel.Select<T, double>(i => Convert.ToDouble(i)).Average();

            foreach (ulong i in pixel)
                ave += Convert.ToDouble(i);

            return ave / Size;
        }
	
	}


    /// <summary>
    /// 符号付き16ビット整数Pixelクラス
    /// </summary>
    public partial class PixelShort
    {
		public CancellationTokenSource token;
		protected short[] pixel;

		public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

		/// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
		public short[] ToArray() => pixel;

		public short this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

		public short this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }


		/* -------------------------------------- */


		public PixelShort(short[] buf, int Width, int Height, CancellationTokenSource tok)
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

        public PixelShort(short[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

		public PixelShort(int Width, int Height) : this(new short[Width * Height], Width, Height, new CancellationTokenSource()) { }


        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも



		/*--------------------------------------*/
		//   operator
		/*--------------------------------------*/
		#region operator 四則演算

        public unsafe static PixelInt operator +(PixelShort x, PixelShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] dst = new int[size];

            fixed (short* p_x = &x.pixel[0])
			fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &dst[0])
            {
                short*	pt_x = p_x;
				short*	pt_y = p_y;
                int*	pt_ret = p_ret;

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

		public unsafe static PixelInt operator -(PixelShort x, PixelShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] dst = new int[size];

			//900
			Parallel.For(0, size, i =>
			{
				dst[i] = x[i] + y[i];
			});
			x.token.Token.ThrowIfCancellationRequested();


			//あまりきれいじゃないけど 以下みたいなのが落としどころ
			for(int j=0;j<2;j++)
			{
				Parallel.For(0, size, i =>
				{
					dst[i] = x[i] + y[i];
				});
				x.token.Token.ThrowIfCancellationRequested();
			}


            return new PixelInt(dst, x.Width, y.Height, x.token);
		}

		#endregion



        /// <summary>
        /// 平均値を求める, 一画素でもDouble介することによる誤差有
        /// </summary>
        /// <returns></returns>
        public double Ave()
        {
            double ave = 0;

            /*
            int s = Size;
            for (int i = 0; i < Size; i++)
                ave += Convert.ToDouble(pixel[i]);
            return ave / Size;
            */

            //ave = pixel.Select<T, double>(i => Convert.ToDouble(i)).Average();

            foreach (short i in pixel)
                ave += Convert.ToDouble(i);

            return ave / Size;
        }
	
	}


    /// <summary>
    /// 符号付き32ビット整数Pixelクラス
    /// </summary>
    public partial class PixelInt
    {
		public CancellationTokenSource token;
		protected int[] pixel;

		public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

		/// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
		public int[] ToArray() => pixel;

		public int this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

		public int this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }


		/* -------------------------------------- */


		public PixelInt(int[] buf, int Width, int Height, CancellationTokenSource tok)
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

        public PixelInt(int[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

		public PixelInt(int Width, int Height) : this(new int[Width * Height], Width, Height, new CancellationTokenSource()) { }


        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも



		/*--------------------------------------*/
		//   operator
		/*--------------------------------------*/
		#region operator 四則演算

        public unsafe static PixelInt operator +(PixelInt x, PixelInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] dst = new int[size];

            fixed (int* p_x = &x.pixel[0])
			fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &dst[0])
            {
                int*	pt_x = p_x;
				int*	pt_y = p_y;
                int*	pt_ret = p_ret;

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

		public unsafe static PixelInt operator -(PixelInt x, PixelInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] dst = new int[size];

			//900
			Parallel.For(0, size, i =>
			{
				dst[i] = x[i] + y[i];
			});
			x.token.Token.ThrowIfCancellationRequested();


			//あまりきれいじゃないけど 以下みたいなのが落としどころ
			for(int j=0;j<2;j++)
			{
				Parallel.For(0, size, i =>
				{
					dst[i] = x[i] + y[i];
				});
				x.token.Token.ThrowIfCancellationRequested();
			}


            return new PixelInt(dst, x.Width, y.Height, x.token);
		}

		#endregion



        /// <summary>
        /// 平均値を求める, 一画素でもDouble介することによる誤差有
        /// </summary>
        /// <returns></returns>
        public double Ave()
        {
            double ave = 0;

            /*
            int s = Size;
            for (int i = 0; i < Size; i++)
                ave += Convert.ToDouble(pixel[i]);
            return ave / Size;
            */

            //ave = pixel.Select<T, double>(i => Convert.ToDouble(i)).Average();

            foreach (int i in pixel)
                ave += Convert.ToDouble(i);

            return ave / Size;
        }
	
	}


    /// <summary>
    /// 符号付き64ビット整数Pixelクラス
    /// </summary>
    public partial class PixelLong
    {
		public CancellationTokenSource token;
		protected long[] pixel;

		public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

		/// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
		public long[] ToArray() => pixel;

		public long this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

		public long this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }


		/* -------------------------------------- */


		public PixelLong(long[] buf, int Width, int Height, CancellationTokenSource tok)
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

        public PixelLong(long[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

		public PixelLong(int Width, int Height) : this(new long[Width * Height], Width, Height, new CancellationTokenSource()) { }


        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも



		/*--------------------------------------*/
		//   operator
		/*--------------------------------------*/
		#region operator 四則演算

        public unsafe static PixelLong operator +(PixelLong x, PixelLong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			long[] dst = new long[size];

            fixed (long* p_x = &x.pixel[0])
			fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &dst[0])
            {
                long*	pt_x = p_x;
				long*	pt_y = p_y;
                long*	pt_ret = p_ret;

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
            return new PixelLong(dst, x.Width, y.Height, x.token);
		}

		public unsafe static PixelLong operator -(PixelLong x, PixelLong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			long[] dst = new long[size];

			//900
			Parallel.For(0, size, i =>
			{
				dst[i] = x[i] + y[i];
			});
			x.token.Token.ThrowIfCancellationRequested();


			//あまりきれいじゃないけど 以下みたいなのが落としどころ
			for(int j=0;j<2;j++)
			{
				Parallel.For(0, size, i =>
				{
					dst[i] = x[i] + y[i];
				});
				x.token.Token.ThrowIfCancellationRequested();
			}


            return new PixelLong(dst, x.Width, y.Height, x.token);
		}

		#endregion



        /// <summary>
        /// 平均値を求める, 一画素でもDouble介することによる誤差有
        /// </summary>
        /// <returns></returns>
        public double Ave()
        {
            double ave = 0;

            /*
            int s = Size;
            for (int i = 0; i < Size; i++)
                ave += Convert.ToDouble(pixel[i]);
            return ave / Size;
            */

            //ave = pixel.Select<T, double>(i => Convert.ToDouble(i)).Average();

            foreach (long i in pixel)
                ave += Convert.ToDouble(i);

            return ave / Size;
        }
	
	}


    /// <summary>
    /// 単精度(32ビット)浮動小数点Pixelクラス
    /// </summary>
    public partial class PixelFloat
    {
		public CancellationTokenSource token;
		protected float[] pixel;

		public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

		/// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
		public float[] ToArray() => pixel;

		public float this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

		public float this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }


		/* -------------------------------------- */


		public PixelFloat(float[] buf, int Width, int Height, CancellationTokenSource tok)
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

        public PixelFloat(float[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

		public PixelFloat(int Width, int Height) : this(new float[Width * Height], Width, Height, new CancellationTokenSource()) { }


        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも



		/*--------------------------------------*/
		//   operator
		/*--------------------------------------*/
		#region operator 四則演算

        public unsafe static PixelFloat operator +(PixelFloat x, PixelFloat y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			float[] dst = new float[size];

            fixed (float* p_x = &x.pixel[0])
			fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &dst[0])
            {
                float*	pt_x = p_x;
				float*	pt_y = p_y;
                float*	pt_ret = p_ret;

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
            return new PixelFloat(dst, x.Width, y.Height, x.token);
		}

		public unsafe static PixelFloat operator -(PixelFloat x, PixelFloat y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			float[] dst = new float[size];

			//900
			Parallel.For(0, size, i =>
			{
				dst[i] = x[i] + y[i];
			});
			x.token.Token.ThrowIfCancellationRequested();


			//あまりきれいじゃないけど 以下みたいなのが落としどころ
			for(int j=0;j<2;j++)
			{
				Parallel.For(0, size, i =>
				{
					dst[i] = x[i] + y[i];
				});
				x.token.Token.ThrowIfCancellationRequested();
			}


            return new PixelFloat(dst, x.Width, y.Height, x.token);
		}

		#endregion



        /// <summary>
        /// 平均値を求める, 一画素でもDouble介することによる誤差有
        /// </summary>
        /// <returns></returns>
        public double Ave()
        {
            double ave = 0;

            /*
            int s = Size;
            for (int i = 0; i < Size; i++)
                ave += Convert.ToDouble(pixel[i]);
            return ave / Size;
            */

            //ave = pixel.Select<T, double>(i => Convert.ToDouble(i)).Average();

            foreach (float i in pixel)
                ave += Convert.ToDouble(i);

            return ave / Size;
        }
	
	}


    /// <summary>
    /// 単精度(64ビット)浮動小数点Pixelクラス
    /// </summary>
    public partial class PixelDouble
    {
		public CancellationTokenSource token;
		protected double[] pixel;

		public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

		/// <summary>
        /// 一次元画素配列を取得する
        /// </summary>
        /// <returns></returns>
		public double[] ToArray() => pixel;

		public double this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

		public double this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }


		/* -------------------------------------- */


		public PixelDouble(double[] buf, int Width, int Height, CancellationTokenSource tok)
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

        public PixelDouble(double[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource()) { }

		public PixelDouble(int Width, int Height) : this(new double[Width * Height], Width, Height, new CancellationTokenSource()) { }


        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも



		/*--------------------------------------*/
		//   operator
		/*--------------------------------------*/
		#region operator 四則演算

        public unsafe static PixelDouble operator +(PixelDouble x, PixelDouble y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			double[] dst = new double[size];

            fixed (double* p_x = &x.pixel[0])
			fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &dst[0])
            {
                double*	pt_x = p_x;
				double*	pt_y = p_y;
                double*	pt_ret = p_ret;

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
            return new PixelDouble(dst, x.Width, y.Height, x.token);
		}

		public unsafe static PixelDouble operator -(PixelDouble x, PixelDouble y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			double[] dst = new double[size];

			//900
			Parallel.For(0, size, i =>
			{
				dst[i] = x[i] + y[i];
			});
			x.token.Token.ThrowIfCancellationRequested();


			//あまりきれいじゃないけど 以下みたいなのが落としどころ
			for(int j=0;j<2;j++)
			{
				Parallel.For(0, size, i =>
				{
					dst[i] = x[i] + y[i];
				});
				x.token.Token.ThrowIfCancellationRequested();
			}


            return new PixelDouble(dst, x.Width, y.Height, x.token);
		}

		#endregion



        /// <summary>
        /// 平均値を求める, 一画素でもDouble介することによる誤差有
        /// </summary>
        /// <returns></returns>
        public double Ave()
        {
            double ave = 0;

            /*
            int s = Size;
            for (int i = 0; i < Size; i++)
                ave += Convert.ToDouble(pixel[i]);
            return ave / Size;
            */

            //ave = pixel.Select<T, double>(i => Convert.ToDouble(i)).Average();

            foreach (double i in pixel)
                ave += Convert.ToDouble(i);

            return ave / Size;
        }
	
	}

}