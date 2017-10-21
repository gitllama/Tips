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
using System.Windows.Media.Imaging;
using System.Windows.Media;

/*******************************************************************************/
/*   Pixels 2016/07/23                                                         */
/*******************************************************************************/
namespace Pixels
{
    /*--------------------------------------*/
    //   読み
    /*--------------------------------------*/
    public unsafe static partial class PixelStream
    {
		/*******************************/
		#region Binラッパー
        public static PixelByte ReadBinToPixelByte(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
            byte[] buf = new byte[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelByte(buf, w, h, new CancellationTokenSource());
        }
        public static PixelUShort ReadBinToPixelUShort(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
            ushort[] buf = new ushort[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelUShort(buf, w, h, new CancellationTokenSource());
        }
        public static PixelUInt ReadBinToPixelUInt(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
            uint[] buf = new uint[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelUInt(buf, w, h, new CancellationTokenSource());
        }
        public static PixelULong ReadBinToPixelULong(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
            ulong[] buf = new ulong[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelULong(buf, w, h, new CancellationTokenSource());
        }
        public static PixelShort ReadBinToPixelShort(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
            short[] buf = new short[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelShort(buf, w, h, new CancellationTokenSource());
        }
        public static PixelInt ReadBinToPixelInt(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
            int[] buf = new int[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelInt(buf, w, h, new CancellationTokenSource());
        }
        public static PixelLong ReadBinToPixelLong(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
            long[] buf = new long[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelLong(buf, w, h, new CancellationTokenSource());
        }
        public static PixelFloat ReadBinToPixelFloat(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
            float[] buf = new float[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelFloat(buf, w, h, new CancellationTokenSource());
        }
        public static PixelDouble ReadBinToPixelDouble(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
            double[] buf = new double[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelDouble(buf, w, h, new CancellationTokenSource());
        }
        public static PixelByte ReadBinToPixelByte(Stream z, int w, int h, int offsetbyte)
        {
			byte[] scr = Stream(z, w*h*1);
            byte[] buf = new byte[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelByte(buf, w, h, new CancellationTokenSource());
        }
        public static PixelUShort ReadBinToPixelUShort(Stream z, int w, int h, int offsetbyte)
        {
			byte[] scr = Stream(z, w*h*2);
            ushort[] buf = new ushort[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelUShort(buf, w, h, new CancellationTokenSource());
        }
        public static PixelUInt ReadBinToPixelUInt(Stream z, int w, int h, int offsetbyte)
        {
			byte[] scr = Stream(z, w*h*4);
            uint[] buf = new uint[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelUInt(buf, w, h, new CancellationTokenSource());
        }
        public static PixelULong ReadBinToPixelULong(Stream z, int w, int h, int offsetbyte)
        {
			byte[] scr = Stream(z, w*h*8);
            ulong[] buf = new ulong[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelULong(buf, w, h, new CancellationTokenSource());
        }
        public static PixelShort ReadBinToPixelShort(Stream z, int w, int h, int offsetbyte)
        {
			byte[] scr = Stream(z, w*h*2);
            short[] buf = new short[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelShort(buf, w, h, new CancellationTokenSource());
        }
        public static PixelInt ReadBinToPixelInt(Stream z, int w, int h, int offsetbyte)
        {
			byte[] scr = Stream(z, w*h*4);
            int[] buf = new int[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelInt(buf, w, h, new CancellationTokenSource());
        }
        public static PixelLong ReadBinToPixelLong(Stream z, int w, int h, int offsetbyte)
        {
			byte[] scr = Stream(z, w*h*8);
            long[] buf = new long[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelLong(buf, w, h, new CancellationTokenSource());
        }
        public static PixelFloat ReadBinToPixelFloat(Stream z, int w, int h, int offsetbyte)
        {
			byte[] scr = Stream(z, w*h*4);
            float[] buf = new float[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelFloat(buf, w, h, new CancellationTokenSource());
        }
        public static PixelDouble ReadBinToPixelDouble(Stream z, int w, int h, int offsetbyte)
        {
			byte[] scr = Stream(z, w*h*8);
            double[] buf = new double[w*h];
			ReadBin(ref scr, ref buf, w * h, offsetbyte);
            return new PixelDouble(buf, w, h, new CancellationTokenSource());
        }
        public static PixelInt ReadBin24ToPixelInt(string filename, int w, int h, int offsetbyte)
        {
			byte[] scr = File.ReadAllBytes(filename);
			int[] buf = new int[w*h];
			ReadBinSift8(ref scr, ref buf, w * h, offsetbyte);
            return new PixelInt(buf, w, h, new CancellationTokenSource());
        }
		#endregion
		/*******************************/
		#region Bin本体
        public static void ReadBin(ref byte[] scr, ref byte[] buf,int size, int offsetbyte)
        {
            fixed (byte* r = &buf[0])
            fixed (byte* s = &scr[0])
            {
                byte* ss = s + offsetbyte;
                byte* rr = r;
                for (int i = 0; i < size; i++)
                    *rr++ = *ss++;
            }
        }
	    public static void ReadBin(ref byte[] scr, ref ushort[] buf,int size, int offsetbyte)
        {
			int sizebyte = 2;
			byte[] Endian = new byte[sizebyte];

            fixed (ushort* r = &buf[0])
            fixed (byte* s = &scr[0])
			fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                ushort* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    *rr++ = BitConverter.ToUInt16(Endian, 0);
                }
            }
        }
	    public static void ReadBin(ref byte[] scr, ref uint[] buf,int size, int offsetbyte)
        {
			int sizebyte = 4;
			byte[] Endian = new byte[sizebyte];

            fixed (uint* r = &buf[0])
            fixed (byte* s = &scr[0])
			fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                uint* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    *(ee + 2) = *ss++;
                    *(ee + 3) = *ss++;
                    *rr++ = BitConverter.ToUInt32(Endian, 0);
                }
            }
        }
	    public static void ReadBin(ref byte[] scr, ref ulong[] buf,int size, int offsetbyte)
        {
			int sizebyte = 8;
			byte[] Endian = new byte[sizebyte];

            fixed (ulong* r = &buf[0])
            fixed (byte* s = &scr[0])
			fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                ulong* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    *(ee + 2) = *ss++;
                    *(ee + 3) = *ss++;
                    *(ee + 4) = *ss++;
                    *(ee + 5) = *ss++;
                    *(ee + 6) = *ss++;
                    *(ee + 7) = *ss++;
                    *rr++ = BitConverter.ToUInt64(Endian, 0);
                }
            }
        }
	    public static void ReadBin(ref byte[] scr, ref short[] buf,int size, int offsetbyte)
        {
			int sizebyte = 2;
			byte[] Endian = new byte[sizebyte];

            fixed (short* r = &buf[0])
            fixed (byte* s = &scr[0])
			fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                short* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    *rr++ = BitConverter.ToInt16(Endian, 0);
                }
            }
        }
	    public static void ReadBin(ref byte[] scr, ref int[] buf,int size, int offsetbyte)
        {
			int sizebyte = 4;
			byte[] Endian = new byte[sizebyte];

            fixed (int* r = &buf[0])
            fixed (byte* s = &scr[0])
			fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                int* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    *(ee + 2) = *ss++;
                    *(ee + 3) = *ss++;
                    *rr++ = BitConverter.ToInt32(Endian, 0);
                }
            }
        }
	    public static void ReadBin(ref byte[] scr, ref long[] buf,int size, int offsetbyte)
        {
			int sizebyte = 8;
			byte[] Endian = new byte[sizebyte];

            fixed (long* r = &buf[0])
            fixed (byte* s = &scr[0])
			fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                long* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    *(ee + 2) = *ss++;
                    *(ee + 3) = *ss++;
                    *(ee + 4) = *ss++;
                    *(ee + 5) = *ss++;
                    *(ee + 6) = *ss++;
                    *(ee + 7) = *ss++;
                    *rr++ = BitConverter.ToInt64(Endian, 0);
                }
            }
        }
	    public static void ReadBin(ref byte[] scr, ref float[] buf,int size, int offsetbyte)
        {
			int sizebyte = 4;
			byte[] Endian = new byte[sizebyte];

            fixed (float* r = &buf[0])
            fixed (byte* s = &scr[0])
			fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                float* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    *(ee + 2) = *ss++;
                    *(ee + 3) = *ss++;
                    *rr++ = BitConverter.ToSingle(Endian, 0);
                }
            }
        }
	    public static void ReadBin(ref byte[] scr, ref double[] buf,int size, int offsetbyte)
        {
			int sizebyte = 8;
			byte[] Endian = new byte[sizebyte];

            fixed (double* r = &buf[0])
            fixed (byte* s = &scr[0])
			fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                double* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    *(ee + 2) = *ss++;
                    *(ee + 3) = *ss++;
                    *(ee + 4) = *ss++;
                    *(ee + 5) = *ss++;
                    *(ee + 6) = *ss++;
                    *(ee + 7) = *ss++;
                    *rr++ = BitConverter.ToDouble(Endian, 0);
                }
            }
        }
        public static void ReadBinSift8(ref byte[] scr, ref int[] buf,int size, int offsetbyte)
        {
			int sizebyte = 4;
			byte[] Endian = new byte[sizebyte];

            fixed (int* r = &buf[0])
            fixed (byte* s = &scr[0])
			fixed (byte* e = &Endian[0])
            {
                byte* ss = s + offsetbyte;
                byte* ee = e;
                int* rr = r;
                for (int i = 0; i < size; i++)
                {
                    *(ee + 0) = *ss++;
                    *(ee + 1) = *ss++;
                    *(ee + 2) = *ss++;
                    *(ee + 3) = *ss++;
                    *rr++ = BitConverter.ToInt32(Endian, 0) >> 8;
                }
            }
        }
		#endregion
	}

	/*--------------------------------------*/
    //   書き
    /*--------------------------------------*/
    public unsafe static partial class PixelStream
    {
		#region Bin
	    public static void WriteBin(this PixelByte p, string filename)
        {
            //フォルダなかったら無理やり作る
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filename)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream r = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                int bytesize = sizeof(byte);
				byte i;
                for (int y = 0; y < p.Height; y++)
                    for (int x = 0; x < p.Width; x++)
                    {
                        i = p[x, y];
                        r.Write(BitConverter.GetBytes(i), 0, bytesize);
                    }
            }
        }
	    public static void WriteBin(this PixelUShort p, string filename)
        {
            //フォルダなかったら無理やり作る
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filename)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream r = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                int bytesize = sizeof(ushort);
				ushort i;
                for (int y = 0; y < p.Height; y++)
                    for (int x = 0; x < p.Width; x++)
                    {
                        i = p[x, y];
                        r.Write(BitConverter.GetBytes(i), 0, bytesize);
                    }
            }
        }
	    public static void WriteBin(this PixelUInt p, string filename)
        {
            //フォルダなかったら無理やり作る
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filename)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream r = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                int bytesize = sizeof(uint);
				uint i;
                for (int y = 0; y < p.Height; y++)
                    for (int x = 0; x < p.Width; x++)
                    {
                        i = p[x, y];
                        r.Write(BitConverter.GetBytes(i), 0, bytesize);
                    }
            }
        }
	    public static void WriteBin(this PixelULong p, string filename)
        {
            //フォルダなかったら無理やり作る
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filename)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream r = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                int bytesize = sizeof(ulong);
				ulong i;
                for (int y = 0; y < p.Height; y++)
                    for (int x = 0; x < p.Width; x++)
                    {
                        i = p[x, y];
                        r.Write(BitConverter.GetBytes(i), 0, bytesize);
                    }
            }
        }
	    public static void WriteBin(this PixelShort p, string filename)
        {
            //フォルダなかったら無理やり作る
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filename)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream r = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                int bytesize = sizeof(short);
				short i;
                for (int y = 0; y < p.Height; y++)
                    for (int x = 0; x < p.Width; x++)
                    {
                        i = p[x, y];
                        r.Write(BitConverter.GetBytes(i), 0, bytesize);
                    }
            }
        }
	    public static void WriteBin(this PixelInt p, string filename)
        {
            //フォルダなかったら無理やり作る
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filename)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream r = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                int bytesize = sizeof(int);
				int i;
                for (int y = 0; y < p.Height; y++)
                    for (int x = 0; x < p.Width; x++)
                    {
                        i = p[x, y];
                        r.Write(BitConverter.GetBytes(i), 0, bytesize);
                    }
            }
        }
	    public static void WriteBin(this PixelLong p, string filename)
        {
            //フォルダなかったら無理やり作る
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filename)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream r = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                int bytesize = sizeof(long);
				long i;
                for (int y = 0; y < p.Height; y++)
                    for (int x = 0; x < p.Width; x++)
                    {
                        i = p[x, y];
                        r.Write(BitConverter.GetBytes(i), 0, bytesize);
                    }
            }
        }
	    public static void WriteBin(this PixelFloat p, string filename)
        {
            //フォルダなかったら無理やり作る
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filename)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream r = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                int bytesize = sizeof(float);
				float i;
                for (int y = 0; y < p.Height; y++)
                    for (int x = 0; x < p.Width; x++)
                    {
                        i = p[x, y];
                        r.Write(BitConverter.GetBytes(i), 0, bytesize);
                    }
            }
        }
	    public static void WriteBin(this PixelDouble p, string filename)
        {
            //フォルダなかったら無理やり作る
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filename)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream r = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                int bytesize = sizeof(double);
				double i;
                for (int y = 0; y < p.Height; y++)
                    for (int x = 0; x < p.Width; x++)
                    {
                        i = p[x, y];
                        r.Write(BitConverter.GetBytes(i), 0, bytesize);
                    }
            }
        }
        public static void WriteTxt(Stream z,PixelByte p)
        {
            byte[] buf;
            Action<string> write = (st) =>
            {
                buf = System.Text.Encoding.ASCII.GetBytes(st);
                z.Write(buf, 0, buf.Length);
            };

            for (int i = 0; i < p.Size; i++)
                write(p[i] + System.Environment.NewLine);
        }
        public static void WriteTxt(Stream z,PixelUShort p)
        {
            byte[] buf;
            Action<string> write = (st) =>
            {
                buf = System.Text.Encoding.ASCII.GetBytes(st);
                z.Write(buf, 0, buf.Length);
            };

            for (int i = 0; i < p.Size; i++)
                write(p[i] + System.Environment.NewLine);
        }
        public static void WriteTxt(Stream z,PixelUInt p)
        {
            byte[] buf;
            Action<string> write = (st) =>
            {
                buf = System.Text.Encoding.ASCII.GetBytes(st);
                z.Write(buf, 0, buf.Length);
            };

            for (int i = 0; i < p.Size; i++)
                write(p[i] + System.Environment.NewLine);
        }
        public static void WriteTxt(Stream z,PixelULong p)
        {
            byte[] buf;
            Action<string> write = (st) =>
            {
                buf = System.Text.Encoding.ASCII.GetBytes(st);
                z.Write(buf, 0, buf.Length);
            };

            for (int i = 0; i < p.Size; i++)
                write(p[i] + System.Environment.NewLine);
        }
        public static void WriteTxt(Stream z,PixelShort p)
        {
            byte[] buf;
            Action<string> write = (st) =>
            {
                buf = System.Text.Encoding.ASCII.GetBytes(st);
                z.Write(buf, 0, buf.Length);
            };

            for (int i = 0; i < p.Size; i++)
                write(p[i] + System.Environment.NewLine);
        }
        public static void WriteTxt(Stream z,PixelInt p)
        {
            byte[] buf;
            Action<string> write = (st) =>
            {
                buf = System.Text.Encoding.ASCII.GetBytes(st);
                z.Write(buf, 0, buf.Length);
            };

            for (int i = 0; i < p.Size; i++)
                write(p[i] + System.Environment.NewLine);
        }
        public static void WriteTxt(Stream z,PixelLong p)
        {
            byte[] buf;
            Action<string> write = (st) =>
            {
                buf = System.Text.Encoding.ASCII.GetBytes(st);
                z.Write(buf, 0, buf.Length);
            };

            for (int i = 0; i < p.Size; i++)
                write(p[i] + System.Environment.NewLine);
        }
        public static void WriteTxt(Stream z,PixelFloat p)
        {
            byte[] buf;
            Action<string> write = (st) =>
            {
                buf = System.Text.Encoding.ASCII.GetBytes(st);
                z.Write(buf, 0, buf.Length);
            };

            for (int i = 0; i < p.Size; i++)
                write(p[i] + System.Environment.NewLine);
        }
        public static void WriteTxt(Stream z,PixelDouble p)
        {
            byte[] buf;
            Action<string> write = (st) =>
            {
                buf = System.Text.Encoding.ASCII.GetBytes(st);
                z.Write(buf, 0, buf.Length);
            };

            for (int i = 0; i < p.Size; i++)
                write(p[i] + System.Environment.NewLine);
        }
	    public static void WriteBin(Stream z, PixelByte p)
        {
            int bytesize = sizeof(byte);
			int pixsize = p.Size;
            for (int i = 0; i < pixsize; i++)
				z.Write(BitConverter.GetBytes(p[i]), 0, bytesize);
        }
	    public static void WriteBin(Stream z, PixelUShort p)
        {
            int bytesize = sizeof(ushort);
			int pixsize = p.Size;
            for (int i = 0; i < pixsize; i++)
				z.Write(BitConverter.GetBytes(p[i]), 0, bytesize);
        }
	    public static void WriteBin(Stream z, PixelUInt p)
        {
            int bytesize = sizeof(uint);
			int pixsize = p.Size;
            for (int i = 0; i < pixsize; i++)
				z.Write(BitConverter.GetBytes(p[i]), 0, bytesize);
        }
	    public static void WriteBin(Stream z, PixelULong p)
        {
            int bytesize = sizeof(ulong);
			int pixsize = p.Size;
            for (int i = 0; i < pixsize; i++)
				z.Write(BitConverter.GetBytes(p[i]), 0, bytesize);
        }
	    public static void WriteBin(Stream z, PixelShort p)
        {
            int bytesize = sizeof(short);
			int pixsize = p.Size;
            for (int i = 0; i < pixsize; i++)
				z.Write(BitConverter.GetBytes(p[i]), 0, bytesize);
        }
	    public static void WriteBin(Stream z, PixelInt p)
        {
            int bytesize = sizeof(int);
			int pixsize = p.Size;
            for (int i = 0; i < pixsize; i++)
				z.Write(BitConverter.GetBytes(p[i]), 0, bytesize);
        }
	    public static void WriteBin(Stream z, PixelLong p)
        {
            int bytesize = sizeof(long);
			int pixsize = p.Size;
            for (int i = 0; i < pixsize; i++)
				z.Write(BitConverter.GetBytes(p[i]), 0, bytesize);
        }
	    public static void WriteBin(Stream z, PixelFloat p)
        {
            int bytesize = sizeof(float);
			int pixsize = p.Size;
            for (int i = 0; i < pixsize; i++)
				z.Write(BitConverter.GetBytes(p[i]), 0, bytesize);
        }
	    public static void WriteBin(Stream z, PixelDouble p)
        {
            int bytesize = sizeof(double);
			int pixsize = p.Size;
            for (int i = 0; i < pixsize; i++)
				z.Write(BitConverter.GetBytes(p[i]), 0, bytesize);
        }










		#endregion

		#region CSV
		public static void WriteCSV(this PixelByte p, string filename,bool pgmheader = false, string delimiter = ", ")
        {
            //Func<T, string> cout = (i) => String.Format("{0}", i);
			int w = p.Width;
			int h = p.Height;

            using (StreamWriter wr = new StreamWriter(filename))
            {
				if(pgmheader)
				{
					wr.Write($"P2{System.Environment.NewLine}");
					wr.Write($"{w} {h}{System.Environment.NewLine}");
					wr.Write($"{p.Max()}{System.Environment.NewLine}");
				}
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        wr.Write(p[x, y]);
                        wr.Write(delimiter);
                    }
                    wr.Write(p[w - 1, y]);
                    wr.Write(System.Environment.NewLine);
                }
            }
		}
		public static void WriteCSV(this PixelUShort p, string filename,bool pgmheader = false, string delimiter = ", ")
        {
            //Func<T, string> cout = (i) => String.Format("{0}", i);
			int w = p.Width;
			int h = p.Height;

            using (StreamWriter wr = new StreamWriter(filename))
            {
				if(pgmheader)
				{
					wr.Write($"P2{System.Environment.NewLine}");
					wr.Write($"{w} {h}{System.Environment.NewLine}");
					wr.Write($"{p.Max()}{System.Environment.NewLine}");
				}
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        wr.Write(p[x, y]);
                        wr.Write(delimiter);
                    }
                    wr.Write(p[w - 1, y]);
                    wr.Write(System.Environment.NewLine);
                }
            }
		}
		public static void WriteCSV(this PixelUInt p, string filename,bool pgmheader = false, string delimiter = ", ")
        {
            //Func<T, string> cout = (i) => String.Format("{0}", i);
			int w = p.Width;
			int h = p.Height;

            using (StreamWriter wr = new StreamWriter(filename))
            {
				if(pgmheader)
				{
					wr.Write($"P2{System.Environment.NewLine}");
					wr.Write($"{w} {h}{System.Environment.NewLine}");
					wr.Write($"{p.Max()}{System.Environment.NewLine}");
				}
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        wr.Write(p[x, y]);
                        wr.Write(delimiter);
                    }
                    wr.Write(p[w - 1, y]);
                    wr.Write(System.Environment.NewLine);
                }
            }
		}
		public static void WriteCSV(this PixelULong p, string filename,bool pgmheader = false, string delimiter = ", ")
        {
            //Func<T, string> cout = (i) => String.Format("{0}", i);
			int w = p.Width;
			int h = p.Height;

            using (StreamWriter wr = new StreamWriter(filename))
            {
				if(pgmheader)
				{
					wr.Write($"P2{System.Environment.NewLine}");
					wr.Write($"{w} {h}{System.Environment.NewLine}");
					wr.Write($"{p.Max()}{System.Environment.NewLine}");
				}
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        wr.Write(p[x, y]);
                        wr.Write(delimiter);
                    }
                    wr.Write(p[w - 1, y]);
                    wr.Write(System.Environment.NewLine);
                }
            }
		}
		public static void WriteCSV(this PixelShort p, string filename,bool pgmheader = false, string delimiter = ", ")
        {
            //Func<T, string> cout = (i) => String.Format("{0}", i);
			int w = p.Width;
			int h = p.Height;

            using (StreamWriter wr = new StreamWriter(filename))
            {
				if(pgmheader)
				{
					wr.Write($"P2{System.Environment.NewLine}");
					wr.Write($"{w} {h}{System.Environment.NewLine}");
					wr.Write($"{p.Max()}{System.Environment.NewLine}");
				}
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        wr.Write(p[x, y]);
                        wr.Write(delimiter);
                    }
                    wr.Write(p[w - 1, y]);
                    wr.Write(System.Environment.NewLine);
                }
            }
		}
		public static void WriteCSV(this PixelInt p, string filename,bool pgmheader = false, string delimiter = ", ")
        {
            //Func<T, string> cout = (i) => String.Format("{0}", i);
			int w = p.Width;
			int h = p.Height;

            using (StreamWriter wr = new StreamWriter(filename))
            {
				if(pgmheader)
				{
					wr.Write($"P2{System.Environment.NewLine}");
					wr.Write($"{w} {h}{System.Environment.NewLine}");
					wr.Write($"{p.Max()}{System.Environment.NewLine}");
				}
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        wr.Write(p[x, y]);
                        wr.Write(delimiter);
                    }
                    wr.Write(p[w - 1, y]);
                    wr.Write(System.Environment.NewLine);
                }
            }
		}
		public static void WriteCSV(this PixelLong p, string filename,bool pgmheader = false, string delimiter = ", ")
        {
            //Func<T, string> cout = (i) => String.Format("{0}", i);
			int w = p.Width;
			int h = p.Height;

            using (StreamWriter wr = new StreamWriter(filename))
            {
				if(pgmheader)
				{
					wr.Write($"P2{System.Environment.NewLine}");
					wr.Write($"{w} {h}{System.Environment.NewLine}");
					wr.Write($"{p.Max()}{System.Environment.NewLine}");
				}
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        wr.Write(p[x, y]);
                        wr.Write(delimiter);
                    }
                    wr.Write(p[w - 1, y]);
                    wr.Write(System.Environment.NewLine);
                }
            }
		}
		public static void WriteCSV(this PixelFloat p, string filename,bool pgmheader = false, string delimiter = ", ")
        {
            //Func<T, string> cout = (i) => String.Format("{0}", i);
			int w = p.Width;
			int h = p.Height;

            using (StreamWriter wr = new StreamWriter(filename))
            {
				if(pgmheader)
				{
					wr.Write($"P2{System.Environment.NewLine}");
					wr.Write($"{w} {h}{System.Environment.NewLine}");
					wr.Write($"{p.Max()}{System.Environment.NewLine}");
				}
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        wr.Write(p[x, y]);
                        wr.Write(delimiter);
                    }
                    wr.Write(p[w - 1, y]);
                    wr.Write(System.Environment.NewLine);
                }
            }
		}
		public static void WriteCSV(this PixelDouble p, string filename,bool pgmheader = false, string delimiter = ", ")
        {
            //Func<T, string> cout = (i) => String.Format("{0}", i);
			int w = p.Width;
			int h = p.Height;

            using (StreamWriter wr = new StreamWriter(filename))
            {
				if(pgmheader)
				{
					wr.Write($"P2{System.Environment.NewLine}");
					wr.Write($"{w} {h}{System.Environment.NewLine}");
					wr.Write($"{p.Max()}{System.Environment.NewLine}");
				}
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        wr.Write(p[x, y]);
                        wr.Write(delimiter);
                    }
                    wr.Write(p[w - 1, y]);
                    wr.Write(System.Environment.NewLine);
                }
            }
		}
		#endregion

		#region BMP
		public static BitmapSource ToBitmapSource(this PixelByte p, double offset, double depth)
        {
			//((pix + offset) * 255 / depth).Limits(typeof(Byte)).ToPixelByte().ToBitmapSource();

			byte[] buf_byte = new byte[p.Width * p.Height];

			for (int i = 0; i < p.Width * p.Height; i++)
			{
				var hoge = ((p[i] + offset) * 255 / depth);
				buf_byte[i] = (byte)(hoge > 255 ? 255 : hoge < 0 ? 0 : hoge);
				
			}

			var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                buf_byte,
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
		}
		public static BitmapSource ToBitmapSource(this PixelUShort p, double offset, double depth)
        {
			//((pix + offset) * 255 / depth).Limits(typeof(Byte)).ToPixelByte().ToBitmapSource();

			byte[] buf_byte = new byte[p.Width * p.Height];

			for (int i = 0; i < p.Width * p.Height; i++)
			{
				var hoge = ((p[i] + offset) * 255 / depth);
				buf_byte[i] = (byte)(hoge > 255 ? 255 : hoge < 0 ? 0 : hoge);
				
			}

			var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                buf_byte,
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
		}
		public static BitmapSource ToBitmapSource(this PixelUInt p, double offset, double depth)
        {
			//((pix + offset) * 255 / depth).Limits(typeof(Byte)).ToPixelByte().ToBitmapSource();

			byte[] buf_byte = new byte[p.Width * p.Height];

			for (int i = 0; i < p.Width * p.Height; i++)
			{
				var hoge = ((p[i] + offset) * 255 / depth);
				buf_byte[i] = (byte)(hoge > 255 ? 255 : hoge < 0 ? 0 : hoge);
				
			}

			var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                buf_byte,
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
		}
		public static BitmapSource ToBitmapSource(this PixelULong p, double offset, double depth)
        {
			//((pix + offset) * 255 / depth).Limits(typeof(Byte)).ToPixelByte().ToBitmapSource();

			byte[] buf_byte = new byte[p.Width * p.Height];

			for (int i = 0; i < p.Width * p.Height; i++)
			{
				var hoge = ((p[i] + offset) * 255 / depth);
				buf_byte[i] = (byte)(hoge > 255 ? 255 : hoge < 0 ? 0 : hoge);
				
			}

			var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                buf_byte,
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
		}
		public static BitmapSource ToBitmapSource(this PixelShort p, double offset, double depth)
        {
			//((pix + offset) * 255 / depth).Limits(typeof(Byte)).ToPixelByte().ToBitmapSource();

			byte[] buf_byte = new byte[p.Width * p.Height];

			for (int i = 0; i < p.Width * p.Height; i++)
			{
				var hoge = ((p[i] + offset) * 255 / depth);
				buf_byte[i] = (byte)(hoge > 255 ? 255 : hoge < 0 ? 0 : hoge);
				
			}

			var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                buf_byte,
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
		}
		public static BitmapSource ToBitmapSource(this PixelInt p, double offset, double depth)
        {
			//((pix + offset) * 255 / depth).Limits(typeof(Byte)).ToPixelByte().ToBitmapSource();

			byte[] buf_byte = new byte[p.Width * p.Height];

			for (int i = 0; i < p.Width * p.Height; i++)
			{
				var hoge = ((p[i] + offset) * 255 / depth);
				buf_byte[i] = (byte)(hoge > 255 ? 255 : hoge < 0 ? 0 : hoge);
				
			}

			var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                buf_byte,
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
		}
		public static BitmapSource ToBitmapSource(this PixelLong p, double offset, double depth)
        {
			//((pix + offset) * 255 / depth).Limits(typeof(Byte)).ToPixelByte().ToBitmapSource();

			byte[] buf_byte = new byte[p.Width * p.Height];

			for (int i = 0; i < p.Width * p.Height; i++)
			{
				var hoge = ((p[i] + offset) * 255 / depth);
				buf_byte[i] = (byte)(hoge > 255 ? 255 : hoge < 0 ? 0 : hoge);
				
			}

			var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                buf_byte,
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
		}
		public static BitmapSource ToBitmapSource(this PixelFloat p, double offset, double depth)
        {
			//((pix + offset) * 255 / depth).Limits(typeof(Byte)).ToPixelByte().ToBitmapSource();

			byte[] buf_byte = new byte[p.Width * p.Height];

			for (int i = 0; i < p.Width * p.Height; i++)
			{
				var hoge = ((p[i] + offset) * 255 / depth);
				buf_byte[i] = (byte)(hoge > 255 ? 255 : hoge < 0 ? 0 : hoge);
				
			}

			var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                buf_byte,
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
		}
		public static BitmapSource ToBitmapSource(this PixelDouble p, double offset, double depth)
        {
			//((pix + offset) * 255 / depth).Limits(typeof(Byte)).ToPixelByte().ToBitmapSource();

			byte[] buf_byte = new byte[p.Width * p.Height];

			for (int i = 0; i < p.Width * p.Height; i++)
			{
				var hoge = ((p[i] + offset) * 255 / depth);
				buf_byte[i] = (byte)(hoge > 255 ? 255 : hoge < 0 ? 0 : hoge);
				
			}

			var buf = BitmapImage.Create(
                p.Width, p.Height, 96, 96,
                System.Windows.Media.PixelFormats.Gray8,
                null,
                buf_byte,
                (p.Width * PixelFormats.Gray8.BitsPerPixel + 7) / 8);
            buf.Freeze();
            return buf;
		}
		#endregion

	}

}