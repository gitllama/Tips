using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

//.Net4.5以降
#if !dNet3_5
using System.Threading.Tasks;
using System.IO.Compression;

/*******************************************************************************/
/*   Pixels                                                                    */
/*   2016/03/23                                                                */
/*******************************************************************************/
namespace Pixels.Zipper
{
	/*--------------------------------------*/
    //   Zipperなラッパー
    /*--------------------------------------*/
    #region 読み書きZip
    public unsafe static partial class PixelStreamZipper
    {
	    #region Reader

        public static dynamic ReadZipPixel(string zipfilename, string statusname = "000")
        {
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
        }

		public static PixelByte ReadZipPixelByte(string zipfilename, string statusname = "000")
		{
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			byte[] buf = new byte[sta.Width * sta.Height];

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelByte)PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelByte)PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelByte)PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelByte)PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelByte)PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelByte)PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelByte)PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelByte)PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelByte)PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
		}
 		public static PixelUShort ReadZipPixelUShort(string zipfilename, string statusname = "000")
		{
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			ushort[] buf = new ushort[sta.Width * sta.Height];

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUShort)PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUShort)PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUShort)PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUShort)PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUShort)PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUShort)PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUShort)PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUShort)PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUShort)PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
		}
 		public static PixelUInt ReadZipPixelUInt(string zipfilename, string statusname = "000")
		{
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			uint[] buf = new uint[sta.Width * sta.Height];

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUInt)PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUInt)PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUInt)PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUInt)PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUInt)PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUInt)PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUInt)PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUInt)PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelUInt)PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
		}
 		public static PixelULong ReadZipPixelULong(string zipfilename, string statusname = "000")
		{
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			ulong[] buf = new ulong[sta.Width * sta.Height];

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelULong)PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelULong)PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelULong)PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelULong)PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelULong)PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelULong)PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelULong)PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelULong)PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelULong)PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
		}
 		public static PixelShort ReadZipPixelShort(string zipfilename, string statusname = "000")
		{
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			short[] buf = new short[sta.Width * sta.Height];

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelShort)PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelShort)PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelShort)PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelShort)PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelShort)PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelShort)PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelShort)PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelShort)PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelShort)PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
		}
 		public static PixelInt ReadZipPixelInt(string zipfilename, string statusname = "000")
		{
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			int[] buf = new int[sta.Width * sta.Height];

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelInt)PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelInt)PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelInt)PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelInt)PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelInt)PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelInt)PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelInt)PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelInt)PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelInt)PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
		}
 		public static PixelLong ReadZipPixelLong(string zipfilename, string statusname = "000")
		{
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			long[] buf = new long[sta.Width * sta.Height];

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelLong)PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelLong)PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelLong)PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelLong)PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelLong)PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelLong)PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelLong)PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelLong)PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelLong)PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
		}
 		public static PixelFloat ReadZipPixelFloat(string zipfilename, string statusname = "000")
		{
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			float[] buf = new float[sta.Width * sta.Height];

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelFloat)PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelFloat)PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelFloat)PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelFloat)PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelFloat)PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelFloat)PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelFloat)PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelFloat)PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelFloat)PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
		}
 		public static PixelDouble ReadZipPixelDouble(string zipfilename, string statusname = "000")
		{
			var sta = XML.ReadZipStatus(zipfilename, statusname);

			double[] buf = new double[sta.Width * sta.Height];

			switch(sta.type)
			{
				case "byte":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelDouble)PixelStream.ReadBinToPixelByte(reader, sta.Width, sta.Height, sta.offset);
				case "ushort":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelDouble)PixelStream.ReadBinToPixelUShort(reader, sta.Width, sta.Height, sta.offset);
				case "uint":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelDouble)PixelStream.ReadBinToPixelUInt(reader, sta.Width, sta.Height, sta.offset);
				case "ulong":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelDouble)PixelStream.ReadBinToPixelULong(reader, sta.Width, sta.Height, sta.offset);
				case "short":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelDouble)PixelStream.ReadBinToPixelShort(reader, sta.Width, sta.Height, sta.offset);
				case "int":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelDouble)PixelStream.ReadBinToPixelInt(reader, sta.Width, sta.Height, sta.offset);
				case "long":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelDouble)PixelStream.ReadBinToPixelLong(reader, sta.Width, sta.Height, sta.offset);
				case "float":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelDouble)PixelStream.ReadBinToPixelFloat(reader, sta.Width, sta.Height, sta.offset);
				case "double":
					using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
					using (var reader = archive.GetEntry(sta.filename).Open())
						return (PixelDouble)PixelStream.ReadBinToPixelDouble(reader, sta.Width, sta.Height, sta.offset);
				default:
					return null;
			}
		}
 		#endregion
        #region Writer
        public static void WriteZipTxt(this PixelByte p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
			var sta = new StaPara();
            sta.Width = p.Width;
            sta.Height = p.Height;
            sta.filename = statusname + ".txt";
			sta.type = "txt";

			ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry(statusname + ext_status, level).Open())
                    WriteStatus(z, sta);
                using (var z = archive.CreateEntry(sta.filename).Open())
					PixelStream.WriteTxt(z, p);
            }
        }
        public static void WriteZipTxt(this PixelUShort p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
			var sta = new StaPara();
            sta.Width = p.Width;
            sta.Height = p.Height;
            sta.filename = statusname + ".txt";
			sta.type = "txt";

			ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry(statusname + ext_status, level).Open())
                    WriteStatus(z, sta);
                using (var z = archive.CreateEntry(sta.filename).Open())
					PixelStream.WriteTxt(z, p);
            }
        }
        public static void WriteZipTxt(this PixelUInt p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
			var sta = new StaPara();
            sta.Width = p.Width;
            sta.Height = p.Height;
            sta.filename = statusname + ".txt";
			sta.type = "txt";

			ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry(statusname + ext_status, level).Open())
                    WriteStatus(z, sta);
                using (var z = archive.CreateEntry(sta.filename).Open())
					PixelStream.WriteTxt(z, p);
            }
        }
        public static void WriteZipTxt(this PixelULong p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
			var sta = new StaPara();
            sta.Width = p.Width;
            sta.Height = p.Height;
            sta.filename = statusname + ".txt";
			sta.type = "txt";

			ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry(statusname + ext_status, level).Open())
                    WriteStatus(z, sta);
                using (var z = archive.CreateEntry(sta.filename).Open())
					PixelStream.WriteTxt(z, p);
            }
        }
        public static void WriteZipTxt(this PixelShort p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
			var sta = new StaPara();
            sta.Width = p.Width;
            sta.Height = p.Height;
            sta.filename = statusname + ".txt";
			sta.type = "txt";

			ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry(statusname + ext_status, level).Open())
                    WriteStatus(z, sta);
                using (var z = archive.CreateEntry(sta.filename).Open())
					PixelStream.WriteTxt(z, p);
            }
        }
        public static void WriteZipTxt(this PixelInt p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
			var sta = new StaPara();
            sta.Width = p.Width;
            sta.Height = p.Height;
            sta.filename = statusname + ".txt";
			sta.type = "txt";

			ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry(statusname + ext_status, level).Open())
                    WriteStatus(z, sta);
                using (var z = archive.CreateEntry(sta.filename).Open())
					PixelStream.WriteTxt(z, p);
            }
        }
        public static void WriteZipTxt(this PixelLong p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
			var sta = new StaPara();
            sta.Width = p.Width;
            sta.Height = p.Height;
            sta.filename = statusname + ".txt";
			sta.type = "txt";

			ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry(statusname + ext_status, level).Open())
                    WriteStatus(z, sta);
                using (var z = archive.CreateEntry(sta.filename).Open())
					PixelStream.WriteTxt(z, p);
            }
        }
        public static void WriteZipTxt(this PixelFloat p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
			var sta = new StaPara();
            sta.Width = p.Width;
            sta.Height = p.Height;
            sta.filename = statusname + ".txt";
			sta.type = "txt";

			ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry(statusname + ext_status, level).Open())
                    WriteStatus(z, sta);
                using (var z = archive.CreateEntry(sta.filename).Open())
					PixelStream.WriteTxt(z, p);
            }
        }
        public static void WriteZipTxt(this PixelDouble p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
			var sta = new StaPara();
            sta.Width = p.Width;
            sta.Height = p.Height;
            sta.filename = statusname + ".txt";
			sta.type = "txt";

			ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry(statusname + ext_status, level).Open())
                    WriteStatus(z, sta);
                using (var z = archive.CreateEntry(sta.filename).Open())
					PixelStream.WriteTxt(z, p);
            }
        }
		public static void WriteZip(this PixelByte p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
            XML.PixelModelDependent.ModelDataXML buf = new XML.PixelModelDependent.ModelDataXML(); 

            buf.Width = p.Width;
            buf.Height = p.Height;
            buf.FileName = new List<string>{ $"^{statusname}.bin$" };
            buf.FileSize = -1;
            buf.SetFileType(typeof(PixelByte));
            //buf.SetFileType(p.GetType());

            ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry($"PixelsModel\\{statusname}.model", level).Open())
                {
                    Pixels.IO.XMLSetter.Write<PixelModelDependent.ModelDataXML>(buf, z);
                }
                using (var z = archive.CreateEntry($"{statusname}.bin").Open())
                {
                    PixelStream.WriteBin(z, p);
                }

            }
        }
		public static void WriteZip(this PixelUShort p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
            XML.PixelModelDependent.ModelDataXML buf = new XML.PixelModelDependent.ModelDataXML(); 

            buf.Width = p.Width;
            buf.Height = p.Height;
            buf.FileName = new List<string>{ $"^{statusname}.bin$" };
            buf.FileSize = -1;
            buf.SetFileType(typeof(PixelUShort));
            //buf.SetFileType(p.GetType());

            ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry($"PixelsModel\\{statusname}.model", level).Open())
                {
                    Pixels.IO.XMLSetter.Write<PixelModelDependent.ModelDataXML>(buf, z);
                }
                using (var z = archive.CreateEntry($"{statusname}.bin").Open())
                {
                    PixelStream.WriteBin(z, p);
                }

            }
        }
		public static void WriteZip(this PixelUInt p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
            XML.PixelModelDependent.ModelDataXML buf = new XML.PixelModelDependent.ModelDataXML(); 

            buf.Width = p.Width;
            buf.Height = p.Height;
            buf.FileName = new List<string>{ $"^{statusname}.bin$" };
            buf.FileSize = -1;
            buf.SetFileType(typeof(PixelUInt));
            //buf.SetFileType(p.GetType());

            ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry($"PixelsModel\\{statusname}.model", level).Open())
                {
                    Pixels.IO.XMLSetter.Write<PixelModelDependent.ModelDataXML>(buf, z);
                }
                using (var z = archive.CreateEntry($"{statusname}.bin").Open())
                {
                    PixelStream.WriteBin(z, p);
                }

            }
        }
		public static void WriteZip(this PixelULong p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
            XML.PixelModelDependent.ModelDataXML buf = new XML.PixelModelDependent.ModelDataXML(); 

            buf.Width = p.Width;
            buf.Height = p.Height;
            buf.FileName = new List<string>{ $"^{statusname}.bin$" };
            buf.FileSize = -1;
            buf.SetFileType(typeof(PixelULong));
            //buf.SetFileType(p.GetType());

            ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry($"PixelsModel\\{statusname}.model", level).Open())
                {
                    Pixels.IO.XMLSetter.Write<PixelModelDependent.ModelDataXML>(buf, z);
                }
                using (var z = archive.CreateEntry($"{statusname}.bin").Open())
                {
                    PixelStream.WriteBin(z, p);
                }

            }
        }
		public static void WriteZip(this PixelShort p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
            XML.PixelModelDependent.ModelDataXML buf = new XML.PixelModelDependent.ModelDataXML(); 

            buf.Width = p.Width;
            buf.Height = p.Height;
            buf.FileName = new List<string>{ $"^{statusname}.bin$" };
            buf.FileSize = -1;
            buf.SetFileType(typeof(PixelShort));
            //buf.SetFileType(p.GetType());

            ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry($"PixelsModel\\{statusname}.model", level).Open())
                {
                    Pixels.IO.XMLSetter.Write<PixelModelDependent.ModelDataXML>(buf, z);
                }
                using (var z = archive.CreateEntry($"{statusname}.bin").Open())
                {
                    PixelStream.WriteBin(z, p);
                }

            }
        }
		public static void WriteZip(this PixelInt p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
            XML.PixelModelDependent.ModelDataXML buf = new XML.PixelModelDependent.ModelDataXML(); 

            buf.Width = p.Width;
            buf.Height = p.Height;
            buf.FileName = new List<string>{ $"^{statusname}.bin$" };
            buf.FileSize = -1;
            buf.SetFileType(typeof(PixelInt));
            //buf.SetFileType(p.GetType());

            ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry($"PixelsModel\\{statusname}.model", level).Open())
                {
                    Pixels.IO.XMLSetter.Write<PixelModelDependent.ModelDataXML>(buf, z);
                }
                using (var z = archive.CreateEntry($"{statusname}.bin").Open())
                {
                    PixelStream.WriteBin(z, p);
                }

            }
        }
		public static void WriteZip(this PixelLong p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
            XML.PixelModelDependent.ModelDataXML buf = new XML.PixelModelDependent.ModelDataXML(); 

            buf.Width = p.Width;
            buf.Height = p.Height;
            buf.FileName = new List<string>{ $"^{statusname}.bin$" };
            buf.FileSize = -1;
            buf.SetFileType(typeof(PixelLong));
            //buf.SetFileType(p.GetType());

            ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry($"PixelsModel\\{statusname}.model", level).Open())
                {
                    Pixels.IO.XMLSetter.Write<PixelModelDependent.ModelDataXML>(buf, z);
                }
                using (var z = archive.CreateEntry($"{statusname}.bin").Open())
                {
                    PixelStream.WriteBin(z, p);
                }

            }
        }
		public static void WriteZip(this PixelFloat p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
            XML.PixelModelDependent.ModelDataXML buf = new XML.PixelModelDependent.ModelDataXML(); 

            buf.Width = p.Width;
            buf.Height = p.Height;
            buf.FileName = new List<string>{ $"^{statusname}.bin$" };
            buf.FileSize = -1;
            buf.SetFileType(typeof(PixelFloat));
            //buf.SetFileType(p.GetType());

            ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry($"PixelsModel\\{statusname}.model", level).Open())
                {
                    Pixels.IO.XMLSetter.Write<PixelModelDependent.ModelDataXML>(buf, z);
                }
                using (var z = archive.CreateEntry($"{statusname}.bin").Open())
                {
                    PixelStream.WriteBin(z, p);
                }

            }
        }
		public static void WriteZip(this PixelDouble p, string zipfilename, string statusname = "000", bool append = false, CompressionLevel level = CompressionLevel.Fastest)
        {
            XML.PixelModelDependent.ModelDataXML buf = new XML.PixelModelDependent.ModelDataXML(); 

            buf.Width = p.Width;
            buf.Height = p.Height;
            buf.FileName = new List<string>{ $"^{statusname}.bin$" };
            buf.FileSize = -1;
            buf.SetFileType(typeof(PixelDouble));
            //buf.SetFileType(p.GetType());

            ZipArchiveMode mode = append ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using (var archive = ZipFile.Open(zipfilename, mode))
            {
                using (var z = archive.CreateEntry($"PixelsModel\\{statusname}.model", level).Open())
                {
                    Pixels.IO.XMLSetter.Write<PixelModelDependent.ModelDataXML>(buf, z);
                }
                using (var z = archive.CreateEntry($"{statusname}.bin").Open())
                {
                    PixelStream.WriteBin(z, p);
                }

            }
        }
        #endregion
	}

    #endregion

}

#endif