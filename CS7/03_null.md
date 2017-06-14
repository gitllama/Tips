## Null Check

```C#
if (o?.Inner?.Value is int i)　WriteLine(new string('*', i));
    
var value = o?.Inner?.Value;

if (value.HasValue)　WriteLine(new string('*', value.Value));

int? z = x ?? y; // x != null ? x : y
int i = z ?? -1; // z != null ? z.Value : -1

var s = obj as string ?? throw new ArgumentException(nameof(obj));

return s.Length == 0 ? "empty" :
        s.Length < 5 ? "short" :
        throw new InvalidOperationException("too long");
        
int hage = hoge.HasValue ? hoge.Value : 0;
int hage = hoge.GetValueOrDefault(0);
int hage = list?[0]?.ToString() ?? "(null)";
```

## Null Check

Null条件演算子は三項演算子と違ってスレッドセーフ

```C#
public event PropertyChangedEventHandler PropertyChanged;
protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
{
  // 従来の書き方：C#ではいったん変数にキャッシュする必要がある
  var eventHandler = PropertyChanged;
  if (eventHandler != null)
  {
    // ここのタイミングで別スレッドからPropertyChanged変数をnullにされても
    // 問題が起きないように、「eventHandler」変数に代入している
    eventHandler(this, new PropertyChangedEventArgs(propertyName));
  }
}

↓

public event PropertyChangedEventHandler PropertyChanged;
protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
{
  // Null条件演算子を使う（スレッドセーフ）
  PropertyChanged?.Invoke(this,　new PropertyChangedEventArgs(propertyName));
}

```
