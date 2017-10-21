using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Pixels;

namespace Pixels.Math
{
    public static class PixelMathEx
    {
        public static byte ConvertToByte(this Byte value) => (byte)(value > 255 ? 255 : value < 0 ? 0 : value);
        public static byte ConvertToByte(this UInt16 value) => (byte)(value > 255 ? 255 : value < 0 ? 0 : value);
        public static byte ConvertToByte(this UInt32 value) => (byte)(value > 255 ? 255 : value < 0 ? 0 : value);
        public static byte ConvertToByte(this UInt64 value) => (byte)(value > 255 ? 255 : value < 0 ? 0 : value);
        public static byte ConvertToByte(this Int16 value) => (byte)(value > 255 ? 255 : value < 0 ? 0 : value);
        public static byte ConvertToByte(this Int32 value) => (byte)(value > 255 ? 255 : value < 0 ? 0 : value);
        public static byte ConvertToByte(this Int64 value) => (byte)(value > 255 ? 255 : value < 0 ? 0 : value);
        public static byte ConvertToByte(this Single value) => (byte)(value > 255 ? 255 : value < 0 ? 0 : value);
        public static byte ConvertToByte(this Double value) => (byte)(value > 255 ? 255 : value < 0 ? 0 : value);

		public static Pixel<Byte> AddSelf(this Pixel<Byte> src, Byte value) => Add(src, src, value);
		public static Pixel<Byte> Add(this Pixel<Byte> src, Byte value) => Add(src, PixelFactory.Create<Byte>(src.Maps), value);
        public static Pixel<Byte> Add(this Pixel<Byte> src, Pixel<Byte> dst, Byte value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Byte)(src[x,y] + value);
            return dst;
        }
		public static Pixel<UInt16> AddSelf(this Pixel<UInt16> src, UInt16 value) => Add(src, src, value);
		public static Pixel<UInt16> Add(this Pixel<UInt16> src, UInt16 value) => Add(src, PixelFactory.Create<UInt16>(src.Maps), value);
        public static Pixel<UInt16> Add(this Pixel<UInt16> src, Pixel<UInt16> dst, UInt16 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt16)(src[x,y] + value);
            return dst;
        }
		public static Pixel<UInt32> AddSelf(this Pixel<UInt32> src, UInt32 value) => Add(src, src, value);
		public static Pixel<UInt32> Add(this Pixel<UInt32> src, UInt32 value) => Add(src, PixelFactory.Create<UInt32>(src.Maps), value);
        public static Pixel<UInt32> Add(this Pixel<UInt32> src, Pixel<UInt32> dst, UInt32 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt32)(src[x,y] + value);
            return dst;
        }
		public static Pixel<UInt64> AddSelf(this Pixel<UInt64> src, UInt64 value) => Add(src, src, value);
		public static Pixel<UInt64> Add(this Pixel<UInt64> src, UInt64 value) => Add(src, PixelFactory.Create<UInt64>(src.Maps), value);
        public static Pixel<UInt64> Add(this Pixel<UInt64> src, Pixel<UInt64> dst, UInt64 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt64)(src[x,y] + value);
            return dst;
        }
		public static Pixel<Int16> AddSelf(this Pixel<Int16> src, Int16 value) => Add(src, src, value);
		public static Pixel<Int16> Add(this Pixel<Int16> src, Int16 value) => Add(src, PixelFactory.Create<Int16>(src.Maps), value);
        public static Pixel<Int16> Add(this Pixel<Int16> src, Pixel<Int16> dst, Int16 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int16)(src[x,y] + value);
            return dst;
        }
		public static Pixel<Int32> AddSelf(this Pixel<Int32> src, Int32 value) => Add(src, src, value);
		public static Pixel<Int32> Add(this Pixel<Int32> src, Int32 value) => Add(src, PixelFactory.Create<Int32>(src.Maps), value);
        public static Pixel<Int32> Add(this Pixel<Int32> src, Pixel<Int32> dst, Int32 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int32)(src[x,y] + value);
            return dst;
        }
		public static Pixel<Int64> AddSelf(this Pixel<Int64> src, Int64 value) => Add(src, src, value);
		public static Pixel<Int64> Add(this Pixel<Int64> src, Int64 value) => Add(src, PixelFactory.Create<Int64>(src.Maps), value);
        public static Pixel<Int64> Add(this Pixel<Int64> src, Pixel<Int64> dst, Int64 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int64)(src[x,y] + value);
            return dst;
        }
		public static Pixel<Single> AddSelf(this Pixel<Single> src, Single value) => Add(src, src, value);
		public static Pixel<Single> Add(this Pixel<Single> src, Single value) => Add(src, PixelFactory.Create<Single>(src.Maps), value);
        public static Pixel<Single> Add(this Pixel<Single> src, Pixel<Single> dst, Single value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Single)(src[x,y] + value);
            return dst;
        }
		public static Pixel<Double> AddSelf(this Pixel<Double> src, Double value) => Add(src, src, value);
		public static Pixel<Double> Add(this Pixel<Double> src, Double value) => Add(src, PixelFactory.Create<Double>(src.Maps), value);
        public static Pixel<Double> Add(this Pixel<Double> src, Pixel<Double> dst, Double value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Double)(src[x,y] + value);
            return dst;
        }
		public static Pixel<Byte> SubSelf(this Pixel<Byte> src, Byte value) => Sub(src, src, value);
		public static Pixel<Byte> Sub(this Pixel<Byte> src, Byte value) => Sub(src, PixelFactory.Create<Byte>(src.Maps), value);
        public static Pixel<Byte> Sub(this Pixel<Byte> src, Pixel<Byte> dst, Byte value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Byte)(src[x,y] - value);
            return dst;
        }
		public static Pixel<UInt16> SubSelf(this Pixel<UInt16> src, UInt16 value) => Sub(src, src, value);
		public static Pixel<UInt16> Sub(this Pixel<UInt16> src, UInt16 value) => Sub(src, PixelFactory.Create<UInt16>(src.Maps), value);
        public static Pixel<UInt16> Sub(this Pixel<UInt16> src, Pixel<UInt16> dst, UInt16 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt16)(src[x,y] - value);
            return dst;
        }
		public static Pixel<UInt32> SubSelf(this Pixel<UInt32> src, UInt32 value) => Sub(src, src, value);
		public static Pixel<UInt32> Sub(this Pixel<UInt32> src, UInt32 value) => Sub(src, PixelFactory.Create<UInt32>(src.Maps), value);
        public static Pixel<UInt32> Sub(this Pixel<UInt32> src, Pixel<UInt32> dst, UInt32 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt32)(src[x,y] - value);
            return dst;
        }
		public static Pixel<UInt64> SubSelf(this Pixel<UInt64> src, UInt64 value) => Sub(src, src, value);
		public static Pixel<UInt64> Sub(this Pixel<UInt64> src, UInt64 value) => Sub(src, PixelFactory.Create<UInt64>(src.Maps), value);
        public static Pixel<UInt64> Sub(this Pixel<UInt64> src, Pixel<UInt64> dst, UInt64 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt64)(src[x,y] - value);
            return dst;
        }
		public static Pixel<Int16> SubSelf(this Pixel<Int16> src, Int16 value) => Sub(src, src, value);
		public static Pixel<Int16> Sub(this Pixel<Int16> src, Int16 value) => Sub(src, PixelFactory.Create<Int16>(src.Maps), value);
        public static Pixel<Int16> Sub(this Pixel<Int16> src, Pixel<Int16> dst, Int16 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int16)(src[x,y] - value);
            return dst;
        }
		public static Pixel<Int32> SubSelf(this Pixel<Int32> src, Int32 value) => Sub(src, src, value);
		public static Pixel<Int32> Sub(this Pixel<Int32> src, Int32 value) => Sub(src, PixelFactory.Create<Int32>(src.Maps), value);
        public static Pixel<Int32> Sub(this Pixel<Int32> src, Pixel<Int32> dst, Int32 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int32)(src[x,y] - value);
            return dst;
        }
		public static Pixel<Int64> SubSelf(this Pixel<Int64> src, Int64 value) => Sub(src, src, value);
		public static Pixel<Int64> Sub(this Pixel<Int64> src, Int64 value) => Sub(src, PixelFactory.Create<Int64>(src.Maps), value);
        public static Pixel<Int64> Sub(this Pixel<Int64> src, Pixel<Int64> dst, Int64 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int64)(src[x,y] - value);
            return dst;
        }
		public static Pixel<Single> SubSelf(this Pixel<Single> src, Single value) => Sub(src, src, value);
		public static Pixel<Single> Sub(this Pixel<Single> src, Single value) => Sub(src, PixelFactory.Create<Single>(src.Maps), value);
        public static Pixel<Single> Sub(this Pixel<Single> src, Pixel<Single> dst, Single value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Single)(src[x,y] - value);
            return dst;
        }
		public static Pixel<Double> SubSelf(this Pixel<Double> src, Double value) => Sub(src, src, value);
		public static Pixel<Double> Sub(this Pixel<Double> src, Double value) => Sub(src, PixelFactory.Create<Double>(src.Maps), value);
        public static Pixel<Double> Sub(this Pixel<Double> src, Pixel<Double> dst, Double value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Double)(src[x,y] - value);
            return dst;
        }
		public static Pixel<Byte> MulSelf(this Pixel<Byte> src, Byte value) => Mul(src, src, value);
		public static Pixel<Byte> Mul(this Pixel<Byte> src, Byte value) => Mul(src, PixelFactory.Create<Byte>(src.Maps), value);
        public static Pixel<Byte> Mul(this Pixel<Byte> src, Pixel<Byte> dst, Byte value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Byte)(src[x,y] * value);
            return dst;
        }
		public static Pixel<UInt16> MulSelf(this Pixel<UInt16> src, UInt16 value) => Mul(src, src, value);
		public static Pixel<UInt16> Mul(this Pixel<UInt16> src, UInt16 value) => Mul(src, PixelFactory.Create<UInt16>(src.Maps), value);
        public static Pixel<UInt16> Mul(this Pixel<UInt16> src, Pixel<UInt16> dst, UInt16 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt16)(src[x,y] * value);
            return dst;
        }
		public static Pixel<UInt32> MulSelf(this Pixel<UInt32> src, UInt32 value) => Mul(src, src, value);
		public static Pixel<UInt32> Mul(this Pixel<UInt32> src, UInt32 value) => Mul(src, PixelFactory.Create<UInt32>(src.Maps), value);
        public static Pixel<UInt32> Mul(this Pixel<UInt32> src, Pixel<UInt32> dst, UInt32 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt32)(src[x,y] * value);
            return dst;
        }
		public static Pixel<UInt64> MulSelf(this Pixel<UInt64> src, UInt64 value) => Mul(src, src, value);
		public static Pixel<UInt64> Mul(this Pixel<UInt64> src, UInt64 value) => Mul(src, PixelFactory.Create<UInt64>(src.Maps), value);
        public static Pixel<UInt64> Mul(this Pixel<UInt64> src, Pixel<UInt64> dst, UInt64 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt64)(src[x,y] * value);
            return dst;
        }
		public static Pixel<Int16> MulSelf(this Pixel<Int16> src, Int16 value) => Mul(src, src, value);
		public static Pixel<Int16> Mul(this Pixel<Int16> src, Int16 value) => Mul(src, PixelFactory.Create<Int16>(src.Maps), value);
        public static Pixel<Int16> Mul(this Pixel<Int16> src, Pixel<Int16> dst, Int16 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int16)(src[x,y] * value);
            return dst;
        }
		public static Pixel<Int32> MulSelf(this Pixel<Int32> src, Int32 value) => Mul(src, src, value);
		public static Pixel<Int32> Mul(this Pixel<Int32> src, Int32 value) => Mul(src, PixelFactory.Create<Int32>(src.Maps), value);
        public static Pixel<Int32> Mul(this Pixel<Int32> src, Pixel<Int32> dst, Int32 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int32)(src[x,y] * value);
            return dst;
        }
		public static Pixel<Int64> MulSelf(this Pixel<Int64> src, Int64 value) => Mul(src, src, value);
		public static Pixel<Int64> Mul(this Pixel<Int64> src, Int64 value) => Mul(src, PixelFactory.Create<Int64>(src.Maps), value);
        public static Pixel<Int64> Mul(this Pixel<Int64> src, Pixel<Int64> dst, Int64 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int64)(src[x,y] * value);
            return dst;
        }
		public static Pixel<Single> MulSelf(this Pixel<Single> src, Single value) => Mul(src, src, value);
		public static Pixel<Single> Mul(this Pixel<Single> src, Single value) => Mul(src, PixelFactory.Create<Single>(src.Maps), value);
        public static Pixel<Single> Mul(this Pixel<Single> src, Pixel<Single> dst, Single value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Single)(src[x,y] * value);
            return dst;
        }
		public static Pixel<Double> MulSelf(this Pixel<Double> src, Double value) => Mul(src, src, value);
		public static Pixel<Double> Mul(this Pixel<Double> src, Double value) => Mul(src, PixelFactory.Create<Double>(src.Maps), value);
        public static Pixel<Double> Mul(this Pixel<Double> src, Pixel<Double> dst, Double value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Double)(src[x,y] * value);
            return dst;
        }
		public static Pixel<Byte> DivSelf(this Pixel<Byte> src, Byte value) => Div(src, src, value);
		public static Pixel<Byte> Div(this Pixel<Byte> src, Byte value) => Div(src, PixelFactory.Create<Byte>(src.Maps), value);
        public static Pixel<Byte> Div(this Pixel<Byte> src, Pixel<Byte> dst, Byte value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Byte)(src[x,y] / value);
            return dst;
        }
		public static Pixel<UInt16> DivSelf(this Pixel<UInt16> src, UInt16 value) => Div(src, src, value);
		public static Pixel<UInt16> Div(this Pixel<UInt16> src, UInt16 value) => Div(src, PixelFactory.Create<UInt16>(src.Maps), value);
        public static Pixel<UInt16> Div(this Pixel<UInt16> src, Pixel<UInt16> dst, UInt16 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt16)(src[x,y] / value);
            return dst;
        }
		public static Pixel<UInt32> DivSelf(this Pixel<UInt32> src, UInt32 value) => Div(src, src, value);
		public static Pixel<UInt32> Div(this Pixel<UInt32> src, UInt32 value) => Div(src, PixelFactory.Create<UInt32>(src.Maps), value);
        public static Pixel<UInt32> Div(this Pixel<UInt32> src, Pixel<UInt32> dst, UInt32 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt32)(src[x,y] / value);
            return dst;
        }
		public static Pixel<UInt64> DivSelf(this Pixel<UInt64> src, UInt64 value) => Div(src, src, value);
		public static Pixel<UInt64> Div(this Pixel<UInt64> src, UInt64 value) => Div(src, PixelFactory.Create<UInt64>(src.Maps), value);
        public static Pixel<UInt64> Div(this Pixel<UInt64> src, Pixel<UInt64> dst, UInt64 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt64)(src[x,y] / value);
            return dst;
        }
		public static Pixel<Int16> DivSelf(this Pixel<Int16> src, Int16 value) => Div(src, src, value);
		public static Pixel<Int16> Div(this Pixel<Int16> src, Int16 value) => Div(src, PixelFactory.Create<Int16>(src.Maps), value);
        public static Pixel<Int16> Div(this Pixel<Int16> src, Pixel<Int16> dst, Int16 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int16)(src[x,y] / value);
            return dst;
        }
		public static Pixel<Int32> DivSelf(this Pixel<Int32> src, Int32 value) => Div(src, src, value);
		public static Pixel<Int32> Div(this Pixel<Int32> src, Int32 value) => Div(src, PixelFactory.Create<Int32>(src.Maps), value);
        public static Pixel<Int32> Div(this Pixel<Int32> src, Pixel<Int32> dst, Int32 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int32)(src[x,y] / value);
            return dst;
        }
		public static Pixel<Int64> DivSelf(this Pixel<Int64> src, Int64 value) => Div(src, src, value);
		public static Pixel<Int64> Div(this Pixel<Int64> src, Int64 value) => Div(src, PixelFactory.Create<Int64>(src.Maps), value);
        public static Pixel<Int64> Div(this Pixel<Int64> src, Pixel<Int64> dst, Int64 value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int64)(src[x,y] / value);
            return dst;
        }
		public static Pixel<Single> DivSelf(this Pixel<Single> src, Single value) => Div(src, src, value);
		public static Pixel<Single> Div(this Pixel<Single> src, Single value) => Div(src, PixelFactory.Create<Single>(src.Maps), value);
        public static Pixel<Single> Div(this Pixel<Single> src, Pixel<Single> dst, Single value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Single)(src[x,y] / value);
            return dst;
        }
		public static Pixel<Double> DivSelf(this Pixel<Double> src, Double value) => Div(src, src, value);
		public static Pixel<Double> Div(this Pixel<Double> src, Double value) => Div(src, PixelFactory.Create<Double>(src.Maps), value);
        public static Pixel<Double> Div(this Pixel<Double> src, Pixel<Double> dst, Double value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Double)(src[x,y] / value);
            return dst;
        }
		public static Pixel<Byte> AddSelf(this Pixel<Byte> src1, Pixel<Byte> src2) => Add(src1, src1, src2);
		public static Pixel<Byte> Add(this Pixel<Byte> src1, Pixel<Byte> src2) => Add(src1, PixelFactory.Create<Byte>(src1.Maps), src2);
        public static Pixel<Byte> Add(this Pixel<Byte> src1, Pixel<Byte> dst, Pixel<Byte> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Byte)(src1[x,y] + src2[x,y]);
            return dst;
        }
		public static Pixel<UInt16> AddSelf(this Pixel<UInt16> src1, Pixel<UInt16> src2) => Add(src1, src1, src2);
		public static Pixel<UInt16> Add(this Pixel<UInt16> src1, Pixel<UInt16> src2) => Add(src1, PixelFactory.Create<UInt16>(src1.Maps), src2);
        public static Pixel<UInt16> Add(this Pixel<UInt16> src1, Pixel<UInt16> dst, Pixel<UInt16> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt16)(src1[x,y] + src2[x,y]);
            return dst;
        }
		public static Pixel<UInt32> AddSelf(this Pixel<UInt32> src1, Pixel<UInt32> src2) => Add(src1, src1, src2);
		public static Pixel<UInt32> Add(this Pixel<UInt32> src1, Pixel<UInt32> src2) => Add(src1, PixelFactory.Create<UInt32>(src1.Maps), src2);
        public static Pixel<UInt32> Add(this Pixel<UInt32> src1, Pixel<UInt32> dst, Pixel<UInt32> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt32)(src1[x,y] + src2[x,y]);
            return dst;
        }
		public static Pixel<UInt64> AddSelf(this Pixel<UInt64> src1, Pixel<UInt64> src2) => Add(src1, src1, src2);
		public static Pixel<UInt64> Add(this Pixel<UInt64> src1, Pixel<UInt64> src2) => Add(src1, PixelFactory.Create<UInt64>(src1.Maps), src2);
        public static Pixel<UInt64> Add(this Pixel<UInt64> src1, Pixel<UInt64> dst, Pixel<UInt64> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt64)(src1[x,y] + src2[x,y]);
            return dst;
        }
		public static Pixel<Int16> AddSelf(this Pixel<Int16> src1, Pixel<Int16> src2) => Add(src1, src1, src2);
		public static Pixel<Int16> Add(this Pixel<Int16> src1, Pixel<Int16> src2) => Add(src1, PixelFactory.Create<Int16>(src1.Maps), src2);
        public static Pixel<Int16> Add(this Pixel<Int16> src1, Pixel<Int16> dst, Pixel<Int16> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int16)(src1[x,y] + src2[x,y]);
            return dst;
        }
		public static Pixel<Int32> AddSelf(this Pixel<Int32> src1, Pixel<Int32> src2) => Add(src1, src1, src2);
		public static Pixel<Int32> Add(this Pixel<Int32> src1, Pixel<Int32> src2) => Add(src1, PixelFactory.Create<Int32>(src1.Maps), src2);
        public static Pixel<Int32> Add(this Pixel<Int32> src1, Pixel<Int32> dst, Pixel<Int32> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int32)(src1[x,y] + src2[x,y]);
            return dst;
        }
		public static Pixel<Int64> AddSelf(this Pixel<Int64> src1, Pixel<Int64> src2) => Add(src1, src1, src2);
		public static Pixel<Int64> Add(this Pixel<Int64> src1, Pixel<Int64> src2) => Add(src1, PixelFactory.Create<Int64>(src1.Maps), src2);
        public static Pixel<Int64> Add(this Pixel<Int64> src1, Pixel<Int64> dst, Pixel<Int64> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int64)(src1[x,y] + src2[x,y]);
            return dst;
        }
		public static Pixel<Single> AddSelf(this Pixel<Single> src1, Pixel<Single> src2) => Add(src1, src1, src2);
		public static Pixel<Single> Add(this Pixel<Single> src1, Pixel<Single> src2) => Add(src1, PixelFactory.Create<Single>(src1.Maps), src2);
        public static Pixel<Single> Add(this Pixel<Single> src1, Pixel<Single> dst, Pixel<Single> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Single)(src1[x,y] + src2[x,y]);
            return dst;
        }
		public static Pixel<Double> AddSelf(this Pixel<Double> src1, Pixel<Double> src2) => Add(src1, src1, src2);
		public static Pixel<Double> Add(this Pixel<Double> src1, Pixel<Double> src2) => Add(src1, PixelFactory.Create<Double>(src1.Maps), src2);
        public static Pixel<Double> Add(this Pixel<Double> src1, Pixel<Double> dst, Pixel<Double> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Double)(src1[x,y] + src2[x,y]);
            return dst;
        }
		public static Pixel<Byte> SubSelf(this Pixel<Byte> src1, Pixel<Byte> src2) => Sub(src1, src1, src2);
		public static Pixel<Byte> Sub(this Pixel<Byte> src1, Pixel<Byte> src2) => Sub(src1, PixelFactory.Create<Byte>(src1.Maps), src2);
        public static Pixel<Byte> Sub(this Pixel<Byte> src1, Pixel<Byte> dst, Pixel<Byte> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Byte)(src1[x,y] - src2[x,y]);
            return dst;
        }
		public static Pixel<UInt16> SubSelf(this Pixel<UInt16> src1, Pixel<UInt16> src2) => Sub(src1, src1, src2);
		public static Pixel<UInt16> Sub(this Pixel<UInt16> src1, Pixel<UInt16> src2) => Sub(src1, PixelFactory.Create<UInt16>(src1.Maps), src2);
        public static Pixel<UInt16> Sub(this Pixel<UInt16> src1, Pixel<UInt16> dst, Pixel<UInt16> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt16)(src1[x,y] - src2[x,y]);
            return dst;
        }
		public static Pixel<UInt32> SubSelf(this Pixel<UInt32> src1, Pixel<UInt32> src2) => Sub(src1, src1, src2);
		public static Pixel<UInt32> Sub(this Pixel<UInt32> src1, Pixel<UInt32> src2) => Sub(src1, PixelFactory.Create<UInt32>(src1.Maps), src2);
        public static Pixel<UInt32> Sub(this Pixel<UInt32> src1, Pixel<UInt32> dst, Pixel<UInt32> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt32)(src1[x,y] - src2[x,y]);
            return dst;
        }
		public static Pixel<UInt64> SubSelf(this Pixel<UInt64> src1, Pixel<UInt64> src2) => Sub(src1, src1, src2);
		public static Pixel<UInt64> Sub(this Pixel<UInt64> src1, Pixel<UInt64> src2) => Sub(src1, PixelFactory.Create<UInt64>(src1.Maps), src2);
        public static Pixel<UInt64> Sub(this Pixel<UInt64> src1, Pixel<UInt64> dst, Pixel<UInt64> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt64)(src1[x,y] - src2[x,y]);
            return dst;
        }
		public static Pixel<Int16> SubSelf(this Pixel<Int16> src1, Pixel<Int16> src2) => Sub(src1, src1, src2);
		public static Pixel<Int16> Sub(this Pixel<Int16> src1, Pixel<Int16> src2) => Sub(src1, PixelFactory.Create<Int16>(src1.Maps), src2);
        public static Pixel<Int16> Sub(this Pixel<Int16> src1, Pixel<Int16> dst, Pixel<Int16> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int16)(src1[x,y] - src2[x,y]);
            return dst;
        }
		public static Pixel<Int32> SubSelf(this Pixel<Int32> src1, Pixel<Int32> src2) => Sub(src1, src1, src2);
		public static Pixel<Int32> Sub(this Pixel<Int32> src1, Pixel<Int32> src2) => Sub(src1, PixelFactory.Create<Int32>(src1.Maps), src2);
        public static Pixel<Int32> Sub(this Pixel<Int32> src1, Pixel<Int32> dst, Pixel<Int32> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int32)(src1[x,y] - src2[x,y]);
            return dst;
        }
		public static Pixel<Int64> SubSelf(this Pixel<Int64> src1, Pixel<Int64> src2) => Sub(src1, src1, src2);
		public static Pixel<Int64> Sub(this Pixel<Int64> src1, Pixel<Int64> src2) => Sub(src1, PixelFactory.Create<Int64>(src1.Maps), src2);
        public static Pixel<Int64> Sub(this Pixel<Int64> src1, Pixel<Int64> dst, Pixel<Int64> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int64)(src1[x,y] - src2[x,y]);
            return dst;
        }
		public static Pixel<Single> SubSelf(this Pixel<Single> src1, Pixel<Single> src2) => Sub(src1, src1, src2);
		public static Pixel<Single> Sub(this Pixel<Single> src1, Pixel<Single> src2) => Sub(src1, PixelFactory.Create<Single>(src1.Maps), src2);
        public static Pixel<Single> Sub(this Pixel<Single> src1, Pixel<Single> dst, Pixel<Single> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Single)(src1[x,y] - src2[x,y]);
            return dst;
        }
		public static Pixel<Double> SubSelf(this Pixel<Double> src1, Pixel<Double> src2) => Sub(src1, src1, src2);
		public static Pixel<Double> Sub(this Pixel<Double> src1, Pixel<Double> src2) => Sub(src1, PixelFactory.Create<Double>(src1.Maps), src2);
        public static Pixel<Double> Sub(this Pixel<Double> src1, Pixel<Double> dst, Pixel<Double> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Double)(src1[x,y] - src2[x,y]);
            return dst;
        }
		public static Pixel<Byte> MulSelf(this Pixel<Byte> src1, Pixel<Byte> src2) => Mul(src1, src1, src2);
		public static Pixel<Byte> Mul(this Pixel<Byte> src1, Pixel<Byte> src2) => Mul(src1, PixelFactory.Create<Byte>(src1.Maps), src2);
        public static Pixel<Byte> Mul(this Pixel<Byte> src1, Pixel<Byte> dst, Pixel<Byte> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Byte)(src1[x,y] * src2[x,y]);
            return dst;
        }
		public static Pixel<UInt16> MulSelf(this Pixel<UInt16> src1, Pixel<UInt16> src2) => Mul(src1, src1, src2);
		public static Pixel<UInt16> Mul(this Pixel<UInt16> src1, Pixel<UInt16> src2) => Mul(src1, PixelFactory.Create<UInt16>(src1.Maps), src2);
        public static Pixel<UInt16> Mul(this Pixel<UInt16> src1, Pixel<UInt16> dst, Pixel<UInt16> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt16)(src1[x,y] * src2[x,y]);
            return dst;
        }
		public static Pixel<UInt32> MulSelf(this Pixel<UInt32> src1, Pixel<UInt32> src2) => Mul(src1, src1, src2);
		public static Pixel<UInt32> Mul(this Pixel<UInt32> src1, Pixel<UInt32> src2) => Mul(src1, PixelFactory.Create<UInt32>(src1.Maps), src2);
        public static Pixel<UInt32> Mul(this Pixel<UInt32> src1, Pixel<UInt32> dst, Pixel<UInt32> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt32)(src1[x,y] * src2[x,y]);
            return dst;
        }
		public static Pixel<UInt64> MulSelf(this Pixel<UInt64> src1, Pixel<UInt64> src2) => Mul(src1, src1, src2);
		public static Pixel<UInt64> Mul(this Pixel<UInt64> src1, Pixel<UInt64> src2) => Mul(src1, PixelFactory.Create<UInt64>(src1.Maps), src2);
        public static Pixel<UInt64> Mul(this Pixel<UInt64> src1, Pixel<UInt64> dst, Pixel<UInt64> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt64)(src1[x,y] * src2[x,y]);
            return dst;
        }
		public static Pixel<Int16> MulSelf(this Pixel<Int16> src1, Pixel<Int16> src2) => Mul(src1, src1, src2);
		public static Pixel<Int16> Mul(this Pixel<Int16> src1, Pixel<Int16> src2) => Mul(src1, PixelFactory.Create<Int16>(src1.Maps), src2);
        public static Pixel<Int16> Mul(this Pixel<Int16> src1, Pixel<Int16> dst, Pixel<Int16> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int16)(src1[x,y] * src2[x,y]);
            return dst;
        }
		public static Pixel<Int32> MulSelf(this Pixel<Int32> src1, Pixel<Int32> src2) => Mul(src1, src1, src2);
		public static Pixel<Int32> Mul(this Pixel<Int32> src1, Pixel<Int32> src2) => Mul(src1, PixelFactory.Create<Int32>(src1.Maps), src2);
        public static Pixel<Int32> Mul(this Pixel<Int32> src1, Pixel<Int32> dst, Pixel<Int32> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int32)(src1[x,y] * src2[x,y]);
            return dst;
        }
		public static Pixel<Int64> MulSelf(this Pixel<Int64> src1, Pixel<Int64> src2) => Mul(src1, src1, src2);
		public static Pixel<Int64> Mul(this Pixel<Int64> src1, Pixel<Int64> src2) => Mul(src1, PixelFactory.Create<Int64>(src1.Maps), src2);
        public static Pixel<Int64> Mul(this Pixel<Int64> src1, Pixel<Int64> dst, Pixel<Int64> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int64)(src1[x,y] * src2[x,y]);
            return dst;
        }
		public static Pixel<Single> MulSelf(this Pixel<Single> src1, Pixel<Single> src2) => Mul(src1, src1, src2);
		public static Pixel<Single> Mul(this Pixel<Single> src1, Pixel<Single> src2) => Mul(src1, PixelFactory.Create<Single>(src1.Maps), src2);
        public static Pixel<Single> Mul(this Pixel<Single> src1, Pixel<Single> dst, Pixel<Single> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Single)(src1[x,y] * src2[x,y]);
            return dst;
        }
		public static Pixel<Double> MulSelf(this Pixel<Double> src1, Pixel<Double> src2) => Mul(src1, src1, src2);
		public static Pixel<Double> Mul(this Pixel<Double> src1, Pixel<Double> src2) => Mul(src1, PixelFactory.Create<Double>(src1.Maps), src2);
        public static Pixel<Double> Mul(this Pixel<Double> src1, Pixel<Double> dst, Pixel<Double> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Double)(src1[x,y] * src2[x,y]);
            return dst;
        }
		public static Pixel<Byte> DivSelf(this Pixel<Byte> src1, Pixel<Byte> src2) => Div(src1, src1, src2);
		public static Pixel<Byte> Div(this Pixel<Byte> src1, Pixel<Byte> src2) => Div(src1, PixelFactory.Create<Byte>(src1.Maps), src2);
        public static Pixel<Byte> Div(this Pixel<Byte> src1, Pixel<Byte> dst, Pixel<Byte> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Byte)(src1[x,y] / src2[x,y]);
            return dst;
        }
		public static Pixel<UInt16> DivSelf(this Pixel<UInt16> src1, Pixel<UInt16> src2) => Div(src1, src1, src2);
		public static Pixel<UInt16> Div(this Pixel<UInt16> src1, Pixel<UInt16> src2) => Div(src1, PixelFactory.Create<UInt16>(src1.Maps), src2);
        public static Pixel<UInt16> Div(this Pixel<UInt16> src1, Pixel<UInt16> dst, Pixel<UInt16> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt16)(src1[x,y] / src2[x,y]);
            return dst;
        }
		public static Pixel<UInt32> DivSelf(this Pixel<UInt32> src1, Pixel<UInt32> src2) => Div(src1, src1, src2);
		public static Pixel<UInt32> Div(this Pixel<UInt32> src1, Pixel<UInt32> src2) => Div(src1, PixelFactory.Create<UInt32>(src1.Maps), src2);
        public static Pixel<UInt32> Div(this Pixel<UInt32> src1, Pixel<UInt32> dst, Pixel<UInt32> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt32)(src1[x,y] / src2[x,y]);
            return dst;
        }
		public static Pixel<UInt64> DivSelf(this Pixel<UInt64> src1, Pixel<UInt64> src2) => Div(src1, src1, src2);
		public static Pixel<UInt64> Div(this Pixel<UInt64> src1, Pixel<UInt64> src2) => Div(src1, PixelFactory.Create<UInt64>(src1.Maps), src2);
        public static Pixel<UInt64> Div(this Pixel<UInt64> src1, Pixel<UInt64> dst, Pixel<UInt64> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (UInt64)(src1[x,y] / src2[x,y]);
            return dst;
        }
		public static Pixel<Int16> DivSelf(this Pixel<Int16> src1, Pixel<Int16> src2) => Div(src1, src1, src2);
		public static Pixel<Int16> Div(this Pixel<Int16> src1, Pixel<Int16> src2) => Div(src1, PixelFactory.Create<Int16>(src1.Maps), src2);
        public static Pixel<Int16> Div(this Pixel<Int16> src1, Pixel<Int16> dst, Pixel<Int16> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int16)(src1[x,y] / src2[x,y]);
            return dst;
        }
		public static Pixel<Int32> DivSelf(this Pixel<Int32> src1, Pixel<Int32> src2) => Div(src1, src1, src2);
		public static Pixel<Int32> Div(this Pixel<Int32> src1, Pixel<Int32> src2) => Div(src1, PixelFactory.Create<Int32>(src1.Maps), src2);
        public static Pixel<Int32> Div(this Pixel<Int32> src1, Pixel<Int32> dst, Pixel<Int32> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int32)(src1[x,y] / src2[x,y]);
            return dst;
        }
		public static Pixel<Int64> DivSelf(this Pixel<Int64> src1, Pixel<Int64> src2) => Div(src1, src1, src2);
		public static Pixel<Int64> Div(this Pixel<Int64> src1, Pixel<Int64> src2) => Div(src1, PixelFactory.Create<Int64>(src1.Maps), src2);
        public static Pixel<Int64> Div(this Pixel<Int64> src1, Pixel<Int64> dst, Pixel<Int64> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Int64)(src1[x,y] / src2[x,y]);
            return dst;
        }
		public static Pixel<Single> DivSelf(this Pixel<Single> src1, Pixel<Single> src2) => Div(src1, src1, src2);
		public static Pixel<Single> Div(this Pixel<Single> src1, Pixel<Single> src2) => Div(src1, PixelFactory.Create<Single>(src1.Maps), src2);
        public static Pixel<Single> Div(this Pixel<Single> src1, Pixel<Single> dst, Pixel<Single> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Single)(src1[x,y] / src2[x,y]);
            return dst;
        }
		public static Pixel<Double> DivSelf(this Pixel<Double> src1, Pixel<Double> src2) => Div(src1, src1, src2);
		public static Pixel<Double> Div(this Pixel<Double> src1, Pixel<Double> src2) => Div(src1, PixelFactory.Create<Double>(src1.Maps), src2);
        public static Pixel<Double> Div(this Pixel<Double> src1, Pixel<Double> dst, Pixel<Double> src2)
        {
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    dst[x, y] = (Double)(src1[x,y] / src2[x,y]);
            return dst;
        }






		public static Pixel<Byte> BitShiftLSelf(this Pixel<Byte> src, int value) => BitShiftL(src, src, value);
		public static Pixel<Byte> BitShiftL(this Pixel<Byte> src, int value) => BitShiftL(src, PixelFactory.Create<Byte>(src.Maps), value);
        public static Pixel<Byte> BitShiftL(this Pixel<Byte> src, Pixel<Byte> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Byte)(src[x,y] << value);
            return src;
        }
		public static Pixel<UInt16> BitShiftLSelf(this Pixel<UInt16> src, int value) => BitShiftL(src, src, value);
		public static Pixel<UInt16> BitShiftL(this Pixel<UInt16> src, int value) => BitShiftL(src, PixelFactory.Create<UInt16>(src.Maps), value);
        public static Pixel<UInt16> BitShiftL(this Pixel<UInt16> src, Pixel<UInt16> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt16)(src[x,y] << value);
            return src;
        }
		public static Pixel<UInt32> BitShiftLSelf(this Pixel<UInt32> src, int value) => BitShiftL(src, src, value);
		public static Pixel<UInt32> BitShiftL(this Pixel<UInt32> src, int value) => BitShiftL(src, PixelFactory.Create<UInt32>(src.Maps), value);
        public static Pixel<UInt32> BitShiftL(this Pixel<UInt32> src, Pixel<UInt32> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt32)(src[x,y] << value);
            return src;
        }
		public static Pixel<UInt64> BitShiftLSelf(this Pixel<UInt64> src, int value) => BitShiftL(src, src, value);
		public static Pixel<UInt64> BitShiftL(this Pixel<UInt64> src, int value) => BitShiftL(src, PixelFactory.Create<UInt64>(src.Maps), value);
        public static Pixel<UInt64> BitShiftL(this Pixel<UInt64> src, Pixel<UInt64> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt64)(src[x,y] << value);
            return src;
        }
		public static Pixel<Int16> BitShiftLSelf(this Pixel<Int16> src, int value) => BitShiftL(src, src, value);
		public static Pixel<Int16> BitShiftL(this Pixel<Int16> src, int value) => BitShiftL(src, PixelFactory.Create<Int16>(src.Maps), value);
        public static Pixel<Int16> BitShiftL(this Pixel<Int16> src, Pixel<Int16> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int16)(src[x,y] << value);
            return src;
        }
		public static Pixel<Int32> BitShiftLSelf(this Pixel<Int32> src, int value) => BitShiftL(src, src, value);
		public static Pixel<Int32> BitShiftL(this Pixel<Int32> src, int value) => BitShiftL(src, PixelFactory.Create<Int32>(src.Maps), value);
        public static Pixel<Int32> BitShiftL(this Pixel<Int32> src, Pixel<Int32> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int32)(src[x,y] << value);
            return src;
        }
		public static Pixel<Int64> BitShiftLSelf(this Pixel<Int64> src, int value) => BitShiftL(src, src, value);
		public static Pixel<Int64> BitShiftL(this Pixel<Int64> src, int value) => BitShiftL(src, PixelFactory.Create<Int64>(src.Maps), value);
        public static Pixel<Int64> BitShiftL(this Pixel<Int64> src, Pixel<Int64> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int64)(src[x,y] << value);
            return src;
        }
		public static Pixel<Byte> BitShiftRSelf(this Pixel<Byte> src, int value) => BitShiftR(src, src, value);
		public static Pixel<Byte> BitShiftR(this Pixel<Byte> src, int value) => BitShiftR(src, PixelFactory.Create<Byte>(src.Maps), value);
        public static Pixel<Byte> BitShiftR(this Pixel<Byte> src, Pixel<Byte> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Byte)(src[x,y] >> value);
            return src;
        }
		public static Pixel<UInt16> BitShiftRSelf(this Pixel<UInt16> src, int value) => BitShiftR(src, src, value);
		public static Pixel<UInt16> BitShiftR(this Pixel<UInt16> src, int value) => BitShiftR(src, PixelFactory.Create<UInt16>(src.Maps), value);
        public static Pixel<UInt16> BitShiftR(this Pixel<UInt16> src, Pixel<UInt16> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt16)(src[x,y] >> value);
            return src;
        }
		public static Pixel<UInt32> BitShiftRSelf(this Pixel<UInt32> src, int value) => BitShiftR(src, src, value);
		public static Pixel<UInt32> BitShiftR(this Pixel<UInt32> src, int value) => BitShiftR(src, PixelFactory.Create<UInt32>(src.Maps), value);
        public static Pixel<UInt32> BitShiftR(this Pixel<UInt32> src, Pixel<UInt32> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt32)(src[x,y] >> value);
            return src;
        }
		public static Pixel<UInt64> BitShiftRSelf(this Pixel<UInt64> src, int value) => BitShiftR(src, src, value);
		public static Pixel<UInt64> BitShiftR(this Pixel<UInt64> src, int value) => BitShiftR(src, PixelFactory.Create<UInt64>(src.Maps), value);
        public static Pixel<UInt64> BitShiftR(this Pixel<UInt64> src, Pixel<UInt64> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (UInt64)(src[x,y] >> value);
            return src;
        }
		public static Pixel<Int16> BitShiftRSelf(this Pixel<Int16> src, int value) => BitShiftR(src, src, value);
		public static Pixel<Int16> BitShiftR(this Pixel<Int16> src, int value) => BitShiftR(src, PixelFactory.Create<Int16>(src.Maps), value);
        public static Pixel<Int16> BitShiftR(this Pixel<Int16> src, Pixel<Int16> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int16)(src[x,y] >> value);
            return src;
        }
		public static Pixel<Int32> BitShiftRSelf(this Pixel<Int32> src, int value) => BitShiftR(src, src, value);
		public static Pixel<Int32> BitShiftR(this Pixel<Int32> src, int value) => BitShiftR(src, PixelFactory.Create<Int32>(src.Maps), value);
        public static Pixel<Int32> BitShiftR(this Pixel<Int32> src, Pixel<Int32> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int32)(src[x,y] >> value);
            return src;
        }
		public static Pixel<Int64> BitShiftRSelf(this Pixel<Int64> src, int value) => BitShiftR(src, src, value);
		public static Pixel<Int64> BitShiftR(this Pixel<Int64> src, int value) => BitShiftR(src, PixelFactory.Create<Int64>(src.Maps), value);
        public static Pixel<Int64> BitShiftR(this Pixel<Int64> src, Pixel<Int64> dst, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                    dst[x, y] = (Int64)(src[x,y] >> value);
            return src;
        }

        public static int Count(Pixel<Byte> src1, Pixel<Byte> src2, Byte thr)
        {
			int count = 0;
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    if((src1[x, y] - src2[x,y]) > thr) count++;
            return count;
        }
        public static int Count(Pixel<UInt16> src1, Pixel<UInt16> src2, UInt16 thr)
        {
			int count = 0;
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    if((src1[x, y] - src2[x,y]) > thr) count++;
            return count;
        }
        public static int Count(Pixel<UInt32> src1, Pixel<UInt32> src2, UInt32 thr)
        {
			int count = 0;
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    if((src1[x, y] - src2[x,y]) > thr) count++;
            return count;
        }
        public static int Count(Pixel<UInt64> src1, Pixel<UInt64> src2, UInt64 thr)
        {
			int count = 0;
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    if((src1[x, y] - src2[x,y]) > thr) count++;
            return count;
        }
        public static int Count(Pixel<Int16> src1, Pixel<Int16> src2, Int16 thr)
        {
			int count = 0;
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    if((src1[x, y] - src2[x,y]) > thr) count++;
            return count;
        }
        public static int Count(Pixel<Int32> src1, Pixel<Int32> src2, Int32 thr)
        {
			int count = 0;
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    if((src1[x, y] - src2[x,y]) > thr) count++;
            return count;
        }
        public static int Count(Pixel<Int64> src1, Pixel<Int64> src2, Int64 thr)
        {
			int count = 0;
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    if((src1[x, y] - src2[x,y]) > thr) count++;
            return count;
        }
        public static int Count(Pixel<Single> src1, Pixel<Single> src2, Single thr)
        {
			int count = 0;
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    if((src1[x, y] - src2[x,y]) > thr) count++;
            return count;
        }
        public static int Count(Pixel<Double> src1, Pixel<Double> src2, Double thr)
        {
			int count = 0;
            for (int y = 0; y < src1.Height; y++)
                for (int x = 0; x < src1.Width; x++)
                    if((src1[x, y] - src2[x,y]) > thr) count++;
            return count;
        }
        public static double Average(this Pixel<Byte> src)
        {
			double dst = 0;
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
					dst+=src[x,y];
			dst/=(src.Width * src.Height);
				
            return dst;
        }
		public static double AverageBayer(this Pixel<Byte> src,int bayer_x,int bayer_y)
        {
			double dst = 0;
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
					dst+=src.Bayer(x,y);
			dst/=(src.BayerWidth * src.BayerHeight);
				
            return dst;
        }
		public static double DeviationBayer(this Pixel<Byte> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			double ave = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					ave+=src.Bayer(x,y);
				}	
			ave/=(src.BayerWidth * src.BayerHeight);

			double dev = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					dev += (src.Bayer(x,y) - ave) * (src.Bayer(x,y) - ave);
				}	
			dev = System.Math.Sqrt(dev / (src.BayerWidth * src.BayerHeight));

            return dev;
        }
		public static double VDeviationBayer(this Pixel<Byte> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			//縦平均
			var c = new double[src.BayerWidth];
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					c[x]+=src.Bayer(x,y);
				}
            for (int x = 0; x < src.BayerWidth; x++)
			{
				c[x]/=src.BayerHeight;
			}
			
			
			double ave = 0;
            foreach (var i in c)
				ave+=i;
			ave/=(src.BayerWidth);


			double dev = 0;
            foreach (var i in c)
				dev += (i - ave) * (i - ave);

			dev = System.Math.Sqrt(dev / src.BayerWidth);

            return dev;
        }
        public static double Average(this Pixel<UInt16> src)
        {
			double dst = 0;
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
					dst+=src[x,y];
			dst/=(src.Width * src.Height);
				
            return dst;
        }
		public static double AverageBayer(this Pixel<UInt16> src,int bayer_x,int bayer_y)
        {
			double dst = 0;
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
					dst+=src.Bayer(x,y);
			dst/=(src.BayerWidth * src.BayerHeight);
				
            return dst;
        }
		public static double DeviationBayer(this Pixel<UInt16> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			double ave = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					ave+=src.Bayer(x,y);
				}	
			ave/=(src.BayerWidth * src.BayerHeight);

			double dev = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					dev += (src.Bayer(x,y) - ave) * (src.Bayer(x,y) - ave);
				}	
			dev = System.Math.Sqrt(dev / (src.BayerWidth * src.BayerHeight));

            return dev;
        }
		public static double VDeviationBayer(this Pixel<UInt16> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			//縦平均
			var c = new double[src.BayerWidth];
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					c[x]+=src.Bayer(x,y);
				}
            for (int x = 0; x < src.BayerWidth; x++)
			{
				c[x]/=src.BayerHeight;
			}
			
			
			double ave = 0;
            foreach (var i in c)
				ave+=i;
			ave/=(src.BayerWidth);


			double dev = 0;
            foreach (var i in c)
				dev += (i - ave) * (i - ave);

			dev = System.Math.Sqrt(dev / src.BayerWidth);

            return dev;
        }
        public static double Average(this Pixel<UInt32> src)
        {
			double dst = 0;
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
					dst+=src[x,y];
			dst/=(src.Width * src.Height);
				
            return dst;
        }
		public static double AverageBayer(this Pixel<UInt32> src,int bayer_x,int bayer_y)
        {
			double dst = 0;
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
					dst+=src.Bayer(x,y);
			dst/=(src.BayerWidth * src.BayerHeight);
				
            return dst;
        }
		public static double DeviationBayer(this Pixel<UInt32> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			double ave = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					ave+=src.Bayer(x,y);
				}	
			ave/=(src.BayerWidth * src.BayerHeight);

			double dev = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					dev += (src.Bayer(x,y) - ave) * (src.Bayer(x,y) - ave);
				}	
			dev = System.Math.Sqrt(dev / (src.BayerWidth * src.BayerHeight));

            return dev;
        }
		public static double VDeviationBayer(this Pixel<UInt32> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			//縦平均
			var c = new double[src.BayerWidth];
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					c[x]+=src.Bayer(x,y);
				}
            for (int x = 0; x < src.BayerWidth; x++)
			{
				c[x]/=src.BayerHeight;
			}
			
			
			double ave = 0;
            foreach (var i in c)
				ave+=i;
			ave/=(src.BayerWidth);


			double dev = 0;
            foreach (var i in c)
				dev += (i - ave) * (i - ave);

			dev = System.Math.Sqrt(dev / src.BayerWidth);

            return dev;
        }
        public static double Average(this Pixel<UInt64> src)
        {
			double dst = 0;
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
					dst+=src[x,y];
			dst/=(src.Width * src.Height);
				
            return dst;
        }
		public static double AverageBayer(this Pixel<UInt64> src,int bayer_x,int bayer_y)
        {
			double dst = 0;
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
					dst+=src.Bayer(x,y);
			dst/=(src.BayerWidth * src.BayerHeight);
				
            return dst;
        }
		public static double DeviationBayer(this Pixel<UInt64> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			double ave = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					ave+=src.Bayer(x,y);
				}	
			ave/=(src.BayerWidth * src.BayerHeight);

			double dev = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					dev += (src.Bayer(x,y) - ave) * (src.Bayer(x,y) - ave);
				}	
			dev = System.Math.Sqrt(dev / (src.BayerWidth * src.BayerHeight));

            return dev;
        }
		public static double VDeviationBayer(this Pixel<UInt64> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			//縦平均
			var c = new double[src.BayerWidth];
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					c[x]+=src.Bayer(x,y);
				}
            for (int x = 0; x < src.BayerWidth; x++)
			{
				c[x]/=src.BayerHeight;
			}
			
			
			double ave = 0;
            foreach (var i in c)
				ave+=i;
			ave/=(src.BayerWidth);


			double dev = 0;
            foreach (var i in c)
				dev += (i - ave) * (i - ave);

			dev = System.Math.Sqrt(dev / src.BayerWidth);

            return dev;
        }
        public static double Average(this Pixel<Int16> src)
        {
			double dst = 0;
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
					dst+=src[x,y];
			dst/=(src.Width * src.Height);
				
            return dst;
        }
		public static double AverageBayer(this Pixel<Int16> src,int bayer_x,int bayer_y)
        {
			double dst = 0;
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
					dst+=src.Bayer(x,y);
			dst/=(src.BayerWidth * src.BayerHeight);
				
            return dst;
        }
		public static double DeviationBayer(this Pixel<Int16> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			double ave = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					ave+=src.Bayer(x,y);
				}	
			ave/=(src.BayerWidth * src.BayerHeight);

			double dev = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					dev += (src.Bayer(x,y) - ave) * (src.Bayer(x,y) - ave);
				}	
			dev = System.Math.Sqrt(dev / (src.BayerWidth * src.BayerHeight));

            return dev;
        }
		public static double VDeviationBayer(this Pixel<Int16> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			//縦平均
			var c = new double[src.BayerWidth];
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					c[x]+=src.Bayer(x,y);
				}
            for (int x = 0; x < src.BayerWidth; x++)
			{
				c[x]/=src.BayerHeight;
			}
			
			
			double ave = 0;
            foreach (var i in c)
				ave+=i;
			ave/=(src.BayerWidth);


			double dev = 0;
            foreach (var i in c)
				dev += (i - ave) * (i - ave);

			dev = System.Math.Sqrt(dev / src.BayerWidth);

            return dev;
        }
        public static double Average(this Pixel<Int32> src)
        {
			double dst = 0;
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
					dst+=src[x,y];
			dst/=(src.Width * src.Height);
				
            return dst;
        }
		public static double AverageBayer(this Pixel<Int32> src,int bayer_x,int bayer_y)
        {
			double dst = 0;
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
					dst+=src.Bayer(x,y);
			dst/=(src.BayerWidth * src.BayerHeight);
				
            return dst;
        }
		public static double DeviationBayer(this Pixel<Int32> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			double ave = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					ave+=src.Bayer(x,y);
				}	
			ave/=(src.BayerWidth * src.BayerHeight);

			double dev = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					dev += (src.Bayer(x,y) - ave) * (src.Bayer(x,y) - ave);
				}	
			dev = System.Math.Sqrt(dev / (src.BayerWidth * src.BayerHeight));

            return dev;
        }
		public static double VDeviationBayer(this Pixel<Int32> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			//縦平均
			var c = new double[src.BayerWidth];
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					c[x]+=src.Bayer(x,y);
				}
            for (int x = 0; x < src.BayerWidth; x++)
			{
				c[x]/=src.BayerHeight;
			}
			
			
			double ave = 0;
            foreach (var i in c)
				ave+=i;
			ave/=(src.BayerWidth);


			double dev = 0;
            foreach (var i in c)
				dev += (i - ave) * (i - ave);

			dev = System.Math.Sqrt(dev / src.BayerWidth);

            return dev;
        }
        public static double Average(this Pixel<Int64> src)
        {
			double dst = 0;
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
					dst+=src[x,y];
			dst/=(src.Width * src.Height);
				
            return dst;
        }
		public static double AverageBayer(this Pixel<Int64> src,int bayer_x,int bayer_y)
        {
			double dst = 0;
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
					dst+=src.Bayer(x,y);
			dst/=(src.BayerWidth * src.BayerHeight);
				
            return dst;
        }
		public static double DeviationBayer(this Pixel<Int64> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			double ave = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					ave+=src.Bayer(x,y);
				}	
			ave/=(src.BayerWidth * src.BayerHeight);

			double dev = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					dev += (src.Bayer(x,y) - ave) * (src.Bayer(x,y) - ave);
				}	
			dev = System.Math.Sqrt(dev / (src.BayerWidth * src.BayerHeight));

            return dev;
        }
		public static double VDeviationBayer(this Pixel<Int64> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			//縦平均
			var c = new double[src.BayerWidth];
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					c[x]+=src.Bayer(x,y);
				}
            for (int x = 0; x < src.BayerWidth; x++)
			{
				c[x]/=src.BayerHeight;
			}
			
			
			double ave = 0;
            foreach (var i in c)
				ave+=i;
			ave/=(src.BayerWidth);


			double dev = 0;
            foreach (var i in c)
				dev += (i - ave) * (i - ave);

			dev = System.Math.Sqrt(dev / src.BayerWidth);

            return dev;
        }
        public static double Average(this Pixel<Single> src)
        {
			double dst = 0;
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
					dst+=src[x,y];
			dst/=(src.Width * src.Height);
				
            return dst;
        }
		public static double AverageBayer(this Pixel<Single> src,int bayer_x,int bayer_y)
        {
			double dst = 0;
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
					dst+=src.Bayer(x,y);
			dst/=(src.BayerWidth * src.BayerHeight);
				
            return dst;
        }
		public static double DeviationBayer(this Pixel<Single> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			double ave = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					ave+=src.Bayer(x,y);
				}	
			ave/=(src.BayerWidth * src.BayerHeight);

			double dev = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					dev += (src.Bayer(x,y) - ave) * (src.Bayer(x,y) - ave);
				}	
			dev = System.Math.Sqrt(dev / (src.BayerWidth * src.BayerHeight));

            return dev;
        }
		public static double VDeviationBayer(this Pixel<Single> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			//縦平均
			var c = new double[src.BayerWidth];
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					c[x]+=src.Bayer(x,y);
				}
            for (int x = 0; x < src.BayerWidth; x++)
			{
				c[x]/=src.BayerHeight;
			}
			
			
			double ave = 0;
            foreach (var i in c)
				ave+=i;
			ave/=(src.BayerWidth);


			double dev = 0;
            foreach (var i in c)
				dev += (i - ave) * (i - ave);

			dev = System.Math.Sqrt(dev / src.BayerWidth);

            return dev;
        }
        public static double Average(this Pixel<Double> src)
        {
			double dst = 0;
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
					dst+=src[x,y];
			dst/=(src.Width * src.Height);
				
            return dst;
        }
		public static double AverageBayer(this Pixel<Double> src,int bayer_x,int bayer_y)
        {
			double dst = 0;
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
					dst+=src.Bayer(x,y);
			dst/=(src.BayerWidth * src.BayerHeight);
				
            return dst;
        }
		public static double DeviationBayer(this Pixel<Double> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			double ave = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					ave+=src.Bayer(x,y);
				}	
			ave/=(src.BayerWidth * src.BayerHeight);

			double dev = 0;
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					dev += (src.Bayer(x,y) - ave) * (src.Bayer(x,y) - ave);
				}	
			dev = System.Math.Sqrt(dev / (src.BayerWidth * src.BayerHeight));

            return dev;
        }
		public static double VDeviationBayer(this Pixel<Double> src,int bayer_x,int bayer_y)
        {
			src.BayerX =bayer_x;
			src.BayerY =bayer_y;

			//縦平均
			var c = new double[src.BayerWidth];
            for (int y = 0; y < src.BayerHeight; y++)
                for (int x = 0; x < src.BayerWidth; x++)
				{
					c[x]+=src.Bayer(x,y);
				}
            for (int x = 0; x < src.BayerWidth; x++)
			{
				c[x]/=src.BayerHeight;
			}
			
			
			double ave = 0;
            foreach (var i in c)
				ave+=i;
			ave/=(src.BayerWidth);


			double dev = 0;
            foreach (var i in c)
				dev += (i - ave) * (i - ave);

			dev = System.Math.Sqrt(dev / src.BayerWidth);

            return dev;
        }
    }
}