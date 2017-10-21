using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixels2.Math
{
    public static class PixelMath
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public static bool isBetween<T>(this T current, T lower, T higher, bool inclusive = true) where T : IComparable
        {
            // 拡張メソッドは1つ目の引数に this キーワードを付ける

            // ジェネリックだと比較演算子が使えなくなってしまうので、
            // where句 で型パラメーター T が IComparable<T> インターフェイスを実装するように指定
            // CompareTo() メソッドが使えるようになる。
            if (lower.CompareTo(higher) > 0) Swap(ref lower, ref higher);

            return inclusive ?
                (lower.CompareTo(current) <= 0 && current.CompareTo(higher) <= 0) :
                (lower.CompareTo(current) < 0 && current.CompareTo(higher) < 0);
        }
    }
}
