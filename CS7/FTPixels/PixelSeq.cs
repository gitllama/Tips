using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Pixels.Sequence
{
    //シーケンス動作に必要な諸設定
    public class PixelSeqParam
    {
        //設定
        public string Regex { get; set; } = @"Lot(?<lot>[0-9]{4})\\wafer(?<wf>[0-9]{2})\\N(?<chip>[0-9]{2})";
        public string searchPattern { get; set; } = "N*";
        public string ChipNote { get; set; } = @"N[0-9]{2}_[a-zA-Z0-9]+_(?<note>[a-zA-Z0-9]+)$";

        public Dictionary<string, string> Condition { get; set; }
        public Dictionary<string, string> Picture { get; set; }
        public Dictionary<string, PixelMap> Map { get; set; }

        //設定の読み込み
        public static PixelSeqParam Create(string yaml)
        {
            using (var sr = new StreamReader(yaml))
            {
                var deserializer = new YamlDotNet.Serialization.Deserializer();
                return deserializer.Deserialize<PixelSeqParam>(sr);
            }
        }

        public List<ChipStatus> CheckedChips(string path)
        {
            var result = new List<ChipStatus>();

            IEnumerable<string> dirs = System.IO.Directory.EnumerateDirectories(
                path,
                searchPattern,
                System.IO.SearchOption.AllDirectories);

            foreach (string dir in dirs)
            {
                var mc = System.Text.RegularExpressions.Regex.Matches(
                    dir,
                    Regex,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                foreach (System.Text.RegularExpressions.Match m in mc)
                {
                    var buf = new ChipStatus()
                    {
                        LotNo = m.Groups["lot"].Value,
                        WfNo = m.Groups["wf"].Value,
                        ChipNo = m.Groups["chip"].Value,
                        FilePath = dir,

                        //!!! 設定値の注入が必要
                        Condition = Condition,
                        Picture = Picture,
                        Map = Map
                    };
                    result.Add(buf);
                }
            }

            return result;
        }
    }

    //チップのデータ
    public class ChipStatus
    {
        public string FilePath { get; set; }
        public string LotNo { get; set; }
        public string WfNo { get; set; }
        public string ChipNo { get; set; }
        public string Note { get; set; }
        public string DateTimeTaken { get; set; }
        public string DateTimeProcessed { get; set; }

        public Dictionary<string, string> Condition;
        public Dictionary<string, string> Picture;
        public Dictionary<string, PixelMap> Map;

        public Dictionary<string, string> Result { get; set; } = new Dictionary<string, string>();

        //public ChipStatus this[string cond, string pic]
        //{
        //    get
        //    {
        //        //var dst = new Pixels.Pixel(Map["Full"].Width, Map["Full"].Height);
        //        //dst.Read($"{Path.Combine(FilePath, Condition[cond], Picture[pic])}");
        //        //dst.Map = Map;
        //        //return dst;


        //    }
        //}

        public string PixelStatus;
        public Pixel pixel;
    }

    public static class SeqExtentions
    {
        //後処理

        public static ChipStatus Set(this ChipStatus src, string cond, string pic)
        {

            var path = $"{Path.Combine(src.FilePath, src.Condition[cond], src.Picture[pic])}";
            var name = $"{src.Condition[cond]}_{src.Picture[pic]}";

            if (!System.IO.File.Exists(path)) return src;
            if (src.PixelStatus == name) return src;

            src.pixel = null;

            var dst = new Pixels.Pixel(src.Map["Full"].Width, src.Map["Full"].Height);
            dst.Read(path);

            src.pixel = dst;
            src.PixelStatus = $"{src.Condition[cond]}_{src.Picture[pic]}";
            return src;
        }

        public static ChipStatus StaggerL(this ChipStatus src)
        {
            if (src.pixel == null) return src;

            var p = src.pixel;
            for (int y = 1; y < p.Height; y += 2)
            {
                for (int x = 1; x < p.Width - 1; x++)
                {
                    p.pixel[x + y * p.Width] = p.pixel[x + y * p.Width + 1];
                }
            }

            src.PixelStatus += $"_StaggerL";
            src.pixel = p;

            return src;
        }

        public static ChipStatus Trim(this ChipStatus src,string map)
        {
            if (src.pixel == null) return src;

            var p = src.pixel;
            src.pixel = p.Trim(src.Map[map].Left, src.Map[map].Top, src.Map[map].Width, src.Map[map].Height); //破壊じゃない<-

            src.PixelStatus += $"_{map}";

            return src;
        }


        //検査項目

        public static ChipStatus CheckFile(this ChipStatus src)
        {
            //var rsult = System.IO.File.Exists(src.FilePath);
            int fileCount = Directory.GetFiles(src.FilePath, "*.bin", SearchOption.AllDirectories).Length;
            src.Result.Add($"{nameof(CheckFile)}", fileCount.ToString());
            return src;
        }

        public static ChipStatus Signal2(this ChipStatus src)
        {
            if (src.pixel == null) return src;

            var p = src.pixel;

            double Gr = 0;
            double R = 0;
            double B = 0;
            double Gb = 0;
            int count = 0;
            for (int y = 0; y < p.Height; y += 2)
            {
                for (int x = 0; x < p.Width; x += 2)
                {
                    Gr += p.pixel[count++];
                    R += p.pixel[count++];
                }
                for (int x = 0; x < p.Width; x += 2)
                {
                    B += p.pixel[count++];
                    Gb += p.pixel[count++];
                }
            }

            Gr /= p.pixel.Length / 4;
            R /= p.pixel.Length / 4;
            B /= p.pixel.Length / 4;
            Gb /= p.pixel.Length / 4;


            src.Result.Add(src.PixelStatus + "_" + nameof(Single) + "_Gr", Gr.ToString());
            src.Result.Add(src.PixelStatus + "_" + nameof(Single) + "_R", R.ToString());
            src.Result.Add(src.PixelStatus + "_" + nameof(Single) + "_B", B.ToString());
            src.Result.Add(src.PixelStatus + "_" + nameof(Single) + "_Gb", Gb.ToString());
            return src;
        }

        public static ChipStatus Signal(this ChipStatus src)
        {
            if (src.pixel == null) return src;

            var p = src.pixel;

            double ave = 0;
            double max = double.MinValue;
            double min = double.MaxValue;
            foreach (var i in p.pixel)
            {
                ave += i;
                if (max < i) max = i;
                if (min > i) min = i;
            }

            ave /= p.pixel.Length;


            src.Result.Add(src.PixelStatus + "_" + nameof(Single) + "_Mono", ave.ToString());
            src.Result.Add(src.PixelStatus + "_" + nameof(Single) + "_MonoMax", max.ToString());
            src.Result.Add(src.PixelStatus + "_" + nameof(Single) + "_MonoMin", min.ToString());
            return src;
        }

        public static ChipStatus VFPN(this ChipStatus src)
        {
            if (src.pixel == null) return src;

            return src;
        }
        public static ChipStatus HFPN(this ChipStatus src)
        {
            if (src.pixel == null) return src;

            return src;
        }
        public static ChipStatus Defect(this ChipStatus src)
        {
            if (src.pixel == null) return src;

            return src;
        }
        public static ChipStatus ClusterDefect(this ChipStatus src)
        {
            if (src.pixel == null) return src;

            return src;
        }
        public static ChipStatus NonUniformity(this ChipStatus src)
        {
            if (src.pixel == null) return src;

            return src;
        }
        public static ChipStatus Shading(this ChipStatus src)
        {
            if (src.pixel == null) return src;

            return src;
        }
    }
}
