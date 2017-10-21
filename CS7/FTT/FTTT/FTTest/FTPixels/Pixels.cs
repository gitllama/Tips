using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Pixels
{

    public class Pixel
    {
        public int Width { get; set; } = 2256;
        public int Height { get; set; } = 1178;

        //public Dictionary<string, PixelMap> Map;

        public int[] pixel;

        public Pixel(int width,int height, int[] src)
        {
            Width = width;
            Height = height;
            pixel = src;
        }
        public Pixel(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Read(string path)
        {
            //ファイルがないときはnull
            if (!System.IO.File.Exists(path)) return;

            pixel = Pixels.Stream.PixelsStream.ReadRaw(
                path,
                Width, Height, 0, typeof(int));
        }

        //public Pixel Trim(string map) => Trim(Map[map].Left, Map[map].Top, Map[map].Width, Map[map].Height);
        public Pixel Trim(int left, int top, int width, int height)
        {
            if (pixel == null) return null;

            int count = 0;
            var dst = new int[width * height];
            for (int y = top; y < height; y ++)
            {
                for (int x = left; x < width; x++)
                {
                    dst[count++] = pixel[x + y * this.Width];
                }
            }
            return new Pixel(width, height, dst);
        }
    }

    public class PixelMap
    {
        public int Left { get; set; } = 0;
        public int Top { get; set; } = 0;
        public int Width { get; set; } = 2;
        public int Height { get; set; } = 2;
    }

}
namespace Pixels.Math
{
    public static class PixelExtensions
    {


    }
}
namespace Pixels.Stream
{
    public static class PixelsStream
    {
        public static int[] ReadRaw(string filename, int w, int h, int offset, Type type)
        {
            byte[] src = System.IO.File.ReadAllBytes(filename);
            int[] dst = new int[w * h];

            int count_byte = offset;
            switch(type.Name)
            {
                case "Int32":
                    for (int i= 0; i< w*h; i++)
                    {
                        dst[i] = (int)BitConverter.ToInt32(src, count_byte);
                        count_byte += sizeof(int);
                    }
                    break;
                default:
                    break;
            }
            return dst;
        }
    }
}
