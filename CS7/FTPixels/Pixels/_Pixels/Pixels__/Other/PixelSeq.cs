using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using System.IO;

/*******************************************************************************/
/*   Pixels                                                                    */
/*   2015/08/24                                                                */
/*******************************************************************************/

namespace Pixels.Sequence
{
    public class SeqDef
    {
        //PixelFloat[] pixel_buffering;

        public Dictionary<string, Func<PixelFloat[]>> DicRead = new Dictionary<string, Func<PixelFloat[]>>();

        #region PassFail


        #endregion


        #region MyRegion
        protected enum Illuminant { Dark, L10, L50, L90, L100, L400 }
        protected enum Register { Full, Half }
        protected enum FileType{ Single, Ave, Dev }
        protected enum Area { Full, Valid, OB }

        protected PixelDouble ReadPixs(Illuminant i, Register r, FileType f, Area a)
        {
            string filename = "current_dir" + IllDic[i] + RegDic[r] + FileDic[f];
            return AreaDic[a](PixelStream.ReadTxtDouble(filename, 1408, 1032));
        }
        private static Dictionary<Illuminant, string> IllDic = new Dictionary<Illuminant, string>()
        {
            [Illuminant.Dark] = @"\Dark",
            [Illuminant.L10] = @"\L10",
            [Illuminant.L50] = @"\L50"
        };
        private static Dictionary<Register, string> RegDic = new Dictionary<Register, string>()
        {
            [Register.Full] = @"",
            [Register.Half] = @"_Half",
        };
        private static Dictionary<FileType, string> FileDic = new Dictionary<FileType, string>()
        {
            [FileType.Single] = @"\000.txt",
            [FileType.Ave] = @"\ave.txt",
        };
        private static Dictionary<Area, Func<PixelDouble, PixelDouble>> AreaDic = new Dictionary<Area, Func<PixelDouble, PixelDouble>>()
        {
            [Area.Full] = (p) => p,
            [Area.Valid] = (p) => p.Trim(0, 0, 10, 10),
            [Area.OB] = (p) => p.Trim(0, 0, 20, 20)
        };
#endregion

        //protected static Illuminant Ill(this Illuminant i) => i;
        //protected static Tuple<string, int, DateTime> Reg(this Illuminant i) => Create<T1>(T1);


        //////条件 > エリア
        ////static Dictionary<String, Action<PixelDouble>> LuxDic = new Dictionary<string, Action<PixelDouble>>()
        ////{
        ////    {"Per50", (f) => PixelStream.ReadTxtDouble(@"L50\ave.txt",1408,1032) }
        ////};
        ////static Dictionary<String, Func<dynamic, dynamic>> AreaDic = new Dictionary<string, Func<dynamic, dynamic>>()
        ////{
        ////    ["Full"] = (p) => p ,
        ////    ["Valid"]C:\Users\brook\Desktop\BTView2.0\v002\BTView\app.config = (p) => p.Trim(0,0,10,10),
        ////    ["OB"] = (p) => p.Trim(0,0,20,20)
        ////};

        //Action<double> =()

        public double Cat00_Test()
        {
            return -1;
        }
        public double Cat02_CLM()
        {
            PixelDouble p = ReadPixs(Illuminant.L50, Register.Full, FileType.Ave, Area.Full);
            return p.Ave();
        }

        /*******************/


        ///*シーケンス*/
        ///*テスト*/

        ///*Cat #1:Pass*/
        //public void SaveBMP()
        //{
        //    Pixel pic = CreatePixs(Per50.Full.Typ.ave, TrimType.Full);
        //    pic.SaveBMP("test.bmp", 0, 256);
        //}
        //static public string CheckFile(string file)
        //{
        //    string tnd = System.IO.Path.GetDirectoryName(file);
        //    string tnf = System.IO.Path.GetFileName(file);

        //    string cdir = System.IO.Directory.GetCurrentDirectory();

        //    string[] tn_c1 = System.IO.Directory.GetDirectories(cdir, tnd);
        //    string[] tn_c2 = System.IO.Directory.GetFiles(tn_c1[0], tnf);
        //    return tn_c2[0];
        //}
        //static public string CheckDir(string dir)
        //{
        //    string tnd = System.IO.Path.GetDirectoryName(dir);

        //    string cdir = System.IO.Directory.GetCurrentDirectory();

        //    string[] tn_c1 = System.IO.Directory.GetDirectories(tnd);
        //    return tn_c1[0];
        //}

        ////ファイルそろってるか存在チェック（ない数返す？）

        ///*Cat #2: AC/DC Open*/
        ///*Cat #3: AC/DC Short/Leak/Current*/
        ///*Cat #4: Func*/
        ///*Cat #5:画像ロジックチェック*/

        ///*Cat #6:固定ノイズ*/
        ///*Cat #7:シェーディング、オフセット*/
        ///*Cat #8:感度・ムラ*/
        ///*Cat #9:固定ノイズ（白傷）*/
        //public double Cat09_WD(double thr)
        //{
        //    PixelDouble pic = CreatePixs(Dark.Full.Typ.ave, TrimType.Full);
        //    return (pic - pic.FilMed).Count(thr, Pixel.INEQUALITY.LessThan);
        //}
        ///*Cat #10:固定ノイズ（黒傷）*/
        ///*Cat #11:固定ノイズ（パタン）*/
        ///*Cat #12:残像*/
        /// 


        /********************/

        public double[] Signal_Ave(params PixelFloat[] pixs) => Array.ConvertAll<PixelFloat, double>(pixs, i => i.Ave());
        public double[] Signal_Med(params PixelFloat[] pixs) => Array.ConvertAll<PixelFloat, double>(pixs, i => i.Med());
        public double[] Signal_IGXL(params PixelFloat[] pixs) => Array.ConvertAll<PixelFloat, double>(pixs, i => i.FilMed().Ave());

    }

    public class SeqBT3300N : SeqDef
    {
        public string filename;

        public SeqBT3300N()
        {

        }
        public double[] Signal(string ConditionName) =>  base.Signal_Med(DicRead[ConditionName]());

    }
}