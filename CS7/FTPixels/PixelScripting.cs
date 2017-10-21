using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * .Net4.6
 * Nuget Microsoft.CodeAnalysis.CSharp
 * Nuget Microsoft.CodeAnalysis.CSharp.Scripting
 */
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

using Pixels;
using System.Reflection;

namespace PixelsExtend
{
    public static class PixelScripting
    {
        /* 画素フィルタスクリプト */
        public static ScriptResult Filter(PixelDouble _inputPixel, string code)
        {
            var ssr = ScriptSourceResolver.Default
                .WithBaseDirectory(Environment.CurrentDirectory);
            var options = ScriptOptions.Default
                .WithSourceResolver(ssr)
                .WithReferences(typeof(object).Assembly)//参照アセンブリを指定
                .WithReferences(typeof(PixelDouble).Assembly)
                .WithReferences(Assembly.GetEntryAssembly())
                .WithImports(//using する名前空間を指定
                    "System",
                    "System.Collections.Generic",
                    "Pixels");

            var hostObject = new hostObject_ { inputPixel = _inputPixel };

            var script = CSharpScript.Create(
                code,
                options,
                typeof(hostObject_)
                );

            var state = script.RunAsync(hostObject).Result;

            ScriptResult ret = new ScriptResult();
            ret.outputPixel = (PixelDouble)state.GetVariable("result")?.Value ?? null;
            ret.outputStr = (string)state.GetVariable("resultStr")?.Value ?? "";
            return ret;
        }

        public class hostObject_
        {
            public PixelDouble inputPixel;
        }

        public class ScriptResult
        {
            public PixelDouble outputPixel;
            public string outputStr = "";
        }
    }
}
