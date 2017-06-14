## index付きforeach

```C#
foreach (var x in items.Select((item, index) => new { item, index }))
    Console.WriteLine($"index: {x.index}, value: {x.item}");

//C#7なら
foreach (var (item, index) in items.Select((item, index) => (item, index)))
    Console.WriteLine($"index: {index}, value: {item}");
```

最適化する場合の参考
https://gist.github.com/ufcpp/2b3e1a5821169f6b21ded175ad05c752
