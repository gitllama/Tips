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

using System.Diagnostics;       //for Conditional
using System.IO.Compression;    //参照追加System.IO.Compression.dll, System.IO.Compression.FileSystem.dll
using System.Threading.Tasks;

/*******************************************************************************/
/*   Pixels Zipper                                                             */
/*   2016/03/23                                                                */
/*******************************************************************************/
namespace Pixels.Zipper
{
    #region 読み書きZip
    public unsafe static partial class PixelStreamZipper
    {
        const string ext_status = ".model";
        const string path_status = "Model";
 
        //ステータスの読み込み
        #region 読み込み

        /// <summary>
        /// Zipファイルのmodel一覧を取得する
        /// </summary>
        /// <param name="zipfilename"></param>
        /// <returns></returns>
        public static List<XML.PixelModelDependent.ModelDataXML> ReadZipModelFiles(string zipfilename)
        {
            List<XML.PixelModelDependent.ModelDataXML> retmodel = new List<XML.PixelModelDependent.ModelDataXML>();
            var retfile = ReadZipModelFileNames(zipfilename);

            foreach(var i in retfile)
            using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
            using (var z = archive.GetEntry(i).Open())
            {
                retmodel.Add(Pixels.IO.XMLSetter.Read<XML.PixelModelDependent.ModelDataXML>(i, z));
            }

            return retmodel;
        }
        public static List<string> ReadZipModelFileNames(string zipfilename)
        {
            List<string> retfile;
            using (var archive = ZipFile.Open(zipfilename, ZipArchiveMode.Read))
            {
                //ZipArchiveEntry entry = archive.Entries[0];	// 最初のエントリ
                //ZipArchiveEntry entry = archive.Entries.Where(e => e.Name.EndsWith(ext_status)).FirstOrDefault();
                retfile = archive.Entries.Where(e => e.Name.EndsWith(ext_status)).Select(e => e.FullName).ToList<string>();
            }
            return retfile;
        }

        public static bool MatchModelFiles(string zipfilename, string filename , XML.PixelModelDependent.ModelDataXML xml)
        {

            return false;
        }


        #endregion

        //ファイルの読み込み
        /*展開済み*/
        #region そのた
        //http://sourcechord.hatenablog.com/entry/2014/07/31/220310

        public static string[] StreamReader(string zipfilename, string statusname)
        {
            //List<string> buf = new List<string>();
            string[] buf;
            using (var entry = ZipFile.Open(zipfilename, ZipArchiveMode.Read, Encoding.GetEncoding("sjis")))
            using (var reader = new StreamReader(entry.CreateEntry(statusname).Open(), Encoding.GetEncoding("sjis")))
            {
                //for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                //    buf.Add(line);

                buf = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            }
            //return buf.ToArray();
            return buf;
        }

        static byte[] Stream(Stream src)
        {
            long size = src.Length;
            byte[] buf = new byte[size];
            fixed (byte* dst = &buf[0])
            using (UnmanagedMemoryStream streamDst = new UnmanagedMemoryStream((byte*)dst, size))
            {
                src.CopyTo(streamDst);
                //CopyStream(streamSrc, streamDst);
            }
            return buf;
        }

        /*** Stream ***/
        //http://schima.hatenablog.com/entry/20100529/1275118995
        static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            input.Position = 0;
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    break;
                output.Write(buffer, 0, read);
            }
            input.Position = 0;
        }
        static unsafe void CopyMemory(IntPtr dst, IntPtr src, int size)
        {
            using (UnmanagedMemoryStream streamSrc = new UnmanagedMemoryStream((byte*)src, size))
            using (UnmanagedMemoryStream streamDst = new UnmanagedMemoryStream((byte*)dst, size))
            {
                CopyStream(streamSrc, streamDst);
            }
        }

        /*
        //private
        private void _output(string startPath)
        {
            using (var zipStream = File.Open(startPath, FileMode.Open))
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                {
                    foreach (var entry in archive.Entries.ToArray())
                    {
                        //entry.Delete();
                        Console.WriteLine("'{0}'", entry.FullName);
                    }
                }
            }

            Console.ReadKey();
        }


        private void _GetEntryContents(ZipArchiveEntry entry)
        {
            using (var reader = new StreamReader(entry.Open(), Encoding.GetEncoding("sjis")))
            {
                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    string[] stArrayData = line.Split(',');
                    dic[stArrayData[0].Trim().ToLower()] = line;//[]のときは上書き
                }
            }
        }
        private void _PrintEntryContents(ZipArchiveEntry entry)
        {
            using (var reader = new StreamReader(entry.Open(), Encoding.GetEncoding("sjis")))
            {
                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    Console.WriteLine(line);
                }
            }
        }
        private void _PrintEntry(ZipArchiveEntry entry)
        {
            Console.WriteLine("[{0}, {1}]", entry.Name, entry.Length);
        }

    }
    */
        #endregion
    }
    #endregion
}