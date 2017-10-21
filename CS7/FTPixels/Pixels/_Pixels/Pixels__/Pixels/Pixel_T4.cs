using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

using System.IO.Compression;
using System.Threading.Tasks;


/*******************************************************************************/
/*   Pixels                                                                    */
/*   2015/09/07                                                                */
/*******************************************************************************/
namespace Pixels
{
    /*--------------------------------------*/
    //   コンストラクタ/プロパティ・インデクサ 
    /*--------------------------------------*/
	#region コンストラクタ
    /// <summary>
    /// 符号なし8ビット整数Pixelクラス
    /// </summary>
    public partial class PixelByte : PixelBase<byte>
    {
		protected PixelByte(int Width, int Height) : base(Width, Height) { }
		public PixelByte(byte[] buf, int Width, int Height) : base(buf, Width, Height) { }
		public PixelByte(byte[] buf, int Width, int Height, CancellationTokenSource tok) : base(buf, Width, Height, tok) { }
		public int bytesize{ get { return sizeof(byte); }  }
	}
    /// <summary>
    /// 符号なし16ビット整数Pixelクラス
    /// </summary>
    public partial class PixelUShort : PixelBase<ushort>
    {
		protected PixelUShort(int Width, int Height) : base(Width, Height) { }
		public PixelUShort(ushort[] buf, int Width, int Height) : base(buf, Width, Height) { }
		public PixelUShort(ushort[] buf, int Width, int Height, CancellationTokenSource tok) : base(buf, Width, Height, tok) { }
		public int bytesize{ get { return sizeof(ushort); }  }
	}
    /// <summary>
    /// 符号なし32ビット整数Pixelクラス
    /// </summary>
    public partial class PixelUInt : PixelBase<uint>
    {
		protected PixelUInt(int Width, int Height) : base(Width, Height) { }
		public PixelUInt(uint[] buf, int Width, int Height) : base(buf, Width, Height) { }
		public PixelUInt(uint[] buf, int Width, int Height, CancellationTokenSource tok) : base(buf, Width, Height, tok) { }
		public int bytesize{ get { return sizeof(uint); }  }
	}
    /// <summary>
    /// 符号なし64ビット整数Pixelクラス
    /// </summary>
    public partial class PixelULong : PixelBase<ulong>
    {
		protected PixelULong(int Width, int Height) : base(Width, Height) { }
		public PixelULong(ulong[] buf, int Width, int Height) : base(buf, Width, Height) { }
		public PixelULong(ulong[] buf, int Width, int Height, CancellationTokenSource tok) : base(buf, Width, Height, tok) { }
		public int bytesize{ get { return sizeof(ulong); }  }
	}
    /// <summary>
    /// 符号付き16ビット整数Pixelクラス
    /// </summary>
    public partial class PixelShort : PixelBase<short>
    {
		protected PixelShort(int Width, int Height) : base(Width, Height) { }
		public PixelShort(short[] buf, int Width, int Height) : base(buf, Width, Height) { }
		public PixelShort(short[] buf, int Width, int Height, CancellationTokenSource tok) : base(buf, Width, Height, tok) { }
		public int bytesize{ get { return sizeof(short); }  }
	}
    /// <summary>
    /// 符号付き32ビット整数Pixelクラス
    /// </summary>
    public partial class PixelInt : PixelBase<int>
    {
		protected PixelInt(int Width, int Height) : base(Width, Height) { }
		public PixelInt(int[] buf, int Width, int Height) : base(buf, Width, Height) { }
		public PixelInt(int[] buf, int Width, int Height, CancellationTokenSource tok) : base(buf, Width, Height, tok) { }
		public int bytesize{ get { return sizeof(int); }  }
	}
    /// <summary>
    /// 符号付き64ビット整数Pixelクラス
    /// </summary>
    public partial class PixelLong : PixelBase<long>
    {
		protected PixelLong(int Width, int Height) : base(Width, Height) { }
		public PixelLong(long[] buf, int Width, int Height) : base(buf, Width, Height) { }
		public PixelLong(long[] buf, int Width, int Height, CancellationTokenSource tok) : base(buf, Width, Height, tok) { }
		public int bytesize{ get { return sizeof(long); }  }
	}
    /// <summary>
    /// 単精度(32ビット)浮動小数点Pixelクラス
    /// </summary>
    public partial class PixelFloat : PixelBase<float>
    {
		protected PixelFloat(int Width, int Height) : base(Width, Height) { }
		public PixelFloat(float[] buf, int Width, int Height) : base(buf, Width, Height) { }
		public PixelFloat(float[] buf, int Width, int Height, CancellationTokenSource tok) : base(buf, Width, Height, tok) { }
		public int bytesize{ get { return sizeof(float); }  }
	}
    /// <summary>
    /// 単精度(64ビット)浮動小数点Pixelクラス
    /// </summary>
    public partial class PixelDouble : PixelBase<double>
    {
		protected PixelDouble(int Width, int Height) : base(Width, Height) { }
		public PixelDouble(double[] buf, int Width, int Height) : base(buf, Width, Height) { }
		public PixelDouble(double[] buf, int Width, int Height, CancellationTokenSource tok) : base(buf, Width, Height, tok) { }
		public int bytesize{ get { return sizeof(double); }  }
	}
    /// <summary>
    /// Pixelクラス拡張メソッド
    /// </summary>
    public static class PixelExtensions
    {
		public static PixelByte Create(this byte[] buf, int w, int h, CancellationTokenSource token) { return new PixelByte(buf, w, h, token); }
		public static PixelUShort Create(this ushort[] buf, int w, int h, CancellationTokenSource token) { return new PixelUShort(buf, w, h, token); }
		public static PixelUInt Create(this uint[] buf, int w, int h, CancellationTokenSource token) { return new PixelUInt(buf, w, h, token); }
		public static PixelULong Create(this ulong[] buf, int w, int h, CancellationTokenSource token) { return new PixelULong(buf, w, h, token); }
		public static PixelShort Create(this short[] buf, int w, int h, CancellationTokenSource token) { return new PixelShort(buf, w, h, token); }
		public static PixelInt Create(this int[] buf, int w, int h, CancellationTokenSource token) { return new PixelInt(buf, w, h, token); }
		public static PixelLong Create(this long[] buf, int w, int h, CancellationTokenSource token) { return new PixelLong(buf, w, h, token); }
		public static PixelFloat Create(this float[] buf, int w, int h, CancellationTokenSource token) { return new PixelFloat(buf, w, h, token); }
		public static PixelDouble Create(this double[] buf, int w, int h, CancellationTokenSource token) { return new PixelDouble(buf, w, h, token); }

		public static PixelByte Create(this PArray<byte> buf, CancellationTokenSource token){ return new PixelByte(buf.p, buf.w, buf.h, token);}
		public static PixelUShort Create(this PArray<ushort> buf, CancellationTokenSource token){ return new PixelUShort(buf.p, buf.w, buf.h, token);}
		public static PixelUInt Create(this PArray<uint> buf, CancellationTokenSource token){ return new PixelUInt(buf.p, buf.w, buf.h, token);}
		public static PixelULong Create(this PArray<ulong> buf, CancellationTokenSource token){ return new PixelULong(buf.p, buf.w, buf.h, token);}
		public static PixelShort Create(this PArray<short> buf, CancellationTokenSource token){ return new PixelShort(buf.p, buf.w, buf.h, token);}
		public static PixelInt Create(this PArray<int> buf, CancellationTokenSource token){ return new PixelInt(buf.p, buf.w, buf.h, token);}
		public static PixelLong Create(this PArray<long> buf, CancellationTokenSource token){ return new PixelLong(buf.p, buf.w, buf.h, token);}
		public static PixelFloat Create(this PArray<float> buf, CancellationTokenSource token){ return new PixelFloat(buf.p, buf.w, buf.h, token);}
		public static PixelDouble Create(this PArray<double> buf, CancellationTokenSource token){ return new PixelDouble(buf.p, buf.w, buf.h, token);}
	}
	#endregion
    /*--------------------------------------*/
    //   operator
    /*--------------------------------------*/
	#region operator
    public partial class PixelByte
    {
		#region operator 四則演算
        public unsafe static PixelInt operator +(PixelByte x, PixelByte y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
			fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte*	pt_x = p_x;
				byte*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator -(PixelByte x, PixelByte y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
			fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte*	pt_x = p_x;
				byte*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator *(PixelByte x, PixelByte y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
			fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte*	pt_x = p_x;
				byte*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator /(PixelByte x, PixelByte y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
			fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte*	pt_x = p_x;
				byte*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator +(PixelByte x, byte y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
        public unsafe static PixelInt operator -(PixelByte x, byte y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator *(PixelByte x, byte y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator /(PixelByte x, byte y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator +(byte x, PixelByte y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x + *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator -(byte x, PixelByte y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x - *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator *(byte x, PixelByte y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x * *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator /(byte x, PixelByte y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x / *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public static PixelByte operator +(PixelByte x) => x;
        public unsafe static PixelInt operator -(PixelByte x)
		{
			int size = x.Size;
			int[] ret = new int[size];
		
            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;
		
                for (int i = 0; i < size; ++i)
                {
                    *pt_ret++ = *pt_x++ * -1;
        			x.token.Token.ThrowIfCancellationRequested();
                }
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		#endregion
		#region operator キャスト
		//public static explicit operator PixelByte(PixelByte x) => x.ToPixelByte();
		public static implicit operator PixelUShort(PixelByte x) => x.ToPixelUShort();
		public static implicit operator PixelUInt(PixelByte x) => x.ToPixelUInt();
		public static implicit operator PixelULong(PixelByte x) => x.ToPixelULong();
		public static implicit operator PixelShort(PixelByte x) => x.ToPixelShort();
		public static implicit operator PixelInt(PixelByte x) => x.ToPixelInt();
		public static implicit operator PixelLong(PixelByte x) => x.ToPixelLong();
		public static implicit operator PixelFloat(PixelByte x) => x.ToPixelFloat();
		public static implicit operator PixelDouble(PixelByte x) => x.ToPixelDouble();
		/// <summary>
        /// 符号なし8ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelByte ToPixelByte()
		{
			int size = this.Size;
			byte[] ret = new byte[size];

            fixed (byte* p_x = &this.pixel[0])
            fixed (byte* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                byte* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (byte)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUShort ToPixelUShort()
		{
			int size = this.Size;
			ushort[] ret = new ushort[size];

            fixed (byte* p_x = &this.pixel[0])
            fixed (ushort* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                ushort* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ushort)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUInt ToPixelUInt()
		{
			int size = this.Size;
			uint[] ret = new uint[size];

            fixed (byte* p_x = &this.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (uint)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelULong ToPixelULong()
		{
			int size = this.Size;
			ulong[] ret = new ulong[size];

            fixed (byte* p_x = &this.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ulong)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelShort ToPixelShort()
		{
			int size = this.Size;
			short[] ret = new short[size];

            fixed (byte* p_x = &this.pixel[0])
            fixed (short* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                short* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (short)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelInt ToPixelInt()
		{
			int size = this.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &this.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (int)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelLong ToPixelLong()
		{
			int size = this.Size;
			long[] ret = new long[size];

            fixed (byte* p_x = &this.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (long)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(32ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelFloat ToPixelFloat()
		{
			int size = this.Size;
			float[] ret = new float[size];

            fixed (byte* p_x = &this.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (float)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(64ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelDouble ToPixelDouble()
		{
			int size = this.Size;
			double[] ret = new double[size];

            fixed (byte* p_x = &this.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (double)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		#endregion
		#region operator ビット演算
		
		public unsafe static PixelInt operator <<(PixelByte x, int y)
		{
			//論理シフト（最下位0埋め
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ << y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator >>(PixelByte x, int y)
		{
			//算術シフト（符号ビットで埋める）符号無しなら論理シフト
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ >> y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator &(PixelByte x, PixelByte y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
			fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte*	pt_x = p_x;
				byte*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator ^(PixelByte x, PixelByte y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
			fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte*	pt_x = p_x;
				byte*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator |(PixelByte x, PixelByte y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
			fixed (byte* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte*	pt_x = p_x;
				byte*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator &(PixelByte x, byte y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator ^(PixelByte x, byte y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator |(PixelByte x, byte y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (byte* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                byte* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public static PixelInt operator &(byte x, PixelByte y) => y & x;
        public static PixelInt operator ^(byte x, PixelByte y) => y ^ x;
        public static PixelInt operator |(byte x, PixelByte y) => y | x;
		
		#endregion
	}
    public partial class PixelUShort
    {
		#region operator 四則演算
        public unsafe static PixelInt operator +(PixelUShort x, PixelUShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
			fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort*	pt_x = p_x;
				ushort*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator -(PixelUShort x, PixelUShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
			fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort*	pt_x = p_x;
				ushort*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator *(PixelUShort x, PixelUShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
			fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort*	pt_x = p_x;
				ushort*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator /(PixelUShort x, PixelUShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
			fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort*	pt_x = p_x;
				ushort*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator +(PixelUShort x, ushort y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
        public unsafe static PixelInt operator -(PixelUShort x, ushort y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator *(PixelUShort x, ushort y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator /(PixelUShort x, ushort y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator +(ushort x, PixelUShort y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x + *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator -(ushort x, PixelUShort y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x - *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator *(ushort x, PixelUShort y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x * *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator /(ushort x, PixelUShort y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x / *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public static PixelUShort operator +(PixelUShort x) => x;
        public unsafe static PixelInt operator -(PixelUShort x)
		{
			int size = x.Size;
			int[] ret = new int[size];
		
            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;
		
                for (int i = 0; i < size; ++i)
                {
                    *pt_ret++ = *pt_x++ * -1;
        			x.token.Token.ThrowIfCancellationRequested();
                }
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		#endregion
		#region operator キャスト
		public static explicit operator PixelByte(PixelUShort x) => x.ToPixelByte();
		//public static explicit operator PixelUShort(PixelUShort x) => x.ToPixelUShort();
		public static explicit operator PixelUInt(PixelUShort x) => x.ToPixelUInt();
		public static explicit operator PixelULong(PixelUShort x) => x.ToPixelULong();
		public static explicit operator PixelShort(PixelUShort x) => x.ToPixelShort();
		public static explicit operator PixelInt(PixelUShort x) => x.ToPixelInt();
		public static explicit operator PixelLong(PixelUShort x) => x.ToPixelLong();
		public static explicit operator PixelFloat(PixelUShort x) => x.ToPixelFloat();
		public static explicit operator PixelDouble(PixelUShort x) => x.ToPixelDouble();
		/// <summary>
        /// 符号なし8ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelByte ToPixelByte()
		{
			int size = this.Size;
			byte[] ret = new byte[size];

            fixed (ushort* p_x = &this.pixel[0])
            fixed (byte* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                byte* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (byte)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUShort ToPixelUShort()
		{
			int size = this.Size;
			ushort[] ret = new ushort[size];

            fixed (ushort* p_x = &this.pixel[0])
            fixed (ushort* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                ushort* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ushort)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUInt ToPixelUInt()
		{
			int size = this.Size;
			uint[] ret = new uint[size];

            fixed (ushort* p_x = &this.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (uint)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelULong ToPixelULong()
		{
			int size = this.Size;
			ulong[] ret = new ulong[size];

            fixed (ushort* p_x = &this.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ulong)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelShort ToPixelShort()
		{
			int size = this.Size;
			short[] ret = new short[size];

            fixed (ushort* p_x = &this.pixel[0])
            fixed (short* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                short* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (short)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelInt ToPixelInt()
		{
			int size = this.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &this.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (int)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelLong ToPixelLong()
		{
			int size = this.Size;
			long[] ret = new long[size];

            fixed (ushort* p_x = &this.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (long)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(32ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelFloat ToPixelFloat()
		{
			int size = this.Size;
			float[] ret = new float[size];

            fixed (ushort* p_x = &this.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (float)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(64ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelDouble ToPixelDouble()
		{
			int size = this.Size;
			double[] ret = new double[size];

            fixed (ushort* p_x = &this.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (double)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		#endregion
		#region operator ビット演算
		
		public unsafe static PixelInt operator <<(PixelUShort x, int y)
		{
			//論理シフト（最下位0埋め
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ << y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator >>(PixelUShort x, int y)
		{
			//算術シフト（符号ビットで埋める）符号無しなら論理シフト
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ >> y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator &(PixelUShort x, PixelUShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
			fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort*	pt_x = p_x;
				ushort*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator ^(PixelUShort x, PixelUShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
			fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort*	pt_x = p_x;
				ushort*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator |(PixelUShort x, PixelUShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
			fixed (ushort* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort*	pt_x = p_x;
				ushort*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator &(PixelUShort x, ushort y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator ^(PixelUShort x, ushort y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator |(PixelUShort x, ushort y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (ushort* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ushort* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public static PixelInt operator &(ushort x, PixelUShort y) => y & x;
        public static PixelInt operator ^(ushort x, PixelUShort y) => y ^ x;
        public static PixelInt operator |(ushort x, PixelUShort y) => y | x;
		
		#endregion
	}
    public partial class PixelUInt
    {
		#region operator 四則演算
        public unsafe static PixelUInt operator +(PixelUInt x, PixelUInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
			fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint*	pt_x = p_x;
				uint*	pt_y = p_y;
                uint*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelUInt operator -(PixelUInt x, PixelUInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
			fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint*	pt_x = p_x;
				uint*	pt_y = p_y;
                uint*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelUInt operator *(PixelUInt x, PixelUInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
			fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint*	pt_x = p_x;
				uint*	pt_y = p_y;
                uint*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelUInt operator /(PixelUInt x, PixelUInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
			fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint*	pt_x = p_x;
				uint*	pt_y = p_y;
                uint*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelUInt operator +(PixelUInt x, uint y)
		{
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
        public unsafe static PixelUInt operator -(PixelUInt x, uint y)
		{
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelUInt operator *(PixelUInt x, uint y)
		{
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelUInt operator /(PixelUInt x, uint y)
		{
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelUInt operator +(uint x, PixelUInt y)
		{
			int size = y.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_y = p_y;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x + *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelUInt operator -(uint x, PixelUInt y)
		{
			int size = y.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_y = p_y;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x - *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelUInt operator *(uint x, PixelUInt y)
		{
			int size = y.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_y = p_y;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x * *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelUInt operator /(uint x, PixelUInt y)
		{
			int size = y.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_y = p_y;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x / *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public static PixelUInt operator +(PixelUInt x) => x;
        public unsafe static PixelLong operator -(PixelUInt x)
		{
			int size = x.Size;
			long[] ret = new long[size];
		
            fixed (uint* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                long* pt_ret = p_ret;
		
                for (int i = 0; i < size; ++i)
                {
                    *pt_ret++ = *pt_x++ * -1;
        			x.token.Token.ThrowIfCancellationRequested();
                }
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		#endregion
		#region operator キャスト
		public static explicit operator PixelByte(PixelUInt x) => x.ToPixelByte();
		public static explicit operator PixelUShort(PixelUInt x) => x.ToPixelUShort();
		//public static explicit operator PixelUInt(PixelUInt x) => x.ToPixelUInt();
		public static implicit operator PixelULong(PixelUInt x) => x.ToPixelULong();
		public static explicit operator PixelShort(PixelUInt x) => x.ToPixelShort();
		public static explicit operator PixelInt(PixelUInt x) => x.ToPixelInt();
		public static implicit operator PixelLong(PixelUInt x) => x.ToPixelLong();
		public static implicit operator PixelFloat(PixelUInt x) => x.ToPixelFloat();
		public static implicit operator PixelDouble(PixelUInt x) => x.ToPixelDouble();
		/// <summary>
        /// 符号なし8ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelByte ToPixelByte()
		{
			int size = this.Size;
			byte[] ret = new byte[size];

            fixed (uint* p_x = &this.pixel[0])
            fixed (byte* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                byte* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (byte)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUShort ToPixelUShort()
		{
			int size = this.Size;
			ushort[] ret = new ushort[size];

            fixed (uint* p_x = &this.pixel[0])
            fixed (ushort* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                ushort* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ushort)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUInt ToPixelUInt()
		{
			int size = this.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &this.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (uint)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelULong ToPixelULong()
		{
			int size = this.Size;
			ulong[] ret = new ulong[size];

            fixed (uint* p_x = &this.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ulong)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelShort ToPixelShort()
		{
			int size = this.Size;
			short[] ret = new short[size];

            fixed (uint* p_x = &this.pixel[0])
            fixed (short* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                short* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (short)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelInt ToPixelInt()
		{
			int size = this.Size;
			int[] ret = new int[size];

            fixed (uint* p_x = &this.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (int)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelLong ToPixelLong()
		{
			int size = this.Size;
			long[] ret = new long[size];

            fixed (uint* p_x = &this.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (long)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(32ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelFloat ToPixelFloat()
		{
			int size = this.Size;
			float[] ret = new float[size];

            fixed (uint* p_x = &this.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (float)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(64ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelDouble ToPixelDouble()
		{
			int size = this.Size;
			double[] ret = new double[size];

            fixed (uint* p_x = &this.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (double)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		#endregion
		#region operator ビット演算
		
		public unsafe static PixelUInt operator <<(PixelUInt x, int y)
		{
			//論理シフト（最下位0埋め
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ << y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelUInt operator >>(PixelUInt x, int y)
		{
			//算術シフト（符号ビットで埋める）符号無しなら論理シフト
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ >> y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelUInt operator &(PixelUInt x, PixelUInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
			fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint*	pt_x = p_x;
				uint*	pt_y = p_y;
                uint*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelUInt operator ^(PixelUInt x, PixelUInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
			fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint*	pt_x = p_x;
				uint*	pt_y = p_y;
                uint*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelUInt operator |(PixelUInt x, PixelUInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
			fixed (uint* p_y = &y.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint*	pt_x = p_x;
				uint*	pt_y = p_y;
                uint*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelUInt operator &(PixelUInt x, uint y)
		{
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelUInt operator ^(PixelUInt x, uint y)
		{
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelUInt operator |(PixelUInt x, uint y)
		{
			int size = x.Size;
			uint[] ret = new uint[size];

            fixed (uint* p_x = &x.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                uint* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public static PixelUInt operator &(uint x, PixelUInt y) => y & x;
        public static PixelUInt operator ^(uint x, PixelUInt y) => y ^ x;
        public static PixelUInt operator |(uint x, PixelUInt y) => y | x;
		
		#endregion
	}
    public partial class PixelULong
    {
		#region operator 四則演算
        public unsafe static PixelULong operator +(PixelULong x, PixelULong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
			fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong*	pt_x = p_x;
				ulong*	pt_y = p_y;
                ulong*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelULong operator -(PixelULong x, PixelULong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
			fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong*	pt_x = p_x;
				ulong*	pt_y = p_y;
                ulong*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelULong operator *(PixelULong x, PixelULong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
			fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong*	pt_x = p_x;
				ulong*	pt_y = p_y;
                ulong*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelULong operator /(PixelULong x, PixelULong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
			fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong*	pt_x = p_x;
				ulong*	pt_y = p_y;
                ulong*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelULong operator +(PixelULong x, ulong y)
		{
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
        public unsafe static PixelULong operator -(PixelULong x, ulong y)
		{
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelULong operator *(PixelULong x, ulong y)
		{
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelULong operator /(PixelULong x, ulong y)
		{
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelULong operator +(ulong x, PixelULong y)
		{
			int size = y.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_y = p_y;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x + *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelULong operator -(ulong x, PixelULong y)
		{
			int size = y.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_y = p_y;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x - *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelULong operator *(ulong x, PixelULong y)
		{
			int size = y.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_y = p_y;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x * *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelULong operator /(ulong x, PixelULong y)
		{
			int size = y.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_y = p_y;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x / *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public static PixelULong operator +(PixelULong x) => x;
        //public unsafe static PixelULong operator -(PixelULong x)
		//{
		//	int size = x.Size;
		//	ulong[] ret = new ulong[size];
		//
        //    fixed (ulong* p_x = &x.pixel[0])
        //    fixed (ulong* p_ret = &ret[0])
        //    {
        //        ulong* pt_x = p_x;
        //        ulong* pt_ret = p_ret;
		//
        //        for (int i = 0; i < size; ++i)
        //        {
        //            *pt_ret++ = *pt_x++ * -1;
        //			x.token.Token.ThrowIfCancellationRequested();
        //        }
        //    }
        //    return ret.Create(x.Width, x.Height, x.token);
        //}
		#endregion
		#region operator キャスト
		public static explicit operator PixelByte(PixelULong x) => x.ToPixelByte();
		public static explicit operator PixelUShort(PixelULong x) => x.ToPixelUShort();
		public static explicit operator PixelUInt(PixelULong x) => x.ToPixelUInt();
		//public static explicit operator PixelULong(PixelULong x) => x.ToPixelULong();
		public static explicit operator PixelShort(PixelULong x) => x.ToPixelShort();
		public static explicit operator PixelInt(PixelULong x) => x.ToPixelInt();
		public static explicit operator PixelLong(PixelULong x) => x.ToPixelLong();
		public static implicit operator PixelFloat(PixelULong x) => x.ToPixelFloat();
		public static implicit operator PixelDouble(PixelULong x) => x.ToPixelDouble();
		/// <summary>
        /// 符号なし8ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelByte ToPixelByte()
		{
			int size = this.Size;
			byte[] ret = new byte[size];

            fixed (ulong* p_x = &this.pixel[0])
            fixed (byte* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                byte* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (byte)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUShort ToPixelUShort()
		{
			int size = this.Size;
			ushort[] ret = new ushort[size];

            fixed (ulong* p_x = &this.pixel[0])
            fixed (ushort* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ushort* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ushort)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUInt ToPixelUInt()
		{
			int size = this.Size;
			uint[] ret = new uint[size];

            fixed (ulong* p_x = &this.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (uint)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelULong ToPixelULong()
		{
			int size = this.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &this.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ulong)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelShort ToPixelShort()
		{
			int size = this.Size;
			short[] ret = new short[size];

            fixed (ulong* p_x = &this.pixel[0])
            fixed (short* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                short* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (short)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelInt ToPixelInt()
		{
			int size = this.Size;
			int[] ret = new int[size];

            fixed (ulong* p_x = &this.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (int)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelLong ToPixelLong()
		{
			int size = this.Size;
			long[] ret = new long[size];

            fixed (ulong* p_x = &this.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (long)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(32ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelFloat ToPixelFloat()
		{
			int size = this.Size;
			float[] ret = new float[size];

            fixed (ulong* p_x = &this.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (float)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(64ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelDouble ToPixelDouble()
		{
			int size = this.Size;
			double[] ret = new double[size];

            fixed (ulong* p_x = &this.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (double)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		#endregion
		#region operator ビット演算
		
		public unsafe static PixelULong operator <<(PixelULong x, int y)
		{
			//論理シフト（最下位0埋め
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ << y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelULong operator >>(PixelULong x, int y)
		{
			//算術シフト（符号ビットで埋める）符号無しなら論理シフト
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ >> y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelULong operator &(PixelULong x, PixelULong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
			fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong*	pt_x = p_x;
				ulong*	pt_y = p_y;
                ulong*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelULong operator ^(PixelULong x, PixelULong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
			fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong*	pt_x = p_x;
				ulong*	pt_y = p_y;
                ulong*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelULong operator |(PixelULong x, PixelULong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
			fixed (ulong* p_y = &y.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong*	pt_x = p_x;
				ulong*	pt_y = p_y;
                ulong*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelULong operator &(PixelULong x, ulong y)
		{
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelULong operator ^(PixelULong x, ulong y)
		{
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelULong operator |(PixelULong x, ulong y)
		{
			int size = x.Size;
			ulong[] ret = new ulong[size];

            fixed (ulong* p_x = &x.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                ulong* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public static PixelULong operator &(ulong x, PixelULong y) => y & x;
        public static PixelULong operator ^(ulong x, PixelULong y) => y ^ x;
        public static PixelULong operator |(ulong x, PixelULong y) => y | x;
		
		#endregion
	}
    public partial class PixelShort
    {
		#region operator 四則演算
        public unsafe static PixelInt operator +(PixelShort x, PixelShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
			fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short*	pt_x = p_x;
				short*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator -(PixelShort x, PixelShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
			fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short*	pt_x = p_x;
				short*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator *(PixelShort x, PixelShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
			fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short*	pt_x = p_x;
				short*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator /(PixelShort x, PixelShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
			fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short*	pt_x = p_x;
				short*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator +(PixelShort x, short y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
        public unsafe static PixelInt operator -(PixelShort x, short y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator *(PixelShort x, short y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator /(PixelShort x, short y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator +(short x, PixelShort y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x + *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator -(short x, PixelShort y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x - *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator *(short x, PixelShort y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x * *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator /(short x, PixelShort y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x / *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public static PixelShort operator +(PixelShort x) => x;
        public unsafe static PixelInt operator -(PixelShort x)
		{
			int size = x.Size;
			int[] ret = new int[size];
		
            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;
		
                for (int i = 0; i < size; ++i)
                {
                    *pt_ret++ = *pt_x++ * -1;
        			x.token.Token.ThrowIfCancellationRequested();
                }
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		#endregion
		#region operator キャスト
		public static explicit operator PixelByte(PixelShort x) => x.ToPixelByte();
		public static explicit operator PixelUShort(PixelShort x) => x.ToPixelUShort();
		public static explicit operator PixelUInt(PixelShort x) => x.ToPixelUInt();
		public static explicit operator PixelULong(PixelShort x) => x.ToPixelULong();
		//public static explicit operator PixelShort(PixelShort x) => x.ToPixelShort();
		public static implicit operator PixelInt(PixelShort x) => x.ToPixelInt();
		public static implicit operator PixelLong(PixelShort x) => x.ToPixelLong();
		public static implicit operator PixelFloat(PixelShort x) => x.ToPixelFloat();
		public static implicit operator PixelDouble(PixelShort x) => x.ToPixelDouble();
		/// <summary>
        /// 符号なし8ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelByte ToPixelByte()
		{
			int size = this.Size;
			byte[] ret = new byte[size];

            fixed (short* p_x = &this.pixel[0])
            fixed (byte* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                byte* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (byte)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUShort ToPixelUShort()
		{
			int size = this.Size;
			ushort[] ret = new ushort[size];

            fixed (short* p_x = &this.pixel[0])
            fixed (ushort* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                ushort* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ushort)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUInt ToPixelUInt()
		{
			int size = this.Size;
			uint[] ret = new uint[size];

            fixed (short* p_x = &this.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (uint)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelULong ToPixelULong()
		{
			int size = this.Size;
			ulong[] ret = new ulong[size];

            fixed (short* p_x = &this.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ulong)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelShort ToPixelShort()
		{
			int size = this.Size;
			short[] ret = new short[size];

            fixed (short* p_x = &this.pixel[0])
            fixed (short* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                short* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (short)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelInt ToPixelInt()
		{
			int size = this.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &this.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (int)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelLong ToPixelLong()
		{
			int size = this.Size;
			long[] ret = new long[size];

            fixed (short* p_x = &this.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (long)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(32ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelFloat ToPixelFloat()
		{
			int size = this.Size;
			float[] ret = new float[size];

            fixed (short* p_x = &this.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (float)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(64ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelDouble ToPixelDouble()
		{
			int size = this.Size;
			double[] ret = new double[size];

            fixed (short* p_x = &this.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (double)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		#endregion
		#region operator ビット演算
		
		public unsafe static PixelInt operator <<(PixelShort x, int y)
		{
			//論理シフト（最下位0埋め
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ << y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator >>(PixelShort x, int y)
		{
			//算術シフト（符号ビットで埋める）符号無しなら論理シフト
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ >> y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator &(PixelShort x, PixelShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
			fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short*	pt_x = p_x;
				short*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator ^(PixelShort x, PixelShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
			fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short*	pt_x = p_x;
				short*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator |(PixelShort x, PixelShort y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
			fixed (short* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short*	pt_x = p_x;
				short*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator &(PixelShort x, short y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator ^(PixelShort x, short y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator |(PixelShort x, short y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (short* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                short* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public static PixelInt operator &(short x, PixelShort y) => y & x;
        public static PixelInt operator ^(short x, PixelShort y) => y ^ x;
        public static PixelInt operator |(short x, PixelShort y) => y | x;
		
		#endregion
	}
    public partial class PixelInt
    {
		#region operator 四則演算
        public unsafe static PixelInt operator +(PixelInt x, PixelInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
			fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int*	pt_x = p_x;
				int*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator -(PixelInt x, PixelInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
			fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int*	pt_x = p_x;
				int*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator *(PixelInt x, PixelInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
			fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int*	pt_x = p_x;
				int*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelInt operator /(PixelInt x, PixelInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
			fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int*	pt_x = p_x;
				int*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator +(PixelInt x, int y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
        public unsafe static PixelInt operator -(PixelInt x, int y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator *(PixelInt x, int y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator /(PixelInt x, int y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelInt operator +(int x, PixelInt y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x + *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator -(int x, PixelInt y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x - *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator *(int x, PixelInt y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x * *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelInt operator /(int x, PixelInt y)
		{
			int size = y.Size;
			int[] ret = new int[size];

            fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_y = p_y;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x / *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public static PixelInt operator +(PixelInt x) => x;
        public unsafe static PixelInt operator -(PixelInt x)
		{
			int size = x.Size;
			int[] ret = new int[size];
		
            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;
		
                for (int i = 0; i < size; ++i)
                {
                    *pt_ret++ = *pt_x++ * -1;
        			x.token.Token.ThrowIfCancellationRequested();
                }
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		#endregion
		#region operator キャスト
		public static explicit operator PixelByte(PixelInt x) => x.ToPixelByte();
		public static explicit operator PixelUShort(PixelInt x) => x.ToPixelUShort();
		public static explicit operator PixelUInt(PixelInt x) => x.ToPixelUInt();
		public static explicit operator PixelULong(PixelInt x) => x.ToPixelULong();
		public static explicit operator PixelShort(PixelInt x) => x.ToPixelShort();
		//public static explicit operator PixelInt(PixelInt x) => x.ToPixelInt();
		public static implicit operator PixelLong(PixelInt x) => x.ToPixelLong();
		public static implicit operator PixelFloat(PixelInt x) => x.ToPixelFloat();
		public static implicit operator PixelDouble(PixelInt x) => x.ToPixelDouble();
		/// <summary>
        /// 符号なし8ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelByte ToPixelByte()
		{
			int size = this.Size;
			byte[] ret = new byte[size];

            fixed (int* p_x = &this.pixel[0])
            fixed (byte* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                byte* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (byte)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUShort ToPixelUShort()
		{
			int size = this.Size;
			ushort[] ret = new ushort[size];

            fixed (int* p_x = &this.pixel[0])
            fixed (ushort* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                ushort* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ushort)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUInt ToPixelUInt()
		{
			int size = this.Size;
			uint[] ret = new uint[size];

            fixed (int* p_x = &this.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (uint)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelULong ToPixelULong()
		{
			int size = this.Size;
			ulong[] ret = new ulong[size];

            fixed (int* p_x = &this.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ulong)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelShort ToPixelShort()
		{
			int size = this.Size;
			short[] ret = new short[size];

            fixed (int* p_x = &this.pixel[0])
            fixed (short* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                short* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (short)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelInt ToPixelInt()
		{
			int size = this.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &this.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (int)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelLong ToPixelLong()
		{
			int size = this.Size;
			long[] ret = new long[size];

            fixed (int* p_x = &this.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (long)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(32ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelFloat ToPixelFloat()
		{
			int size = this.Size;
			float[] ret = new float[size];

            fixed (int* p_x = &this.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (float)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(64ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelDouble ToPixelDouble()
		{
			int size = this.Size;
			double[] ret = new double[size];

            fixed (int* p_x = &this.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (double)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		#endregion
		#region operator ビット演算
		
		public unsafe static PixelInt operator <<(PixelInt x, int y)
		{
			//論理シフト（最下位0埋め
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ << y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator >>(PixelInt x, int y)
		{
			//算術シフト（符号ビットで埋める）符号無しなら論理シフト
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ >> y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator &(PixelInt x, PixelInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
			fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int*	pt_x = p_x;
				int*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator ^(PixelInt x, PixelInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
			fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int*	pt_x = p_x;
				int*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator |(PixelInt x, PixelInt y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
			fixed (int* p_y = &y.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int*	pt_x = p_x;
				int*	pt_y = p_y;
                int*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator &(PixelInt x, int y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator ^(PixelInt x, int y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelInt operator |(PixelInt x, int y)
		{
			int size = x.Size;
			int[] ret = new int[size];

            fixed (int* p_x = &x.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                int* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public static PixelInt operator &(int x, PixelInt y) => y & x;
        public static PixelInt operator ^(int x, PixelInt y) => y ^ x;
        public static PixelInt operator |(int x, PixelInt y) => y | x;
		
		#endregion
	}
    public partial class PixelLong
    {
		#region operator 四則演算
        public unsafe static PixelLong operator +(PixelLong x, PixelLong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
			fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long*	pt_x = p_x;
				long*	pt_y = p_y;
                long*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelLong operator -(PixelLong x, PixelLong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
			fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long*	pt_x = p_x;
				long*	pt_y = p_y;
                long*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelLong operator *(PixelLong x, PixelLong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
			fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long*	pt_x = p_x;
				long*	pt_y = p_y;
                long*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelLong operator /(PixelLong x, PixelLong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
			fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long*	pt_x = p_x;
				long*	pt_y = p_y;
                long*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelLong operator +(PixelLong x, long y)
		{
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
        public unsafe static PixelLong operator -(PixelLong x, long y)
		{
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelLong operator *(PixelLong x, long y)
		{
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelLong operator /(PixelLong x, long y)
		{
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelLong operator +(long x, PixelLong y)
		{
			int size = y.Size;
			long[] ret = new long[size];

            fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_y = p_y;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x + *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelLong operator -(long x, PixelLong y)
		{
			int size = y.Size;
			long[] ret = new long[size];

            fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_y = p_y;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x - *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelLong operator *(long x, PixelLong y)
		{
			int size = y.Size;
			long[] ret = new long[size];

            fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_y = p_y;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x * *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelLong operator /(long x, PixelLong y)
		{
			int size = y.Size;
			long[] ret = new long[size];

            fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_y = p_y;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x / *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public static PixelLong operator +(PixelLong x) => x;
        public unsafe static PixelLong operator -(PixelLong x)
		{
			int size = x.Size;
			long[] ret = new long[size];
		
            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;
		
                for (int i = 0; i < size; ++i)
                {
                    *pt_ret++ = *pt_x++ * -1;
        			x.token.Token.ThrowIfCancellationRequested();
                }
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		#endregion
		#region operator キャスト
		public static explicit operator PixelByte(PixelLong x) => x.ToPixelByte();
		public static explicit operator PixelUShort(PixelLong x) => x.ToPixelUShort();
		public static explicit operator PixelUInt(PixelLong x) => x.ToPixelUInt();
		public static explicit operator PixelULong(PixelLong x) => x.ToPixelULong();
		public static explicit operator PixelShort(PixelLong x) => x.ToPixelShort();
		public static explicit operator PixelInt(PixelLong x) => x.ToPixelInt();
		//public static explicit operator PixelLong(PixelLong x) => x.ToPixelLong();
		public static implicit operator PixelFloat(PixelLong x) => x.ToPixelFloat();
		public static implicit operator PixelDouble(PixelLong x) => x.ToPixelDouble();
		/// <summary>
        /// 符号なし8ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelByte ToPixelByte()
		{
			int size = this.Size;
			byte[] ret = new byte[size];

            fixed (long* p_x = &this.pixel[0])
            fixed (byte* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                byte* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (byte)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUShort ToPixelUShort()
		{
			int size = this.Size;
			ushort[] ret = new ushort[size];

            fixed (long* p_x = &this.pixel[0])
            fixed (ushort* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                ushort* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ushort)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUInt ToPixelUInt()
		{
			int size = this.Size;
			uint[] ret = new uint[size];

            fixed (long* p_x = &this.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (uint)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelULong ToPixelULong()
		{
			int size = this.Size;
			ulong[] ret = new ulong[size];

            fixed (long* p_x = &this.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ulong)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelShort ToPixelShort()
		{
			int size = this.Size;
			short[] ret = new short[size];

            fixed (long* p_x = &this.pixel[0])
            fixed (short* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                short* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (short)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelInt ToPixelInt()
		{
			int size = this.Size;
			int[] ret = new int[size];

            fixed (long* p_x = &this.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (int)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelLong ToPixelLong()
		{
			int size = this.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &this.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (long)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(32ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelFloat ToPixelFloat()
		{
			int size = this.Size;
			float[] ret = new float[size];

            fixed (long* p_x = &this.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (float)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(64ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelDouble ToPixelDouble()
		{
			int size = this.Size;
			double[] ret = new double[size];

            fixed (long* p_x = &this.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (double)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		#endregion
		#region operator ビット演算
		
		public unsafe static PixelLong operator <<(PixelLong x, int y)
		{
			//論理シフト（最下位0埋め
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ << y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelLong operator >>(PixelLong x, int y)
		{
			//算術シフト（符号ビットで埋める）符号無しなら論理シフト
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ >> y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelLong operator &(PixelLong x, PixelLong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
			fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long*	pt_x = p_x;
				long*	pt_y = p_y;
                long*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelLong operator ^(PixelLong x, PixelLong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
			fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long*	pt_x = p_x;
				long*	pt_y = p_y;
                long*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelLong operator |(PixelLong x, PixelLong y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
			fixed (long* p_y = &y.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long*	pt_x = p_x;
				long*	pt_y = p_y;
                long*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelLong operator &(PixelLong x, long y)
		{
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelLong operator ^(PixelLong x, long y)
		{
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelLong operator |(PixelLong x, long y)
		{
			int size = x.Size;
			long[] ret = new long[size];

            fixed (long* p_x = &x.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                long* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public static PixelLong operator &(long x, PixelLong y) => y & x;
        public static PixelLong operator ^(long x, PixelLong y) => y ^ x;
        public static PixelLong operator |(long x, PixelLong y) => y | x;
		
		#endregion
	}
    public partial class PixelFloat
    {
		#region operator 四則演算
        public unsafe static PixelFloat operator +(PixelFloat x, PixelFloat y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
			fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float*	pt_x = p_x;
				float*	pt_y = p_y;
                float*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelFloat operator -(PixelFloat x, PixelFloat y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
			fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float*	pt_x = p_x;
				float*	pt_y = p_y;
                float*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelFloat operator *(PixelFloat x, PixelFloat y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
			fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float*	pt_x = p_x;
				float*	pt_y = p_y;
                float*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelFloat operator /(PixelFloat x, PixelFloat y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
			fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float*	pt_x = p_x;
				float*	pt_y = p_y;
                float*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelFloat operator +(PixelFloat x, float y)
		{
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
        public unsafe static PixelFloat operator -(PixelFloat x, float y)
		{
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelFloat operator *(PixelFloat x, float y)
		{
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelFloat operator /(PixelFloat x, float y)
		{
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelFloat operator +(float x, PixelFloat y)
		{
			int size = y.Size;
			float[] ret = new float[size];

            fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_y = p_y;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x + *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelFloat operator -(float x, PixelFloat y)
		{
			int size = y.Size;
			float[] ret = new float[size];

            fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_y = p_y;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x - *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelFloat operator *(float x, PixelFloat y)
		{
			int size = y.Size;
			float[] ret = new float[size];

            fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_y = p_y;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x * *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelFloat operator /(float x, PixelFloat y)
		{
			int size = y.Size;
			float[] ret = new float[size];

            fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_y = p_y;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x / *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public static PixelFloat operator +(PixelFloat x) => x;
        public unsafe static PixelFloat operator -(PixelFloat x)
		{
			int size = x.Size;
			float[] ret = new float[size];
		
            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;
		
                for (int i = 0; i < size; ++i)
                {
                    *pt_ret++ = *pt_x++ * -1;
        			x.token.Token.ThrowIfCancellationRequested();
                }
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		#endregion
		#region operator キャスト
		public static explicit operator PixelByte(PixelFloat x) => x.ToPixelByte();
		public static explicit operator PixelUShort(PixelFloat x) => x.ToPixelUShort();
		public static explicit operator PixelUInt(PixelFloat x) => x.ToPixelUInt();
		public static explicit operator PixelULong(PixelFloat x) => x.ToPixelULong();
		public static explicit operator PixelShort(PixelFloat x) => x.ToPixelShort();
		public static explicit operator PixelInt(PixelFloat x) => x.ToPixelInt();
		public static explicit operator PixelLong(PixelFloat x) => x.ToPixelLong();
		//public static explicit operator PixelFloat(PixelFloat x) => x.ToPixelFloat();
		public static implicit operator PixelDouble(PixelFloat x) => x.ToPixelDouble();
		/// <summary>
        /// 符号なし8ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelByte ToPixelByte()
		{
			int size = this.Size;
			byte[] ret = new byte[size];

            fixed (float* p_x = &this.pixel[0])
            fixed (byte* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                byte* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (byte)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUShort ToPixelUShort()
		{
			int size = this.Size;
			ushort[] ret = new ushort[size];

            fixed (float* p_x = &this.pixel[0])
            fixed (ushort* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                ushort* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ushort)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUInt ToPixelUInt()
		{
			int size = this.Size;
			uint[] ret = new uint[size];

            fixed (float* p_x = &this.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (uint)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelULong ToPixelULong()
		{
			int size = this.Size;
			ulong[] ret = new ulong[size];

            fixed (float* p_x = &this.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ulong)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelShort ToPixelShort()
		{
			int size = this.Size;
			short[] ret = new short[size];

            fixed (float* p_x = &this.pixel[0])
            fixed (short* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                short* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (short)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelInt ToPixelInt()
		{
			int size = this.Size;
			int[] ret = new int[size];

            fixed (float* p_x = &this.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (int)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelLong ToPixelLong()
		{
			int size = this.Size;
			long[] ret = new long[size];

            fixed (float* p_x = &this.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (long)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(32ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelFloat ToPixelFloat()
		{
			int size = this.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &this.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (float)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(64ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelDouble ToPixelDouble()
		{
			int size = this.Size;
			double[] ret = new double[size];

            fixed (float* p_x = &this.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (double)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		#endregion
		#region operator ビット演算
		/*
		public unsafe static PixelFloat operator <<(PixelFloat x, int y)
		{
			//論理シフト（最下位0埋め
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ << y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelFloat operator >>(PixelFloat x, int y)
		{
			//算術シフト（符号ビットで埋める）符号無しなら論理シフト
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ >> y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelFloat operator &(PixelFloat x, PixelFloat y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
			fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float*	pt_x = p_x;
				float*	pt_y = p_y;
                float*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelFloat operator ^(PixelFloat x, PixelFloat y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
			fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float*	pt_x = p_x;
				float*	pt_y = p_y;
                float*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelFloat operator |(PixelFloat x, PixelFloat y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
			fixed (float* p_y = &y.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float*	pt_x = p_x;
				float*	pt_y = p_y;
                float*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelFloat operator &(PixelFloat x, float y)
		{
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelFloat operator ^(PixelFloat x, float y)
		{
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelFloat operator |(PixelFloat x, float y)
		{
			int size = x.Size;
			float[] ret = new float[size];

            fixed (float* p_x = &x.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                float* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public static PixelFloat operator &(float x, PixelFloat y) => y & x;
        public static PixelFloat operator ^(float x, PixelFloat y) => y ^ x;
        public static PixelFloat operator |(float x, PixelFloat y) => y | x;
		*/
		#endregion
	}
    public partial class PixelDouble
    {
		#region operator 四則演算
        public unsafe static PixelDouble operator +(PixelDouble x, PixelDouble y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
			fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double*	pt_x = p_x;
				double*	pt_y = p_y;
                double*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelDouble operator -(PixelDouble x, PixelDouble y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
			fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double*	pt_x = p_x;
				double*	pt_y = p_y;
                double*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelDouble operator *(PixelDouble x, PixelDouble y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
			fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double*	pt_x = p_x;
				double*	pt_y = p_y;
                double*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(x.Width, x.Height, x.token);
		}
		public unsafe static PixelDouble operator /(PixelDouble x, PixelDouble y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
			fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double*	pt_x = p_x;
				double*	pt_y = p_y;
                double*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelDouble operator +(PixelDouble x, double y)
		{
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ + y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
        public unsafe static PixelDouble operator -(PixelDouble x, double y)
		{
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ - y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelDouble operator *(PixelDouble x, double y)
		{
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ * y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelDouble operator /(PixelDouble x, double y)
		{
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ / y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		public unsafe static PixelDouble operator +(double x, PixelDouble y)
		{
			int size = y.Size;
			double[] ret = new double[size];

            fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_y = p_y;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x + *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelDouble operator -(double x, PixelDouble y)
		{
			int size = y.Size;
			double[] ret = new double[size];

            fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_y = p_y;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x - *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelDouble operator *(double x, PixelDouble y)
		{
			int size = y.Size;
			double[] ret = new double[size];

            fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_y = p_y;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x * *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public unsafe static PixelDouble operator /(double x, PixelDouble y)
		{
			int size = y.Size;
			double[] ret = new double[size];

            fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_y = p_y;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = x / *pt_y++;
					y.token.Token.ThrowIfCancellationRequested();
				}
			}
            return ret.Create(y.Width, y.Height, y.token);
        }
		public static PixelDouble operator +(PixelDouble x) => x;
        public unsafe static PixelDouble operator -(PixelDouble x)
		{
			int size = x.Size;
			double[] ret = new double[size];
		
            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;
		
                for (int i = 0; i < size; ++i)
                {
                    *pt_ret++ = *pt_x++ * -1;
        			x.token.Token.ThrowIfCancellationRequested();
                }
            }
            return ret.Create(x.Width, x.Height, x.token);
        }
		#endregion
		#region operator キャスト
		public static explicit operator PixelByte(PixelDouble x) => x.ToPixelByte();
		public static explicit operator PixelUShort(PixelDouble x) => x.ToPixelUShort();
		public static explicit operator PixelUInt(PixelDouble x) => x.ToPixelUInt();
		public static explicit operator PixelULong(PixelDouble x) => x.ToPixelULong();
		public static explicit operator PixelShort(PixelDouble x) => x.ToPixelShort();
		public static explicit operator PixelInt(PixelDouble x) => x.ToPixelInt();
		public static explicit operator PixelLong(PixelDouble x) => x.ToPixelLong();
		public static explicit operator PixelFloat(PixelDouble x) => x.ToPixelFloat();
		//public static explicit operator PixelDouble(PixelDouble x) => x.ToPixelDouble();
		/// <summary>
        /// 符号なし8ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelByte ToPixelByte()
		{
			int size = this.Size;
			byte[] ret = new byte[size];

            fixed (double* p_x = &this.pixel[0])
            fixed (byte* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                byte* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (byte)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUShort ToPixelUShort()
		{
			int size = this.Size;
			ushort[] ret = new ushort[size];

            fixed (double* p_x = &this.pixel[0])
            fixed (ushort* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                ushort* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ushort)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelUInt ToPixelUInt()
		{
			int size = this.Size;
			uint[] ret = new uint[size];

            fixed (double* p_x = &this.pixel[0])
            fixed (uint* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                uint* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (uint)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号なし64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelULong ToPixelULong()
		{
			int size = this.Size;
			ulong[] ret = new ulong[size];

            fixed (double* p_x = &this.pixel[0])
            fixed (ulong* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                ulong* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (ulong)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き16ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelShort ToPixelShort()
		{
			int size = this.Size;
			short[] ret = new short[size];

            fixed (double* p_x = &this.pixel[0])
            fixed (short* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                short* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (short)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き32ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelInt ToPixelInt()
		{
			int size = this.Size;
			int[] ret = new int[size];

            fixed (double* p_x = &this.pixel[0])
            fixed (int* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                int* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (int)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 符号付き64ビット整数へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelLong ToPixelLong()
		{
			int size = this.Size;
			long[] ret = new long[size];

            fixed (double* p_x = &this.pixel[0])
            fixed (long* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                long* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (long)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(32ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelFloat ToPixelFloat()
		{
			int size = this.Size;
			float[] ret = new float[size];

            fixed (double* p_x = &this.pixel[0])
            fixed (float* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                float* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (float)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		/// <summary>
        /// 単精度(64ビット)浮動小数点へキャストを行う（値チェックなし）
        /// </summary>
        /// <returns></returns>
		public unsafe PixelDouble ToPixelDouble()
		{
			int size = this.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &this.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = (double)*pt_x++;
					this.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(this.Width, this.Height, token);
        }
		#endregion
		#region operator ビット演算
		/*
		public unsafe static PixelDouble operator <<(PixelDouble x, int y)
		{
			//論理シフト（最下位0埋め
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ << y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelDouble operator >>(PixelDouble x, int y)
		{
			//算術シフト（符号ビットで埋める）符号無しなら論理シフト
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ >> y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelDouble operator &(PixelDouble x, PixelDouble y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
			fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double*	pt_x = p_x;
				double*	pt_y = p_y;
                double*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelDouble operator ^(PixelDouble x, PixelDouble y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
			fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double*	pt_x = p_x;
				double*	pt_y = p_y;
                double*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelDouble operator |(PixelDouble x, PixelDouble y)
		{
			if (x.Size != y.Size) throw new ArgumentOutOfRangeException("operator Array Size");
			
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
			fixed (double* p_y = &y.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double*	pt_x = p_x;
				double*	pt_y = p_y;
                double*	pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | *pt_y++;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelDouble operator &(PixelDouble x, double y)
		{
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ & y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelDouble operator ^(PixelDouble x, double y)
		{
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ ^ y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public unsafe static PixelDouble operator |(PixelDouble x, double y)
		{
			int size = x.Size;
			double[] ret = new double[size];

            fixed (double* p_x = &x.pixel[0])
            fixed (double* p_ret = &ret[0])
            {
                double* pt_x = p_x;
                double* pt_ret = p_ret;

                for (int i = 0; i < size; ++i)
				{
                    *pt_ret++ = *pt_x++ | y;
					x.token.Token.ThrowIfCancellationRequested();
				}
            }
            return ret.Create(x.Width, x.Height, x.token);
		}
        public static PixelDouble operator &(double x, PixelDouble y) => y & x;
        public static PixelDouble operator ^(double x, PixelDouble y) => y ^ x;
        public static PixelDouble operator |(double x, PixelDouble y) => y | x;
		*/
		#endregion
	}
	#endregion

    /*--------------------------------------*/
    //   PixelMath
    /*--------------------------------------*/
	#region PixelMath
    public static class PixelMath
    {
        /// <summary>
        /// 符号なし8ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public unsafe static PixelDouble Sqrt(PixelByte value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号なし16ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public unsafe static PixelDouble Sqrt(PixelUShort value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号なし32ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public unsafe static PixelDouble Sqrt(PixelUInt value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号なし64ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public unsafe static PixelDouble Sqrt(PixelULong value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号付き16ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public unsafe static PixelDouble Sqrt(PixelShort value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号付き32ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public unsafe static PixelDouble Sqrt(PixelInt value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号付き64ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public unsafe static PixelDouble Sqrt(PixelLong value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 単精度(32ビット)浮動小数点Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public unsafe static PixelDouble Sqrt(PixelFloat value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 単精度(64ビット)浮動小数点Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public unsafe static PixelDouble Sqrt(PixelDouble value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }

        /// <summary>
        /// 符号なし8ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(32ビット)浮動小数点Pixel</returns>
		public static PixelFloat SqrtSingle(PixelByte value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号なし16ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(32ビット)浮動小数点Pixel</returns>
		public static PixelFloat SqrtSingle(PixelUShort value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号なし32ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(32ビット)浮動小数点Pixel</returns>
		public static PixelFloat SqrtSingle(PixelUInt value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号なし64ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(32ビット)浮動小数点Pixel</returns>
		public static PixelFloat SqrtSingle(PixelULong value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号付き16ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(32ビット)浮動小数点Pixel</returns>
		public static PixelFloat SqrtSingle(PixelShort value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号付き32ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(32ビット)浮動小数点Pixel</returns>
		public static PixelFloat SqrtSingle(PixelInt value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号付き64ビット整数Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(32ビット)浮動小数点Pixel</returns>
		public static PixelFloat SqrtSingle(PixelLong value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 単精度(32ビット)浮動小数点Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(32ビット)浮動小数点Pixel</returns>
		public static PixelFloat SqrtSingle(PixelFloat value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 単精度(64ビット)浮動小数点Pixelの平方根を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(32ビット)浮動小数点Pixel</returns>
		public static PixelFloat SqrtSingle(PixelDouble value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Sqrt(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(value.Width, value.Height, value.token);
        }

        /// <summary>
        /// 単精度(64ビット)浮動小数点Pixelの絶対値を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public static PixelDouble Abs(PixelDouble value)
        {
			int s = value.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Abs(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
			return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 単精度(32ビット)浮動小数点Pixelの絶対値を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public static PixelFloat Abs(PixelFloat value)
        {
			int s = value.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Abs(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
			return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号付き64ビット整数Pixelの絶対値を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public static PixelLong Abs(PixelLong value)
        {
			int s = value.Size;
            long[] ret = new long[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Abs(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
			return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号付き32ビット整数Pixelの絶対値を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public static PixelInt Abs(PixelInt value)
        {
			int s = value.Size;
            int[] ret = new int[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Abs(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
			return ret.Create(value.Width, value.Height, value.token);
        }
        /// <summary>
        /// 符号付き16ビット整数Pixelの絶対値を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public static PixelShort Abs(PixelShort value)
        {
			int s = value.Size;
            short[] ret = new short[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = Math.Abs(value[i]);
				value.token.Token.ThrowIfCancellationRequested();
			}
			return ret.Create(value.Width, value.Height, value.token);
        }

        /// <summary>
        /// 単精度(64ビット)浮動小数点Pixelのべき乗を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public static PixelDouble Pow(PixelDouble x,double y)
        {
			int s = x.Size;
            double[] ret = new double[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (double)Math.Pow(x[i],y);
				x.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(x.Width, x.Height, x.token);
        }
        /// <summary>
        /// 単精度(32ビット)浮動小数点Pixelのべき乗を求める
        /// </summary>
        /// <param name="value"></param>
        /// <returns>単精度(64ビット)浮動小数点Pixel</returns>
		public static PixelFloat Pow(PixelFloat x,float y)
        {
			int s = x.Size;
            float[] ret = new float[s];
            for (int i = 0; i < s; i++)
			{
                ret[i] = (float)Math.Pow(x[i],y);
				x.token.Token.ThrowIfCancellationRequested();
			}
            return ret.Create(x.Width, x.Height, x.token);
        }
            //Math.Exp
            //Math.Log
            //Math.Log10



	}
	#endregion
}







