using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

using System.IO.Compression;
using System.Threading.Tasks;

namespace Pixels.Math
{
    /*--------------------------------------*/
    //   読み
    /*--------------------------------------*/
    public static class PixelsMathEx
    {
        public static void BitShiftL(this Byte a, int b) => a <<= b;
        public static void BitShiftR(this Byte a, int b) => a >>= b;

        public static void BitShiftL(this UInt16 a, int b) => a <<= b;
        public static void BitShiftR(this UInt16 a, int b) => a >>= b;

        public static void BitShiftL(this UInt32 a, int b) => a <<= b;
        public static void BitShiftR(this UInt32 a, int b) => a >>= b;

        public static void BitShiftL(this UInt64 a, int b) => a <<= b;
        public static void BitShiftR(this UInt64 a, int b) => a >>= b;

        public static void BitShiftL(this Int16 a, int b) => a <<= b;
        public static void BitShiftR(this Int16 a, int b) => a >>= b;

        public static void BitShiftL(this Int32 a, int b) => a <<= b;
        public static void BitShiftR(this Int32 a, int b) => a >>= b;

        public static void BitShiftL(this Int64 a, int b) => a <<= b;
        public static void BitShiftR(this Int64 a, int b) => a >>= b;

    }
}