## 例外(WPF Prism)

```C#
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            MessageBox.Show(ex.ToString(), "UnhandledException",
                      MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
```

## 例外

```C#
try
{
    A();
    B("c");
    
    Parallel.For(0, 10000, F);
}
catch (Exception e) when (e is DirectoryNotFoundException || e is FileNotFoundException)
{
    Console.WriteLine(e);
}
catch (AggregateException e) when (e.InnerExceptions.Any(i => i is ArgumentException))
{
    // Parellel.For を通した結果、ここに来る例外は AggregateException
    // AggregateException.InnerExceptions の中に ArgumentException が入っている。
}

static void A() => throw new NotImplementedException();

static string B(object obj)
{
    var s = obj as string ?? throw new ArgumentException(nameof(obj));

    return 
      s.Length == 0 ? "empty"
      : s.Length < 5 ? "short"
      : throw new InvalidOperationException("too long");
}
```

| | |
|:---|:---|
|ArgumentException|	メソッドの引数が変な場合。ArgumentNullExceptionやArgumentOutOfRangeException以外の場合で変な時に使う。|
|ArgumentNullException|	引数がnullの場合。|
|ArgumentOutOfRangeException|	メソッドの許容範囲外の値が引数として渡された場合。|
|ArithmeticException|	算術演算によるエラーの基本クラス。OverflowException, DivideByZeroException, NotFiniteNumberException以外の算術エラーを示したければ使う。|
|OverflowException|	算術演算やキャストでオーバーフローが起きた場合。|
|DivideByZeroException|	0で割ったときのエラー。|
|NotFiniteNumberException|	浮動小数点値が無限大の場合。|
|FormatException|	引数の書式が仕様に一致していない場合。|
|IndexOutOfRangeException|	配列のインデックスが変な場合。|
|InvalidCastException|	無効なキャストの場合。|
|InvalidOperationException|	引数以外の原因でエラーが起きた場合。|
|ObjectDisposedException|	Dispose済みのオブジェクトで操作が実行される場合。|
|NotImplementedException|	メソッドが未実装の場合。|
|NotSupportedException|	呼び出されたメソッドがサポートされていない場合、または呼び出された機能を備えていないストリームに対して読み取り、シーク、書き込みが試行された場合|。
|NullReferenceException|	nullオブジェクト参照を逆参照しようとした場合。|
|PlatformNotSupportException|	特定のプラットフォームで機能が実行されない場合。|
|TimeoutException|	指定したタイムアウト時間が経過した場合。|
|KeyNotFoundException|	コレクションに該当するキーが無い場合。|
|DirectoryNotFoundException|	ディレクトリが無い場合。|
|FileNotFoundException|	ファイルが無い場合。|
|EndOfStreamException|	ストリームの末尾を超えて読み込もうとしている場合。|
