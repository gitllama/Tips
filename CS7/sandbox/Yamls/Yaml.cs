using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

using System.Windows;
using System.ComponentModel;
using System.IO.Compression;

namespace BTUSB.Yamls
{

    public class BTYaml
    {
        public YamlMappingNode mapping = null;
        public YamlScalarNode node = null;
        public YamlSequenceNode nodeSeq = null;
        public Encoding encode = Encoding.UTF8;

        public string filepath { get; private set; } = "";

        /*コンストラクタ*/

        public BTYaml() { }
        public BTYaml(string path) => Load(path);


        /*インデクサ*/

        public BTYaml this[string i]
        {
            get
            {
                var dst = new BTYaml();

                if (mapping == null) return dst;

                if (!mapping.Children.ContainsKey(new YamlScalarNode(i))) return dst;

                switch (mapping[new YamlScalarNode(i)])
                {
                    case YamlMappingNode n:
                        dst.mapping = n;
                        break;
                    case YamlScalarNode n:
                        dst.node = n;
                        break;
                    case YamlSequenceNode n:
                        dst.nodeSeq = n;
                        break;
                    default:
                        break;
                }

                return dst;
            }
        }

        public T Parse<T>(T defaultValue) => TryParse<T>(out T result) ? result : defaultValue;
        public T Parse<T>() => TryParse<T>(out T result) ? result : default(T);

        public bool TryParse<T>(out T result)
        {
            result = default(T);
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    //ConvertFromString(string text)の戻りは object なので T型でキャストする
                    result = (T)converter.ConvertFromString((dynamic)(node.Value));
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

            //if (typeof(T) == typeof(bool)) return bool.TryParse(node.Value, out _);
            //return Enum.TryParse((dynamic)(node.Value), out hoge);

            //return
            //      typeof(T) == typeof(Int32) & int.TryParse(node.Value, out int i) ? (dynamic)(i)
            //    : typeof(T) == typeof(double) & double.TryParse(node.Value, out double j) ? (dynamic)(j)
            //    : typeof(T) == typeof(bool) & bool.TryParse(node.Value, out bool k) ? (dynamic)(k)
            //    : typeof(T) == typeof(string) ? (dynamic)(node.Value)
            //    : (T)Enum.Parse(typeof(T), node.Value, true);
            //switch (typeof(T))
            //{
            //    case :
            //        break;

            //}
            //return ;

            //foreach (var tuple in valuesMapping.Children)
            //{
            //    Console.WriteLine("{0} => {1}", tuple.Key, tuple.Value);
            //    Console.WriteLine(((YamlScalarNode)entry.Key).Value);
            //}
        }

        public Dictionary<string, BTYaml> ToDictionary()
        {
            if (mapping == null) return null;

            var hoge = new Dictionary<string, BTYaml>();
            foreach(var i in this.Keys)
            {
                hoge.Add(i, this[i]);
            }
            return hoge;
        }

        public List<string> Keys
        {
            get
            {
                return mapping.Children.Keys.Select(x => x.ToString()).ToList();
            }
        }

        public List<string> Values
        {
            get => 
                  nodeSeq != null ? nodeSeq.Select(x => x.ToString()).ToList()
                : node != null ? new List<string>() { node.Value.ToString() }
                : null;
        }

        /*Deserialize*/

        public static T Deserialize<T>(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var deserializer = new Deserializer();
                return deserializer.Deserialize<T>(sr);
            }
        }

        public T Deserialize<T>()
        {
            var deserializer = new Deserializer();
            return deserializer.Deserialize<T>(Serialize());
        }

        public void Load(string path)
        {
            if (path == "")
            {
                var a = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"config.yaml");
                var b = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"config.conf");

                if (System.IO.File.Exists(a)) filepath = a; 
                else if (System.IO.File.Exists(b)) filepath = b; 
                else throw new IOException("Config Not found");
            }        
            else
            {
                filepath = path;
                if (!System.IO.File.Exists(path)) throw new IOException("Config Not found");
            }

            Load();
        }

        public void Load()
        {
            if (IsTextFile(filepath))
            {
                using (var input = new StreamReader(filepath))
                {
                    var yaml = new YamlStream();
                    yaml.Load(input);

                    //ルートマッピングの取得
                    mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

                    //var g = mapping[new YamlScalarNode("b")];
                    //var Year = (YamlScalarNode)mapping.Children[new YamlScalarNode("b")];
                }
            }
            else
            {
                using (var archive = ZipFile.OpenRead(filepath))
                {
                    var entry = archive.GetEntry("config.yaml");
                    using (var input = new StreamReader(entry.Open()))
                    {
                        var yaml = new YamlStream();
                        yaml.Load(input);
                        mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
                    }
                }
            }

            using (var input = new StreamReader(filepath))
            {
                var yaml = new YamlStream();
                yaml.Load(input);

                //ルートマッピングの取得
                mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

                //var g = mapping[new YamlScalarNode("b")];
                //var Year = (YamlScalarNode)mapping.Children[new YamlScalarNode("b")];
            }
        }

        /*Serialize*/

        public static void Serialize(string path, object obj)
        {
            using (var sw = new StreamWriter(path))
            {
                var serializer = new Serializer();
                sw.WriteLine(serializer.Serialize(obj));
            }
        }

        public static string Serialize(object obj)
        {
            var serializer = new Serializer();
            return serializer.Serialize(obj);
        }

        public string Serialize()
        {
            var serializer = new Serializer();
            return serializer.Serialize(mapping);
        }

        public string Save()
        {
            var serializer = new Serializer();
            return serializer.Serialize(mapping);
        }


        /**/
        private bool IsTextFile(string filePath)
        {
            FileStream file = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] byteData = new byte[1];
            while (file.Read(byteData, 0, byteData.Length) > 0)
            {
                if (byteData[0] == 0)
                    return false;
            }
            return true;
        }

        public static explicit operator Dictionary<object, object>(BTYaml v)
        {
            throw new NotImplementedException();
        }
    }

}


//public class _BTYaml
//{
//    public dynamic data;
//    public string yamlname { get; private set; }

//    public BTYaml()
//    {

//    }
//    public BTYaml(string path = "")
//    {

//        if (path == "")
//        {
//            var a = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"config.yaml");
//            var b = Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory), @"config.conf");

//            if (System.IO.File.Exists(a))
//                yamlname = a;
//            else if (System.IO.File.Exists(b))
//                yamlname = b;
//            else
//            {
//                yamlname = "";
//                throw new IOException("Config Not found");
//            }

//        }
//        else
//        {
//            yamlname = path;
//            if (!System.IO.File.Exists(path))
//                throw new IOException("Config Not found");
//        }

//        if (IsTextFile(yamlname))
//        {
//            using (var reader = new StreamReader(yamlname))
//            {
//                var deserializer = new Deserializer();
//                data = deserializer.Deserialize(reader);
//            }
//        }
//        else
//        {
//            using (var archive = ZipFile.OpenRead(yamlname))
//            {
//                var entry = archive.GetEntry("config.yaml");
//                using (var r = new StreamReader(entry.Open()))
//                {
//                    var deserializer = new Deserializer();
//                    data = deserializer.Deserialize(r);
//                }
//            }
//        }
//    }

//    /* インデクサ */
//    public BTYaml this[string key]
//    {
//        get
//        {
//            BTYaml dst = new BTYaml();
//            dst.data
//                = data == null ? null
//                : data.ContainsKey(key) ? data?[key]
//                : null;
//            return dst;
//        }
//    }

//    /*値の取り出し便利*/

//    public int GetValue(int defaultvalue)
//    {
//        int dst;
//        return int.TryParse(data, out dst) ? dst : defaultvalue;
//    }
//    public double GetValue(double defaultvalue)
//    {
//        double dst;
//        return double.TryParse(data, out dst) ? dst : defaultvalue;
//    }
//    public bool GetValue(bool defaultvalue)
//    {
//        bool dst;
//        return Boolean.TryParse(data, out dst) ? dst : defaultvalue;
//    }
//    public string GetValue(string defaultvalue)
//    {
//        return data ?? defaultvalue;
//    }

//    public T GetValue<T>(string defaultvalue) where T : struct
//    {
//        //if (typeof(T) != typeof(Enum)) throw new ArgumentException();
//        return (T)Enum.Parse(typeof(T), data ?? defaultvalue, true);
//    }





//    public static string Serialize(object a)
//    {
//        var serializer = new Serializer();
//        return serializer.Serialize(a);
//    }
