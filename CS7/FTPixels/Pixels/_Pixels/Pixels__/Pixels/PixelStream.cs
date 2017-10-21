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

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

/*******************************************************************************/
/*   Pixels 2016/07/21                                                         */
/*******************************************************************************/
//
// 2016/07/21 PixelModelDependentからRead移管
//

namespace Pixels
{
    /*--------------------------------------*/
    //   読み書き
    /*--------------------------------------*/
    #region 読み

    public unsafe static partial class PixelStream
    {
        /* 読み ****************************** */
        #region 読み

        static byte[] Stream(Stream src, long size)
        {
            //long size2 = src.Length;
            byte[] buf = new byte[size];
            //int numBytes;

            int s = 4096;
            byte[] buffer = new byte[s];

            fixed (byte* dst = &buf[0])
            using (MemoryStream streamDst = new MemoryStream())
            {
                src.CopyTo(streamDst);
                //while ((numBytes = src.Read(buffer, 0, s)) > 0)
                //{
                //    streamDst.Write(buffer, 0, numBytes);
                //}
                buf = streamDst.ToArray();
            }

            /*
            fixed (byte* dst = &buf[0])
            using (UnmanagedMemoryStream streamDst = new UnmanagedMemoryStream((byte*)dst, size))
            {
            UnmanagedMemoryStreamつかえないでやんの
                src.CopyTo(streamDst);
                //CopyStream(src, streamDst);
            }
            */
            return buf;
        }
        /*** GDIBitmap bin ***/
        #region GDIBitmap bin

        public static PixelByte ReadGDIBitmap(string f, BMPColor color = BMPColor.G)
        {
            int w, h;
            byte[] buf;
            ReadGDIBitmap(f, out buf, out w, out h, color);
            return new PixelByte(buf, w, h, new CancellationTokenSource());
        }

        private unsafe static void ReadGDIBitmap(string f, out byte[] buf, out int w, out int h, BMPColor color)
        {
            Bitmap img = new Bitmap(f);
            ReadGDIBitmap(img,out buf,out w,out h, color);
        }

        public static PixelByte ToPixelByte(this Bitmap f, BMPColor color = BMPColor.G)
        {
            int w, h;
            byte[] buf;
            ReadGDIBitmap(f, out buf, out w, out h, color);
            return new PixelByte(buf, w, h, new CancellationTokenSource());
        }

        private unsafe static void ReadGDIBitmap(Bitmap img, out byte[] buf, out int w, out int h, BMPColor color)
        {
            BitmapData bmpData = img.LockBits(new Rectangle(System.Drawing.Point.Empty, img.Size), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            //int c = 0;
            //byte[] buf = new byte[img.Width * img.Height];

            buf = new byte[img.Width * img.Height];
            w = img.Width;
            h = img.Height;

            byte* p = (byte*)bmpData.Scan0;
            int dataLength = bmpData.Stride * bmpData.Height;

            switch (color)
            {
                case BMPColor.R: p += 2; break;
                case BMPColor.B: p += 0; break;
                case BMPColor.A: p += 3; break;
                case BMPColor.G: p += 1; break;
                default: p += 1; break;
            }

            fixed (byte* i = &buf[0])
            {
                byte* j = i;
                int s = img.Width * img.Height;//ダントツはやい、ループ内だと毎回評価してる？

                for (int y = 0; y < s; y++)
                {
                    *j++ = *p;
                    p += 4;
                }

            }

            img.UnlockBits(bmpData);
        }

        #endregion

        /*** bmp bin ***/
        #region BitmapSource bin

        public static PixelByte ReadBitmapSource(string f, BMPColor color = BMPColor.G)
        {
            //http://pieceofnostalgy.blogspot.jp/2012/02/wpf-bitmapsource.html

            using (MemoryStream data = new MemoryStream(File.ReadAllBytes(f)))
            {
                int w, h;
                byte[] buf;

                WriteableBitmap wbmp = new WriteableBitmap(BitmapFrame.Create(data));
                data.Close();

                ReadBitmapSource(wbmp, out buf, out w, out h, color);
                return new PixelByte(buf, w, h, new CancellationTokenSource());
            }

            //try
            //{
            //    using (Stream stream = new FileStream(
            //        f,
            //        FileMode.Open,
            //        FileAccess.Read,
            //        FileShare.ReadWrite | FileShare.Delete
            //    ))
            //    {
            //        // ロックしないように指定したstreamを使用する。
            //        BitmapDecoder decoder = BitmapDecoder.Create(
            //            stream,
            //            BitmapCreateOptions.None, // この辺のオプションは適宜
            //            BitmapCacheOption.Default // これも
            //        );
            //        BitmapSource bmp = new WriteableBitmap(decoder.Frames[0]);
            //        bmp.Freeze();

            //        // xamlでImageを記述 → imgSync
            //        //return bmp;
            //    }
            //}
            //catch (Exception exc)
            //{
            //    //MessageBox.Show(this, "[" + exc.Message + "]\n" + exc.StackTrace, this.Title);
            //    //return null;
            //}
        }

        public static PixelByte ToPixelByte(this BitmapSource f, BMPColor color = BMPColor.G)
        {
            int w, h;
            byte[] buf;
            ReadBitmapSource(f, out buf, out w, out h, color);
            return new PixelByte(buf, w, h, new CancellationTokenSource());
        }

        private unsafe static void ReadBitmapSource(BitmapSource img, out byte[] buf, out int w, out int h, BMPColor color)
        {
            //http://www.ruche-home.net/boyaki/2011-08-02/CopyPixe

            // フォーマットが異なるならば変換
            BitmapSource s = img;
            if (s.Format != PixelFormats.Bgra32)
            {
                s = new FormatConvertedBitmap(
                    s,
                    PixelFormats.Bgra32,
                    null,
                    0);
                s.Freeze();
            }

            w = (int)s.Width;
            h = (int)s.Height;

            buf = new byte[w * h];

            int stride = w * s.Format.BitsPerPixel / 8;
            byte[] data = new byte[stride * h];
            s.CopyPixels(data, stride, 0);

            fixed (byte* d = &data[0])
            fixed (byte* i = &buf[0])
            {
                byte* e = d;
                byte* j = i;

                switch (color)
                {
                    case BMPColor.R: e += 2; break;
                    case BMPColor.B: e += 0; break;
                    case BMPColor.A: e += 3; break;
                    case BMPColor.G: e += 1; break;
                    default: e += 1; break;
                }

                int size = w * h;//ダントツはやい、ループ内だと毎回評価してる？

                for (int y = 0; y < size; y++)
                {
                    *j++ = *e;
                    e += 4;
                }

            }


        }

        #endregion
        
        /*** Tester bin ***/
        #region Tester bin

        public static PixelDouble ReadTRWDouble(string f)
        {
            int w;
            int h;
            int filetype;
            #region ヘッダの読み込み
            using (FileStream r = new FileStream(f, FileMode.Open, FileAccess.Read))
            {
                byte[] raw = new byte[16];
                r.Read(raw, 0, 16);
                //r.Close();

                byte[] Endian = new byte[4];
                Array.Copy(raw, 4, Endian, 0, 4);
                filetype = BitConverter.ToInt32(Endian, 0);
                Array.Copy(raw, 8, Endian, 0, 4);
                w = BitConverter.ToInt32(Endian, 0);
                Array.Copy(raw, 12, Endian, 0, 4);
                h = BitConverter.ToInt32(Endian, 0);
            }
            #endregion

            switch (filetype)
            {
                case 0:
                    return (PixelDouble)ReadBinToPixelShort(f, w, h, 16);
                case 1:
                    return (PixelDouble)ReadBinToPixelInt(f, w, h, 16);
                default:
                    return (PixelDouble)ReadBinToPixelFloat(f, w, h, 16);

            }
        }

        public static PixelFloat ReadTRWFloat(string f)
        {
            int w;
            int h;
            int filetype;
            #region ヘッダの読み込み
            using (FileStream r = new FileStream(f, FileMode.Open, FileAccess.Read))
            {
                byte[] raw = new byte[16];
                r.Read(raw, 0, 16);
                //r.Close();

                byte[] Endian = new byte[4];
                Array.Copy(raw, 4, Endian, 0, 4);
                filetype = BitConverter.ToInt32(Endian, 0);
                Array.Copy(raw, 8, Endian, 0, 4);
                w = BitConverter.ToInt32(Endian, 0);
                Array.Copy(raw, 12, Endian, 0, 4);
                h = BitConverter.ToInt32(Endian, 0);
            }
            #endregion

            switch (filetype)
            {
                case 0:
                    return (PixelFloat)PixelStream.ReadBinToPixelShort(f, w, h, 16);
                case 1:
                    return (PixelFloat)ReadBinToPixelInt(f, w, h, 16);
                default:
                    return ReadBinToPixelFloat(f, w, h, 16);
            }
        }

        #endregion
        
        /*** raw bin ***/
        #region raw bin
        /*
        public static PixelByte ReadByte(string f, int w, int h, int offsetbyte)
        {
            byte[] raw = File.ReadAllBytes(f);
            return Bin8(ref raw, w * h, offsetbyte).Create(w, h);
        }
        public static PixelShort ReadShort(string f, int w, int h, int offsetbyte)
        {
            byte[] raw = File.ReadAllBytes(f);
            return Bin16(ref raw, w * h, offsetbyte).Create(w, h);
        }
        public static PixelInt Read24(string f, int w, int h, int offsetbyte)
        {
            byte[] raw = File.ReadAllBytes(f);
            return Bin24(ref raw, w * h, offsetbyte).Create(w, h);
        }
        public static PixelInt ReadInt(string f, int w, int h, int offsetbyte)
        {
            byte[] raw = File.ReadAllBytes(f);
            return Bin32(ref raw, w * h, offsetbyte).Create(w, h);
        }
        public static PixelLong ReadLong(string f, int w, int h, int offsetbyte)
        {
            byte[] raw = File.ReadAllBytes(f);
            return Bin64(ref raw, w * h, offsetbyte).Create(w, h);
        }
        public static PixelFloat ReadFloat(string f, int w, int h, int offsetbyte)
        {
            byte[] raw = File.ReadAllBytes(f);
            float[] buf;
            //return BinFloat(ref raw, w * h, offsetbyte).Create(w, h);
            Bin<float>(ref raw, out buf, w * h, offsetbyte, 4, i => BitConverter.ToSingle(i,0));
            return buf.Create(w, h);
        }
        public static PixelDouble ReadDouble(string f, int w, int h, int offsetbyte)
        {
            byte[] raw = File.ReadAllBytes(f);
            return BinDouble(ref raw, w * h, offsetbyte).Create(w, h);
        }

        private static byte[] Bin8(ref byte[] raw, int size, int offsetbyte)
        {
            byte[] buf = new byte[size];

            fixed (byte* r = &buf[0])
            fixed (byte* a = &raw[0])
            {
                byte* aa = a + offsetbyte;
                byte* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *rr++ = *aa++;
                }
            }
            return buf;
        }
        private static short[] Bin16(ref byte[] raw, int size, int offsetbyte)
        {
            short[] buf = new short[size];
            byte[] Endian = new byte[2];

            fixed (short* r = &buf[0])
            fixed (byte* a = &raw[0])
            fixed (byte* e = &Endian[0])
            {
                byte* aa = a + offsetbyte;
                byte* ee = e;
                short* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *aa++;
                    *(ee + 1) = *aa++;
                    *rr++ = BitConverter.ToInt16(Endian, 0);
                }
            }
            return buf;
        }
        private static int[] Bin24(ref byte[] raw, int size, int offsetbyte)
        {
            int[] buf = new int[size];
            byte[] Endian = new byte[4];

            fixed (int* r = &buf[0])
            fixed (byte* a = &raw[0])
            fixed (byte* e = &Endian[0])
            {
                byte* aa = a + offsetbyte;
                byte* ee = e;
                int* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *aa++;
                    *(ee + 1) = *aa++;
                    *(ee + 2) = *aa++;
                    *(ee + 3) = *aa++;
                    *rr++ = BitConverter.ToInt32(Endian, 0) >> 8;
                }
            }
            return buf;
        }
        private static int[] Bin32(ref byte[] raw, int size, int offsetbyte)
        {
            int[] buf = new int[size];
            byte[] Endian = new byte[4];

            fixed (int* r = &buf[0])
            fixed (byte* a = &raw[0])
            fixed (byte* e = &Endian[0])
            {
                byte* aa = a + offsetbyte;
                byte* ee = e;
                int* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *aa++;
                    *(ee + 1) = *aa++;
                    *(ee + 2) = *aa++;
                    *(ee + 3) = *aa++;
                    *rr++ = BitConverter.ToInt32(Endian, 0);
                }
            }
            return buf;
        }
        private static long[] Bin64(ref byte[] raw, int size, int offsetbyte)
        {
            long[] buf = new long[size];
            byte[] Endian = new byte[8];

            fixed (long* r = &buf[0])
            fixed (byte* a = &raw[0])
            fixed (byte* e = &Endian[0])
            {
                byte* aa = a + offsetbyte;
                byte* ee = e;
                long* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *aa++;
                    *(ee + 1) = *aa++;
                    *(ee + 2) = *aa++;
                    *(ee + 3) = *aa++;
                    *(ee + 4) = *aa++;
                    *(ee + 5) = *aa++;
                    *(ee + 6) = *aa++;
                    *(ee + 7) = *aa++;
                    *rr++ = BitConverter.ToInt64(Endian, 0);
                }
            }
            return buf;
        }
        private static float[] BinFloat(ref byte[] raw, int size, int offsetbyte)
        {
            float[] buf = new float[size];
            byte[] Endian = new byte[4];

            fixed (float* r = &buf[0])
            fixed (byte* a = &raw[0])
            fixed (byte* e = &Endian[0])
            {
                byte* aa = a + offsetbyte;
                byte* ee = e;
                float* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *aa++;
                    *(ee + 1) = *aa++;
                    *(ee + 2) = *aa++;
                    *(ee + 3) = *aa++;

                    *rr++ = BitConverter.ToSingle(Endian, 0);
                }
            }
            return buf;
        }
        private static double[] BinDouble(ref byte[] raw, int size, int offsetbyte)
        {
            double[] buf = new double[size];
            byte[] Endian = new byte[8];

            fixed (double* r = &buf[0])
            fixed (byte* a = &raw[0])
            fixed (byte* e = &Endian[0])
            {
                byte* aa = a + offsetbyte;
                byte* ee = e;
                double* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *aa++;
                    *(ee + 1) = *aa++;
                    *(ee + 2) = *aa++;
                    *(ee + 3) = *aa++;
                    *(ee + 4) = *aa++;
                    *(ee + 5) = *aa++;
                    *(ee + 6) = *aa++;
                    *(ee + 7) = *aa++;
                    *rr++ = BitConverter.ToDouble(Endian, 0);
                }
            }
            return buf;
        }
        */

        #endregion
        
        /*** raw txt dec ***/
        #region raw txt

        public static PixelDouble ReadTxtDouble(string f, int w, int h, int offset = 0, string delimiterChars = "\n")
        {
            using (StreamReader r = new StreamReader(f))
                return ReadTxtDec(r, w * h, offset, delimiterChars).Create(w, h, new CancellationTokenSource());
        }

        public static double[] ReadTxtDec(StreamReader r, int size, int offset, string delimiterChars)
        {
            char[] del = delimiterChars.ToCharArray();
            string raw = r.ReadToEnd();
            r.Close();
            string[] data = raw.Split(del);// 改行区切りで分割して配列に格納する

            double[] buf = new double[size];

            //#if MultipleCore
            //            //そんなに効かない
            //            ParallelOptions options = new ParallelOptions();
            //            options.MaxDegreeOfParallelism = 4;
            //            Parallel.For(0, size, options,
            //                (i) => buf[i] = double.Parse(data[i + offset], System.Globalization.NumberStyles.Number)
            //                );
            //#else
            for (int i = 0; i < size; i++)
                buf[i] = double.Parse(data[i + offset], System.Globalization.NumberStyles.Number);
            //#endif

            return buf;
        }


        public static PixelDouble ReadCSVDouble(string f, int w, int h, int offsetLine, string delimiterChars)
        {
            char[] del = delimiterChars.ToCharArray();
            double[] buf = new double[w * h];
            string raw;
            string[] data;

            using (StreamReader r = new StreamReader(f))
            {
                string[] hogehoge = r.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                for (int y = offsetLine; y < h + offsetLine; y++)
                {
                    raw = hogehoge[y];
                    data = raw.Split(del);// delimiter区切りで分割して配列に格納する

                    for (int x = 0; x < w; x++)
                        buf[x + y * w] = double.Parse(data[x], System.Globalization.NumberStyles.Number);
                }
                return buf.Create(w, h, new CancellationTokenSource());
            }
        }
        public static PixelInt ReadCSVInt(string f, int w, int h, int offsetLine, string delimiterChars)
        {
            char[] del = delimiterChars.ToCharArray();
            int[] buf = new int[w * h];
            string raw;
            string[] data;

            using (StreamReader r = new StreamReader(f))
            {
                string[] hogehoge = r.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                for (int y = offsetLine; y < h + offsetLine; y++)
                {
                    raw = hogehoge[y];
                    data = raw.Split(del);// delimiter区切りで分割して配列に格納する

                    for (int x = 0; x < w; x++)
                        buf[x + y * w] = int.Parse(data[x], System.Globalization.NumberStyles.Number);
                }
                return buf.Create(w, h, new CancellationTokenSource());
            }
        }

        #endregion
        
        /*** raw txt hex ***/
        
        #region raw txt

        public static PixelDouble ReadTxtHex(string f, int w, int h, int offset = 0, string delimiterChars = "\n")
        {
            using (StreamReader r = new StreamReader(f))
                return ReadTxtHex(r, w * h, offset, delimiterChars, true).Create(w, h, new CancellationTokenSource());
        }
        private static double[] ReadTxtHex(StreamReader r, int size, int offset, string delimiterChars, bool sign)
        {
            char[] del = delimiterChars.ToCharArray();
            double[] buf = new double[size];
            string raw = r.ReadToEnd();
            r.Close();
            string[] data = raw.Split(del);// 改行区切りで分割して配列に格納する

            Func<string, double> convert = (ed) => 0;
            #region convert
            switch (sign ? data[offset].Trim().Length : data[offset].Trim().Length * -1)
            {
                case 4: convert = (ed) => Int16.Parse(ed, System.Globalization.NumberStyles.HexNumber); break;
                case 6: convert = (ed) => Int32.Parse(ed.Trim() + "00", System.Globalization.NumberStyles.HexNumber) >> 8; break;
                //this[i] = (j < 8388608 ? j : (j - 16777216)); //24bit1フラグ（符号ビット判定

                case 8: convert = (ed) => Int32.Parse(ed, System.Globalization.NumberStyles.HexNumber); break;
                case 16: convert = (ed) => Int64.Parse(ed, System.Globalization.NumberStyles.HexNumber); break;

                case -4: convert = (ed) => UInt16.Parse(ed, System.Globalization.NumberStyles.HexNumber); break;
                case -6: convert = (ed) => UInt32.Parse(ed.Trim() + "00", System.Globalization.NumberStyles.HexNumber) >> 8; break;
                case -8: convert = (ed) => UInt32.Parse(ed, System.Globalization.NumberStyles.HexNumber); break;
                case -16: convert = (ed) => UInt64.Parse(ed, System.Globalization.NumberStyles.HexNumber); break;
                default: throw new ArgumentNullException();
            }
            /*
            case FileType.TxtHex16LittleEndian:
                convert = (ed) =>
                {
                    byte[] byteArray = BitConverter.GetBytes(Int16.Parse(ed, System.Globalization.NumberStyles.HexNumber));
                    Array.Reverse(byteArray);
                    return BitConverter.ToInt16(byteArray, 0);
                };
                break;
            case FileType.TxtHex24LittleEndian:
                convert = (ed) =>
                {
                    byte[] byteArray = BitConverter.GetBytes(Int32.Parse(ed, System.Globalization.NumberStyles.HexNumber));
                    Array.Reverse(byteArray);
                    return BitConverter.ToInt32(byteArray, 0) >> 8;
                };
                break;
            case FileType.TxtHex32LittleEndian:
                convert = (ed) =>
                {
                    byte[] byteArray = BitConverter.GetBytes(Int32.Parse(ed, System.Globalization.NumberStyles.HexNumber));
                    Array.Reverse(byteArray);
                    return BitConverter.ToInt32(byteArray, 0);
                };
                break;
            case FileType.TxtHex64LittleEndian:
                convert = (ed) =>
                {
                    byte[] byteArray = BitConverter.GetBytes(Int64.Parse(ed, System.Globalization.NumberStyles.HexNumber));
                    Array.Reverse(byteArray);
                    return BitConverter.ToInt64(byteArray, 0);
                };
                break;
                */
            #endregion

#if MultipleCore
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 4;
            Parallel.For(0, size, options,
                (i) => buf[i] = convert(data[i + offset])
                );

            /**/
            //Parallel.Invoke(
            //() =>
            //{
            //    for (int i = 0; i < size / 2; i++)
            //        buf[i] = convert(data[i + offset]);
            //},
            //() =>
            //{
            //    for (int i = size / 2; i < size; i++)
            //        buf[i] = convert(data[i + offset]);
            //}
            //);
#else
            for (int i = 0; i < size; i++)
                buf[i] = convert(data[i + offset]);
#endif
            return buf;

        }
        ////protected void ReadFileHexText(Stream f, FileType bytelength, int offset = 0, string delimiterChars = "\n")
        ////{
        ////    using (StreamReader r = new StreamReader(f))
        ////        ReadFileHexText(r, bytelength, offset, delimiterChars);
        ////}

        #endregion


        #endregion

        #region Xml

        public enum XmlFolder
        {
            ExecutingAssemblyLocation,
            ExecutingAssemblyLocationPixelModels,
            ParentOfExecutingAssembly,
            ParentOfExecutingAssemblyPixelModels,
            CurrentDirectory
        }

        public static dynamic ReadUsingXml(string FileFullPath, string XmlDir)
        {
            foreach (var i in Directory.EnumerateFiles(XmlDir, "*.model", SearchOption.TopDirectoryOnly))
            {
                XML.PixelModelDependent.ModelDataXML buf = XML.PixelModelDependent.ReadXML(FileFullPath, XmlDir);
                if (buf != null)
                {
                    return buf.ReadFile(FileFullPath);
                }

                //Tuple<string, Func<string, dynamic>> buf_dic = CheckFileXML(FileFullPath, XmlDir);
                //if (buf_dic != null)
                //{
                //    return buf_dic.Item2(FileFullPath);
                //}
            }
            throw new KeyNotFoundException("そんなファイルしらん");
        }

        public static dynamic ReadUsingXml(string FileFullPath,XmlFolder xmlfolder)
        {
            string buf;
            switch(xmlfolder)
            {
                case XmlFolder.ExecutingAssemblyLocation:
                    buf = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    break;
                case XmlFolder.ExecutingAssemblyLocationPixelModels:
                    buf = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "PixelModels");
                    break;
                case XmlFolder.ParentOfExecutingAssembly:
                    buf = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "..\\");
                    break;
                case XmlFolder.ParentOfExecutingAssemblyPixelModels:
                    buf = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "..\\PixelModels");
                    break;
                case XmlFolder.CurrentDirectory:
                default:
                    buf = System.IO.Directory.GetCurrentDirectory();
                    break;
            }
            return ReadUsingXml(FileFullPath, buf);
        }

        public static XML.PixelModelDependent.ModelDataXML SearchXml(string FileFullPath, XmlFolder xmlfolder)
        {
            string buf;
            switch (xmlfolder)
            {
                case XmlFolder.ExecutingAssemblyLocation:
                    buf = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    break;
                case XmlFolder.ExecutingAssemblyLocationPixelModels:
                    buf = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "PixelModels");
                    break;
                case XmlFolder.ParentOfExecutingAssembly:
                    buf = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "..\\");
                    break;
                case XmlFolder.ParentOfExecutingAssemblyPixelModels:
                    buf = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "..\\PixelModels");
                    break;
                case XmlFolder.CurrentDirectory:
                default:
                    buf = System.IO.Directory.GetCurrentDirectory();
                    break;
            }
            return SearchXml(FileFullPath, buf);
        }

        public static XML.PixelModelDependent.ModelDataXML SearchXml(string FileFullPath, string XmlDir)
        {
            foreach (var i in Directory.EnumerateFiles(XmlDir, "*.model", SearchOption.TopDirectoryOnly))
            {
                XML.PixelModelDependent.ModelDataXML buf = XML.PixelModelDependent.ReadXML(FileFullPath, XmlDir);
                if (buf != null)
                {
                    return buf;
                }
            }
            throw new KeyNotFoundException("そんなファイルしらん");
        }

        #endregion

        /* 書き ****************************** */
        #region 書き

        public static Bitmap ToBitmap(this PixelByte p)
        {
            Bitmap b = new Bitmap(p.Width, p.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            BitmapData bmpdat = b.LockBits(new Rectangle(0, 0, p.Width, p.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            ColorPalette grayPal = b.Palette;
            for (int i = 0; i < 256; i++)
                grayPal.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
            b.Palette = grayPal;

            int h = p.Height;
            int w = p.Width;
            Byte[] a = p.ToArray();
            for (int j = 0; j < h; ++j)
            {
                IntPtr dst_line = (IntPtr)((Int64)bmpdat.Scan0 + j * bmpdat.Stride);
                Marshal.Copy(a, j * w, dst_line, w);
            }
            b.UnlockBits(bmpdat);

            //    Byte[] a = p.GetArray();
            //    Marshal.Copy(a, 0, bmpdat.Scan0, p.Size);
            //    b.UnlockBits(bmpdat);

            return b;
        }

        /// <summary>
        ///  dpiは標準96 Freezeかけてる
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static BitmapSource ToBitmapSource(this PixelByte p)
        {
            var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                p.ToArray(),
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
        }

        /**/

        /*bmpは列毎にオフセットビットが仕込まれる*/
        public static void WriteBMP24(this PixelByte p, string filename)
        {
            using (Bitmap b = new Bitmap(p.Width, p.Height))
            {
                BitmapData bmpdat = b.LockBits(new Rectangle(0, 0, p.Width, p.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                byte[] buf = new byte[p.Size * 3];
                int s = p.Size;
                int c = 0;
                int h = p.Height;
                int w = p.Width;
                for (int i = 0; i < s; i++)
                {
                    byte j = p[i];
                    buf[c + 2] = j;
                    buf[c + 1] = j;
                    buf[c + 0] = j;
                    //buf[c + 3] = 0;
                    c += 3;
                }
                for (int j = 0; j < h; ++j)
                {
                    IntPtr dst_line = (IntPtr)((Int64)bmpdat.Scan0 + j * bmpdat.Stride);
                    Marshal.Copy(buf, j * w * 3, dst_line, w * 3);
                }
                //Marshal.Copy(buf, 0, bmpdat.Scan0, buf.Length);
                b.UnlockBits(bmpdat);
                b.Save(filename, ImageFormat.Bmp);
            }
        }

        public static void WriteBMP8(this PixelByte p, string filename)
        {
            p.ToBitmap().Save(filename);
        }

        public static void WriteBitmapSource(this BitmapSource src, string f)
        {
            // BitmapSourceを保存する
            using (Stream stream = new FileStream(f, FileMode.Create))
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(src));
                encoder.Save(stream);
            }
        }

        /*WriteCSV*/
        /*
        private static void WriteCSV<T>(string filename,int width,int height,T[] p)
        {
            Func<T, string> cout = (i) => String.Format("{0}", i);

            using (StreamWriter w = new StreamWriter(filename))
            {
                w.Write("P2\r\n", width, height);
                w.Write("{0} {1}\r\n", width, height);
                w.Write("{0}\r\n", 0);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width - 1; x++)
                    {
                        w.Write(cout(p[x + y * width]));
                        w.Write(", ");
                    }
                    w.Write(cout(p[width - 1 + y*width]));
                    w.Write("\r\n");
                }
            }
            */
        ////フラグ管理
        //int flag_Round = 0;
        //int flag_Sig = 0;
        //foreach(SaveCSVParams i in par)
        //{
        //    if(i == SaveCSVParams.Round) flag_Round = 1;
        //    if(i == SaveCSVParams.Unsigned) flag_Sig = 1;
        //}

        //if( (flag_Round == 1) && (flag_Sig == 0) )
        //cout = (double i) =>
        //{
        //w.Write("{0, 8},", Math.Round(i));
        //};
        //else if ((flag_Round == 1) && (flag_Sig == 1))
        //cout = (double i) =>
        //{
        //w.Write("{0, 8},", (i < 0) ? 0 : Math.Round(i));
        //};
        //else if( (flag_Round == 0) && (flag_Sig == 1) )
        //cout = (double i) =>
        //{
        //w.Write("{0, 8},", (i < 0) ? 0 : i);
        //};
        //else
        //cout = (double i) =>
        //{
        //w.Write("{0, 8},", i);
        //};
        #endregion

    }

    #endregion
}

/*******************************************************************************/
/*   Pixels.XML 2016/07/21                                                     */
/*******************************************************************************/
//
// Read追い出し
//

namespace Pixels.XML
{
    public class PixelModelDependent
    {
        #region XML本体

        public class ModelDataXML
        {
            public enum FileType
            {
                Byte,
                UShort,
                UInt,
                ULong,
                Short,
                Int,
                Long,
                Float,
                Double,

                Int_24bit,

                TxtDec,
                TxtHex,

                BMP
            }
            private Dictionary<Type, FileType> FileTypeConvert = new Dictionary<Type, FileType>()
            {
                [typeof(PixelByte)] = FileType.Byte,
                [typeof(PixelUShort)] = FileType.UShort,
                [typeof(PixelUInt)] = FileType.UInt,
                [typeof(PixelULong)] = FileType.ULong,
                [typeof(PixelShort)] = FileType.Short,
                [typeof(PixelInt)] = FileType.Int,
                [typeof(PixelLong)] = FileType.Long,
                [typeof(PixelFloat)] = FileType.Float,
                [typeof(PixelDouble)] = FileType.Double,
            };


            public string ModelName { get; set; } = "";
            public List<string> FileName { get; set; } = new List<string>();
            public FileType Type { get; set; } = FileType.Double;
            public int FileSize { get; set; } = -1;

            public int Width { get; set; }
            public int Height { get; set; }
            public int Offset { get; set; } = 0;
            public string Delimiter { get; set; } = @"\n";

            public string Note { get; set; } = "";

            public List<string> Area { get; set; } = new List<string>();

            public dynamic ReadFile(string filename)
            {
                switch (Type)
                {
                    case FileType.Byte:
                        return (PixelStream.ReadBinToPixelByte(filename, Width, Height, Offset));
                    case FileType.UShort:
                        return (PixelStream.ReadBinToPixelUShort(filename, Width, Height, Offset));
                    case FileType.UInt:
                        return (PixelStream.ReadBinToPixelUInt(filename, Width, Height, Offset));
                    case FileType.ULong:
                        return (PixelStream.ReadBinToPixelULong(filename, Width, Height, Offset));
                    case FileType.Short:
                        return (PixelStream.ReadBinToPixelShort(filename, Width, Height, Offset));
                    case FileType.Int:
                        return (PixelStream.ReadBinToPixelInt(filename, Width, Height, Offset));
                    case FileType.Long:
                        return (PixelStream.ReadBinToPixelLong(filename, Width, Height, Offset));
                    case FileType.Float:
                        return (PixelStream.ReadBinToPixelFloat(filename, Width, Height, Offset));
                    case FileType.Double:
                        return (PixelStream.ReadBinToPixelDouble(filename, Width, Height, Offset));

                    case FileType.Int_24bit:
                        return (PixelStream.ReadBin24ToPixelInt(filename, Width, Height, Offset));

                    case FileType.TxtDec:
                        return (PixelStream.ReadTxtDouble(filename, Width, Height, Offset, Delimiter.Replace(@"\n", "\n").Replace(@"\r", "\r").Replace(@"\t", "\t").Replace(@"\s", " ")));
                    case FileType.TxtHex:
                        return (PixelStream.ReadTxtHex(filename, Width, Height, Offset, Delimiter.Replace(@"\n", "\n").Replace(@"\r", "\r").Replace(@"\t", "\t").Replace(@"\s", " ")));

                    case FileType.BMP:
                        return PixelStream.ReadGDIBitmap(filename);

                    default:
                        return null;
                }
            }

            //public FileType GetFileType(Type t)
            //{
            //    return FileTypeConvert[t];
            //}
            public void SetFileType(Type t)
            {
                Type = FileTypeConvert[t];
            }
        }

        #endregion


        #region XML読み込み

        public static ModelDataXML ReadXML(string FileFullPath, string XmlDir)
        {
            foreach (var i in Directory.EnumerateFiles(XmlDir, "*.model", SearchOption.TopDirectoryOnly))
            {
                //設定の読み込み
                ModelDataXML mm = Pixels.IO.XMLSetter.Read<ModelDataXML>(i);

                //ファイルの種類のチェック
                if (IsTextFile(FileFullPath) ^ (mm.Type == ModelDataXML.FileType.TxtDec || mm.Type == ModelDataXML.FileType.TxtHex)) continue;
                //ファイルサイズチェック
                if (mm.FileSize > 0)
                {
                    if ((new System.IO.FileInfo(FileFullPath)).Length != mm.FileSize) continue;
                }
                //ファイル名前/拡張子チェック
                if (mm.FileName.Count > 0)
                {
                    //Regex regex = new System.Text.RegularExpressions.Regex("ここに正規表現");
                    var c = from str in mm.FileName
                            where (new System.Text.RegularExpressions.Regex(str)).IsMatch(Path.GetFileName(FileFullPath))
                            select str;
                    if (c.Count() <= 0) continue;
                }
                return mm;
            }
            return null;
        }

        public static ModelDataXML ReadXMLZipper(string FileFullPath, string XmlDir)
        {
            foreach (var i in Directory.EnumerateFiles(XmlDir, "*.model", SearchOption.TopDirectoryOnly))
            {
                //設定の読み込み
                ModelDataXML mm = Pixels.IO.XMLSetter.Read<ModelDataXML>(i);

                //ファイルの種類のチェック
                if (IsTextFile(FileFullPath) ^ (mm.Type == ModelDataXML.FileType.TxtDec || mm.Type == ModelDataXML.FileType.TxtHex)) continue;
                //ファイルサイズチェック
                if (mm.FileSize > 0)
                {
                    if ((new System.IO.FileInfo(FileFullPath)).Length != mm.FileSize) continue;
                }
                //ファイル名前/拡張子チェック
                if (mm.FileName.Count > 0)
                {
                    //Regex regex = new System.Text.RegularExpressions.Regex("ここに正規表現");
                    var c = from str in mm.FileName
                            where (new System.Text.RegularExpressions.Regex(str)).IsMatch(Path.GetFileName(FileFullPath))
                            select str;
                    if (c.Count() <= 0) continue;
                }
                return mm;
            }
            return null;
        }

        public static List<ModelDataXML> ReadModelFiles(string XmlDir)
        {
            List<ModelDataXML> mm = new List<ModelDataXML>();

            foreach (var i in Directory.EnumerateFiles(XmlDir, "*.model", SearchOption.TopDirectoryOnly))
            {
                mm.Add(Pixels.IO.XMLSetter.Read<ModelDataXML>(i));
            }
            return mm;
        }

        #endregion

        #region めそっど

        //バイナリチェック（画像ファイルは00が入るので精度なし                                         
        static public bool IsBinaryFile(string filePath)
        {
            FileStream fs = File.OpenRead(filePath);
            int len = (int)fs.Length;
            int count = 0;
            byte[] content = new byte[len];
            int size = fs.Read(content, 0, len);

            for (int i = 0; i < size; i++)
            {
                if (content[i] == 0)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return false;
        }
        static public bool IsTextFile_(string filePath)
        {
            FileStream file = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] byteData = new byte[1];
            int c = 0;
            while (file.Read(byteData, 0, byteData.Length) > 0)
            {
                if (byteData[0] == 0)
                    return false;
                if (c++ > 10000) return true;
            }
            return true;
        }

        static public bool IsTextFile(string filePath)
        {
            Regex regex = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9!-/:-@¥[-`{-~\r\n\t ]+$");

            FileStream file = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] byteData = new byte[1];
            int c = 0;
            while (file.Read(byteData, 0, byteData.Length) > 0)
            {
                char i = Convert.ToChar(byteData[0]);
                if (!regex.IsMatch(Convert.ToChar(byteData[0]).ToString()))
                    return false;
                if (c++ > 10000) return true;
            }
            return true;
        }

        //アスキーコードチェック
        static public bool IsTextFileCode(string filePath)
        {
            ////ファイルがないときはfalse
            //if(!System.IO.File.Exists(filePath)) return false;
            //テキストファイルを開く
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] bs = new byte[fs.Length];
            //byte配列に読み込む
            fs.Read(bs, 0, bs.Length);
            fs.Close();

            //文字コードを取得する
            System.Text.Encoding a = GetCode(bs);
            if (a == System.Text.Encoding.Unicode)
                return false;
            else
                return true;
        }
        public static System.Text.Encoding GetCode(byte[] bytes)
        {
            const byte bEscape = 0x1B;
            const byte bAt = 0x40;
            const byte bDollar = 0x24;
            const byte bAnd = 0x26;
            const byte bOpen = 0x28;    //'('
            const byte bB = 0x42;
            const byte bD = 0x44;
            const byte bJ = 0x4A;
            const byte bI = 0x49;

            int len = bytes.Length;
            byte b1, b2, b3, b4;

            //Encode::is_utf8 は無視

            bool isBinary = false;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 <= 0x06 || b1 == 0x7F || b1 == 0xFF)
                {
                    //'binary'
                    isBinary = true;
                    if (b1 == 0x00 && i < len - 1 && bytes[i + 1] <= 0x7F)
                    {
                        //smells like raw unicode
                        return System.Text.Encoding.Unicode;
                    }
                }
            }
            if (isBinary)
            {
                return null;
            }

            //not Japanese
            bool notJapanese = true;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 == bEscape || 0x80 <= b1)
                {
                    notJapanese = false;
                    break;
                }
            }
            if (notJapanese)
            {
                return System.Text.Encoding.ASCII;
            }

            for (int i = 0; i < len - 2; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                b3 = bytes[i + 2];

                if (b1 == bEscape)
                {
                    if (b2 == bDollar && b3 == bAt)
                    {
                        //JIS_0208 1978
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bDollar && b3 == bB)
                    {
                        //JIS_0208 1983
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && (b3 == bB || b3 == bJ))
                    {
                        //JIS_ASC
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && b3 == bI)
                    {
                        //JIS_KANA
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    if (i < len - 3)
                    {
                        b4 = bytes[i + 3];
                        if (b2 == bDollar && b3 == bOpen && b4 == bD)
                        {
                            //JIS_0212
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                        if (i < len - 5 &&
                            b2 == bAnd && b3 == bAt && b4 == bEscape &&
                            bytes[i + 4] == bDollar && bytes[i + 5] == bB)
                        {
                            //JIS_0208 1990
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                    }
                }
            }

            //should be euc|sjis|utf8
            //use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
            int sjis = 0;
            int euc = 0;
            int utf8 = 0;
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0x81 <= b1 && b1 <= 0x9F) || (0xE0 <= b1 && b1 <= 0xFC)) &&
                    ((0x40 <= b2 && b2 <= 0x7E) || (0x80 <= b2 && b2 <= 0xFC)))
                {
                    //SJIS_C
                    sjis += 2;
                    i++;
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0xA1 <= b1 && b1 <= 0xFE) && (0xA1 <= b2 && b2 <= 0xFE)) ||
                    (b1 == 0x8E && (0xA1 <= b2 && b2 <= 0xDF)))
                {
                    //EUC_C
                    //EUC_KANA
                    euc += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if (b1 == 0x8F && (0xA1 <= b2 && b2 <= 0xFE) &&
                        (0xA1 <= b3 && b3 <= 0xFE))
                    {
                        //EUC_0212
                        euc += 3;
                        i += 2;
                    }
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if ((0xC0 <= b1 && b1 <= 0xDF) && (0x80 <= b2 && b2 <= 0xBF))
                {
                    //UTF8
                    utf8 += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if ((0xE0 <= b1 && b1 <= 0xEF) && (0x80 <= b2 && b2 <= 0xBF) &&
                        (0x80 <= b3 && b3 <= 0xBF))
                    {
                        //UTF8
                        utf8 += 3;
                        i += 2;
                    }
                }
            }
            //M. Takahashi's suggestion
            //utf8 += utf8 / 2;

            System.Diagnostics.Debug.WriteLine(
                string.Format("sjis = {0}, euc = {1}, utf8 = {2}", sjis, euc, utf8));
            if (euc > sjis && euc > utf8)
            {
                //EUC
                return System.Text.Encoding.GetEncoding(51932);
            }
            else if (sjis > euc && sjis > utf8)
            {
                //SJIS
                return System.Text.Encoding.GetEncoding(932);
            }
            else if (utf8 > euc && utf8 > sjis)
            {
                //UTF8
                return System.Text.Encoding.UTF8;
            }

            return null;
        }

        #endregion

        #region テストめそど

        public static void Test_WriteXML()
        {
            ModelDataXML test = new ModelDataXML();

            test.ModelName = "BT130C_000";
            test.FileName = new List<string>() { @"^000\.txt$" };
            test.Type = ModelDataXML.FileType.TxtHex;

            test.Width = 1408;
            test.Height = 1032;
            test.Area = new List<string>()
            {
                "valid, 72, 0, 1288, 1032",
                "ob_l, 2,  0,  28,  1032" ,
                "ob_r, 38,  0,  27,  1032" ,
                "ob_pd, 31,  0,  6,  1032 "
            };

            Pixels.IO.XMLSetter.Write<ModelDataXML>(test, "BT130C000.model");

            test = new ModelDataXML();
            test.ModelName = "BT130C_Ave";
            test.FileName = new List<string>() { @"^ave\.txt$" };
            test.Type = ModelDataXML.FileType.TxtDec;

            test.Width = 1408;
            test.Height = 1032;
            test.Area = new List<string>()
            {
                "valid, 72, 0, 1288, 1032",
                "ob_l, 2,  0,  28,  1032" ,
                "ob_r, 38,  0,  27,  1032" ,
                "ob_pd, 31,  0,  6,  1032 "
            };

            Pixels.IO.XMLSetter.Write<ModelDataXML>(test, "BT130CAve.model");

            test = new ModelDataXML();
            test.ModelName = "BMP";
            test.FileName = new List<string>() { @"^*\.bmp$", @"^*\.jpg$", @"^*\.png$" };
            test.Type = ModelDataXML.FileType.BMP;

            Pixels.IO.XMLSetter.Write<ModelDataXML>(test, "BMP.model");
        }

        #endregion

    }
}





        /*** raw pgm ***/

        //protected void ReadFileBin(string f, FileType bytelength, int offsetbyte = 0)
        //{
        //    byte[] raw = File.ReadAllBytes(f);
        //    ReadBin(ref raw, bytelength, offsetbyte);
        //}

        /*
        public void ReadFlameSkewering(string[] filename,out PixelFloat ave, PixelFloat dev)
        {
            float[] ave = new float[];
            float[] dev = new float[];

            byte[] scr = Stream(z, w * h * 8);
            double[] buf = new double[w * h];
            ReadBinFlameSkewering(ref scr, ref buf, w * h, offsetbyte);
            return new PixelDouble(buf, w, h);

        }
        public static void ReadBinFlameSkewering(ref byte[] scr, ref float[] ave, ref float[] dev, int size, int offsetbyte)
        {
            int sizebyte = 2;
            byte[] Endian = new byte[sizebyte];
            float buf;

            fixed (float* pave = &ave[0])
            fixed (float* pdev = &dev[0])
            fixed (byte* s = &scr[0])
            fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                float* ppave = pave;
                float* ppdev = pdev;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    buf = BitConverter.ToInt16(Endian, 0);
                    pave 
                }
            }
        }
        */

//public static class TestMeathed
//{
//    public static double Ave(Dictionary<string, string> dic, string s)
//    {
//        //int w = 88 + 7696 + 88;
//        //int h = 8 + 4332 + 8;
//        PixelFloat buf = PixelStreamZipper.ReadZipPixelFloat(dic[s], "ave").Trim(88, 8, 7696, 4332).TrimBayer(0, 0);
//        return buf.Ave();
//    }
//    //public static double FrameAveragingBTN(string dir, string filename, int w,int h,int start_i,int n)
//    //{
//    //    string zipname = filename;

//    //    PixelShort ooo = PixelStream.ReadBinToPixelShort($"{dir}\\{start_i:000}.btn", w, h, 0);
//    //    PixelFloat ad1 = ooo;
//    //    PixelFloat ad2 = ooo * ooo;
//    //    ooo.WriteZipBin(zipname);
//    //    for (int i = 1; i < n; i++)
//    //    {
//    //        ooo = PixelStream.ReadBinToPixelShort($"{dir}\\{start_i + i:000}.btn", w, h, 0);
//    //        ad1 += ooo;
//    //        ad2 += (ooo * ooo);
//    //    }
//    //    ad1 /= n;
//    //    ad1.WriteZipBin(zipname, "ave", true);
//    //    PixelMath.Sqrt((ad2 / n) - (ad1 * ad1)).WriteZipBin(zipname, "dev", true);

//    //    return 1;
//    //}
//    //public static double FrameAveragingBTN_OnlyOB(string dir, string filename, int w, int h, int start_i, int n)
//    //{
//    //    string zipname = filename;

//    //    PixelShort ooo = PixelStream.ReadBinToPixelShort($"{dir}\\{start_i:000}.btn", w, h, 0);
//    //    PixelFloat ad1 = ooo;
//    //    ooo.WriteZipBin(zipname);
//    //    for (int i = 1; i < n; i++)
//    //    {
//    //        ooo = PixelStream.ReadBinToPixelShort($"{dir}\\{start_i + i:000}.btn", w, h, 0);
//    //        ad1 += ooo;
//    //    }
//    //    ad1 /= n;
//    //    ad1 = PixelFloat.JoinV(ad1.Trim(0, 0, ad1.Width, 8), ad1.Trim(0, ad1.Height - 8, ad1.Width, 8));
//    //    ad1.WriteZipBin(zipname, "vob", true);

//    //    return 1;
//    //}

//    public static string SignalBTN(PixelFloat in_p)
//    {
//        double ave = in_p.FilMed().Trim(2,2,in_p.Width -2, in_p.Height -2).Ave();
//        double med = in_p.Med();
//        double dev = in_p.Dev();

//        return $"Ave(MedFil), Med, Dev = {ave}, {med}, {dev}";
//    }

//    public static int WD(PixelFloat in_p, float thr) => (in_p - in_p.FilMed().Complement(2,2,2,2)).Count(INEQUALITY.GreaterThan, thr);
//    public static int BD(PixelFloat in_p, float thr) => (in_p.FilMed().Complement(2, 2, 2, 2) - in_p).Count(INEQUALITY.GreaterThan, thr);

//}