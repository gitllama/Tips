using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

//.Net4.5以降
#if !dNet3_5
using System.Threading.Tasks;
#endif

/*******************************************************************************/
/*   Pixels                                                                    */
/*   2016/03/18                                                                */
/*******************************************************************************/
namespace Pixels
{
    /*--------------------------------------*/
    //   画素操作
    /*--------------------------------------*/
	#region 画素操作
    public partial class PixelByte
    {
		#region 深度
		
		/// <summary>
        /// 型名にあった上限値, 下限値に丸める
        /// </summary>
        /// <param name="type">typeof(型名)</param>
        /// <returns></returns>
		public PixelByte Limits(Type type) => (base.limits(type)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値, 下限値に丸める
        /// </summary>
        /// <param name="Upper">上限値</param>
        /// <param name="Lower">下限値</param>
        /// <returns></returns>
		public PixelByte Limits(byte Upper,byte Lower) => (base.limits(Upper, Lower)).Create(Width, Height, base.token);
		/// <summary>
        /// 下限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelByte LimitsLower(byte thr) => (base.limitsLower(thr)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelByte LimitsUpper(byte thr) => (base.limitsUpper(thr)).Create(Width, Height, base.token);

		#endregion
		#region サイズ変更

		/// <summary>
        /// 画像をトリムする
        /// </summary>
        /// <param name="left">x原点</param>
        /// <param name="top">y原点</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns></returns>
        public PixelByte Trim(int left, int top, int width, int height) => (base.trim(left, top, width, height)).Create(width, height, base.token);
		/// <summary>
        /// 特定のベイヤを取り出す
        /// </summary>
        /// <param name="x">取り出しベイヤのx座標</param>
        /// <param name="y">取り出しベイヤのy座標</param>
        /// <param name="width">ベイヤの幅（省略時 = 2）</param>
        /// <param name="height">ベイヤの幅（省略時 = 2）</param>
        /// <returns></returns>
        public PixelByte TrimBayer(int x, int y,int width = 2,int height = 2) => (base.trimBayer(x, y, width , height)).Create(this.Width / width, this.Height / height, base.token);
		
		/// <summary>
        /// 縦方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelByte InsertRow(PixelByte inPix) => (base.InsertRow(inPix.ToArray())).Create(Width, Height * 2, base.token);
		/// <summary>
        /// 横方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelByte InsertCol(PixelByte inPix) => (base.InsertCol(inPix.ToArray())).Create(Width * 2, Height, base.token);
		/// <summary>
        /// 4画素ベイヤーに並び替える
        /// </summary>
        /// <returns></returns>
		public static PixelByte InsertBayer(PixelByte UpperLeft,PixelByte UpperRight,PixelByte LowerLeft, PixelByte LowerRight) => (UpperLeft.InsertCol(UpperRight)).InsertRow(LowerLeft.InsertCol(LowerRight));

		/// <summary>
        /// 横に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelByte JoinH(PixelByte left, PixelByte right) => PixelBase<byte>.JoinHorizontal(left, right).Create(left.token);
		/// <summary>
        /// 縦に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelByte JoinV(PixelByte top, PixelByte bottom) => PixelBase<byte>.JoinVertical(top, bottom).Create(top.token);

		#endregion


		
        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/
		#region エリア補間

		/// <summary>
        /// 周辺の補間をする（フィルタ使用時など）
		/// V補間->H補間の順,（ストリーキング考慮）
        /// </summary>
        /// <returns></returns>
		public PixelByte Complement(int left, int right, int top, int bottom) => (base.complement(left, right, top, bottom)).Create(Width, Height, base.token);

		#endregion
		#region フィルタ/畳み込み演算
		
		/// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <param name="rank"></param>
        /// <returns></returns>
		public PixelByte FilMed(int WindowX = 5, int WindowY = 5, int rank = 12) => FilterMedian(WindowX, WindowY, rank).Create(Width, Height, base.token);
        /// <summary>
        /// [フィルタ]アベレージフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <returns></returns>
		public PixelByte FilAve(int WindowX = 5, int WindowY = 5) => FilterAverage(WindowX, WindowY, i => (byte)i).Create(Width, Height, base.token);
 
		/// <summary>
        /// [フィルタ]行方向プロファイル/列方向平均
        /// </summary>
        /// <returns></returns>
 		public PixelByte FilRowProfileAve() => FilterAverage(1, Height, i => (byte)i).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
		
		/// <summary>
        /// [フィルタ]列方向プロファイル/行方向平均
        /// </summary>
        /// <returns></returns>
        public PixelByte FilColProfileAve() => FilterAverage(Width, 1, i => (byte)i).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);

		public PixelByte FilRowProfileMed() => FilterMedian(1, Height, Height/2).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
        public PixelByte FilColProfileMed() => FilterMedian(Width, 1, Width/2).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);
  
		public PixelByte FilRowProfileIGXL() 
			=> FilMed(1, 63, 31).Trim(0, 31, Width, Height - 62).FilAve(1,Height - 62).Trim(0, (Height - 62)/2, Width, 1);
        public PixelByte FilColProfileIGXL()
			=> FilMed(63, 1, 31).Trim(31, 0, Width - 62, Height).FilAve(Width - 62,1).Trim(0, (Width - 62)/2, 1, Height);

		public PixelByte FilWDPick(byte thr) => FilterWDPick(thr).Create(Width, Height, base.token);
		
		
		/// <summary>
        /// Windowに対してオフセット減算する
        /// （ColProfileならばVOB補正）
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
		public PixelInt OffsetCorrection(PixelByte window)
		{
		    int[] ret = new int[Size];

			int w = window.Width;
			int h = window.Height;
            Func<int,int,int> co = (x,y) =>
            {
				int xx = x % w;
                int yy = y % h;
                return xx + yy * w;
            };

            int c = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
				{
                    ret[c] = (int)(pixel[c] - window[co(x,y)]);
					c++;
				}

            return ret.Create(Width, Height, base.token);		
		}

		/// <summary>
        /// 奇数行（0開始）をshift画素ズラす
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
		public PixelByte Stagger(int shift = 1) => FilterStagger(shift).Create(Width, Height, base.token);

		#endregion
	}
    public partial class PixelUShort
    {
		#region 深度
		
		/// <summary>
        /// 型名にあった上限値, 下限値に丸める
        /// </summary>
        /// <param name="type">typeof(型名)</param>
        /// <returns></returns>
		public PixelUShort Limits(Type type) => (base.limits(type)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値, 下限値に丸める
        /// </summary>
        /// <param name="Upper">上限値</param>
        /// <param name="Lower">下限値</param>
        /// <returns></returns>
		public PixelUShort Limits(ushort Upper,ushort Lower) => (base.limits(Upper, Lower)).Create(Width, Height, base.token);
		/// <summary>
        /// 下限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelUShort LimitsLower(ushort thr) => (base.limitsLower(thr)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelUShort LimitsUpper(ushort thr) => (base.limitsUpper(thr)).Create(Width, Height, base.token);

		#endregion
		#region サイズ変更

		/// <summary>
        /// 画像をトリムする
        /// </summary>
        /// <param name="left">x原点</param>
        /// <param name="top">y原点</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns></returns>
        public PixelUShort Trim(int left, int top, int width, int height) => (base.trim(left, top, width, height)).Create(width, height, base.token);
		/// <summary>
        /// 特定のベイヤを取り出す
        /// </summary>
        /// <param name="x">取り出しベイヤのx座標</param>
        /// <param name="y">取り出しベイヤのy座標</param>
        /// <param name="width">ベイヤの幅（省略時 = 2）</param>
        /// <param name="height">ベイヤの幅（省略時 = 2）</param>
        /// <returns></returns>
        public PixelUShort TrimBayer(int x, int y,int width = 2,int height = 2) => (base.trimBayer(x, y, width , height)).Create(this.Width / width, this.Height / height, base.token);
		
		/// <summary>
        /// 縦方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelUShort InsertRow(PixelUShort inPix) => (base.InsertRow(inPix.ToArray())).Create(Width, Height * 2, base.token);
		/// <summary>
        /// 横方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelUShort InsertCol(PixelUShort inPix) => (base.InsertCol(inPix.ToArray())).Create(Width * 2, Height, base.token);
		/// <summary>
        /// 4画素ベイヤーに並び替える
        /// </summary>
        /// <returns></returns>
		public static PixelUShort InsertBayer(PixelUShort UpperLeft,PixelUShort UpperRight,PixelUShort LowerLeft, PixelUShort LowerRight) => (UpperLeft.InsertCol(UpperRight)).InsertRow(LowerLeft.InsertCol(LowerRight));

		/// <summary>
        /// 横に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelUShort JoinH(PixelUShort left, PixelUShort right) => PixelBase<ushort>.JoinHorizontal(left, right).Create(left.token);
		/// <summary>
        /// 縦に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelUShort JoinV(PixelUShort top, PixelUShort bottom) => PixelBase<ushort>.JoinVertical(top, bottom).Create(top.token);

		#endregion


		
        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/
		#region エリア補間

		/// <summary>
        /// 周辺の補間をする（フィルタ使用時など）
		/// V補間->H補間の順,（ストリーキング考慮）
        /// </summary>
        /// <returns></returns>
		public PixelUShort Complement(int left, int right, int top, int bottom) => (base.complement(left, right, top, bottom)).Create(Width, Height, base.token);

		#endregion
		#region フィルタ/畳み込み演算
		
		/// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <param name="rank"></param>
        /// <returns></returns>
		public PixelUShort FilMed(int WindowX = 5, int WindowY = 5, int rank = 12) => FilterMedian(WindowX, WindowY, rank).Create(Width, Height, base.token);
        /// <summary>
        /// [フィルタ]アベレージフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <returns></returns>
		public PixelUShort FilAve(int WindowX = 5, int WindowY = 5) => FilterAverage(WindowX, WindowY, i => (ushort)i).Create(Width, Height, base.token);
 
		/// <summary>
        /// [フィルタ]行方向プロファイル/列方向平均
        /// </summary>
        /// <returns></returns>
 		public PixelUShort FilRowProfileAve() => FilterAverage(1, Height, i => (ushort)i).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
		
		/// <summary>
        /// [フィルタ]列方向プロファイル/行方向平均
        /// </summary>
        /// <returns></returns>
        public PixelUShort FilColProfileAve() => FilterAverage(Width, 1, i => (ushort)i).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);

		public PixelUShort FilRowProfileMed() => FilterMedian(1, Height, Height/2).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
        public PixelUShort FilColProfileMed() => FilterMedian(Width, 1, Width/2).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);
  
		public PixelUShort FilRowProfileIGXL() 
			=> FilMed(1, 63, 31).Trim(0, 31, Width, Height - 62).FilAve(1,Height - 62).Trim(0, (Height - 62)/2, Width, 1);
        public PixelUShort FilColProfileIGXL()
			=> FilMed(63, 1, 31).Trim(31, 0, Width - 62, Height).FilAve(Width - 62,1).Trim(0, (Width - 62)/2, 1, Height);

		public PixelUShort FilWDPick(ushort thr) => FilterWDPick(thr).Create(Width, Height, base.token);
		
		
		/// <summary>
        /// Windowに対してオフセット減算する
        /// （ColProfileならばVOB補正）
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
		public PixelInt OffsetCorrection(PixelUShort window)
		{
		    int[] ret = new int[Size];

			int w = window.Width;
			int h = window.Height;
            Func<int,int,int> co = (x,y) =>
            {
				int xx = x % w;
                int yy = y % h;
                return xx + yy * w;
            };

            int c = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
				{
                    ret[c] = (int)(pixel[c] - window[co(x,y)]);
					c++;
				}

            return ret.Create(Width, Height, base.token);		
		}

		/// <summary>
        /// 奇数行（0開始）をshift画素ズラす
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
		public PixelUShort Stagger(int shift = 1) => FilterStagger(shift).Create(Width, Height, base.token);

		#endregion
	}
    public partial class PixelUInt
    {
		#region 深度
		
		/// <summary>
        /// 型名にあった上限値, 下限値に丸める
        /// </summary>
        /// <param name="type">typeof(型名)</param>
        /// <returns></returns>
		public PixelUInt Limits(Type type) => (base.limits(type)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値, 下限値に丸める
        /// </summary>
        /// <param name="Upper">上限値</param>
        /// <param name="Lower">下限値</param>
        /// <returns></returns>
		public PixelUInt Limits(uint Upper,uint Lower) => (base.limits(Upper, Lower)).Create(Width, Height, base.token);
		/// <summary>
        /// 下限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelUInt LimitsLower(uint thr) => (base.limitsLower(thr)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelUInt LimitsUpper(uint thr) => (base.limitsUpper(thr)).Create(Width, Height, base.token);

		#endregion
		#region サイズ変更

		/// <summary>
        /// 画像をトリムする
        /// </summary>
        /// <param name="left">x原点</param>
        /// <param name="top">y原点</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns></returns>
        public PixelUInt Trim(int left, int top, int width, int height) => (base.trim(left, top, width, height)).Create(width, height, base.token);
		/// <summary>
        /// 特定のベイヤを取り出す
        /// </summary>
        /// <param name="x">取り出しベイヤのx座標</param>
        /// <param name="y">取り出しベイヤのy座標</param>
        /// <param name="width">ベイヤの幅（省略時 = 2）</param>
        /// <param name="height">ベイヤの幅（省略時 = 2）</param>
        /// <returns></returns>
        public PixelUInt TrimBayer(int x, int y,int width = 2,int height = 2) => (base.trimBayer(x, y, width , height)).Create(this.Width / width, this.Height / height, base.token);
		
		/// <summary>
        /// 縦方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelUInt InsertRow(PixelUInt inPix) => (base.InsertRow(inPix.ToArray())).Create(Width, Height * 2, base.token);
		/// <summary>
        /// 横方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelUInt InsertCol(PixelUInt inPix) => (base.InsertCol(inPix.ToArray())).Create(Width * 2, Height, base.token);
		/// <summary>
        /// 4画素ベイヤーに並び替える
        /// </summary>
        /// <returns></returns>
		public static PixelUInt InsertBayer(PixelUInt UpperLeft,PixelUInt UpperRight,PixelUInt LowerLeft, PixelUInt LowerRight) => (UpperLeft.InsertCol(UpperRight)).InsertRow(LowerLeft.InsertCol(LowerRight));

		/// <summary>
        /// 横に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelUInt JoinH(PixelUInt left, PixelUInt right) => PixelBase<uint>.JoinHorizontal(left, right).Create(left.token);
		/// <summary>
        /// 縦に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelUInt JoinV(PixelUInt top, PixelUInt bottom) => PixelBase<uint>.JoinVertical(top, bottom).Create(top.token);

		#endregion


		
        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/
		#region エリア補間

		/// <summary>
        /// 周辺の補間をする（フィルタ使用時など）
		/// V補間->H補間の順,（ストリーキング考慮）
        /// </summary>
        /// <returns></returns>
		public PixelUInt Complement(int left, int right, int top, int bottom) => (base.complement(left, right, top, bottom)).Create(Width, Height, base.token);

		#endregion
		#region フィルタ/畳み込み演算
		
		/// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <param name="rank"></param>
        /// <returns></returns>
		public PixelUInt FilMed(int WindowX = 5, int WindowY = 5, int rank = 12) => FilterMedian(WindowX, WindowY, rank).Create(Width, Height, base.token);
        /// <summary>
        /// [フィルタ]アベレージフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <returns></returns>
		public PixelUInt FilAve(int WindowX = 5, int WindowY = 5) => FilterAverage(WindowX, WindowY, i => (uint)i).Create(Width, Height, base.token);
 
		/// <summary>
        /// [フィルタ]行方向プロファイル/列方向平均
        /// </summary>
        /// <returns></returns>
 		public PixelUInt FilRowProfileAve() => FilterAverage(1, Height, i => (uint)i).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
		
		/// <summary>
        /// [フィルタ]列方向プロファイル/行方向平均
        /// </summary>
        /// <returns></returns>
        public PixelUInt FilColProfileAve() => FilterAverage(Width, 1, i => (uint)i).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);

		public PixelUInt FilRowProfileMed() => FilterMedian(1, Height, Height/2).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
        public PixelUInt FilColProfileMed() => FilterMedian(Width, 1, Width/2).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);
  
		public PixelUInt FilRowProfileIGXL() 
			=> FilMed(1, 63, 31).Trim(0, 31, Width, Height - 62).FilAve(1,Height - 62).Trim(0, (Height - 62)/2, Width, 1);
        public PixelUInt FilColProfileIGXL()
			=> FilMed(63, 1, 31).Trim(31, 0, Width - 62, Height).FilAve(Width - 62,1).Trim(0, (Width - 62)/2, 1, Height);

		public PixelUInt FilWDPick(uint thr) => FilterWDPick(thr).Create(Width, Height, base.token);
		
		
		/// <summary>
        /// Windowに対してオフセット減算する
        /// （ColProfileならばVOB補正）
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
		public PixelUInt OffsetCorrection(PixelUInt window)
		{
		    uint[] ret = new uint[Size];

			int w = window.Width;
			int h = window.Height;
            Func<int,int,int> co = (x,y) =>
            {
				int xx = x % w;
                int yy = y % h;
                return xx + yy * w;
            };

            int c = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
				{
                    ret[c] = (uint)(pixel[c] - window[co(x,y)]);
					c++;
				}

            return ret.Create(Width, Height, base.token);		
		}

		/// <summary>
        /// 奇数行（0開始）をshift画素ズラす
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
		public PixelUInt Stagger(int shift = 1) => FilterStagger(shift).Create(Width, Height, base.token);

		#endregion
	}
    public partial class PixelULong
    {
		#region 深度
		
		/// <summary>
        /// 型名にあった上限値, 下限値に丸める
        /// </summary>
        /// <param name="type">typeof(型名)</param>
        /// <returns></returns>
		public PixelULong Limits(Type type) => (base.limits(type)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値, 下限値に丸める
        /// </summary>
        /// <param name="Upper">上限値</param>
        /// <param name="Lower">下限値</param>
        /// <returns></returns>
		public PixelULong Limits(ulong Upper,ulong Lower) => (base.limits(Upper, Lower)).Create(Width, Height, base.token);
		/// <summary>
        /// 下限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelULong LimitsLower(ulong thr) => (base.limitsLower(thr)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelULong LimitsUpper(ulong thr) => (base.limitsUpper(thr)).Create(Width, Height, base.token);

		#endregion
		#region サイズ変更

		/// <summary>
        /// 画像をトリムする
        /// </summary>
        /// <param name="left">x原点</param>
        /// <param name="top">y原点</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns></returns>
        public PixelULong Trim(int left, int top, int width, int height) => (base.trim(left, top, width, height)).Create(width, height, base.token);
		/// <summary>
        /// 特定のベイヤを取り出す
        /// </summary>
        /// <param name="x">取り出しベイヤのx座標</param>
        /// <param name="y">取り出しベイヤのy座標</param>
        /// <param name="width">ベイヤの幅（省略時 = 2）</param>
        /// <param name="height">ベイヤの幅（省略時 = 2）</param>
        /// <returns></returns>
        public PixelULong TrimBayer(int x, int y,int width = 2,int height = 2) => (base.trimBayer(x, y, width , height)).Create(this.Width / width, this.Height / height, base.token);
		
		/// <summary>
        /// 縦方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelULong InsertRow(PixelULong inPix) => (base.InsertRow(inPix.ToArray())).Create(Width, Height * 2, base.token);
		/// <summary>
        /// 横方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelULong InsertCol(PixelULong inPix) => (base.InsertCol(inPix.ToArray())).Create(Width * 2, Height, base.token);
		/// <summary>
        /// 4画素ベイヤーに並び替える
        /// </summary>
        /// <returns></returns>
		public static PixelULong InsertBayer(PixelULong UpperLeft,PixelULong UpperRight,PixelULong LowerLeft, PixelULong LowerRight) => (UpperLeft.InsertCol(UpperRight)).InsertRow(LowerLeft.InsertCol(LowerRight));

		/// <summary>
        /// 横に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelULong JoinH(PixelULong left, PixelULong right) => PixelBase<ulong>.JoinHorizontal(left, right).Create(left.token);
		/// <summary>
        /// 縦に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelULong JoinV(PixelULong top, PixelULong bottom) => PixelBase<ulong>.JoinVertical(top, bottom).Create(top.token);

		#endregion


		
        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/
		#region エリア補間

		/// <summary>
        /// 周辺の補間をする（フィルタ使用時など）
		/// V補間->H補間の順,（ストリーキング考慮）
        /// </summary>
        /// <returns></returns>
		public PixelULong Complement(int left, int right, int top, int bottom) => (base.complement(left, right, top, bottom)).Create(Width, Height, base.token);

		#endregion
		#region フィルタ/畳み込み演算
		
		/// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <param name="rank"></param>
        /// <returns></returns>
		public PixelULong FilMed(int WindowX = 5, int WindowY = 5, int rank = 12) => FilterMedian(WindowX, WindowY, rank).Create(Width, Height, base.token);
        /// <summary>
        /// [フィルタ]アベレージフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <returns></returns>
		public PixelULong FilAve(int WindowX = 5, int WindowY = 5) => FilterAverage(WindowX, WindowY, i => (ulong)i).Create(Width, Height, base.token);
 
		/// <summary>
        /// [フィルタ]行方向プロファイル/列方向平均
        /// </summary>
        /// <returns></returns>
 		public PixelULong FilRowProfileAve() => FilterAverage(1, Height, i => (ulong)i).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
		
		/// <summary>
        /// [フィルタ]列方向プロファイル/行方向平均
        /// </summary>
        /// <returns></returns>
        public PixelULong FilColProfileAve() => FilterAverage(Width, 1, i => (ulong)i).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);

		public PixelULong FilRowProfileMed() => FilterMedian(1, Height, Height/2).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
        public PixelULong FilColProfileMed() => FilterMedian(Width, 1, Width/2).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);
  
		public PixelULong FilRowProfileIGXL() 
			=> FilMed(1, 63, 31).Trim(0, 31, Width, Height - 62).FilAve(1,Height - 62).Trim(0, (Height - 62)/2, Width, 1);
        public PixelULong FilColProfileIGXL()
			=> FilMed(63, 1, 31).Trim(31, 0, Width - 62, Height).FilAve(Width - 62,1).Trim(0, (Width - 62)/2, 1, Height);

		public PixelULong FilWDPick(ulong thr) => FilterWDPick(thr).Create(Width, Height, base.token);
		
		
		/// <summary>
        /// Windowに対してオフセット減算する
        /// （ColProfileならばVOB補正）
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
		public PixelULong OffsetCorrection(PixelULong window)
		{
		    ulong[] ret = new ulong[Size];

			int w = window.Width;
			int h = window.Height;
            Func<int,int,int> co = (x,y) =>
            {
				int xx = x % w;
                int yy = y % h;
                return xx + yy * w;
            };

            int c = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
				{
                    ret[c] = (ulong)(pixel[c] - window[co(x,y)]);
					c++;
				}

            return ret.Create(Width, Height, base.token);		
		}

		/// <summary>
        /// 奇数行（0開始）をshift画素ズラす
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
		public PixelULong Stagger(int shift = 1) => FilterStagger(shift).Create(Width, Height, base.token);

		#endregion
	}
    public partial class PixelShort
    {
		#region 深度
		
		/// <summary>
        /// 型名にあった上限値, 下限値に丸める
        /// </summary>
        /// <param name="type">typeof(型名)</param>
        /// <returns></returns>
		public PixelShort Limits(Type type) => (base.limits(type)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値, 下限値に丸める
        /// </summary>
        /// <param name="Upper">上限値</param>
        /// <param name="Lower">下限値</param>
        /// <returns></returns>
		public PixelShort Limits(short Upper,short Lower) => (base.limits(Upper, Lower)).Create(Width, Height, base.token);
		/// <summary>
        /// 下限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelShort LimitsLower(short thr) => (base.limitsLower(thr)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelShort LimitsUpper(short thr) => (base.limitsUpper(thr)).Create(Width, Height, base.token);

		#endregion
		#region サイズ変更

		/// <summary>
        /// 画像をトリムする
        /// </summary>
        /// <param name="left">x原点</param>
        /// <param name="top">y原点</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns></returns>
        public PixelShort Trim(int left, int top, int width, int height) => (base.trim(left, top, width, height)).Create(width, height, base.token);
		/// <summary>
        /// 特定のベイヤを取り出す
        /// </summary>
        /// <param name="x">取り出しベイヤのx座標</param>
        /// <param name="y">取り出しベイヤのy座標</param>
        /// <param name="width">ベイヤの幅（省略時 = 2）</param>
        /// <param name="height">ベイヤの幅（省略時 = 2）</param>
        /// <returns></returns>
        public PixelShort TrimBayer(int x, int y,int width = 2,int height = 2) => (base.trimBayer(x, y, width , height)).Create(this.Width / width, this.Height / height, base.token);
		
		/// <summary>
        /// 縦方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelShort InsertRow(PixelShort inPix) => (base.InsertRow(inPix.ToArray())).Create(Width, Height * 2, base.token);
		/// <summary>
        /// 横方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelShort InsertCol(PixelShort inPix) => (base.InsertCol(inPix.ToArray())).Create(Width * 2, Height, base.token);
		/// <summary>
        /// 4画素ベイヤーに並び替える
        /// </summary>
        /// <returns></returns>
		public static PixelShort InsertBayer(PixelShort UpperLeft,PixelShort UpperRight,PixelShort LowerLeft, PixelShort LowerRight) => (UpperLeft.InsertCol(UpperRight)).InsertRow(LowerLeft.InsertCol(LowerRight));

		/// <summary>
        /// 横に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelShort JoinH(PixelShort left, PixelShort right) => PixelBase<short>.JoinHorizontal(left, right).Create(left.token);
		/// <summary>
        /// 縦に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelShort JoinV(PixelShort top, PixelShort bottom) => PixelBase<short>.JoinVertical(top, bottom).Create(top.token);

		#endregion


		
        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/
		#region エリア補間

		/// <summary>
        /// 周辺の補間をする（フィルタ使用時など）
		/// V補間->H補間の順,（ストリーキング考慮）
        /// </summary>
        /// <returns></returns>
		public PixelShort Complement(int left, int right, int top, int bottom) => (base.complement(left, right, top, bottom)).Create(Width, Height, base.token);

		#endregion
		#region フィルタ/畳み込み演算
		
		/// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <param name="rank"></param>
        /// <returns></returns>
		public PixelShort FilMed(int WindowX = 5, int WindowY = 5, int rank = 12) => FilterMedian(WindowX, WindowY, rank).Create(Width, Height, base.token);
        /// <summary>
        /// [フィルタ]アベレージフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <returns></returns>
		public PixelShort FilAve(int WindowX = 5, int WindowY = 5) => FilterAverage(WindowX, WindowY, i => (short)i).Create(Width, Height, base.token);
 
		/// <summary>
        /// [フィルタ]行方向プロファイル/列方向平均
        /// </summary>
        /// <returns></returns>
 		public PixelShort FilRowProfileAve() => FilterAverage(1, Height, i => (short)i).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
		
		/// <summary>
        /// [フィルタ]列方向プロファイル/行方向平均
        /// </summary>
        /// <returns></returns>
        public PixelShort FilColProfileAve() => FilterAverage(Width, 1, i => (short)i).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);

		public PixelShort FilRowProfileMed() => FilterMedian(1, Height, Height/2).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
        public PixelShort FilColProfileMed() => FilterMedian(Width, 1, Width/2).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);
  
		public PixelShort FilRowProfileIGXL() 
			=> FilMed(1, 63, 31).Trim(0, 31, Width, Height - 62).FilAve(1,Height - 62).Trim(0, (Height - 62)/2, Width, 1);
        public PixelShort FilColProfileIGXL()
			=> FilMed(63, 1, 31).Trim(31, 0, Width - 62, Height).FilAve(Width - 62,1).Trim(0, (Width - 62)/2, 1, Height);

		public PixelShort FilWDPick(short thr) => FilterWDPick(thr).Create(Width, Height, base.token);
		
		
		/// <summary>
        /// Windowに対してオフセット減算する
        /// （ColProfileならばVOB補正）
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
		public PixelInt OffsetCorrection(PixelShort window)
		{
		    int[] ret = new int[Size];

			int w = window.Width;
			int h = window.Height;
            Func<int,int,int> co = (x,y) =>
            {
				int xx = x % w;
                int yy = y % h;
                return xx + yy * w;
            };

            int c = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
				{
                    ret[c] = (int)(pixel[c] - window[co(x,y)]);
					c++;
				}

            return ret.Create(Width, Height, base.token);		
		}

		/// <summary>
        /// 奇数行（0開始）をshift画素ズラす
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
		public PixelShort Stagger(int shift = 1) => FilterStagger(shift).Create(Width, Height, base.token);

		#endregion
	}
    public partial class PixelInt
    {
		#region 深度
		
		/// <summary>
        /// 型名にあった上限値, 下限値に丸める
        /// </summary>
        /// <param name="type">typeof(型名)</param>
        /// <returns></returns>
		public PixelInt Limits(Type type) => (base.limits(type)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値, 下限値に丸める
        /// </summary>
        /// <param name="Upper">上限値</param>
        /// <param name="Lower">下限値</param>
        /// <returns></returns>
		public PixelInt Limits(int Upper,int Lower) => (base.limits(Upper, Lower)).Create(Width, Height, base.token);
		/// <summary>
        /// 下限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelInt LimitsLower(int thr) => (base.limitsLower(thr)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelInt LimitsUpper(int thr) => (base.limitsUpper(thr)).Create(Width, Height, base.token);

		#endregion
		#region サイズ変更

		/// <summary>
        /// 画像をトリムする
        /// </summary>
        /// <param name="left">x原点</param>
        /// <param name="top">y原点</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns></returns>
        public PixelInt Trim(int left, int top, int width, int height) => (base.trim(left, top, width, height)).Create(width, height, base.token);
		/// <summary>
        /// 特定のベイヤを取り出す
        /// </summary>
        /// <param name="x">取り出しベイヤのx座標</param>
        /// <param name="y">取り出しベイヤのy座標</param>
        /// <param name="width">ベイヤの幅（省略時 = 2）</param>
        /// <param name="height">ベイヤの幅（省略時 = 2）</param>
        /// <returns></returns>
        public PixelInt TrimBayer(int x, int y,int width = 2,int height = 2) => (base.trimBayer(x, y, width , height)).Create(this.Width / width, this.Height / height, base.token);
		
		/// <summary>
        /// 縦方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelInt InsertRow(PixelInt inPix) => (base.InsertRow(inPix.ToArray())).Create(Width, Height * 2, base.token);
		/// <summary>
        /// 横方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelInt InsertCol(PixelInt inPix) => (base.InsertCol(inPix.ToArray())).Create(Width * 2, Height, base.token);
		/// <summary>
        /// 4画素ベイヤーに並び替える
        /// </summary>
        /// <returns></returns>
		public static PixelInt InsertBayer(PixelInt UpperLeft,PixelInt UpperRight,PixelInt LowerLeft, PixelInt LowerRight) => (UpperLeft.InsertCol(UpperRight)).InsertRow(LowerLeft.InsertCol(LowerRight));

		/// <summary>
        /// 横に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelInt JoinH(PixelInt left, PixelInt right) => PixelBase<int>.JoinHorizontal(left, right).Create(left.token);
		/// <summary>
        /// 縦に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelInt JoinV(PixelInt top, PixelInt bottom) => PixelBase<int>.JoinVertical(top, bottom).Create(top.token);

		#endregion


		
        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/
		#region エリア補間

		/// <summary>
        /// 周辺の補間をする（フィルタ使用時など）
		/// V補間->H補間の順,（ストリーキング考慮）
        /// </summary>
        /// <returns></returns>
		public PixelInt Complement(int left, int right, int top, int bottom) => (base.complement(left, right, top, bottom)).Create(Width, Height, base.token);

		#endregion
		#region フィルタ/畳み込み演算
		
		/// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <param name="rank"></param>
        /// <returns></returns>
		public PixelInt FilMed(int WindowX = 5, int WindowY = 5, int rank = 12) => FilterMedian(WindowX, WindowY, rank).Create(Width, Height, base.token);
        /// <summary>
        /// [フィルタ]アベレージフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <returns></returns>
		public PixelInt FilAve(int WindowX = 5, int WindowY = 5) => FilterAverage(WindowX, WindowY, i => (int)i).Create(Width, Height, base.token);
 
		/// <summary>
        /// [フィルタ]行方向プロファイル/列方向平均
        /// </summary>
        /// <returns></returns>
 		public PixelInt FilRowProfileAve() => FilterAverage(1, Height, i => (int)i).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
		
		/// <summary>
        /// [フィルタ]列方向プロファイル/行方向平均
        /// </summary>
        /// <returns></returns>
        public PixelInt FilColProfileAve() => FilterAverage(Width, 1, i => (int)i).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);

		public PixelInt FilRowProfileMed() => FilterMedian(1, Height, Height/2).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
        public PixelInt FilColProfileMed() => FilterMedian(Width, 1, Width/2).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);
  
		public PixelInt FilRowProfileIGXL() 
			=> FilMed(1, 63, 31).Trim(0, 31, Width, Height - 62).FilAve(1,Height - 62).Trim(0, (Height - 62)/2, Width, 1);
        public PixelInt FilColProfileIGXL()
			=> FilMed(63, 1, 31).Trim(31, 0, Width - 62, Height).FilAve(Width - 62,1).Trim(0, (Width - 62)/2, 1, Height);

		public PixelInt FilWDPick(int thr) => FilterWDPick(thr).Create(Width, Height, base.token);
		
		
		/// <summary>
        /// Windowに対してオフセット減算する
        /// （ColProfileならばVOB補正）
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
		public PixelInt OffsetCorrection(PixelInt window)
		{
		    int[] ret = new int[Size];

			int w = window.Width;
			int h = window.Height;
            Func<int,int,int> co = (x,y) =>
            {
				int xx = x % w;
                int yy = y % h;
                return xx + yy * w;
            };

            int c = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
				{
                    ret[c] = (int)(pixel[c] - window[co(x,y)]);
					c++;
				}

            return ret.Create(Width, Height, base.token);		
		}

		/// <summary>
        /// 奇数行（0開始）をshift画素ズラす
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
		public PixelInt Stagger(int shift = 1) => FilterStagger(shift).Create(Width, Height, base.token);

		#endregion
	}
    public partial class PixelLong
    {
		#region 深度
		
		/// <summary>
        /// 型名にあった上限値, 下限値に丸める
        /// </summary>
        /// <param name="type">typeof(型名)</param>
        /// <returns></returns>
		public PixelLong Limits(Type type) => (base.limits(type)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値, 下限値に丸める
        /// </summary>
        /// <param name="Upper">上限値</param>
        /// <param name="Lower">下限値</param>
        /// <returns></returns>
		public PixelLong Limits(long Upper,long Lower) => (base.limits(Upper, Lower)).Create(Width, Height, base.token);
		/// <summary>
        /// 下限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelLong LimitsLower(long thr) => (base.limitsLower(thr)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelLong LimitsUpper(long thr) => (base.limitsUpper(thr)).Create(Width, Height, base.token);

		#endregion
		#region サイズ変更

		/// <summary>
        /// 画像をトリムする
        /// </summary>
        /// <param name="left">x原点</param>
        /// <param name="top">y原点</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns></returns>
        public PixelLong Trim(int left, int top, int width, int height) => (base.trim(left, top, width, height)).Create(width, height, base.token);
		/// <summary>
        /// 特定のベイヤを取り出す
        /// </summary>
        /// <param name="x">取り出しベイヤのx座標</param>
        /// <param name="y">取り出しベイヤのy座標</param>
        /// <param name="width">ベイヤの幅（省略時 = 2）</param>
        /// <param name="height">ベイヤの幅（省略時 = 2）</param>
        /// <returns></returns>
        public PixelLong TrimBayer(int x, int y,int width = 2,int height = 2) => (base.trimBayer(x, y, width , height)).Create(this.Width / width, this.Height / height, base.token);
		
		/// <summary>
        /// 縦方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelLong InsertRow(PixelLong inPix) => (base.InsertRow(inPix.ToArray())).Create(Width, Height * 2, base.token);
		/// <summary>
        /// 横方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelLong InsertCol(PixelLong inPix) => (base.InsertCol(inPix.ToArray())).Create(Width * 2, Height, base.token);
		/// <summary>
        /// 4画素ベイヤーに並び替える
        /// </summary>
        /// <returns></returns>
		public static PixelLong InsertBayer(PixelLong UpperLeft,PixelLong UpperRight,PixelLong LowerLeft, PixelLong LowerRight) => (UpperLeft.InsertCol(UpperRight)).InsertRow(LowerLeft.InsertCol(LowerRight));

		/// <summary>
        /// 横に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelLong JoinH(PixelLong left, PixelLong right) => PixelBase<long>.JoinHorizontal(left, right).Create(left.token);
		/// <summary>
        /// 縦に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelLong JoinV(PixelLong top, PixelLong bottom) => PixelBase<long>.JoinVertical(top, bottom).Create(top.token);

		#endregion


		
        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/
		#region エリア補間

		/// <summary>
        /// 周辺の補間をする（フィルタ使用時など）
		/// V補間->H補間の順,（ストリーキング考慮）
        /// </summary>
        /// <returns></returns>
		public PixelLong Complement(int left, int right, int top, int bottom) => (base.complement(left, right, top, bottom)).Create(Width, Height, base.token);

		#endregion
		#region フィルタ/畳み込み演算
		
		/// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <param name="rank"></param>
        /// <returns></returns>
		public PixelLong FilMed(int WindowX = 5, int WindowY = 5, int rank = 12) => FilterMedian(WindowX, WindowY, rank).Create(Width, Height, base.token);
        /// <summary>
        /// [フィルタ]アベレージフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <returns></returns>
		public PixelLong FilAve(int WindowX = 5, int WindowY = 5) => FilterAverage(WindowX, WindowY, i => (long)i).Create(Width, Height, base.token);
 
		/// <summary>
        /// [フィルタ]行方向プロファイル/列方向平均
        /// </summary>
        /// <returns></returns>
 		public PixelLong FilRowProfileAve() => FilterAverage(1, Height, i => (long)i).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
		
		/// <summary>
        /// [フィルタ]列方向プロファイル/行方向平均
        /// </summary>
        /// <returns></returns>
        public PixelLong FilColProfileAve() => FilterAverage(Width, 1, i => (long)i).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);

		public PixelLong FilRowProfileMed() => FilterMedian(1, Height, Height/2).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
        public PixelLong FilColProfileMed() => FilterMedian(Width, 1, Width/2).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);
  
		public PixelLong FilRowProfileIGXL() 
			=> FilMed(1, 63, 31).Trim(0, 31, Width, Height - 62).FilAve(1,Height - 62).Trim(0, (Height - 62)/2, Width, 1);
        public PixelLong FilColProfileIGXL()
			=> FilMed(63, 1, 31).Trim(31, 0, Width - 62, Height).FilAve(Width - 62,1).Trim(0, (Width - 62)/2, 1, Height);

		public PixelLong FilWDPick(long thr) => FilterWDPick(thr).Create(Width, Height, base.token);
		
		
		/// <summary>
        /// Windowに対してオフセット減算する
        /// （ColProfileならばVOB補正）
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
		public PixelLong OffsetCorrection(PixelLong window)
		{
		    long[] ret = new long[Size];

			int w = window.Width;
			int h = window.Height;
            Func<int,int,int> co = (x,y) =>
            {
				int xx = x % w;
                int yy = y % h;
                return xx + yy * w;
            };

            int c = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
				{
                    ret[c] = (long)(pixel[c] - window[co(x,y)]);
					c++;
				}

            return ret.Create(Width, Height, base.token);		
		}

		/// <summary>
        /// 奇数行（0開始）をshift画素ズラす
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
		public PixelLong Stagger(int shift = 1) => FilterStagger(shift).Create(Width, Height, base.token);

		#endregion
	}
    public partial class PixelFloat
    {
		#region 深度
		
		/// <summary>
        /// 型名にあった上限値, 下限値に丸める
        /// </summary>
        /// <param name="type">typeof(型名)</param>
        /// <returns></returns>
		public PixelFloat Limits(Type type) => (base.limits(type)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値, 下限値に丸める
        /// </summary>
        /// <param name="Upper">上限値</param>
        /// <param name="Lower">下限値</param>
        /// <returns></returns>
		public PixelFloat Limits(float Upper,float Lower) => (base.limits(Upper, Lower)).Create(Width, Height, base.token);
		/// <summary>
        /// 下限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelFloat LimitsLower(float thr) => (base.limitsLower(thr)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelFloat LimitsUpper(float thr) => (base.limitsUpper(thr)).Create(Width, Height, base.token);

		#endregion
		#region サイズ変更

		/// <summary>
        /// 画像をトリムする
        /// </summary>
        /// <param name="left">x原点</param>
        /// <param name="top">y原点</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns></returns>
        public PixelFloat Trim(int left, int top, int width, int height) => (base.trim(left, top, width, height)).Create(width, height, base.token);
		/// <summary>
        /// 特定のベイヤを取り出す
        /// </summary>
        /// <param name="x">取り出しベイヤのx座標</param>
        /// <param name="y">取り出しベイヤのy座標</param>
        /// <param name="width">ベイヤの幅（省略時 = 2）</param>
        /// <param name="height">ベイヤの幅（省略時 = 2）</param>
        /// <returns></returns>
        public PixelFloat TrimBayer(int x, int y,int width = 2,int height = 2) => (base.trimBayer(x, y, width , height)).Create(this.Width / width, this.Height / height, base.token);
		
		/// <summary>
        /// 縦方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelFloat InsertRow(PixelFloat inPix) => (base.InsertRow(inPix.ToArray())).Create(Width, Height * 2, base.token);
		/// <summary>
        /// 横方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelFloat InsertCol(PixelFloat inPix) => (base.InsertCol(inPix.ToArray())).Create(Width * 2, Height, base.token);
		/// <summary>
        /// 4画素ベイヤーに並び替える
        /// </summary>
        /// <returns></returns>
		public static PixelFloat InsertBayer(PixelFloat UpperLeft,PixelFloat UpperRight,PixelFloat LowerLeft, PixelFloat LowerRight) => (UpperLeft.InsertCol(UpperRight)).InsertRow(LowerLeft.InsertCol(LowerRight));

		/// <summary>
        /// 横に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelFloat JoinH(PixelFloat left, PixelFloat right) => PixelBase<float>.JoinHorizontal(left, right).Create(left.token);
		/// <summary>
        /// 縦に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelFloat JoinV(PixelFloat top, PixelFloat bottom) => PixelBase<float>.JoinVertical(top, bottom).Create(top.token);

		#endregion


		
        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/
		#region エリア補間

		/// <summary>
        /// 周辺の補間をする（フィルタ使用時など）
		/// V補間->H補間の順,（ストリーキング考慮）
        /// </summary>
        /// <returns></returns>
		public PixelFloat Complement(int left, int right, int top, int bottom) => (base.complement(left, right, top, bottom)).Create(Width, Height, base.token);

		#endregion
		#region フィルタ/畳み込み演算
		
		/// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <param name="rank"></param>
        /// <returns></returns>
		public PixelFloat FilMed(int WindowX = 5, int WindowY = 5, int rank = 12) => FilterMedian(WindowX, WindowY, rank).Create(Width, Height, base.token);
        /// <summary>
        /// [フィルタ]アベレージフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <returns></returns>
		public PixelFloat FilAve(int WindowX = 5, int WindowY = 5) => FilterAverage(WindowX, WindowY, i => (float)i).Create(Width, Height, base.token);
 
		/// <summary>
        /// [フィルタ]行方向プロファイル/列方向平均
        /// </summary>
        /// <returns></returns>
 		public PixelFloat FilRowProfileAve() => FilterAverage(1, Height, i => (float)i).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
		
		/// <summary>
        /// [フィルタ]列方向プロファイル/行方向平均
        /// </summary>
        /// <returns></returns>
        public PixelFloat FilColProfileAve() => FilterAverage(Width, 1, i => (float)i).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);

		public PixelFloat FilRowProfileMed() => FilterMedian(1, Height, Height/2).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
        public PixelFloat FilColProfileMed() => FilterMedian(Width, 1, Width/2).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);
  
		public PixelFloat FilRowProfileIGXL() 
			=> FilMed(1, 63, 31).Trim(0, 31, Width, Height - 62).FilAve(1,Height - 62).Trim(0, (Height - 62)/2, Width, 1);
        public PixelFloat FilColProfileIGXL()
			=> FilMed(63, 1, 31).Trim(31, 0, Width - 62, Height).FilAve(Width - 62,1).Trim(0, (Width - 62)/2, 1, Height);

		public PixelFloat FilWDPick(float thr) => FilterWDPick(thr).Create(Width, Height, base.token);
		
		
		/// <summary>
        /// Windowに対してオフセット減算する
        /// （ColProfileならばVOB補正）
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
		public PixelFloat OffsetCorrection(PixelFloat window)
		{
		    float[] ret = new float[Size];

			int w = window.Width;
			int h = window.Height;
            Func<int,int,int> co = (x,y) =>
            {
				int xx = x % w;
                int yy = y % h;
                return xx + yy * w;
            };

            int c = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
				{
                    ret[c] = (float)(pixel[c] - window[co(x,y)]);
					c++;
				}

            return ret.Create(Width, Height, base.token);		
		}

		/// <summary>
        /// 奇数行（0開始）をshift画素ズラす
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
		public PixelFloat Stagger(int shift = 1) => FilterStagger(shift).Create(Width, Height, base.token);

		#endregion
	}
    public partial class PixelDouble
    {
		#region 深度
		
		/// <summary>
        /// 型名にあった上限値, 下限値に丸める
        /// </summary>
        /// <param name="type">typeof(型名)</param>
        /// <returns></returns>
		public PixelDouble Limits(Type type) => (base.limits(type)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値, 下限値に丸める
        /// </summary>
        /// <param name="Upper">上限値</param>
        /// <param name="Lower">下限値</param>
        /// <returns></returns>
		public PixelDouble Limits(double Upper,double Lower) => (base.limits(Upper, Lower)).Create(Width, Height, base.token);
		/// <summary>
        /// 下限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelDouble LimitsLower(double thr) => (base.limitsLower(thr)).Create(Width, Height, base.token);
		/// <summary>
        /// 上限値に丸める
        /// </summary>
        /// <param name="thr">閾値</param>
        /// <returns></returns>
        public PixelDouble LimitsUpper(double thr) => (base.limitsUpper(thr)).Create(Width, Height, base.token);

		#endregion
		#region サイズ変更

		/// <summary>
        /// 画像をトリムする
        /// </summary>
        /// <param name="left">x原点</param>
        /// <param name="top">y原点</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns></returns>
        public PixelDouble Trim(int left, int top, int width, int height) => (base.trim(left, top, width, height)).Create(width, height, base.token);
		/// <summary>
        /// 特定のベイヤを取り出す
        /// </summary>
        /// <param name="x">取り出しベイヤのx座標</param>
        /// <param name="y">取り出しベイヤのy座標</param>
        /// <param name="width">ベイヤの幅（省略時 = 2）</param>
        /// <param name="height">ベイヤの幅（省略時 = 2）</param>
        /// <returns></returns>
        public PixelDouble TrimBayer(int x, int y,int width = 2,int height = 2) => (base.trimBayer(x, y, width , height)).Create(this.Width / width, this.Height / height, base.token);
		
		/// <summary>
        /// 縦方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelDouble InsertRow(PixelDouble inPix) => (base.InsertRow(inPix.ToArray())).Create(Width, Height * 2, base.token);
		/// <summary>
        /// 横方向に交互に画素を挿入する
        /// </summary>
        /// <returns></returns>
		public PixelDouble InsertCol(PixelDouble inPix) => (base.InsertCol(inPix.ToArray())).Create(Width * 2, Height, base.token);
		/// <summary>
        /// 4画素ベイヤーに並び替える
        /// </summary>
        /// <returns></returns>
		public static PixelDouble InsertBayer(PixelDouble UpperLeft,PixelDouble UpperRight,PixelDouble LowerLeft, PixelDouble LowerRight) => (UpperLeft.InsertCol(UpperRight)).InsertRow(LowerLeft.InsertCol(LowerRight));

		/// <summary>
        /// 横に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelDouble JoinH(PixelDouble left, PixelDouble right) => PixelBase<double>.JoinHorizontal(left, right).Create(left.token);
		/// <summary>
        /// 縦に並べて画像を繋ぐ
        /// </summary>
        /// <returns></returns>
		public static PixelDouble JoinV(PixelDouble top, PixelDouble bottom) => PixelBase<double>.JoinVertical(top, bottom).Create(top.token);

		#endregion


		
        /*--------------------------------------*/
        //   フィルタ/畳み込み演算
        /*--------------------------------------*/
		#region エリア補間

		/// <summary>
        /// 周辺の補間をする（フィルタ使用時など）
		/// V補間->H補間の順,（ストリーキング考慮）
        /// </summary>
        /// <returns></returns>
		public PixelDouble Complement(int left, int right, int top, int bottom) => (base.complement(left, right, top, bottom)).Create(Width, Height, base.token);

		#endregion
		#region フィルタ/畳み込み演算
		
		/// <summary>
        /// [フィルタ]メディアンフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <param name="rank"></param>
        /// <returns></returns>
		public PixelDouble FilMed(int WindowX = 5, int WindowY = 5, int rank = 12) => FilterMedian(WindowX, WindowY, rank).Create(Width, Height, base.token);
        /// <summary>
        /// [フィルタ]アベレージフィルタ（境界処理無し）
        /// </summary>
        /// <param name="WindowX">convolution</param>
        /// <param name="WindowY">convolution</param>
        /// <returns></returns>
		public PixelDouble FilAve(int WindowX = 5, int WindowY = 5) => FilterAverage(WindowX, WindowY, i => (double)i).Create(Width, Height, base.token);

        /// <summary>
        /// [フィルタ]行方向プロファイル/列方向平均(Width = 1)
        /// </summary>
        /// <returns></returns>
        public PixelDouble FilRowProfileAve() => FilterAverage(1, Height, i => (double)i).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
		
		/// <summary>
        /// [フィルタ]列方向プロファイル/行方向平均(Height = 1)
        /// </summary>
        /// <returns></returns>
        public PixelDouble FilColProfileAve() => FilterAverage(Width, 1, i => (double)i).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);

		public PixelDouble FilRowProfileMed() => FilterMedian(1, Height, Height/2).Create(Width, Height, base.token).Trim(0, Height / 2, Width,1);
        public PixelDouble FilColProfileMed() => FilterMedian(Width, 1, Width/2).Create(Width, Height, base.token).Trim(Width/2, 0, 1, Height);
  
		public PixelDouble FilRowProfileIGXL() 
			=> FilMed(1, 63, 31).Trim(0, 31, Width, Height - 62).FilAve(1,Height - 62).Trim(0, (Height - 62)/2, Width, 1);
        public PixelDouble FilColProfileIGXL()
			=> FilMed(63, 1, 31).Trim(31, 0, Width - 62, Height).FilAve(Width - 62,1).Trim(0, (Width - 62)/2, 1, Height);

		public PixelDouble FilWDPick(double thr) => FilterWDPick(thr).Create(Width, Height, base.token);
		
		
		/// <summary>
        /// Windowに対してオフセット減算する
        /// （RowProfileならばVOB補正）
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
		public PixelDouble OffsetCorrection(PixelDouble window)
		{
		    double[] ret = new double[Size];

			int w = window.Width;
			int h = window.Height;
            Func<int,int,int> co = (x,y) =>
            {
				int xx = x % w;
                int yy = y % h;
                return xx + yy * w;
            };

            int c = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
				{
                    ret[c] = (double)(pixel[c] - window[co(x,y)]);
					c++;
				}

            return ret.Create(Width, Height, base.token);		
		}

		/// <summary>
        /// 奇数行（0開始）をshift画素ズラす
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
		public PixelDouble Stagger(int shift = 1) => FilterStagger(shift).Create(Width, Height, base.token);

		#endregion
	}
	#endregion
}