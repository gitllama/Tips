






using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

using System.IO.Compression;
using System.Threading.Tasks;

namespace Pixels.Stream
{
    public static class PixelStream
    {

        public static Pixel<Byte> Read(this Pixel<Byte> src, string filename, int offsetbyte, String type) =>  Read(src, filename, offsetbyte, System.Type.GetType($"System.{type}"));
        public static Pixel<Byte> Read(this Pixel<Byte> src, string filename, int offsetbyte = 0, Type type = null)
        {
            byte[] data;
            int count_byte = offsetbyte;
            switch(type?.Name ?? "Byte")
            {

				case "UInt16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Byte)BitConverter.ToUInt16(data, count_byte);
                        count_byte += sizeof(UInt16);
                    }
                    break;

				case "UInt32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Byte)BitConverter.ToUInt32(data, count_byte);
                        count_byte += sizeof(UInt32);
                    }
                    break;

				case "UInt64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Byte)BitConverter.ToUInt64(data, count_byte);
                        count_byte += sizeof(UInt64);
                    }
                    break;

				case "Int16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Byte)BitConverter.ToInt16(data, count_byte);
                        count_byte += sizeof(Int16);
                    }
                    break;

				case "Int32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Byte)BitConverter.ToInt32(data, count_byte);
                        count_byte += sizeof(Int32);
                    }
                    break;

				case "Int64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Byte)BitConverter.ToInt64(data, count_byte);
                        count_byte += sizeof(Int64);
                    }
                    break;

				case "Single":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Byte)BitConverter.ToSingle(data, count_byte);
                        count_byte += sizeof(Single);
                    }
                    break;

				case "Double":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Byte)BitConverter.ToDouble(data, count_byte);
                        count_byte += sizeof(Double);
                    }
                    break;

                case "Byte":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                        src.pixel[i] = data[count_byte ++];
                    break;
                case "String":
                    break;
                default:
                    
                    break;
            }
            return src;
        }

        public static Pixel<UInt16> Read(this Pixel<UInt16> src, string filename, int offsetbyte, String type) =>  Read(src, filename, offsetbyte, System.Type.GetType($"System.{type}"));
        public static Pixel<UInt16> Read(this Pixel<UInt16> src, string filename, int offsetbyte = 0, Type type = null)
        {
            byte[] data;
            int count_byte = offsetbyte;
            switch(type?.Name ?? "UInt16")
            {

				case "UInt16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt16)BitConverter.ToUInt16(data, count_byte);
                        count_byte += sizeof(UInt16);
                    }
                    break;

				case "UInt32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt16)BitConverter.ToUInt32(data, count_byte);
                        count_byte += sizeof(UInt32);
                    }
                    break;

				case "UInt64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt16)BitConverter.ToUInt64(data, count_byte);
                        count_byte += sizeof(UInt64);
                    }
                    break;

				case "Int16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt16)BitConverter.ToInt16(data, count_byte);
                        count_byte += sizeof(Int16);
                    }
                    break;

				case "Int32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt16)BitConverter.ToInt32(data, count_byte);
                        count_byte += sizeof(Int32);
                    }
                    break;

				case "Int64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt16)BitConverter.ToInt64(data, count_byte);
                        count_byte += sizeof(Int64);
                    }
                    break;

				case "Single":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt16)BitConverter.ToSingle(data, count_byte);
                        count_byte += sizeof(Single);
                    }
                    break;

				case "Double":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt16)BitConverter.ToDouble(data, count_byte);
                        count_byte += sizeof(Double);
                    }
                    break;

                case "Byte":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                        src.pixel[i] = data[count_byte ++];
                    break;
                case "String":
                    break;
                default:
                    
                    break;
            }
            return src;
        }

        public static Pixel<UInt32> Read(this Pixel<UInt32> src, string filename, int offsetbyte, String type) =>  Read(src, filename, offsetbyte, System.Type.GetType($"System.{type}"));
        public static Pixel<UInt32> Read(this Pixel<UInt32> src, string filename, int offsetbyte = 0, Type type = null)
        {
            byte[] data;
            int count_byte = offsetbyte;
            switch(type?.Name ?? "UInt32")
            {

				case "UInt16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt32)BitConverter.ToUInt16(data, count_byte);
                        count_byte += sizeof(UInt16);
                    }
                    break;

				case "UInt32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt32)BitConverter.ToUInt32(data, count_byte);
                        count_byte += sizeof(UInt32);
                    }
                    break;

				case "UInt64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt32)BitConverter.ToUInt64(data, count_byte);
                        count_byte += sizeof(UInt64);
                    }
                    break;

				case "Int16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt32)BitConverter.ToInt16(data, count_byte);
                        count_byte += sizeof(Int16);
                    }
                    break;

				case "Int32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt32)BitConverter.ToInt32(data, count_byte);
                        count_byte += sizeof(Int32);
                    }
                    break;

				case "Int64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt32)BitConverter.ToInt64(data, count_byte);
                        count_byte += sizeof(Int64);
                    }
                    break;

				case "Single":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt32)BitConverter.ToSingle(data, count_byte);
                        count_byte += sizeof(Single);
                    }
                    break;

				case "Double":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt32)BitConverter.ToDouble(data, count_byte);
                        count_byte += sizeof(Double);
                    }
                    break;

                case "Byte":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                        src.pixel[i] = data[count_byte ++];
                    break;
                case "String":
                    break;
                default:
                    
                    break;
            }
            return src;
        }

        public static Pixel<UInt64> Read(this Pixel<UInt64> src, string filename, int offsetbyte, String type) =>  Read(src, filename, offsetbyte, System.Type.GetType($"System.{type}"));
        public static Pixel<UInt64> Read(this Pixel<UInt64> src, string filename, int offsetbyte = 0, Type type = null)
        {
            byte[] data;
            int count_byte = offsetbyte;
            switch(type?.Name ?? "UInt64")
            {

				case "UInt16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt64)BitConverter.ToUInt16(data, count_byte);
                        count_byte += sizeof(UInt16);
                    }
                    break;

				case "UInt32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt64)BitConverter.ToUInt32(data, count_byte);
                        count_byte += sizeof(UInt32);
                    }
                    break;

				case "UInt64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt64)BitConverter.ToUInt64(data, count_byte);
                        count_byte += sizeof(UInt64);
                    }
                    break;

				case "Int16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt64)BitConverter.ToInt16(data, count_byte);
                        count_byte += sizeof(Int16);
                    }
                    break;

				case "Int32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt64)BitConverter.ToInt32(data, count_byte);
                        count_byte += sizeof(Int32);
                    }
                    break;

				case "Int64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt64)BitConverter.ToInt64(data, count_byte);
                        count_byte += sizeof(Int64);
                    }
                    break;

				case "Single":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt64)BitConverter.ToSingle(data, count_byte);
                        count_byte += sizeof(Single);
                    }
                    break;

				case "Double":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (UInt64)BitConverter.ToDouble(data, count_byte);
                        count_byte += sizeof(Double);
                    }
                    break;

                case "Byte":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                        src.pixel[i] = data[count_byte ++];
                    break;
                case "String":
                    break;
                default:
                    
                    break;
            }
            return src;
        }

        public static Pixel<Int16> Read(this Pixel<Int16> src, string filename, int offsetbyte, String type) =>  Read(src, filename, offsetbyte, System.Type.GetType($"System.{type}"));
        public static Pixel<Int16> Read(this Pixel<Int16> src, string filename, int offsetbyte = 0, Type type = null)
        {
            byte[] data;
            int count_byte = offsetbyte;
            switch(type?.Name ?? "Int16")
            {

				case "UInt16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int16)BitConverter.ToUInt16(data, count_byte);
                        count_byte += sizeof(UInt16);
                    }
                    break;

				case "UInt32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int16)BitConverter.ToUInt32(data, count_byte);
                        count_byte += sizeof(UInt32);
                    }
                    break;

				case "UInt64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int16)BitConverter.ToUInt64(data, count_byte);
                        count_byte += sizeof(UInt64);
                    }
                    break;

				case "Int16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int16)BitConverter.ToInt16(data, count_byte);
                        count_byte += sizeof(Int16);
                    }
                    break;

				case "Int32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int16)BitConverter.ToInt32(data, count_byte);
                        count_byte += sizeof(Int32);
                    }
                    break;

				case "Int64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int16)BitConverter.ToInt64(data, count_byte);
                        count_byte += sizeof(Int64);
                    }
                    break;

				case "Single":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int16)BitConverter.ToSingle(data, count_byte);
                        count_byte += sizeof(Single);
                    }
                    break;

				case "Double":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int16)BitConverter.ToDouble(data, count_byte);
                        count_byte += sizeof(Double);
                    }
                    break;

                case "Byte":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                        src.pixel[i] = data[count_byte ++];
                    break;
                case "String":
                    break;
                default:
                    
                    break;
            }
            return src;
        }

        public static Pixel<Int32> Read(this Pixel<Int32> src, string filename, int offsetbyte, String type) =>  Read(src, filename, offsetbyte, System.Type.GetType($"System.{type}"));
        public static Pixel<Int32> Read(this Pixel<Int32> src, string filename, int offsetbyte = 0, Type type = null)
        {
            byte[] data;
            int count_byte = offsetbyte;
            switch(type?.Name ?? "Int32")
            {

				case "UInt16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int32)BitConverter.ToUInt16(data, count_byte);
                        count_byte += sizeof(UInt16);
                    }
                    break;

				case "UInt32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int32)BitConverter.ToUInt32(data, count_byte);
                        count_byte += sizeof(UInt32);
                    }
                    break;

				case "UInt64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int32)BitConverter.ToUInt64(data, count_byte);
                        count_byte += sizeof(UInt64);
                    }
                    break;

				case "Int16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int32)BitConverter.ToInt16(data, count_byte);
                        count_byte += sizeof(Int16);
                    }
                    break;

				case "Int32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int32)BitConverter.ToInt32(data, count_byte);
                        count_byte += sizeof(Int32);
                    }
                    break;

				case "Int64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int32)BitConverter.ToInt64(data, count_byte);
                        count_byte += sizeof(Int64);
                    }
                    break;

				case "Single":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int32)BitConverter.ToSingle(data, count_byte);
                        count_byte += sizeof(Single);
                    }
                    break;

				case "Double":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int32)BitConverter.ToDouble(data, count_byte);
                        count_byte += sizeof(Double);
                    }
                    break;

                case "Byte":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                        src.pixel[i] = data[count_byte ++];
                    break;
                case "String":
                    break;
                default:
                    
                    break;
            }
            return src;
        }

        public static Pixel<Int64> Read(this Pixel<Int64> src, string filename, int offsetbyte, String type) =>  Read(src, filename, offsetbyte, System.Type.GetType($"System.{type}"));
        public static Pixel<Int64> Read(this Pixel<Int64> src, string filename, int offsetbyte = 0, Type type = null)
        {
            byte[] data;
            int count_byte = offsetbyte;
            switch(type?.Name ?? "Int64")
            {

				case "UInt16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int64)BitConverter.ToUInt16(data, count_byte);
                        count_byte += sizeof(UInt16);
                    }
                    break;

				case "UInt32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int64)BitConverter.ToUInt32(data, count_byte);
                        count_byte += sizeof(UInt32);
                    }
                    break;

				case "UInt64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int64)BitConverter.ToUInt64(data, count_byte);
                        count_byte += sizeof(UInt64);
                    }
                    break;

				case "Int16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int64)BitConverter.ToInt16(data, count_byte);
                        count_byte += sizeof(Int16);
                    }
                    break;

				case "Int32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int64)BitConverter.ToInt32(data, count_byte);
                        count_byte += sizeof(Int32);
                    }
                    break;

				case "Int64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int64)BitConverter.ToInt64(data, count_byte);
                        count_byte += sizeof(Int64);
                    }
                    break;

				case "Single":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int64)BitConverter.ToSingle(data, count_byte);
                        count_byte += sizeof(Single);
                    }
                    break;

				case "Double":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Int64)BitConverter.ToDouble(data, count_byte);
                        count_byte += sizeof(Double);
                    }
                    break;

                case "Byte":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                        src.pixel[i] = data[count_byte ++];
                    break;
                case "String":
                    break;
                default:
                    
                    break;
            }
            return src;
        }

        public static Pixel<Single> Read(this Pixel<Single> src, string filename, int offsetbyte, String type) =>  Read(src, filename, offsetbyte, System.Type.GetType($"System.{type}"));
        public static Pixel<Single> Read(this Pixel<Single> src, string filename, int offsetbyte = 0, Type type = null)
        {
            byte[] data;
            int count_byte = offsetbyte;
            switch(type?.Name ?? "Single")
            {

				case "UInt16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Single)BitConverter.ToUInt16(data, count_byte);
                        count_byte += sizeof(UInt16);
                    }
                    break;

				case "UInt32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Single)BitConverter.ToUInt32(data, count_byte);
                        count_byte += sizeof(UInt32);
                    }
                    break;

				case "UInt64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Single)BitConverter.ToUInt64(data, count_byte);
                        count_byte += sizeof(UInt64);
                    }
                    break;

				case "Int16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Single)BitConverter.ToInt16(data, count_byte);
                        count_byte += sizeof(Int16);
                    }
                    break;

				case "Int32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Single)BitConverter.ToInt32(data, count_byte);
                        count_byte += sizeof(Int32);
                    }
                    break;

				case "Int64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Single)BitConverter.ToInt64(data, count_byte);
                        count_byte += sizeof(Int64);
                    }
                    break;

				case "Single":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Single)BitConverter.ToSingle(data, count_byte);
                        count_byte += sizeof(Single);
                    }
                    break;

				case "Double":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Single)BitConverter.ToDouble(data, count_byte);
                        count_byte += sizeof(Double);
                    }
                    break;

                case "Byte":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                        src.pixel[i] = data[count_byte ++];
                    break;
                case "String":
                    break;
                default:
                    
                    break;
            }
            return src;
        }

        public static Pixel<Double> Read(this Pixel<Double> src, string filename, int offsetbyte, String type) =>  Read(src, filename, offsetbyte, System.Type.GetType($"System.{type}"));
        public static Pixel<Double> Read(this Pixel<Double> src, string filename, int offsetbyte = 0, Type type = null)
        {
            byte[] data;
            int count_byte = offsetbyte;
            switch(type?.Name ?? "Double")
            {

				case "UInt16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Double)BitConverter.ToUInt16(data, count_byte);
                        count_byte += sizeof(UInt16);
                    }
                    break;

				case "UInt32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Double)BitConverter.ToUInt32(data, count_byte);
                        count_byte += sizeof(UInt32);
                    }
                    break;

				case "UInt64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Double)BitConverter.ToUInt64(data, count_byte);
                        count_byte += sizeof(UInt64);
                    }
                    break;

				case "Int16":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Double)BitConverter.ToInt16(data, count_byte);
                        count_byte += sizeof(Int16);
                    }
                    break;

				case "Int32":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Double)BitConverter.ToInt32(data, count_byte);
                        count_byte += sizeof(Int32);
                    }
                    break;

				case "Int64":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Double)BitConverter.ToInt64(data, count_byte);
                        count_byte += sizeof(Int64);
                    }
                    break;

				case "Single":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Double)BitConverter.ToSingle(data, count_byte);
                        count_byte += sizeof(Single);
                    }
                    break;

				case "Double":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                    {
                        src.pixel[i] = (Double)BitConverter.ToDouble(data, count_byte);
                        count_byte += sizeof(Double);
                    }
                    break;

                case "Byte":
					data = System.IO.File.ReadAllBytes(filename);
                    for (int i= 0; i< src.pixel.Length; i++)
                        src.pixel[i] = data[count_byte ++];
                    break;
                case "String":
                    break;
                default:
                    
                    break;
            }
            return src;
        }

    }
}