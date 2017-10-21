using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Pixels.IO
{
    #region XML

    public static class XMLSetter
    {
        //＜XMLファイルに書き込む＞
        public static void Write<T>(T obj, string filename)
        {    
            using (var sw = new StreamWriter(filename, false, new UTF8Encoding(false)))
            {
                //シリアル化し、XMLファイルに保存する
                var serializer1 = new XmlSerializer(typeof(T));
                serializer1.Serialize(sw, obj);
            }
        }
        public static void Write<T>(T obj, Stream z)
        {
            //＜XMLファイルに書き込む＞
            var serializer1 = new XmlSerializer(typeof(T));
            //シリアル化し、XMLファイルに保存する
            serializer1.Serialize(z, obj);
        }

        public static T Read<T>(string filename)
        {
            //＜XMLファイルから読み込む＞
            using (var sr = new StreamReader(filename, new UTF8Encoding(false)))
            {
                var serializer2 = new XmlSerializer(typeof(T));
                return (T)serializer2.Deserialize(sr);
            }
        }
        public static T Read<T>(string filename, Stream z)
        {
            //＜XMLファイルから読み込む＞
            var serializer2 = new XmlSerializer(typeof(T));
            return (T)serializer2.Deserialize(z);
        }

    }

    #endregion
}

namespace Pixels.Util
{
    public enum ImageType
    {
        JPEG = 0,
        PNG,
        BMP,
        TIFF
    }

    public static class BitmapSourceUtil
    {
        public static bool SaveImage(string szFile, BitmapImage bmp, ImageType Type)
    {
        BitmapEncoder enc = null;
        switch (Type)
        {
            case ImageType.BMP:
                enc = new BmpBitmapEncoder();
                break;
            case ImageType.PNG:
                enc = new PngBitmapEncoder();
                break;
            case ImageType.TIFF:
                enc = new TiffBitmapEncoder();
                break;
            case ImageType.JPEG:
                enc = new JpegBitmapEncoder();
                break;

        }
        if (enc == null) return false;

        try
        {
            FileStream fs = new FileStream(szFile, FileMode.Create);
            enc.Frames.Add(BitmapFrame.Create(bmp));
            enc.Save(fs);

            fs.Close();
            fs.Dispose();
        }
        catch
        {
            return false;
        }

        return true;
    }


        public static BitmapSource ToWPFBitmap(this System.Drawing.Bitmap bitmap)
        {
            var hBitmap = bitmap.GetHbitmap();

            BitmapSource source;
            try
            {
                source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap, IntPtr.Zero, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
            return source;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
 
}
namespace Pixels.Interface
{

    interface InterfacePixels
    {
    }

    public interface InterfaceHoge
    {
        string Message { get; }
    }

}