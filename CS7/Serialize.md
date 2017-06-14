# Serialize

- Binary (System.Runtime.Serialization.Formatters.Binary)  
- XML  
- Json  
- Yaml  

## シリアライズを使用したオブジェクトのディープコピー

参考までにシャローコピー

```C#
public MyClass Clone()
{
    return base.MemberwiseClone() as MyClass;
}
```

```C#
[Serializable]
public class Obj
{
    public int id;
    public string name;
    
    public static Obj DeepCopy()
    {
        using (var stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream);
        }
    }
}
```

## Jsonの例

## Yamlの例
