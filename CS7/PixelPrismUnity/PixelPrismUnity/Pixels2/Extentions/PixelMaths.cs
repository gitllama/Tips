using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pixels.Math;

namespace Pixels
{
    public enum BMPColor
    {
        R, G, B, A
    }
    public enum INEQUALITY
    {
        GreaterThan,    // >
        LessThan,       // <
        GreaterThanOrEqual,
        LessThanOrEqual,
        Equal,
        NotEqual
    }
    public static class PixelMath
    {
        public static Pixel<int> Average(this Pixel<int> src)
        {
            double dst = 0;
            int count = 0;
            for(int y=0;y<src.Height;y++)
                for(int x=0;x<src.Width;x++)
                {
                    dst += src[x, y];
                    count++;
                }
            dst /= count;
            Debug.WriteLine(dst);
            Debug.WriteLine(count);

            return src;
        }

        public static Pixel<T> BitShiftL<T>(this Pixel<T> src, int value) where T : struct, IComparable
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                {
                    src[x, y].BitShiftL(value);
                    //src[x, y] <<= value;
                }
            return src;
        }
        public static Pixel<int> BitShiftR(this Pixel<int> src, int value)
        {
            for (int y = 0; y < src.Height; y++)
                for (int x = 0; x < src.Width; x++)
                {
                    src[x, y] >>= value;
                }
            return src;
        }

    }

    

    public static class PixelFilter
    {
        public static Pixel<T> Cut<T>(this Pixel<T> src) where T : struct, IComparable
        {
            var dst = new T[src.Width * src.Height];
            for (int i = 0; i < dst.Length; i++)
                    dst[i] = src[i];
            return new Pixel<T>(src.Width, src.Height, dst);
        }

        public static Pixel<T> StaggerL<T>(this Pixel<T> src) where T : struct, IComparable
        {
            for (int y = 1; y < src.Height; y += 2)
                for (int x = 0; x < src.Width -1; x++)
                    src[x,y] = src[x+1, y];
            return src;
        }

        //protected static Pixel<T> FilterMedian<T>(this Pixel<T> src, int WindowX = 5, int WindowY = 5, int rank = 12) where T : struct, IComparable
        //{
        //    int boxsize = WindowX * WindowY;

        //    int before_center = rank - 1;

        //    int startX = WindowX / 2;
        //    int startY = WindowY / 2;
        //    int endX = src.Width - (WindowX - startX - 1);
        //    int endY = src.Height - (WindowY - startY - 1);

        //    int endline = WindowX - 1;

        //    //配列生成
        //    T[] result = new T[src.Height * src.Width];
        //    T[] box = new T[boxsize];

        //    //参照座標生成
        //    int[] col = new int[boxsize];
        //    int col_r;
        //    int i = 0;
        //    for (int y = 0; y < WindowY; y++)
        //        for (int x = 0; x < WindowX; x++)
        //            col[i++] = x + y * src.Width;
        //    col_r = col[(int)(WindowX / 2) + (int)(WindowY / 2) * WindowX];

        //    //本体
        //    for (int y = startY; y < endY; y++)
        //    {
        //        for (int x = startX; x < endX; x++)
        //        {
        //            for (int n = 0; n < boxsize; n++) box[n] = src.pixel[col[n]++];
        //            Array.Sort(box);
        //            result[col_r++] = box[rank];

        //            //token.Token.ThrowIfCancellationRequested();
        //        }
        //        //ライン送り
        //        for (int n = 0; n < boxsize; n++) col[n] += endline;
        //        col_r += endline;
        //    }
        //    return null;
        //}
    }
}


//switch (Type.GetTypeCode(type))
//{
//    case TypeCode.Int32:
//        // It's an int
//        break;

//    case TypeCode.String:
//        // It's a string
//        break;