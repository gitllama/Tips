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
using Pixels.Sequence;

namespace FTTest
{
    public class Globals
    {
        public ChipStatus Chip;
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //初期化

                var Seq = PixelSeqParam.Create("Config.yaml");
                var chips = Seq.CheckedChips(@"D:\Lot0002\");


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
                        "Pixels.Sequence",
                        "Pixels.Math"
                    })
                    .WithSourceResolver(ssr)
                    .WithReferences(System.Reflection.Assembly.GetEntryAssembly()),
                    typeof(Globals));


                //実行
                    
                using (var sw = new StreamWriter("output.yaml"))
                {
                    var serializer = new YamlDotNet.Serialization.Serializer();
                    var globals = new Globals();
                    foreach (var chip in chips)
                    {
                        globals.Chip = chip;
                        var state = script.RunAsync(globals).Result;

                        foreach (var variable in state.Variables)
                            Console.WriteLine($"{variable.Name} = {variable.Value} of type {variable.Type}");
                        
                        //シリアライズ
                        sw.WriteLine(serializer.Serialize(chip));
                    }
                }

                //using (var sw = new StreamWriter("result.yaml"))
                //{
                //    var serializer = new YamlDotNet.Serialization.Serializer();
                //    sw.WriteLine(serializer.Serialize(chips));
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }
    }
}
