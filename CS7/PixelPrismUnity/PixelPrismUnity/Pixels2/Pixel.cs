using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Pixels
{
    public class PixelMap
    {
        public int Left { get; set; } = 0;
        public int Top { get; set; } = 0;
        public int Width { get; set; } = 2;
        public int Height { get; set; } = 2;
    }



    public class Pixel<T> : ICloneable where T : struct, IComparable
    {
        public int Left { get; set; } = 0;
        public int Top { get; set; } = 0;
        public int Width { get; set; } = 1;
        public int Height { get; set; } = 1;
        public int Stride { get; set; } = 1;


        public Type _type;
        public string Type { get=>_type?.Name; set=> _type = System.Type.GetType($"System.{value}"); }

        public CancellationTokenSource token;

        public Dictionary<string, PixelMap> Maps { get; set; }

        public T[] pixel;
        public ref T this[int value] { get => ref pixel[value]; }
        public ref T this[int x, int y] { get => ref pixel[(x + Left) + (y + Top) * Stride]; }

        public Pixel<T> this[string map]
        {
            get
            {
                Left = Maps[map].Left;
                Top = Maps[map].Top;
                Width = Maps[map].Width;
                Height = Maps[map].Height;

                return this;
            }
        }

        public Pixel(int width, int height, T[] src)
        {
            Width = width;
            Height = height;
            Stride = Width;
            pixel = src;
        }
        public Pixel(){ }

        public Pixel<T> Create()
        {
            Stride = Width;
            pixel = new T[Width * Height];
            return this;
        }
        public static Pixel<T> Create<T>(int width, int height) where T : struct, IComparable
        {
            return new Pixel<T>()
            {
                Width = width,
                Height = height
            };
        }
        public static Pixel<T> Create<T>(int width, int height, T[] src) where T : struct, IComparable
        {
            return new Pixel<T>(width, height, src);
        }

        public Pixel<T> Map(string value)
        {
            Left = Maps[value].Left;
            Top  = Maps[value].Top;
            Width = Maps[value].Width;
            Height = Maps[value].Height;

            return this;
        }

        public Pixel<T> Cancellation(CancellationTokenSource token)
        {
            this.token = token;
            return this;
        }

        public object Clone()
        {
            var i = new Pixel<T>(Width, Height, pixel)
            {
                Maps = Maps,
                Type = Type
            };
            return i;
        }
    }

    //public class PixelVector
    //{
    //    public int Left { get; set; } = 0;
    //    public int Top { get; set; } = 0;
    //    public int Width { get; set; } = 2256;
    //    public int Height { get; set; } = 1178;

    //    public Dictionary<string, PixelMap> Maps;

    //    public Vector[] pixel;
    //    //public ref T this[int value] { get => ref pixel[value]; }
    //    //public ref T this[int x, int y] { get => ref pixel[(x + Left) + (y + Top) * Width]; }

    //    public PixelVector(int width, int height, T[] src)
    //    {
    //        Width = width;
    //        Height = height;
    //        pixel = src;
    //    }
    //    public PixelVector(int width, int height)
    //    {
    //        Width = width;
    //        Height = height;
    //        pixel = new T[Width * Height];

    //    }

    //    public Pixel<T> Map(string value)
    //    {
    //        Left = Maps[value].Left;
    //        Top = Maps[value].Top;
    //        Width = Maps[value].Width;
    //        Height = Maps[value].Height;

    //        return this;
    //    }
    //}
}
