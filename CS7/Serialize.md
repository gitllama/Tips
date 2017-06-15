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
    
    [NonSerialized]
    public string name;
    
    public Obj DeepCopy()
    {
        using (var stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            
            stream.Position = 0; 
            //stream.Seek(0, System.IO.SeekOrigin.Begin);
            
            return (Obj)formatter.Deserialize(stream);
        }
    }
}
```

## Jsonの例

## Yamlの例
