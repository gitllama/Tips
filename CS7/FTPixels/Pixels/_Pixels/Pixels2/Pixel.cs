using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixels2
{
    public enum INEQUALITY
    {
        GreaterThan,    // >
        LessThan,       // <
        GreaterThanOrEqual,
        LessThanOrEqual,
        Equal,
        NotEqual
    }
    public enum BMPColor
    {
        R, G, B, A
    }
    public class Pixel<T> where T : struct, IConvertible, IComparable
    {
        public T[] pixel;

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Size { get; protected set; }

        public Type Type { get => typeof(T); }

        //public T this[int x]{ get => this.pixel[x]; set => pixel[x] = value; }
        //public T this[int x, int y] { get => this.pixel[x + y * this.Width]; set => this.pixel[x + y * this.Width] = value;}
        public ref T this[int v] { get => ref this.pixel[v]; }
        public ref T this[int x, int y] { get=> ref this.pixel[x + y * this.Width]; }


        public Pixel(int Width, int Height, T[] buf)
        {
            this.Width = Width;
            this.Height = Height;
            this.Size = Width * Height;

            if (buf.Length != Size) throw new ArgumentOutOfRangeException("new");
            if (int.MaxValue <= Size) throw new OverflowException("Over MaxValue of int");

            this.pixel = buf;//参照コピー（コンストラクタ呼び出し時は値コピー
        }
        public Pixel(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            this.Size = Width * Height;

            this.pixel = new T[Width * Height];
        }


        public Pixel FilterMedian()
        {
            var a = new T[25];

            ref[0] a = ref pixel[1];

        }
    }
}

