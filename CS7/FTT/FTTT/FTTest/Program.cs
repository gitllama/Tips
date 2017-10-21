using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pixels;
using Pixels.Math;
using Pixels.Stream;
using Pixels.Extend;

namespace FTTest
{
    public class Globals
    {
        public ChipStatusMediator Chip;
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Test.RunSingle();
            //テストコード
            Test.Run();

            return;

            try
            {
                var Seq = PixelSeqParam.Create("Config.yaml");
                var chips = Seq.CheckedChips(@"D:\200CFT\");//Mono
                //スクリプト読み込み,コンパイル

                var ssr = ScriptSourceResolver.Default.WithBaseDirectory(Environment.CurrentDirectory);
                var script = CSharpScript.Create(
                    File.ReadAllText("Script.csx"),
                    ScriptOptions.Default.WithImports(new string[]
                    {
                        "System",
                        "System.Math",
                        "FTTest",
                        "Pixels",
                        "Pixels.Extend",
                        "Pixels.Math"
                    })
                    .WithSourceResolver(ssr)
                    .WithReferences(System.Reflection.Assembly.GetEntryAssembly()),
                    typeof(Globals));

                //実行

                var serializer = new YamlDotNet.Serialization.Serializer();
                var globals = new Globals();
                foreach (var chip in chips)
                {
                    globals.Chip = ChipStatusMediator.Create(chip);
                    var state = script.RunAsync(globals).Result;

                    foreach (var variable in state.Variables)
                        Console.WriteLine($"{variable.Name} = {variable.Value} of type {variable.Type}");

                    globals.Chip.OutputFile("output.yaml");


                //using (var sw = new StreamWriter("output.yaml"))
                //{
                //    //シリアライズ
                //    sw.WriteLine(serializer.Serialize(chip));
                //}
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
            Console.WriteLine("-----end-----");
            Console.ReadKey();
        }
    }

    public static class Test
    {
        public static void RunSingle()
        {
            var i = PixelFactory
                .Create<float>(PixelSeqParam.Create("Config.yaml").Maps)
                .Read(@"D:\200CFT\Lot0002\wafer01\N01_C170414_大きい点傷_左下\Dark\Ave.bin");

            var j = i.Sub(i.FilterMedianBayer());
            
            Console.WriteLine(j["Active"].SubSelf(255).ToColorBG().Labling());

        }
        public static void Run()
        {
            //Console.WriteLine($"{Chip.LotNo}_{Chip.WfNo}_{Chip.ChipNo}");
            //sw.WriteLine($"{Chip.LotNo}_{Chip.WfNo}_{Chip.ChipNo}, {A}, {B}, {C}, {D}, {E}, {F}, {G}, {H}, {I}");

            var Seq = PixelSeqParam.Create("Config.yaml");
            var chips = Seq.CheckedChips(@"D:\200CFT\");


            ChipStatusMediator Chip;
            foreach (var _Chip in chips)
            {
                Chip = ChipStatusMediator.Create(_Chip);

                //Chip["Dark60", "Ave"]
                //    .Intermediate(x => x.FilterMedianBayer()["Normal"].StaggerR())
                //    .Filter(x => x["Normal"].StaggerR())
                //    ["Active"]
                //    .Labeling();

                Chip["Dark60", "Ave"]
                    .Intermediate(x => x.FilterMedianBayer()["Normal"].StaggerR())
                    .Filter(x => x["Normal"].StaggerR())
                    ["Active"]
                    .Defect(255)
                    .Defect(125)
                    .Defect(64);

                //Chip["Dark", "Ave"]
                //    .Filter(x =>
                //    {
                //        x["Normal"].StaggerR();
                //        x["Active"].SubSelf(255).ToColorBG().Labling();
                //    });

                //using (var sw = new StreamWriter("vfpn.txt", true))
                //{
                //    var j = Chip["VNtest1", "Ave"].pixel?["Active"]?.AverageBayer(0, 1) ?? null;

                //    var A = Chip["VNtest1", "Ave"].pixel?["Active"]?.VDeviationBayer(0, 1) ?? null;
                //    var B = Chip["VNtest2", "Ave"].pixel?["Active"]?.VDeviationBayer(0, 1) ?? null;
                //    var C = Chip["VNtest3", "Ave"].pixel?["Active"]?.VDeviationBayer(0, 1) ?? null;
                //    var D = Chip["VNtest4", "Ave"].pixel?["Active"]?.VDeviationBayer(0, 1) ?? null;
                //    var E = Chip["VNtest5", "Ave"].pixel?["Active"]?.VDeviationBayer(0, 1) ?? null;
                //    var F = Chip["VNtest6", "Ave"].pixel?["Active"]?.VDeviationBayer(0, 1) ?? null;
                //    var G = Chip["VNtest7", "Ave"].pixel?["Active"]?.VDeviationBayer(0, 1) ?? null;
                //    var H = Chip["VNtest8", "Ave"].pixel?["Active"]?.VDeviationBayer(0, 1) ?? null;
                //    var I = Chip["VNtest9", "Ave"].pixel?["Active"]?.VDeviationBayer(0, 1) ?? null;



                //    Console.WriteLine($"{Chip.LotNo}_{Chip.WfNo}_{Chip.ChipNo}");
                //    sw.WriteLine($"{Chip.LotNo}_{Chip.WfNo}_{Chip.ChipNo}, {j}, {A}, {B}, {C}, {D}, {E}, {F}, {G}, {H}, {I}");
                //}

                //平均・偏差

                //Chip["Dark", "Ave"]
                //    .Intermediate(x => x.FilterMedianBayer()["Normal"].StaggerR())
                //    .Filter(x => x["Normal"].StaggerR())
                //    ["Active"]
                //    .Signal()
                //    .Defect(255)
                //    .Defect(125)
                //    .Defect(64);

                //Chip["L50", "Ave"]
                //    .Filter(x =>
                //    {
                //        var i = x.FilterMedianBayer();
                //        return i["Normal"].StaggerR();
                //    })
                //    .Convert(x => x["Normal"].StaggerR())
                //    ["Active"].Signal2();


                //平均（中央値フィルタ後）
                //Chip["Dark", "Ave"]
                //    .Convert(x => x["Normal"].StaggerR())
                //    ["Active"].Signal();


                //chip単位の結果出力, 追記
                Chip.OutputFile("output.yaml");


            }
        }
    }

}