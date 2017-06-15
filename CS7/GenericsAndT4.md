# Genericsによる演算とT4によるコード自動生成

## C#におけるGenerics

同じ処理を複数の方に適応する際

### 1.オーバーロード

```C#
void A(int key){ }
void A(double key){ }
```

もっとも速い  
C++のTempleteはコンパイル時インライン展開されるので同等

### 2.Generics

```C#
void A<T>(T key) where T : struct { }
```

IL上では共通コード。  
Cast,boxing/unboxingがないのでそこそこの実行も良い  
(正確には値型のみ展開して、参照型は型消去に近いコード)

静的メソッドがよべない

### 3.Object/Dynamic

```C#
void A(object key) { }
void A(dynamic key) { }
```

ボックス化/リフレクションするので遅い  

## ジェネリックでの演算子使用の解決方法

演算子が使えない問題がある

### 1.式木での自動生成

複雑なループ処理は無理  
キャッシュされるのでそこそこ速い

### 2.デリゲート

```C#
void A(Func<T,T> func) { }
var sum = items.Aggregate(0, (x, y) => x + y);
```

デリゲートの呼び出し分オーバーヘッドあり  
そこまで遅くはない

### 3.ポリシー パターン/ポリシーベース設計

```C#
var sum = Sum1(items, default(Add));
var sum = Sum2<int, Add>(items);

static T Sum1<T, TOperator>(T[] items, TOperator op) where TOperator : struct, IBinaryOperator<T>
{
    var sum = op.Zero;
    foreach (var item in items)
        sum = op.Operate(sum, item);
    return sum;
}
static T Sum2<T, TOperator>(T[] items) where TOperator : struct, IBinaryOperator<T>
{
    var sum = default(TOperator).Zero;
    foreach (var item in items)
        sum = default(TOperator).Operate(sum, item);
    // ↑ メソッド内で default()  空の構造体なのでほぼノーコスト
    return sum;
}
interface IBinaryOperator<T>
{
    T Zero { get; }
    T Operate(T x, T y);
}
struct Add : IBinaryOperator<int>
{
    public int Zero => 0;
    public int Operate(int x, int y) => x + y;
}

```

Genericsの値型全展開を利用 
デリゲートやインターフェイスを介するよりも最適化が掛かりやすく  
静的メソッドに近い性能

## T4によるコード自動生成

以上をふまえ、実際にコードを記述する際  
T4によるコード生成を活用するとよい

T4をそのまま使用するとVSのインテリセンスやリファクタリングが効かないので  
CSファイルを介してコードの生成を行うと安心。

### T4

外部DLLとしてDynamicJson.dllを参照し、T4.csの特定のコメントで囲まれた領域を  
T4側で展開。

多重ループは実装していません。  
置換部分が被りそうな場合は ```/*1*/Int32/**/ ``` のようにコメントで挟んで  
区別するようにしてもよし。

T4.cs
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace T4TEST/*T4namespace{*/.Base/*}T4namespace*/
{
    public class T4Class
    {
        /*T4{[
            {"Key": ["Add","(+=)"], "Value": [["Add","(+=)"],["Sub","(-=)"],["Mul","(*=)"]]},
            {"Key": ["Int32"], "Value": [["Int16"],["Int32"],["Int64"],["Single"],["Double"]]}
        ]T4h*/
        public Int32 Add(Int32[] src)
        {
            Int32 dst = 1;
            for (int i = 0; i < src.Length; i++)
                dst += src[i];
            return dst;
        }/*}T4*/
    }
}
```

T4t.tt
```
<#@ template debug="true" hostspecific="true" language="C#" #>
<#@ assembly name="System.Core" #>
<#@ assembly name="Microsoft.Csharp" #>
<#@ assembly name="$(SolutionDir)DynamicJson.dll" #>
<#@ import namespace="System.Linq" #>
<#@ import namespace="System.Text" #>
<#@ import namespace="System.IO" #>
<#@ import namespace="System" #>
<#@ import namespace="System.Text.RegularExpressions" #>
<#@ import namespace="System.Collections.Generic" #>
<#@ import namespace="Codeplex.Data" #>
<#@ output extension=".cs" #>
<# 
	//namespaceの変更（競合を避けるため）
	var src = Regex.Replace(
		File.ReadAllText(Host.ResolvePath(@"T4t.cs")),
		@"\/\*T4namespace\{([\s\S]*?)\}T4namespace\*\/",
		(match) => 
		{
			return "";
		});
	//展開
	var dst = Regex.Replace(
		src,
        @"\/\*T4\{(?<json>[\s\S]*?)T4h\*\/(?<m>[\s\S]*?)\/\*\}T4\*\/",
		(match) => 
		{
			List<Obj> json = DynamicJson.Parse(match.Groups["json"].Value);

			//ダミーに置き換え
			var dummy = dummystr(match.Groups["m"].Value, json);

			return convert(
				dummy, 
				json, "");
		});
#><#= dst #>
<#+
    public class Obj
    {
        public string[] Key { get; set; }
        public List<string[]> Value { get; set; }
    }
	public string dummystr(string src, List<Obj> obj)
	{
		string dst = src;
		int c = 0;
		foreach (var n in obj)
        {
			for(int i = 0; i < n.Key.Length; i++)
			{
				dst = dst.Replace(n.Key[i], $"t4dummystr{c:0000}");
				n.Key[i] = $"t4dummystr{c:0000}";
				c++;
			}
        }
		return dst;
	}
    public string convert(string src, List<Obj> obj, string str)
    {
        if (obj.Count < 1) return str + src;

        var now = obj.First();
        foreach (var v in now.Value)
        {
            var buf = src;
            foreach (var k in now.Key.Select((item, index) => new { item, index }))
                buf = buf.Replace(k.item, v[k.index]);
            str = convert(buf, obj.Where(x => x.Key != now.Key).ToList(), str);
        }
        return str;
    }
#>
```

### 型スイッチ

