# Serialize

- Binary (System.Runtime.Serialization.Formatters.Binary)  
- XML  
- Json  
- Yaml  

## シリアライズを使用したオブジェクトのディープコピー
        
```C#
[Serializable()]
public class Obj
{
  public int id;
  public string name;
  
  public static Obj DeepCopy()
  {
    MemoryStream mem = new MemoryStream();
    try
    {
      (new BinaryFormatter()).Serialize(mem, this);
      mem.Position = 0;
      return b.Deserialize(mem);
    }
    finally
    {
      mem.Close();
    }
    return null;
  }
}
```

## Jsonの例

## Yamlの例
