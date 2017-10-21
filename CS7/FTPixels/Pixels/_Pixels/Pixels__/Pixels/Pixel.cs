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

using System.Diagnostics;       //for Conditional
using System.Threading.Tasks;


////.Net4.5以降
//#if !dNet3_5
//#endif

//６４ビット化
/**/
//低精度のオペレータ内キャスト未フォロ
//ヴゅわーはサイズ分割でメモリ保存
/**/
//fixedを入れるよ

/*リリースの時だけDLLマージするとき（NET4.0以降)*/
//set fname_new =$(TargetName)_merge$(TargetExt)
//if $(ConfigurationName) == Release(
//  "C:\Program Files\Microsoft\ILMerge\ILMerge" /wildcards /out:%fname_new% $(TargetFileName) *.dll /targetplatform:v4,"C:\Windows\Microsoft.NET\Framework\v4.0.30319"
//)

//キャンセルトークンの仕込み

/*******************************************************************************/
/*   Pixels                                                                    */
/*   2016/03/22                                                                */
/*******************************************************************************/
[assembly: AssemblyVersionAttribute("0.2.0.0")]
[assembly: ComVisible(false)]
namespace Pixels
{
    #region 列挙

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
    public class PArray<T> where T : struct, IConvertible, IComparable
    {
        public T[] p;
        public int w;
        public int h;
    }

    #endregion
    #region PixelBase<T>
    /// <summary>
    /// Pixelクラスの基底/アブストラクト
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class PixelBase<T> where T : struct, IConvertible, IComparable
    {
        /*--------------------------------------*/
        //   コンストラクタ/プロパティ・インデクサ 列挙
        /*--------------------------------------*/
        #region プロパティ・インデクサ

        protected T[] pixel;
        public CancellationTokenSource token;

        public int Width { get; protected set; }    //幅
        public int Height { get; protected set; }   //高さ
        public int Size { get; protected set; }     //サイズ

        //値変化チェック
        /// <summary>
        /// 画素配列を取得する
        /// </summary>
        /// <returns></returns>
        public T[] ToArray() => pixel;

        /// <summary>
        /// 画素配列と幅、高さをクラスとして取得する
        /// </summary>
        /// <returns></returns>
        public PArray<T> ToPArray()
        {
            PArray<T> buf = new PArray<T>();
            buf.p = pixel;
            buf.w = Width;
            buf.h = Height;
            return buf;
        }

        public T this[int x, int y]
        {
            protected set { pixel[x + y * this.Width] = value; }
            get { return this.pixel[x + y * this.Width]; }
        }
        public T this[int x]
        {
            protected set { pixel[x] = value; }
            get { return this.pixel[x]; }
        }

        #endregion
        #region 初期化
        public PixelBase(T[] buf, int Width, int Height, CancellationTokenSource tok)
        {
            this.Width = Width;
            this.Height = Height;
            this.Size = Width * Height;

            if (buf.Length != Size) throw new ArgumentOutOfRangeException("new");
            if (int.MaxValue <= Size) throw new OverflowException("Over MaxValue of int");

            this.pixel = buf;//参照コピー（コンストラクタ呼び出し時は値コピー

            //this.pixel = new double[Size]; <-いらんかったんや
            //buf.CopyTo(pixel, 0);//値コピー
            //initmemo();
            token = tok;
        }
        public PixelBase(T[] buf, int Width, int Height) : this(buf, Width, Height, new CancellationTokenSource())
        {

        }
        public PixelBase(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            this.Size = Width * Height;

            if (int.MaxValue <= Size) throw new OverflowException("Over MaxValue of int");

            this.pixel = new T[this.Size];
            token = new CancellationTokenSource();
        }
        //後評価なのでrefでなくても速度低下しないかも
        //メモリかぶりしないなら、pixelはコンストラクタのみ初期化統一でいいかも

        #endregion

        #region 変換サポート

        public TOutput[] clone<TOutput>()
        {
            TOutput[] buf = new TOutput[this.Size];
            //Buffer.BlockCopy(this.pixel,0,buf,バイト数);
            Array.Copy(this.pixel, buf, buf.Length);
            return buf;
        }
        //public PixelByte Clone() => base.clone<byte>().Create(Width, Height);



        //        public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array1, TInput[] array2, Func<TInput, TInput, TOutput> conv)
        //            where TOutput : struct
        //        {
        //            if (array1.Length != array2.Length)
        //                throw new ArgumentOutOfRangeException("Array Size");

        //            TOutput[] result = new TOutput[array1.Length];

        //#if MultipleCore
        //            普通の方が圧倒的に早い
        //                        ParallelOptions options = new ParallelOptions(){ MaxDegreeOfParallelism = 4};
        //                        Parallel.For(0, array1.Length, options,
        //                            (i) => result[i] = conv(array1[i], array2[i]));
        //#else
        //            for (int i = 0; i < array1.Length; i++)
        //                result[i] = conv(array1[i], array2[i]);
        //#endif
        //            return result;
        //        }

        #endregion
        #region チェック

        protected static void OperatorCheck(PixelBase<T> x, PixelBase<T> y)
        {
            if (x.Size != y.Size)
                throw new ArgumentOutOfRangeException("OperatorCheck");
        }

        public bool IsNaN<Tin>()
        {
            Func<T, bool> IsNaN;
            
            if (typeof(Tin) == typeof(float)) IsNaN = (n) => float.IsNaN((dynamic)n);
            else if (typeof(Tin) == typeof(double)) IsNaN = (n) => double.IsNaN((dynamic)n);
            else throw new ArgumentOutOfRangeException("typeof");
            
            for (int i = 0; i < this.Size; i++)
                if (IsNaN(this[i])) return true;

            return false;
        }
        public bool IsInfinity<Tin>()
        {
            Func<T, bool> IsInfinity;

            if (typeof(Tin) == typeof(float)) IsInfinity = (n) => float.IsInfinity((dynamic)n);
            else if (typeof(Tin) == typeof(double)) IsInfinity = (n) => double.IsInfinity((dynamic)n);
            else throw new ArgumentOutOfRangeException("typeof");

            for (int i = 0; i < this.Size; i++)
                if (IsInfinity(this[i])) return true;

            return false;
        }
        #endregion

        /*--------------------------------------*/
        //   統計処理
        /*--------------------------------------*/
        #region 統計処理

        //double ave;
        //double dev;
        //double med;
        //double max;
        //double min;

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

            foreach (T i in pixel)
                ave += Convert.ToDouble(i);
            return ave / Size;
        }
        public T Max() { return pixel.Max(); }
        public T Min() { return pixel.Min(); }
        public T Med() { return Rank(0.5); }
        public T Rank(double thr)
        {
            T[] buf = new T[Size];
            pixel.CopyTo(buf, 0);
            Array.Sort(buf);
            return buf[(int)(Size * thr)];
        }

        public double Dev()
        {
            double dev = 0;
            double hoge;
            double buf_ave = Ave();
            for (int i = 0; i < Size; i++)
            {
                hoge = Convert.ToDouble(this[i]);
                dev += (hoge - buf_ave) * (hoge - buf_ave);
            }
            return Math.Sqrt(dev / Size);
        }

        public int Count(INEQUALITY inequality, T thr)
        {
            int count = 0;
            //NaN処理入れる？
            //foreach (double i in pixel)
            //{
            //    if (double.IsNaN(i)) return -1;
            //}
            //fixed (double* p = &pixel[0])

            Func<T, T, bool> judge;
            switch (inequality)
            {
                case INEQUALITY.LessThan: judge = (i, j) => i.CompareTo(j) < 0; break;
                case INEQUALITY.GreaterThan: judge = (i, j) => i.CompareTo(j) > 0; break;
                case INEQUALITY.LessThanOrEqual: judge = (i, j) => i.CompareTo(j) <= 0; break;
                case INEQUALITY.GreaterThanOrEqual: judge = (i, j) => i.CompareTo(j) >= 0; break;
                case INEQUALITY.Equal: judge = (i, j) => i.CompareTo(j) == 0; break;
                case INEQUALITY.NotEqual: judge = (i, j) => i.CompareTo(j) != 0; break;
                default: judge = (i, j) => false; break;
            }

            foreach (T p in pixel) if (judge(p, thr)) count++;
            return count;
        }
        public int[] CountDic(double start, double step, uint n)
        {
            int[] count = new int[n + 2];
            double hoge = start - step;
            int max = (int)(n + 1);

            int buf;
            foreach (T p in pixel)
            {
                buf = (int)Math.Floor((Convert.ToDouble(p) - hoge) / step);
                count[buf < 0 ? 0 : buf > max ? max : buf]++;
            }
            return count;
        }
        public string[] CountDicStr(double start, double step, uint n)
        {
            string[] count = new string[n + 2];
            count[0] = String.Format("n < {0}", start);
            for (int i = 0; i < n; i++)
                count[i + 1] = String.Format("{0} <= n < {1}", start + step * i, start + step * (i + 1));
            count[n + 1] = String.Format("{0} <= n", start + step * n);

            return count;
        }
        public Point[] Pickup(INEQUALITY inequality, T thr)
        {
            Func<T, T, bool> judge;
            List<Point> buf = new List<Point>();
            switch (inequality)
            {
                case INEQUALITY.LessThan: judge = (i, j) => i.CompareTo(j) < 0; break;
                case INEQUALITY.GreaterThan: judge = (i, j) => i.CompareTo(j) > 0; break;
                case INEQUALITY.LessThanOrEqual: judge = (i, j) => i.CompareTo(j) <= 0; break;
                case INEQUALITY.GreaterThanOrEqual: judge = (i, j) => i.CompareTo(j) >= 0; break;
                case INEQUALITY.Equal: judge = (i, j) => i.CompareTo(j) == 0; break;
                case INEQUALITY.NotEqual: judge = (i, j) => i.CompareTo(j) != 0; break;
                default: judge = (i, j) => false; break;
            }

            for (int i=0;i<Size;i++) if (judge(pixel[i], thr)) buf.Add(new Point(i%Width,(int)(i/ Width)));
            return buf.ToArray();
        }

        public Point[] Pickup(T thrUpper, INEQUALITY inequalityUpper, INEQUALITY inequalityLower, T thrLower)
        {
            Func<T, T, bool> judgeU, judgeL;
            List<Point> buf = new List<Point>();
            switch (inequalityUpper)
            {
                case INEQUALITY.LessThan: judgeU = (i, j) => i.CompareTo(j) > 0; break;
                case INEQUALITY.GreaterThan: judgeU = (i, j) => i.CompareTo(j) < 0; break;
                case INEQUALITY.LessThanOrEqual: judgeU = (i, j) => i.CompareTo(j) >= 0; break;
                case INEQUALITY.GreaterThanOrEqual: judgeU = (i, j) => i.CompareTo(j) <= 0; break;
                case INEQUALITY.Equal: judgeU = (i, j) => i.CompareTo(j) == 0; break;
                case INEQUALITY.NotEqual: judgeU = (i, j) => i.CompareTo(j) != 0; break;
                default: judgeU = (i, j) => false; break;
            }
            switch (inequalityLower)
            {
                case INEQUALITY.LessThan: judgeL = (i, j) => i.CompareTo(j) < 0; break;
                case INEQUALITY.GreaterThan: judgeL = (i, j) => i.CompareTo(j) > 0; break;
                case INEQUALITY.LessThanOrEqual: judgeL = (i, j) => i.CompareTo(j) <= 0; break;
                case INEQUALITY.GreaterThanOrEqual: judgeL = (i, j) => i.CompareTo(j) >= 0; break;
                case INEQUALITY.Equal: judgeL = (i, j) => i.CompareTo(j) == 0; break;
                case INEQUALITY.NotEqual: judgeL = (i, j) => i.CompareTo(j) != 0; break;
                default: judgeL = (i, j) => false; break;
            }

            for (int i = 0; i < Size; i++)
            {
                T buf_pix = pixel[i];
                if (judgeU(buf_pix, thrUpper) && judgeL(buf_pix, thrLower))
                    buf.Add(new Point(i % Width, (int)(i / Width)));
            }
                
            return buf.ToArray();
        }

        #endregion

        /*--------------------------------------*/
        //   画素操作
        /*--------------------------------------*/
        #region 深度

        protected T[] limits(Type t)
        {
            T max, min;
            #region Limit Set
            if (t == typeof(byte))
            {
                max = (dynamic)byte.MaxValue;
                min = (dynamic)byte.MinValue;
            }
            else if (t == typeof(ushort))
            {
                max = (dynamic)ushort.MaxValue;
                min = (dynamic)ushort.MinValue;
            }
            else if (t == typeof(uint))
            {
                max = (dynamic)uint.MaxValue;
                min = (dynamic)uint.MinValue;
            }
            else if (t == typeof(ulong))
            {
                max = (dynamic)ulong.MaxValue;
                min = (dynamic)ulong.MinValue;
            }
            else if (t == typeof(short))
            {
                max = (dynamic)short.MaxValue;
                min = (dynamic)short.MinValue;
            }
            else if (t == typeof(int))
            {
                max = (dynamic)int.MaxValue;
                min = (dynamic)int.MinValue;
            }
            else if (t == typeof(long))
            {
                max = (dynamic)long.MaxValue;
                min = (dynamic)long.MinValue;
            }
            else if (t == typeof(float))
            {
                max = (dynamic)float.MaxValue;
                min = (dynamic)float.MinValue;
            }
            else if (t == typeof(double))
            {
                max = (dynamic)double.MaxValue;
                min = (dynamic)double.MinValue;
            }
            else
            {
                throw new ArgumentOutOfRangeException("typeof");
            }
            #endregion

            return limits(max, min);
        }
        protected T[] limits(T Upper,T Lower)
        {
            return Array.ConvertAll<T, T>(pixel, i => i.CompareTo(Upper) > 0 ? Upper : i.CompareTo(Lower) < 0 ? Lower : i);
        }
        protected T[] limitsUpper(T thr)
        {
            T max = (dynamic)thr;
            return Array.ConvertAll<T, T>(pixel, i => i.CompareTo(max) > 0 ? max : i);
        }
        protected T[] limitsLower(T thr)
        {
            T min = (dynamic)thr;
            return Array.ConvertAll<T, T>(pixel, i => i.CompareTo(min) < 0 ? min : i);
        }

        #endregion
        #region サイズ変更
        protected T[] trim(int Left, int Top, int newWidth, int newHeight)
        {
            if ((Left + newWidth) > Width || (Top + newHeight) > Height)
                throw new ArgumentOutOfRangeException("Trim");

            int c = 0;
            T[] result = new T[newWidth * newHeight];
            for (int y = Top; y < newHeight + Top; y++)
                for (int x = Left; x < newWidth + Left; x++)
                    result[c++] = this[x, y];

            return result;
        }
        protected T[] trimBayer(int x, int y, int w = 2, int h = 2)
        {
            int ww = this.Width / w;
            int hh = this.Height / h;
            T[] result = new T[ww * hh];

            int off_x = this.Width - (this.Width % w);
            int off_y = this.Height - (this.Height % h);

            int c = 0;
            for (int j = y; j < off_y; j += h)
                for (int i = x; i < off_x; i += w)
                    result[c++] = this[i, j];
            return result;
        }

        protected T[] InsertRow(T[] inPix)
        {
            T[] ret = new T[Size * 2];

            int c = 0;
            int c_in1 = 0;
            int c_in2 = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                    ret[c++] = this[c_in1++];
                for (int x = 0; x < Width; x++)
                    ret[c++] = inPix[c_in2++];
            }
            return ret;
        }
        protected T[] InsertCol(T[] inPix)
        {
            T[] ret = new T[Size * 2];

            int c = 0;
            int c_in = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    ret[c++] = this[c_in];
                    ret[c++] = inPix[c_in++];
                }
            return ret;
        }

        protected static PArray<T> JoinHorizontal(PixelBase<T> left, PixelBase<T> right)
        {
            T[] buf = new T[left.Size + right.Size];

            int count = 0;
            for (int y = 0; y < left.Height; y++)
            {
                for (int x_left = 0; x_left < left.Width; x_left++) buf[count++] = left[x_left, y];
                for (int x_right = 0; x_right < right.Width; x_right++) buf[count++] = right[x_right, y];
            }
            return new PArray<T>() { p = buf, w = left.Width + right.Width, h = left.Height };
        }
        protected static PArray<T> JoinVertical(PixelBase<T> top, PixelBase<T> bottom)
        {
            T[] buf = new T[top.Size + bottom.Size];
            int count = 0;
            for (int i = 0; i < top.Size; i++) buf[count++] = top[i];
            for (int i = 0; i < bottom.Size; i++) buf[count++] = bottom[i];

            return new PArray<T>() { p = buf, h = top.Height + bottom.Height, w = top.Width };
        }

        /*
    protected T[] Trim2(int Left, int Top, int newWidth, int newHeight)
    {
        if ((Left + newWidth) > Width || (Top + newHeight) > Height)
            throw new ArgumentOutOfRangeException("Trim");
        T[] result = new T[newWidth * newHeight];
        int i = 0;
        int j = Top * Width + Left;
        int ret = Width - newWidth;
        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                result[i++] = this.pixel[j++];
            }
            //new line
            j += ret;
        }
        return result;
    }
    protected T[] GetBayer2(int x, int y, int w = 2, int h = 2)
    {
        int c = 0;
        int newWidth = Width / w
    T[] result = new T[newWidth * newHeight];
        int i = 0;
        int j = Top * Width + Left;
        int ret = Width - newWidth;
        for (int j = 0; j < Height; j += h)
        {
            for (int i = 0; i < Width; i += w)
                result[c++] = this[i + x, j + y];
        }
        return result;
    }
    */
        #endregion

        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/

        #region エリア補間

        protected T[] complement(int left, int right, int top, int bottom)
        {
            T[] ret = new T[Size];

            //真ん中
            for (int y = top; y < Height - bottom; y++)
                for (int x = left; x < Width - right; x++)
                    ret[x + y * Width] = this[x, y];
            //上
            for (int y = 0; y < top; y++)
                for (int x = left; x < Width - right; x++)
                    ret[x + y * Width] = this[x, top];
            //下
            for (int y = Height - bottom; y < Height; y++)
                for (int x = left; x < Width - right; x++)
                    ret[x + y * Width] = this[x, Height - bottom - 1];

            //左
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < left; x++)
                    ret[x + y * Width] = this[left, y];
            //右
            for (int y = 0; y < Height; y++)
                for (int x = Width - right; x < Width; x++)
                    ret[x + y * Width] = this[Width - right - 1, y];

            return ret;
        }

        #endregion

        #region フィルタ/畳み込み演算

        /// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX"></param>
        /// <param name="WindowY"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        protected unsafe T[] FilterMedian(int WindowX = 5, int WindowY = 5, int rank = 12)
        {
            int boxsize = WindowX * WindowY;

            int before_center = rank - 1;

            int startX = WindowX / 2;
            int startY = WindowY / 2;
            int endX = Width - (WindowX - startX - 1);
            int endY = Height - (WindowY - startY - 1);

            int endline = WindowX - 1;

            //配列生成
            T[] result = new T[this.Size];
            T[] box = new T[boxsize];

            //参照座標生成
            int* col = stackalloc int[boxsize];
            int col_r;
            int i = 0;
            for (int y = 0; y < WindowY; y++)
                for (int x = 0; x < WindowX; x++)
                    col[i++] = x + y * Width;
            col_r = col[(int)(WindowX / 2) + (int)(WindowY / 2) * WindowX];

            //本体
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    for (int n = 0; n < boxsize; n++) box[n] = this.pixel[col[n]++];
                    Array.Sort(box);
                    result[col_r++] = box[rank];

                    token.Token.ThrowIfCancellationRequested();
                }
                //ライン送り
                for (int n = 0; n < boxsize; n++) col[n] += endline;
                col_r += endline;
            }
            return result;
        }

        protected unsafe T[] FilterAverage(int WindowX, int WindowY, Func<double, T> Conv)
        {
            int boxsize = WindowX * WindowY;

            int startX = WindowX / 2;
            int startY = WindowY / 2;
            int endX = Width - (WindowX - startX - 1);
            int endY = Height - (WindowY - startY - 1);

            int endline = WindowX - 1;

            //配列生成
            T[] result = new T[this.Size];
            double box;

            //参照座標生成
            int* col = stackalloc int[boxsize];
            int col_r;
            int i = 0;
            for (int y = 0; y < WindowY; y++)
                for (int x = 0; x < WindowX; x++)
                    col[i++] = x + y * Width;
            col_r = col[(int)(WindowX / 2) + (int)(WindowY / 2) * WindowX];

            //本体
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    box = 0;
                    for (int n = 0; n < boxsize; n++) box += Convert.ToDouble(this.pixel[col[n]++]);
                    result[col_r++] = Conv(box / boxsize);
                    token.Token.ThrowIfCancellationRequested();
                }
                //ライン送り
                for (int n = 0; n < boxsize; n++) col[n] += endline;
                col_r += endline;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Xin"></param>
        /// <param name="Yin"></param>
        /// <returns></returns>
        protected unsafe T[] FilterMedian_p0(int Xin, int Yin)
        {
            int boxsideX = Xin * 2 + 1;
            int boxsideY = Yin * 2 + 1;
            int boxsize = boxsideX * boxsideY;
            int boxcenter = boxsize / 2;
            int before_center = (boxsize / 2) - 1;
            int endline = Xin * 2;
            int startX = Xin;
            int startY = Yin;
            int endX = Width - Xin;
            int endY = Height - Yin;
            //配列生成
            T[] result = new T[this.Size];
            T[] box = new T[boxsize];
            //参照座標生成
            int* col = stackalloc int[boxsize];
            int col_r;
            int i = 0;
            for (int y = 0; y < boxsideY; y++)
                for (int x = 0; x < boxsideX; x++)
                    col[i++] = x + y * Width;
            col_r = col[boxcenter];
            //本体
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    for (int n = 0; n < boxsize; n++) box[n] = this.pixel[col[n]++];
                    Array.Sort(box);
                    result[col[before_center]] = box[boxcenter];
                }
                //ライン送り
                for (int n = 0; n < boxsize; n++) col[n] += endline;
                //col_r += endline;
            }
            return result;
        }

        protected unsafe T[] FilterMedian_p2(int Xin, int Yin)
        {
            int boxsideX = Xin * 2 + 1;
            int boxsideY = Yin * 2 + 1;
            int boxsize = boxsideX * boxsideY;
            int boxcenter = boxsize / 2;
            int before_center = (boxsize / 2) - 1;
            int endline = Xin * 2;
            int startX = Xin;
            int startY = Yin;
            int endX = Width - Xin;
            int endY = (Height - Yin) / 2;
            int startY2 = endY;
            int endY2 = Height - Yin;
            //配列生成
            T[] result = new T[this.Size];
            T[] box = new T[boxsize];
            T[] box2 = new T[boxsize];
            //参照座標生成
            int* col = stackalloc int[boxsize];
            int* col2 = stackalloc int[boxsize];

            int i = 0;
            for (int y = 0; y < boxsideY; y++)
                for (int x = 0; x < boxsideX; x++)
                    col[i++] = x + y * Width;

            for (int n = 0; n < boxsize; n++) col2[n] = col[n] + (startY2 - Yin) * Width;
            /*
            Parallel.Invoke(
            () =>
            {
                for (int y = startY; y < endY; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        for (int n = 0; n < boxsize; n++) box[n] = this.pixel[col[n]++];
                        Array.Sort(box);
                        result[col[before_center]] = box[boxcenter];
                    }
                    //ライン送り
                    for (int n = 0; n < boxsize; n++) col[n] += endline;
                }
            },
            () =>
            {
                for (int y2 = startY2; y2 < endY2; y2++)
                {
                    for (int x2 = startX; x2 < endX; x2++)
                    {
                        for (int n2 = 0; n2 < boxsize; n2++) box2[n2] = this.pixel[col2[n2]++];
                        Array.Sort(box2);
                        result[col2[before_center]] = box2[boxcenter];
                    }
                    //ライン送り
                    for (int n2 = 0; n2 < boxsize; n2++) col2[n2] += endline;
                }
            });
            */
            return result;
        }
        protected unsafe T[] FilterMedian_p4(int Xin, int Yin)
        {
            int boxsideX = Xin * 2 + 1;
            int boxsideY = Yin * 2 + 1;
            int boxsize = boxsideX * boxsideY;
            int boxcenter = boxsize / 2;
            int before_center = (boxsize / 2) - 1;
            int endline = Xin * 2;
            int startX = Xin;
            int startY = Yin;
            int endX = Width - Xin;


            int T1 = (Height - Yin * 2) / 4;
            int T2 = T1 + T1;

            int T3 = T2 + T1;
            int endY = Height - Yin;
            //配列生成
            T[] result = new T[this.Size];
            T[] box = new T[boxsize];
            T[] box2 = new T[boxsize];
            T[] box3 = new T[boxsize];
            T[] box4 = new T[boxsize];
            //参照座標生成
            int* col = stackalloc int[boxsize];
            int* col2 = stackalloc int[boxsize];
            int* col3 = stackalloc int[boxsize];
            int* col4 = stackalloc int[boxsize];

            int i = 0;
            for (int y = 0; y < boxsideY; y++)
                for (int x = 0; x < boxsideX; x++)
                    col[i++] = x + y * Width;

            for (int n = 0; n < boxsize; n++) col2[n] = col[n] + (T1 - Yin) * Width;
            for (int n = 0; n < boxsize; n++) col3[n] = col[n] + (T2 - Yin) * Width;
            for (int n = 0; n < boxsize; n++) col4[n] = col[n] + (T3 - Yin) * Width;
            /*
            Parallel.Invoke(
            () =>
            {
                for (int y = startY; y < T1; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        for (int n = 0; n < boxsize; n++) box[n] = this.pixel[col[n]++];
                        Array.Sort(box);
                        result[col[before_center]] = box[boxcenter];
                    }
                    //ライン送り
                    for (int n = 0; n < boxsize; n++) col[n] += endline;
                }
            },
            () =>
            {
                for (int y2 = T1; y2 < T2; y2++)
                {
                    for (int x2 = startX; x2 < endX; x2++)
                    {
                        for (int n2 = 0; n2 < boxsize; n2++) box2[n2] = this.pixel[col2[n2]++];
                        Array.Sort(box2);
                        result[col2[before_center]] = box2[boxcenter];
                    }
                    //ライン送り
                    for (int n2 = 0; n2 < boxsize; n2++) col2[n2] += endline;
                }
            },
            () =>
            {
                for (int y3 = T2; y3 < T3; y3++)
                {
                    for (int x3 = startX; x3 < endX; x3++)
                    {
                        for (int n3 = 0; n3 < boxsize; n3++) box3[n3] = this.pixel[col3[n3]++];
                        Array.Sort(box3);
                        result[col3[before_center]] = box3[boxcenter];
                    }
                    //ライン送り
                    for (int n3 = 0; n3 < boxsize; n3++) col3[n3] += endline;
                }
            },
            () =>
            {
                for (int y4 = T3; y4 < endY; y4++)
                {
                    for (int x4 = startX; x4 < endX; x4++)
                    {
                        for (int n4 = 0; n4 < boxsize; n4++) box4[n4] = this.pixel[col4[n4]++];
                        Array.Sort(box4);
                        result[col4[before_center]] = box4[boxcenter];
                    }
                    //ライン送り
                    for (int n4 = 0; n4 < boxsize; n4++) col4[n4] += endline;

                }
            }
            );
            */
            return result;
        }
        protected unsafe T[] old_FilterMedian()
        {
            T[] box = new T[25];
            T[] result = new T[Size];
            int* co = stackalloc int[25];
            int d;

            //座標の先生成
            d = 0;
            for (int y = -2; y <= 2; y++)
                for (int x = -2; x <= 2; x++)
                    co[d++] = x + Width * y;

            int hoge;
            for (int y = 2; y < Height - 2; y++)
                for (int x = 2; x < Width - 2; x++)
                {
                    d = 0;
                    hoge = x + y * Width;

                    //展開しても良いけどたいしてかわらん
                    for (int i = 0; i < 25; i++)
                        box[i] = this.pixel[hoge + co[i]];

                    //Parallel.For(0, 25, i =>
                    //{
                    //    box[i] = this.pixel[hoge + co[i]];
                    //});
                    Array.Sort(box);
                    result[hoge] = box[12];
                }
            return result;
        }

        //テスト　傷探し
        protected unsafe T[] FilterWDPick(T thr)
        {
            T[] buf = new T[Size];

            Action<int, int> cc = (x, y) =>
            {
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x > Width - 1) x = Width - 1;
                if (y > Height - 1) y = Height - 1;

                buf[x + y * Width] = thr;
            };

            Action<int, int> ch = (x, y) =>
            {
                for (int i = -10; i < 10; i++)
                {
                    cc(x + i, y - 2);
                    cc(x + i, y - 1);
                    cc(x + i, y);
                    cc(x + i, y + 1);
                    cc(x + i, y + 2);
                    cc(x - 2, y + i);
                    cc(x - 1, y + i);
                    cc(x + 0, y + i);
                    cc(x + 1, y + i);
                    cc(x + 2, y + i);
                }

            };

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    if (this[x, y].CompareTo(thr) > 0)
                    {
                        ch(x, y);
                    }
                }

            return buf;
        }


        protected T[] FilterStagger(int shift)
        {
            T[] ret = new T[Size];

            for (int y = 0; y < Height; y+=2)
                for (int x = 0; x < Width; x++)
                    ret[x + y * Width] = this[x, y];

            for (int y = 1; y < Height; y += 2)
                for (int x = 0; x < Width- shift; x++)
                    ret[x+ shift + y * Width] = this[x, y];

            return ret;
        }

        #endregion

        //ThresholdingFilter



    }
    #endregion

}
