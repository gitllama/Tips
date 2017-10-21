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

    public static class PixelFilter
    {
        public static Pixel<T> Cut<T>(this Pixel<T> src) where T : struct, IComparable
        {
            var dst = new T[src.Width * src.Height];
            for (int i = 0; i < dst.Length; i++)
                    dst[i] = src[i];
            return PixelFactory.Create(src.Width, src.Height, dst);
        }

        public static Pixel<T> StaggerL<T>(this Pixel<T> src) where T : struct, IComparable
        {
            for (int y = 1; y < src.Height; y += 2)
                for (int x = 0; x < src.Width -1; x++)
                    src[x,y] = src[x+1, y];
            return src;
        }
        public static Pixel<T> StaggerR<T>(this Pixel<T> src) where T : struct, IComparable
        {
            for (int y = 1; y < src.Height; y += 2)
                for (int x = src.Width - 1; x > 0; x--)
                    src[x, y] = src[x-1, y];
            return src;
        }

        public static Pixel<T> FilterMedian<T>(this Pixel<T> src, int WindowX = 5, int WindowY = 5, int rank = 12) where T : struct, IComparable
        {
            return src.FilterMedian(null, WindowX, WindowY, rank);
        }
        public static Pixel<T> FilterMedian<T>(this Pixel<T> src, Pixel<T> dst, int WindowX = 5, int WindowY = 5, int rank = 12) where T : struct, IComparable
        {
            if (dst == null) dst = src.Clone();

            int boxsize = WindowX * WindowY;

            int before_center = rank - 1;

            int startX = WindowX / 2;
            int startY = WindowY / 2;
            int endX = src.Width - (WindowX - startX - 1);
            int endY = src.Height - (WindowY - startY - 1);

            int endline = WindowX - 1;

            //配列生成
            T[] box = new T[boxsize];

            //参照座標生成
            int[] col = new int[boxsize];
            int i = 0;
            for (int y = 0; y < WindowY; y++)
                for (int x = 0; x < WindowX; x++)
                    col[i++] = x + y * src.Width;
            //本体
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    for (int n = 0; n < boxsize; n++) box[n] = src.pixel[col[n]++];
                    Array.Sort(box);
                    dst[x,y] = box[rank];
                }
                //ライン送り
                for (int n = 0; n < boxsize; n++) col[n] += endline;
                //token.Token.ThrowIfCancellationRequested();
            }
            return dst;
        }

        public static Pixel<T> FilterMedianBayer<T>(this Pixel<T> src, int WindowX = 5, int WindowY = 5, int rank = 12) where T : struct, IComparable
        {
            return src.FilterMedianBayer(null, WindowX, WindowY, rank);
        }
        public static Pixel<T> FilterMedianBayer<T>(this Pixel<T> src, Pixel<T> dst, int WindowX = 5, int WindowY = 5, int rank = 12) where T : struct, IComparable
        {
            if (dst == null) dst = src.Clone();

            //matrix
            int[] matrix = new int[WindowX * WindowY];
            int c = 0;
            for (int y = 0; y < WindowY; y++)
                for (int x = 0; x < WindowX; x++)
                    matrix[c++] = src.ConvBayerPoison(x - (WindowX / 2), y - (WindowY / 2));

            //本体
            _FilterMedian(
                src, 
                dst, 
                rank,
                matrix,
                (WindowX / 2) * src.BayerSizeX,
                (WindowY / 2) * src.BayerSizeY,
                (WindowX - WindowX / 2) * src.BayerSizeX,
                (WindowY - WindowY / 2) * src.BayerSizeY);

            
                return dst;
        }

        private static void _FilterMedian<T>(Pixel<T> src, Pixel<T> dst, int rank, int[] matrix, int sx,int sy,int ex,int ey) where T : struct, IComparable
        {
            T[] box = new T[matrix.Length];
            for (int y = sy; y < src.Height - ey; y++)
                for (int x = sx; x < src.Width - ex; x++)
                {
                    for (int i = 0; i < matrix.Length; i++)
                    {
                        box[i] = src.pixel[src.ConvMapPoison(x, y) + matrix[i]];
                    }
                    Array.Sort(box);
                    dst.pixel[dst.ConvMapPoison(x, y)] = box[rank];
                }
        }

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

//public static Pixel<T> BitShift<T>(this Pixel<T> src, int value) where T : struct, IComparable
//{
//    switch ((object)src)
//    {
//        case Pixel<int> p:
//            for (int y = 0; y < p.Height; y++)
//                for (int x = 0; x < p.Width; x++)
//                    p[x, y].BitShiftR(value);
//            break;
//    }
//    return src;
//}