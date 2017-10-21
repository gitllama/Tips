using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

/*
 * .Net4.6
 * Nuget Microsoft.CodeAnalysis.CSharp
 * Nuget Microsoft.CodeAnalysis.CSharp.Scripting
 */
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Reflection;

using Pixels;


namespace ConsoleScriptingPixels
{
    class Program
    {
        static void Main(string[] args)
        {
            //PixelDouble p = new PixelDouble(new double[] { 1, 2, 3, 4 }, 2, 2);

            PixelDouble p = new PixelDouble(10000, 10000);


            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            PixelScripting.hostObject_ obj = new PixelScripting.hostObject_() { pixel = p };

            Console.WriteLine($"{p[0]}");
            Console.WriteLine($"{p[1]}");
            Console.WriteLine($"{p[2]}");
            Console.WriteLine($"{p[3]}");
            Console.WriteLine($"{p.Ave()}");

            Vector4[] a = new Vector4[10000 * 10000 / 4];
            for (int i=0;i< 10000 * 10000 /4;i++)
            {
                a[i] = new Vector4(0, 1, 2, 3);
            }

            sw.Start();

            //for (int i = 0; i < 3; i++)
            //{
            //    p = p - p;
            //}
            for (int j = 0; j < 3; j++)
                for (int i = 0;i <a.Length;i++)
                {
                    a[i] = a[i] + a[i]; //234はやい
                }
           


            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);

            Console.WriteLine("Scripting : ");
            for (;;)
            {
                Console.Write("> ");
                var str = Console.ReadLine();
                if (str == "quit") break;

                try
                {
                    var result = PixelScripting.Run(str, obj);
                }
                catch
                {
                    Console.WriteLine("Script Err.");
                }
            }
        }
    }

    public static class PixelScripting
    {

        public class hostObject_
        {
            public PixelDouble pixel;
        }

        public class ScriptResult
        {
            //public PixelDouble outputPixel;
            //public string outputStr = "";
        }

        /* 画素フィルタスクリプト */
        public static ScriptResult Run(string code) => Run(code, new hostObject_());
        public static ScriptResult Run(string code, hostObject_ obj)
        {
            //設定
            var ssr = ScriptSourceResolver.Default.WithBaseDirectory(Environment.CurrentDirectory);
            var options = ScriptOptions.Default
                .WithSourceResolver(ssr)
                .WithReferences(typeof(object).Assembly)//参照アセンブリを指定
                .WithReferences(Assembly.GetEntryAssembly())
                .WithImports(//using する名前空間を指定
                    "System",
                    "System.Collections.Generic"
                    );

            //コードの生成
            var script = CSharpScript.Create(
                code,
                options,
                typeof(hostObject_)
                );

            //実行
            var state = script.RunAsync(obj).Result;

            //結果の取り出し
            ScriptResult ret = new ScriptResult();
            //ret.outputPixel = (PixelDouble)state.GetVariable("result")?.Value ?? null;
            //ret.outputStr = (string)state.GetVariable("resultStr")?.Value ?? "";
            return ret;
        }

        //public static ScriptResult Filter(PixelDouble _inputPixel, string code)
        //{
        //    var ssr = ScriptSourceResolver.Default
        //        .WithBaseDirectory(Environment.CurrentDirectory);
        //    var options = ScriptOptions.Default
        //        .WithSourceResolver(ssr)
        //        .WithReferences(typeof(object).Assembly)//参照アセンブリを指定
        //        .WithReferences(typeof(PixelDouble).Assembly)
        //        .WithReferences(Assembly.GetEntryAssembly())
        //        .WithImports(//using する名前空間を指定
        //            "System",
        //            "System.Collections.Generic",
        //            "Pixels");

        //    var hostObject = new hostObject_ { inputPixel = _inputPixel };

        //    var script = CSharpScript.Create(
        //        code,
        //        options,
        //        typeof(hostObject_)
        //        );

        //    var state = script.RunAsync(hostObject).Result;

        //    ScriptResult ret = new ScriptResult();
        //    ret.outputPixel = (PixelDouble)state.GetVariable("result")?.Value ?? null;
        //    ret.outputStr = (string)state.GetVariable("resultStr")?.Value ?? "";
        //    return ret;
        //}


    }


    public class PixelTestCode
    {


    }

  

}
