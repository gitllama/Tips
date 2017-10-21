using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using System.Windows;


namespace BTCV.Models.DSP
{

    public class DigitalProcess
    {
        /*変数*/

        public DSPParam param = new DSPParam();

        ushort[] img16uc1;
        ushort[] img16uc3;
        byte[] img8uc3;
        public WriteableBitmap img { get; private set; }

        int Width;
        int Height;
        int Canvas_Width;
        int Canvas_Height;

        public ushort[] lut { get; set; }


#if OpenCL
        OpenCL.OpeCLDemosaic opencl;
#endif


        /*コンストラクタ*/

        public DigitalProcess(WriteableBitmap bitmap)
        {
            this.Width = bitmap.PixelWidth;
            this.Height = bitmap.PixelHeight;
            this.Canvas_Width = Width;
            this.Canvas_Height = Height;

            img = bitmap;
        }

        public DigitalProcess(int Width, int Height, int Canvas_Width, int Canvas_Height)
        {
            //if (Width != width || Height != height)
            //if (Canvas_Width != Canvas_Width || Canvas_Height != Canvas_Height)

            this.Width = Width;
            this.Height = Height;

            this.Canvas_Width = Canvas_Width;
            this.Canvas_Height = Canvas_Height;
            img16uc1 = new ushort[Canvas_Width * Canvas_Height];
            img16uc3 = new ushort[Canvas_Width * Canvas_Height * 3];
            img8uc3 = new byte[Canvas_Width * Canvas_Height * 3];
#if OpenCL
            opencl = new OpenCL.OpeCLDemosaic(Canvas_Width, Canvas_Height);
#endif
            img = new WriteableBitmap(Canvas_Width, Canvas_Height, 96, 96, System.Windows.Media.PixelFormats.Bgr24, null);
        }


        public void Run(int[] src, int[] imagdarksub, out int[] SamplingValue, CaptureParam caps)
        {
            var DSP = DSPFactory.Create(param.DemosaicSelect);

            DSP.param = param;
            DSP.Width = Width;
            DSP.Height = Height;
            DSP.Canvas_Width = Canvas_Width;
            DSP.Canvas_Height = Canvas_Height;


            /*SaveProcess*/

            //saves.save_pre(src);

            if(caps.enableresult)
            {
                caps.GetResult();
                src = Array.ConvertAll(caps.ave, n=> (int)n);

                caps.ave.SaveRawFloat(caps.savepath, $"ave.bin");
                caps.dev.SaveRawFloat(caps.savepath, $"dev.bin");
            }

            //DSP.Unfolding(src);
            DSP.UnfoldingCorrection(src);

            if (caps.enable1) src.SaveRaw(caps.savepath, $"{caps.filename}.bin");
            if (caps.enableadd1) caps.Add(src);



            DSP.DarkSub(src, imagdarksub);
            DSP.HOB(src);

            if (caps.enable2) src.SaveRaw(caps.savepath, $"{caps.filename}_HOB.bin");

            DSP.Sort(src);

            SamplingValue = DSP.Sampling(src);

            DSP.Offset(src);

            TrimCanvas(src, param.Canvas_X, param.Canvas_Y);

            DSP.Demosaic(
                Canvas_Width, Canvas_Height,
                img16uc1, img16uc3, img8uc3,
                GetInputMat((float)param.RGain, (float)param.GGain, (float)param.BGain, param.Matrix),
                param.bayerpattern
                );

            if (lut != null) LookUp();

            Convert8UC3();

            Blur();

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ConvertBitmap();
            }), DispatcherPriority.Background, new object[] { });

            if (caps.enableBMP) img.SaveBMP(caps.savepath, $"{caps.filename}.bmp");

            //後処理
            //return (img, new int[] { 1 }); 
        }

        public void TrimCanvas(int[] src, int Canvas_X, int Canvas_Y)
        {
            int count = 0;
            for (int y = Canvas_Y; y < Canvas_Y + Canvas_Height; y++)
                for (int x = Canvas_X; x < Canvas_X + Canvas_Width; x++)
                {
                    int buf = src[x + y * Width];
                    img16uc1[count] = (ushort)(buf > ushort.MaxValue ? ushort.MaxValue : buf < 0 ? 0 : buf);
                    count++;
                }
        }

        public void LookUp()
        {
            //using (Mat mat16uc3 = new Mat(Canvas_Height, Canvas_Width, MatType.CV_16UC3, img16uc3))
            //{
            //    Cv2.LUT(mat16uc3, lut, mat16uc3);
            //}
            for (int i = 0; i < img16uc3.Length; i++)
            {
                img16uc3[i] = lut[img16uc3[i]];
            }
        }

        public void Convert8UC3()
        {
            using (Mat mat16uc3 = new Mat(Canvas_Height, Canvas_Width, MatType.CV_16UC3, img16uc3))
            using (Mat mat8uc3 = new Mat(Canvas_Height, Canvas_Width, MatType.CV_8UC3, img8uc3))
            {
                mat16uc3.ConvertTo(mat8uc3, MatType.CV_8UC3);
            }
        }

        public void Blur()
        {
            using (Mat mat8uc3 = new Mat(Canvas_Height, Canvas_Width, MatType.CV_8UC3, img8uc3))
            {
                if (param.MedianBlur > 1) Cv2.MedianBlur(mat8uc3, mat8uc3, param.MedianBlur);
                if (param.GaussianBlur > 0) Cv2.GaussianBlur(mat8uc3, mat8uc3, new OpenCvSharp.Size(0, 0), param.GaussianBlur);
            }
        }

        public void ConvertBitmap()
        {
            using (Mat mat8 = new Mat(Canvas_Height, Canvas_Width, MatType.CV_8UC3, img8uc3))
                WriteableBitmapConverter.ToWriteableBitmap(mat8, img);
        }

        static float[] GetInputMat(float RGain, float GGain, float BGain, float[] Matrix)
        {
            //マトリックス*色ゲインの再計算
            float[] m = new float[9];
            m[0] = (float)(Matrix[0] * BGain);
            m[1] = (float)(Matrix[1] * BGain);
            m[2] = (float)(Matrix[2] * BGain);
            m[3] = (float)(Matrix[3] * GGain);
            m[4] = (float)(Matrix[4] * GGain);
            m[5] = (float)(Matrix[5] * GGain);
            m[6] = (float)(Matrix[6] * RGain);
            m[7] = (float)(Matrix[7] * RGain);
            m[8] = (float)(Matrix[8] * RGain);

            return m;
        }

    }


    /******************/


    public static class DSPFactory
    {
        static DSPProcess Through = new DSPThroughState();
        static DSPProcess Off = new DSPOffState();
        static DSPProcess On_mono = new DSPOn_monoState();
        static DSPProcess On_color = new DSPOn_colorState();
        static DSPProcess Quadrant = new DSPQuadrantState();
        static DSPProcess QuadColor = new DSPQuadColorState();
        static DSPProcess Binning = new DSPBinningState();

        public static DSPProcess Create(DemosaicEnum demosaic)
        {
            switch (demosaic)
            {
                case DemosaicEnum.Through: return Through;
                case DemosaicEnum.Off: return Off;
                case DemosaicEnum.On_mono: return On_mono;
                case DemosaicEnum.On_color: return On_color;
                case DemosaicEnum.Quadrant: return Quadrant;
                case DemosaicEnum.QuadColor: return QuadColor;
                case DemosaicEnum.Binning: return Binning;
                default: return Off;
            }
        }
    }

    public class DSPProcess
    {
        public int Width;
        public int Height;
        public int Canvas_Width;
        public int Canvas_Height;

        public DSPParam param;

        public virtual DarkSubEnum DarkSubSelect { get => param.DarkSubSelect; }
        public virtual SoftHOBEnum SoftHOBSelect { get => param.SoftHOBSelect; }
        public virtual SortEnum SortSelect { get => param.SortSelect; }

        public void DarkSub(int[] src, int[] sub)
        {
            if (DarkSubSelect == DarkSubEnum.Off) return;

            for (int i = 0; i < src.Length; i++)
                src[i] -= sub[i];
        }

        public void HOB(int[] src)
        {
            switch (SoftHOBSelect)
            {
                case SoftHOBEnum.Left: HOBLeft(src); break;
                case SoftHOBEnum.Both: HOBBoth(src); break;
                case SoftHOBEnum.Linear: HOBLinear(src); break;
                case SoftHOBEnum.Test: HOBTest(src); break;
                default:
                    break;
            }
        }

        void HOBLeft(int[] src)
        {
            int hob = 0;
            int size = (param.HOBLEnd - param.HOBLStart);
            for (int y = 0; y < Height; y += 1)
            {
                for (int x = param.HOBLStart; x < param.HOBLEnd; x++) hob += src[x + y * Width];
                hob /= size;
                for (int x = 0; x < Width; x++) src[x + y * Width] -= hob;
            }
        }
        void HOBBoth(int[] src)
        {
            int hob = 0;
            int size = (param.HOBLEnd - param.HOBLStart) + (param.HOBREnd - param.HOBRStart);
            for (int y = 0; y < Height; y += 1)
            {
                for (int x = param.HOBLStart; x < param.HOBLEnd; x++) hob += src[x + y * Width];
                for (int x = param.HOBRStart; x < param.HOBREnd; x++) hob += src[x + y * Width];
                hob /= size;
                for (int x = 0; x < Width; x++) src[x + y * Width] -= hob;
            }
        }
        void HOBLinear(int[] src)
        {
            int hobl = 0;
            int hobr = 0;
            int sizel = param.HOBLEnd - param.HOBLStart;
            int sizer = param.HOBREnd - param.HOBRStart;
            for (int y = 0; y < Height; y += 1)
            {
                for (int x = param.HOBLStart; x < param.HOBLEnd; x++) hobl += src[x + y * Width];
                for (int x = param.HOBRStart; x < param.HOBREnd; x++) hobr += src[x + y * Width];
                hobl /= sizel;
                hobr /= sizer;
                for (int x = 0; x < Width; x++) src[x + y * Width] -= (int)(hobl + (hobr - hobl) * ((double)x / Width));
            }
        }
        void HOBTest(int[] src)
        {
            int hobl = 0;
            int hobr = 0;
            int sizel = param.HOBLEnd - param.HOBLStart;
            int sizer = param.HOBREnd - param.HOBRStart;

            List<int> max = new List<int>();
            List<int> min = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                max.Add(int.MinValue);
                min.Add(int.MaxValue);
            }


            for (int y = 0; y < Height; y += 1)
            {
                for (int x = param.HOBLStart; x < param.HOBLEnd; x++)
                {
                    int h = x + y * Width;

                    for (int i = 0; i < max.Count; i++)
                    {
                        if (max[i] < src[h])
                        {
                            max.Insert(i, src[h]);
                            max.RemoveAt(max.Count - 1);
                            break;
                        }
                    }
                    for (int i = 0; i < min.Count; i++)
                    {
                        if (min[i] > src[h])
                        {
                            min.Insert(i, src[h]);
                            min.RemoveAt(min.Count - 1);
                            break;
                        }
                    }

                    hobl += src[h];
                }
                hobl -= (max.Sum() + min.Sum());
                hobl /= (sizel - 6);

                max.Clear();
                min.Clear();
                for (int i = 0; i < 3; i++)
                {
                    max.Add(int.MinValue);
                    min.Add(int.MaxValue);
                }
                for (int x = param.HOBRStart; x < param.HOBREnd; x++)
                {
                    int h = x + y * Width;
                    for (int i = 0; i < max.Count; i++)
                    {
                        if (max[i] < src[h])
                        {
                            max.Insert(i, src[h]);
                            max.RemoveAt(max.Count - 1);
                            break;
                        }
                    }
                    for (int i = 0; i < min.Count; i++)
                    {
                        if (min[i] > src[h])
                        {
                            min.Insert(i, src[h]);
                            min.RemoveAt(min.Count - 1);
                            break;
                        }
                    }
                    hobr += src[h];
                }
                hobr -= (max.Sum() + min.Sum()); ;
                hobr /= (sizer - 6);

                for (int x = 0; x < Width; x++)
                {
                    int h = x + y * Width;

                    src[h] -= (int)(hobl + (hobr - hobl) * ((double)x / Width));
                }
            }
        }


        public void Sort(int[] src)
        {
            switch (SortSelect)
            {
                case SortEnum.StaggerL:
                    for (int y = 0; y < Height; y += 2)
                    {
                        for (int x = 0; x < Width - 1; x++)
                        {
                            int h = x + (y + 1) * 2256;

                            src[h] = src[h + 1];
                        }
                    }
                    break;
                case SortEnum.StaggerR:
                    for (int y = 0; y < Height; y += 2)
                    {
                        for (int x = Width - 1; x > 1; x--)
                        {
                            int h = x + (y + 1) * 2256;

                            src[h] = src[h - 1];
                        }
                    }
                    break;
                default:
                    break;

            }
        }

        public void Offset(int[] src)
        {
            int Size = Width * Height;
            if (param.bitshiftsub < 0)
            {
                for (int i = 0; i < Size; i++) src[i] = (src[i] + param.DarkOffset) >> param.bitshift;
            }
            else
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width / 2; x++)
                    {
                        src[x + y * Width] = (src[x + y * Width] + param.DarkOffset) >> param.bitshift;
                    }
                    for (int x = Width / 2; x < Width; x++)
                    {
                        src[x + y * Width] = (src[x + y * Width] + param.DarkOffset) >> param.bitshiftsub;
                    }
                }

            }

        }

        //Photometry
        public int[] Sampling(int[] src)
        {
            if (param.Sampling_Width < 1 || param.Sampling_Height < 1) return new int[4];

            var dst = new int[4];
            var count = new int[4];
            for (int y = param.Sampling_Y; y < param.Sampling_Y + param.Sampling_Height; y++)
                for (int x = param.Sampling_X; x < param.Sampling_X + param.Sampling_Width; x++)
                {
                    var i = x % 2 + (y % 2) * 2;
                    dst[i] += src[x + y * Width];
                    count[i]++;

                    //if (param.SamplingMark) src[x + y * Width] = int.MaxValue;
                }

            for (int i = 0; i < 4; i++)
                if (count[i] != 0) dst[i] /= count[i];

            return dst;


        }

        public virtual void Demosaic(int w, int h, ushort[] img16uc1, ushort[] img16uc3, byte[] img8uc3, float[] m, ColorConversionCodes bayerpattern)
        {
            using (Mat mat16uc1 = new Mat(h, w, MatType.CV_16UC1, img16uc1))
            using (Mat mat16uc3 = new Mat(h, w, MatType.CV_16UC3, img16uc3))
            using (Mat mat8uc3 = new Mat(h, w, MatType.CV_8UC3, img8uc3))
            using (Mat matrix = new Mat(3, 3, MatType.CV_32FC1, m))
            {
                Cv2.Merge(new Mat[] { mat16uc1, mat16uc1, mat16uc1 }, mat16uc3);
                //mat16uc3.ConvertTo(mat8uc3, MatType.CV_8UC3);
            }
        }

        public void Unfolding(int[] src)
        {
            //for BT200C

            if (param.Unfolding < 0) return;

            var FI = 11 - param.Unfolding;
            var Cyc = 12 + param.Unfolding;

            var FI_u = 9;
            var FI_d = 9 + Cyc;
            var FI_u2 = Cyc - 2;
            var cyc = 9 + FI;

            for (int i = 0; i < Width * Height; i++)
            {
                var wclp    = (src[i] & 0b00000000_10000000_00000000_00000000);
                var counter = ((src[i] << FI_u) >> FI_d) << FI_u2;
                var ad      = (src[i] << cyc) >> cyc;

                if (wclp != 0) src[i] = int.MaxValue;
                else src[i] = ad + counter;
            }
        }


        public void UnfoldingCorrection(int[] src)
        {
            int Line = 16;
            //8平均
            // tbbttbbttbbttbbt

            if (param.Unfolding < 0) return;

            var FI = 11 - param.Unfolding;
            var Cyc = 12 + param.Unfolding;

            var FI_u = 9;
            var FI_d = 9 + Cyc;
            var FI_u2 = Cyc - 2;
            var cyc = 9 + FI;

            int[] u = new int[Width];
            int[] d = new int[Width];
            for(int y=0;y<Line;y++)
            {
                var p = y * Width;

                if (((y + 1) / 2)%2 == 0) for (int i = 0; i < Width; i++) u[i] += ((src[i + p] << cyc) >> cyc);
                else for (int i = 0; i < Width; i++) d[i] += ((src[i + p] << cyc) >> cyc);
            }

            for(int y= Line; y < Height;y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var i = x + Width * y;

                    var wclp = (src[i] & 0b00000000_10000000_00000000_00000000);
                    var counter = ((src[i] << FI_u) >> FI_d) << FI_u2;
                    var ad = (src[i] << cyc) >> cyc;

                    var col = (((y + 1) / 2) % 2 == 0)
                        ? ((src[i] << FI_u) >> FI_d) * u[x] / 8   
                        : ((src[i] << FI_u) >> FI_d) * d[x] / 8;

                    if (wclp != 0) src[i] = int.MaxValue;
                    else src[i] = ad + counter + col;

                }
            }
        }
    }


    public class DSPThroughState : DSPProcess
    {
        //public DarkSubEnum DarkSubSelect { get=> DarkSubEnum.Off }
        public override SoftHOBEnum SoftHOBSelect { get => SoftHOBEnum.Off; }
        public override SortEnum SortSelect { get => SortEnum.Off; }

        public override void Demosaic(int w, int h, ushort[] img16uc1, ushort[] img16uc3, byte[] img8uc3, float[] m, ColorConversionCodes bayerpattern)
        {
            using (Mat mat16uc1 = new Mat(h, w, MatType.CV_16UC1, img16uc1))
            using (Mat mat16uc3 = new Mat(h, w, MatType.CV_16UC3, img16uc3))
            using (Mat mat8uc3 = new Mat(h, w, MatType.CV_8UC3, img8uc3))
            using (Mat matrix = new Mat(3, 3, MatType.CV_32FC1, m))
            {
                Cv2.Merge(new Mat[] { mat16uc1, mat16uc1, mat16uc1 }, mat16uc3);
                //mat16uc3.ConvertTo(mat8uc3, MatType.CV_8UC3);
            }
        }

    }
    public class DSPOffState : DSPProcess
    {
    }
    public class DSPOn_monoState : DSPProcess
    {
        public override void Demosaic(int w, int h, ushort[] img16uc1, ushort[] img16uc3, byte[] img8uc3, float[] m, ColorConversionCodes bayerpattern)
        {
            using (Mat mat16uc1 = new Mat(h, w, MatType.CV_16UC1, img16uc1))
            using (Mat mat16uc3 = new Mat(h, w, MatType.CV_16UC3, img16uc3))
            using (Mat mat8uc3 = new Mat(h, w, MatType.CV_8UC3, img8uc3))
            using (Mat matrix = new Mat(3, 3, MatType.CV_32FC1, m))
            {
                switch (bayerpattern)
                {
                    case ColorConversionCodes.BayerBG2BGR:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerBG2BGR);
                        break;
                    case ColorConversionCodes.BayerGB2BGR:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerGB2BGR);
                        break;
                    case ColorConversionCodes.BayerGR2BGR:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerGR2BGR);
                        break;
                    case ColorConversionCodes.BayerRG2BGR:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerRG2BGR);
                        break;
                    default:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerRG2BGR);
                        break;
                }

                Cv2.Transform(mat16uc3, mat16uc3, matrix);
                Cv2.CvtColor(mat16uc3, mat16uc1, OpenCvSharp.ColorConversionCodes.BGR2GRAY);
                Cv2.Merge(new Mat[] { mat16uc1, mat16uc1, mat16uc1 }, mat16uc3);
                //mat16uc3.ConvertTo(mat8uc3, MatType.CV_8UC3);
            }
        }

    }
    public class DSPOn_colorState : DSPProcess
    {
        public override void Demosaic(int w, int h, ushort[] img16uc1, ushort[] img16uc3, byte[] img8uc3, float[] m, ColorConversionCodes bayerpattern)
        {
            using (Mat mat16uc1 = new Mat(h, w, MatType.CV_16UC1, img16uc1))
            using (Mat mat16uc3 = new Mat(h, w, MatType.CV_16UC3, img16uc3))
            using (Mat mat8uc3 = new Mat(h, w, MatType.CV_8UC3, img8uc3))
            using (Mat matrix = new Mat(3, 3, MatType.CV_32FC1, m))
            {
                switch (bayerpattern)
                {
                    case ColorConversionCodes.BayerBG2BGR:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerBG2BGR);
                        break;
                    case ColorConversionCodes.BayerGB2BGR:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerGB2BGR);
                        break;
                    case ColorConversionCodes.BayerGR2BGR:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerGR2BGR);
                        break;
                    case ColorConversionCodes.BayerRG2BGR:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerRG2BGR);
                        break;
                    default:
                        Cv2.CvtColor(mat16uc1, mat16uc3, OpenCvSharp.ColorConversionCodes.BayerRG2BGR);
                        break;
                }

                Cv2.Transform(mat16uc3, mat16uc3, matrix);

                //mat16uc3.ConvertTo(mat8uc3, MatType.CV_8UC3);
            }
        }

    }
    public class DSPQuadrantState : DSPProcess
    {
        public override void Demosaic(int w, int h, ushort[] img16uc1, ushort[] img16uc3, byte[] img8uc3, float[] m, ColorConversionCodes bayerpattern)
        {
            using (Mat mat16uc1 = new Mat(h, w, MatType.CV_16UC1, img16uc1))
            using (Mat mat16uc3 = new Mat(h, w, MatType.CV_16UC3, img16uc3))
            using (Mat mat8uc3 = new Mat(h, w, MatType.CV_8UC3, img8uc3))
            using (Mat matrix = new Mat(3, 3, MatType.CV_32FC1, m))
            {
                Quadrant(img16uc1, img16uc3, w, h);
                //mat16uc3.ConvertTo(mat8uc3, MatType.CV_8UC3);
            }
        }
        void Quadrant(ushort[] src, ushort[] dst, int w, int h)
        {
            int qCount1 = 0;
            int qCount2 = (w / 2) * 3;
            int qCount3 = (w * (h / 2)) * 3;
            int qCount4 = (w * (h / 2) + w / 2) * 3;

            for (int y = 0; y < h; y += 2)
            {
                for (int x = 0; x < w; x += 2)
                {
                    int i = x + y * w;

                    dst[qCount1++] = src[i];
                    dst[qCount1++] = src[i];
                    dst[qCount1++] = src[i];

                    dst[qCount2++] = src[i + 1];
                    dst[qCount2++] = src[i + 1];
                    dst[qCount2++] = src[i + 1];

                    dst[qCount3++] = src[i + w];
                    dst[qCount3++] = src[i + w];
                    dst[qCount3++] = src[i + w];

                    dst[qCount4++] = src[i + w + 1];
                    dst[qCount4++] = src[i + w + 1];
                    dst[qCount4++] = src[i + w + 1];
                }
                int inc = (w / 2) * 3;
                qCount1 += inc;
                qCount2 += inc;
                qCount3 += inc;
                qCount4 += inc;
            }
        }
    }
    public class DSPQuadColorState : DSPProcess
    {
        public override void Demosaic(int w, int h, ushort[] img16uc1, ushort[] img16uc3, byte[] img8uc3, float[] m, ColorConversionCodes bayerpattern)
        {
            using (Mat mat16uc1 = new Mat(h, w, MatType.CV_16UC1, img16uc1))
            using (Mat mat16uc3 = new Mat(h, w, MatType.CV_16UC3, img16uc3))
            using (Mat mat8uc3 = new Mat(h, w, MatType.CV_8UC3, img8uc3))
            using (Mat matrix = new Mat(3, 3, MatType.CV_32FC1, m))
            {
                switch (bayerpattern)
                {
                    case ColorConversionCodes.BayerBG2BGR:
                        QuadColorBG(img16uc1, img16uc3, w, h);
                        break;
                    case ColorConversionCodes.BayerGB2BGR:
                        QuadColorGB(img16uc1, img16uc3, w, h);
                        break;
                    case ColorConversionCodes.BayerGR2BGR:
                        QuadColorGR(img16uc1, img16uc3, w, h);
                        break;
                    case ColorConversionCodes.BayerRG2BGR:
                        QuadColorRG(img16uc1, img16uc3, w, h);
                        break;
                    default:
                        QuadColorRG(img16uc1, img16uc3, w, h);
                        break;
                }
                //mat16uc3.ConvertTo(mat8uc3, MatType.CV_8UC3);
            }
        }
        void QuadColorBG(ushort[] src, ushort[] dst, int w, int h)
        {
            int qCount1 = 0;
            int qCount2 = (w / 2) * 3;
            int qCount3 = (w * (h / 2)) * 3;
            int qCount4 = (w * (h / 2) + w / 2) * 3;

            for (int y = 0; y < h; y += 2)
            {
                for (int x = 0; x < w; x += 2)
                {
                    int i = x + y * w;

                    dst[qCount1++] = 0;//src[i];
                    dst[qCount1++] = 0;//src[i];
                    dst[qCount1++] = src[i];

                    dst[qCount2++] = 0;//src[i + 1];
                    dst[qCount2++] = src[i + 1];
                    dst[qCount2++] = 0;//src[i + 1];

                    dst[qCount3++] = 0;//src[i + w];
                    dst[qCount3++] = src[i + w];
                    dst[qCount3++] = 0;//src[i + w];

                    dst[qCount4++] = src[i + w + 1];
                    dst[qCount4++] = 0;//src[i + w + 1];
                    dst[qCount4++] = 0;//src[i + w + 1];
                }
                int inc = (w / 2) * 3;
                qCount1 += inc;
                qCount2 += inc;
                qCount3 += inc;
                qCount4 += inc;
            }
        }
        void QuadColorGB(ushort[] src, ushort[] dst, int w, int h)
        {
            int qCount1 = 0;
            int qCount2 = (w / 2) * 3;
            int qCount3 = (w * (h / 2)) * 3;
            int qCount4 = (w * (h / 2) + w / 2) * 3;

            for (int y = 0; y < h; y += 2)
            {
                for (int x = 0; x < w; x += 2)
                {
                    int i = x + y * w;

                    dst[qCount1++] = 0;//src[i];
                    dst[qCount1++] = src[i];
                    dst[qCount1++] = 0;//src[i];

                    dst[qCount2++] = 0;//src[i + 1];
                    dst[qCount2++] = 0;//src[i + 1];
                    dst[qCount2++] = src[i + 1];

                    dst[qCount3++] = src[i + w];
                    dst[qCount3++] = 0;//src[i + w];
                    dst[qCount3++] = 0;//src[i + w];

                    dst[qCount4++] = 0;//src[i + w + 1];
                    dst[qCount4++] = src[i + w + 1];
                    dst[qCount4++] = 0;//src[i + w + 1];
                }
                int inc = (w / 2) * 3;
                qCount1 += inc;
                qCount2 += inc;
                qCount3 += inc;
                qCount4 += inc;
            }
        }
        void QuadColorRG(ushort[] src, ushort[] dst, int w, int h)
        {
            int qCount1 = 0;
            int qCount2 = (w / 2) * 3;
            int qCount3 = (w * (h / 2)) * 3;
            int qCount4 = (w * (h / 2) + w / 2) * 3;

            for (int y = 0; y < h; y += 2)
            {
                for (int x = 0; x < w; x += 2)
                {
                    int i = x + y * w;

                    dst[qCount1++] = src[i];
                    dst[qCount1++] = 0;//src[i];
                    dst[qCount1++] = 0;//src[i];

                    dst[qCount2++] = 0;//src[i + 1];
                    dst[qCount2++] = src[i + 1];
                    dst[qCount2++] = 0;//src[i + 1];

                    dst[qCount3++] = 0;//src[i + w];
                    dst[qCount3++] = src[i + w];
                    dst[qCount3++] = 0;//src[i + w];

                    dst[qCount4++] = 0;//src[i + w + 1];
                    dst[qCount4++] = 0;//src[i + w + 1];
                    dst[qCount4++] = src[i + w + 1];
                }
                int inc = (w / 2) * 3;
                qCount1 += inc;
                qCount2 += inc;
                qCount3 += inc;
                qCount4 += inc;
            }
        }
        void QuadColorGR(ushort[] src, ushort[] dst, int w, int h)
        {
            int qCount1 = 0;
            int qCount2 = (w / 2) * 3;
            int qCount3 = (w * (h / 2)) * 3;
            int qCount4 = (w * (h / 2) + w / 2) * 3;

            for (int y = 0; y < h; y += 2)
            {
                for (int x = 0; x < w; x += 2)
                {
                    int i = x + y * w;

                    dst[qCount1++] = 0;//src[i];
                    dst[qCount1++] = src[i];
                    dst[qCount1++] = 0;//src[i];

                    dst[qCount2++] = src[i + 1];
                    dst[qCount2++] = 0;//src[i + 1];
                    dst[qCount2++] = 0;//src[i + 1];

                    dst[qCount3++] = 0;//src[i + w];
                    dst[qCount3++] = 0;//src[i + w];
                    dst[qCount3++] = src[i + w];

                    dst[qCount4++] = 0;//src[i + w + 1];
                    dst[qCount4++] = src[i + w + 1];
                    dst[qCount4++] = 0;//src[i + w + 1];
                }
                int inc = (w / 2) * 3;
                qCount1 += inc;
                qCount2 += inc;
                qCount3 += inc;
                qCount4 += inc;
            }
        }
    }
    public class DSPBinningState : DSPProcess
    {
        //public DarkSubEnum DarkSubSelect { get=> DarkSubEnum.Off }
        //public new SoftHOBEnum SoftHOBSelect { get => SoftHOBEnum.Off; }
        public override SortEnum SortSelect { get => SortEnum.Off; }

        public override void Demosaic(int w, int h, ushort[] img16uc1, ushort[] img16uc3, byte[] img8uc3, float[] m, ColorConversionCodes bayerpattern)
        {
            using (Mat mat16uc1 = new Mat(h, w, MatType.CV_16UC1, img16uc1))
            using (Mat mat16uc3 = new Mat(h, w, MatType.CV_16UC3, img16uc3))
            using (Mat mat8uc3 = new Mat(h, w, MatType.CV_8UC3, img8uc3))
            using (Mat matrix = new Mat(3, 3, MatType.CV_32FC1, m))
            {
                Binning(img16uc1, img16uc3, w, h);
                //mat16uc3.ConvertTo(mat8uc3, MatType.CV_8UC3);
            }



        }
        //Colorでは横拡大必要
        void Binning(ushort[] src, ushort[] dst, int w, int h)
        {
            int qCount1 = 0;
            int qCount2 = w * 1 * 3;
            int qCount3 = w * 2 * 3;
            int qCount4 = w * 3 * 3;
            int inc = w * 3 * 3;

            for (int y = 0; y < h / 4; y += 1)
            {
                if (y % 2 == 0)
                {
                    for (int x = 0; x < w; x += 1)
                    {
                        int i = x + y * 2 * w;

                        dst[qCount1++] = src[i + w];
                        dst[qCount1++] = src[i + w];
                        dst[qCount1++] = src[i + w];

                        dst[qCount2++] = src[i + w];
                        dst[qCount2++] = src[i + w];
                        dst[qCount2++] = src[i + w];

                        dst[qCount3++] = src[i];
                        dst[qCount3++] = src[i];
                        dst[qCount3++] = src[i];

                        dst[qCount4++] = src[i];
                        dst[qCount4++] = src[i];
                        dst[qCount4++] = src[i];
                    }

                    qCount1 += inc;
                    qCount2 += inc;
                    qCount3 += inc;
                    qCount4 += inc;
                }
                else
                {
                    for (int x = 0; x < w; x += 1)
                    {
                        int i = x + y * 2 * w;

                        dst[qCount1++] = src[i];
                        dst[qCount1++] = src[i];
                        dst[qCount1++] = src[i];

                        dst[qCount2++] = src[i];
                        dst[qCount2++] = src[i];
                        dst[qCount2++] = src[i];

                        dst[qCount3++] = src[i + w];
                        dst[qCount3++] = src[i + w];
                        dst[qCount3++] = src[i + w];

                        dst[qCount4++] = src[i + w];
                        dst[qCount4++] = src[i + w];
                        dst[qCount4++] = src[i + w];
                    }
                    qCount1 += inc;
                    qCount2 += inc;
                    qCount3 += inc;
                    qCount4 += inc;
                }
            }
        }
    }

}
