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
```
