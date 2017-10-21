using Pixels;
using Pixels.Math;
using Pixels.Stream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet;

namespace Pixels.Extend
{
    //シーケンス動作に必要な諸設定
    public class PixelSeqParam
    {
        //設定
        public string Regex { get; set; } = @"Lot(?<lot>[0-9]{4})\\wafer(?<wf>[0-9]{2})\\N(?<chip>[0-9]{2})";
        public string searchPattern { get; set; } = "N*";
        public string ChipNote { get; set; } = @"N[0-9]{2}_[a-zA-Z0-9]+_(?<note>[a-zA-Z0-9]+)$";

        public Dictionary<string, string> Condition { get; set; }
        public Dictionary<string, PictureTypes> Picture { get; set; }
        public Dictionary<string, PixelMap> Maps { get; set; }

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
                        Maps = Maps
                    };
                    result.Add(buf);
                }
            }

            return result;
        }
    }

    public class PictureTypes
    {
        public string Name { get; set; }
        public string Type { get; set; }
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

        public Dictionary<string, string> Condition { get; set; }
        public Dictionary<string, PictureTypes> Picture { get; set; }
        public Dictionary<string, PixelMap> Maps { get; set; }

        public Dictionary<(string cond, string pic, string name), string> Result { get; set; }
    }

    public class ChipStatusMediator
    {
        public static ChipStatusMediator Create(ChipStatus src)
        {
            return (new Deserializer())
                .Deserialize<ChipStatusMediator>
                (
                    (new Serializer()).Serialize(src)
                );
        }

        public string FilePath { get; set; }
        public string LotNo { get; set; }
        public string WfNo { get; set; }
        public string ChipNo { get; set; }
        public string Note { get; set; }
        public string DateTimeTaken { get; set; }
        public string DateTimeProcessed { get; set; }

        public Dictionary<string, string> Condition { get; set; }
        public Dictionary<string, PictureTypes> Picture { get; set; }
        public Dictionary<string, PixelMap> Maps { get; set; }

        public Dictionary<(string cond, string pic, string name), string> Result { get; set; }

        //画像操作
        public (string cond,string pic) PixelStatus;
        public Pixel<float> pixel;
        public ChipStatusMediator this[string cond, string pic]
        {
            get
            {
                SetPixel(cond, pic);
                return this;
            }
        }
        public ChipStatusMediator this[string map]
        {
            get
            {
                if(pixel != null) pixel.Map(map);
                if (pixelfilter != null) pixelfilter.Map(map);
                //!!!注意
                return this;
            }
        }
        private void SetPixel(string cond, string pic)
        {
            var path = $"{Path.Combine(FilePath, Condition[cond], Picture[pic].Name)}";
            (string cond, string pic) name = (Condition[cond], Picture[pic].Name);

            if (!System.IO.File.Exists(path))
            {
                pixel = null;
                return;
            }
            if (PixelStatus.CompareTo(name) == 0) return;

            pixel = null;
            pixel = PixelFactory.Create<float>(Maps).Read(path, 0, Picture[pic].Type);
            PixelStatus = name;
        }

        public Pixel<float> pixelfilter;

        public void AddResult(string key,string value)
        {
            if (Result == null) Result = new Dictionary<(string cond, string pic, string name), string>();

            Result.Add((PixelStatus.cond, PixelStatus.pic, key), value);
        }




    }

    public static class PixelSeqExtensions
    {
        public static void Output(this ChipStatusMediator src, string key, string value)
        {
            Console.WriteLine($"{key} : {value}");

            using (var sw = new StreamWriter("log.txt", true))
            {
                sw.WriteLine($"{src.LotNo}_{src.WfNo}_{src.ChipNo}, {key}, {value}");
            }
            src.AddResult(key, value);
        }

        public static ChipStatusMediator Filter(this ChipStatusMediator src, Action<Pixel<float>> action)
        {
            if (src.pixel == null) return src;
            action(src.pixel);

            return src;
        }
        public static ChipStatusMediator Intermediate(this ChipStatusMediator src, Func<Pixel<float>, Pixel<float>> action)
        {
            if (src.pixel == null) return src;
            src.pixelfilter = action(src.pixel);

            return src;
        }

        public static ChipStatusMediator Signal(this ChipStatusMediator src)
        {
            if (src.pixel == null) return src;

            var p = src.pixel;

            double[] ave = new double[p.BayerSizeX * p.BayerSizeY];
            double[] dev = new double[p.BayerSizeX * p.BayerSizeY];

            //Ave
            for (int by = 0; by < p.BayerSizeY; by++)
                for (int bx = 0; bx < p.BayerSizeX; bx++)
                {
                    p.BayerX = bx;
                    p.BayerY = by;
                    ref double buf = ref ave[bx + by * p.BayerSizeX];
                    int c = 0;
                    for (int y = 0; y < p.BayerHeight; y ++)
                        for (int x = 0; x < p.BayerWidth; x ++)
                        {
                            buf += p.Bayer(x, y);
                            c++;
                        }
                    buf /= c;
                }
            //dev
            for (int by = 0; by < p.BayerSizeY; by++)
                for (int bx = 0; bx < p.BayerSizeX; bx++)
                {
                    p.BayerX = bx;
                    p.BayerY = by;
                    ref double buf = ref dev[bx + by * p.BayerSizeX];
                    ref double buf_ave = ref ave[bx + by * p.BayerSizeX];
                    int c = 0;
                    for (int y = 0; y < p.BayerHeight; y++)
                        for (int x = 0; x < p.BayerWidth; x++)
                        {
                            buf += System.Math.Pow(p.Bayer(x, y) - buf_ave, 2);
                            c++;
                        }
                    buf = System.Math.Sqrt(buf / c);

                }

            foreach (var n in ave.Select((v,i)=>(v,i)))
            {
                var key   = $"{nameof(Signal)}_Averaging{n.Item2}";
                var value = n.Item1;
                src.Output(key, value.ToString());
            }
            foreach (var n in dev.Select((v, i) => (v, i)))
            {
                var key = $"{nameof(Signal)}_Deviation{n.Item2}";
                var value = n.Item1;
                src.Output(key, value.ToString());
            }

            return src;
        }

        public static ChipStatusMediator Signal2(this ChipStatusMediator src)
        {
            if (src.pixelfilter == null) return src;

            var p = src.pixelfilter;

            double[] ave = new double[p.BayerSizeX * p.BayerSizeY];
            double[] dev = new double[p.BayerSizeX * p.BayerSizeY];

            //Ave
            for (int by = 0; by < p.BayerSizeY; by++)
                for (int bx = 0; bx < p.BayerSizeX; bx++)
                {
                    p.BayerX = bx;
                    p.BayerY = by;
                    ref double buf = ref ave[bx + by * p.BayerSizeX];
                    int c = 0;
                    for (int y = 0; y < p.BayerHeight; y++)
                        for (int x = 0; x < p.BayerWidth; x++)
                        {
                            buf += p.Bayer(x, y);
                            c++;
                        }
                    buf /= c;
                }
            //dev
            for (int by = 0; by < p.BayerSizeY; by++)
                for (int bx = 0; bx < p.BayerSizeX; bx++)
                {
                    p.BayerX = bx;
                    p.BayerY = by;
                    ref double buf = ref dev[bx + by * p.BayerSizeX];
                    ref double buf_ave = ref ave[bx + by * p.BayerSizeX];
                    int c = 0;
                    for (int y = 0; y < p.BayerHeight; y++)
                        for (int x = 0; x < p.BayerWidth; x++)
                        {
                            buf += System.Math.Pow(p.Bayer(x, y) - buf_ave, 2);
                            c++;
                        }
                    buf = System.Math.Sqrt(buf / c);

                }

            foreach (var n in ave.Select((v, i) => (v, i)))
            {
                var key = $"{nameof(Signal2)}_Averaging{n.Item2}";
                var value = n.Item1;
                src.Output(key, value.ToString());
            }
            foreach (var n in dev.Select((v, i) => (v, i)))
            {
                var key = $"{nameof(Signal2)}_Deviation{n.Item2}";
                var value = n.Item1;
                src.Output(key, value.ToString());
            }

            return src;
        }


        public static ChipStatusMediator Defect(this ChipStatusMediator src, params int[] thr)
        {
            if (src.pixel == null) return src;
            if (src.pixelfilter == null) return src;
            if (thr == null) return src;

            var p1 = src.pixel;
            var p2 = src.pixelfilter;

            int[,] count = new int[p1.BayerSizeX * p1.BayerSizeY, thr.Length];


            for (int by = 0; by < p1.BayerSizeY; by++)
                for (int bx = 0; bx < p1.BayerSizeX; bx++)
                {
                    p1.BayerX = bx;
                    p1.BayerY = by;
                    //ref int[] buf = ref count[bx + by * p1.BayerSizeX];
                    for (int y = 0; y < p1.BayerHeight; y++)
                        for (int x = 0; x < p1.BayerWidth; x++)
                        {
                            var hoge = p1.Bayer(x, y) - p2.Bayer(x, y);
                            for(int i =0;i<thr.Length;i++)
                            {
                                count[bx + by * p1.BayerSizeX,i] += hoge > thr[i] ? 1 : 0;
                            }
                        }
                }
            //foreach ((int[] thrs, int index) n in count.Select((v, i) => (v, i)))
                //foreach ((int value, int index) k in n.thrs.Select((v, i) => (v, i)))
            for(int y=0;y< count.GetLength(0);y++)
                for (int x = 0; x < count.GetLength(1); x++)
                {
                    var key = $"{nameof(Defect)}_{thr[x]}_{y}";
                    var value = count[y,x];
                    src.Output(key, value.ToString());
                }
            return src;
        }

        //検査項目

        public static ChipStatusMediator CheckFile(this ChipStatusMediator src)
        {
            //var rsult = System.IO.File.Exists(src.FilePath);
            int fileCount = Directory.GetFiles(src.FilePath, "*.bin", SearchOption.AllDirectories).Length;
            //src.Result.Add($"{nameof(CheckFile)}", fileCount.ToString());
            return src;
        }


        public static ChipStatusMediator Labeling(this ChipStatusMediator src)
        {
            if (src.pixel == null) return src;
            if (src.pixelfilter == null) return src;

            var p = src.pixel.Sub(src.pixelfilter);

            var v = p.SubSelf(255).ToMono().Labling();

            var key = $"{nameof(Labeling)}";
            var value = v;
            src.Output(key, value.ToString());

            return src;
        }

        public static ChipStatusMediator VFPN(this ChipStatusMediator src)
        {
            if (src.pixel == null) return src;

            return src;
        }
        public static ChipStatusMediator HFPN(this ChipStatusMediator src)
        {
            if (src.pixel == null) return src;

            return src;
        }

        public static ChipStatusMediator ClusterDefect(this ChipStatusMediator src)
        {
            if (src.pixel == null) return src;

            return src;
        }
        public static ChipStatusMediator NonUniformity(this ChipStatusMediator src)
        {
            if (src.pixel == null) return src;

            return src;
        }
        public static ChipStatusMediator Shading(this ChipStatusMediator src)
        {
            if (src.pixel == null) return src;

            return src;
        }

        public static void OutputFile(this ChipStatusMediator src, string name)
        {
            using (var sw = new StreamWriter(name, true))
            {
                var result = (new Serializer()).Serialize(new List<ChipStatusMediator>() { src });
                Console.WriteLine(result);
                sw.WriteLine(result);
            }
        }
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



