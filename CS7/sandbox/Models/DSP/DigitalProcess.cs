using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BTCV.Models.DSP
{
    public enum CaptureEnum
    {
        BurstBMP = 0,
        BurstRaw = 1,
        RecRaw = 2,
        RecRawAveraging = 3
    }
    public enum DemosaicEnum
    {
        Through,
        Off,
        On_mono,
        On_color,
        Quadrant,
        QuadColor,
#if OpenCL
            OpenCL,
#endif
        Binning
    }
    public enum ColorConversionCodes
    {
        BayerGB2BGR = 0,
        BayerBG2BGR = 1,
        BayerRG2BGR = 2,
        BayerGR2BGR = 3,
    }
    public enum DarkSubEnum
    {
        Off = 0,
        On = 1
    }
    public enum SoftHOBEnum
    {
        Off = 0,
        Left = 1,
        Both = 2,
        Linear = 3,
        Test,
    }
    public enum SortEnum
    {
        Off = 0,
        StaggerL = 1,
        StaggerR = 2
    }

    public class DSPParam
    {
        public DSPParam Clone() => (DSPParam)MemberwiseClone();

        private int _Canvas_X = 0;
        private int _Canvas_Y = 0;

        private int Width { get; set; } = 1;
        private int Height { get; set; } = 1;
        private int Canvas_Width;
        private int Canvas_Height;

        /*Overall Process*/

        [Category("DigitalColor")]
        public DemosaicEnum DemosaicSelect { get => _DemosaicSelect; set => SetProperty(ref _DemosaicSelect, value); }
        private DemosaicEnum _DemosaicSelect = DemosaicEnum.On_color;

        /*PreProcess*/

        [Category("DigitalColor")]
        public DarkSubEnum DarkSubSelect { get => _DarkSubSelect; set => SetProperty(ref _DarkSubSelect, value); }
        private DarkSubEnum _DarkSubSelect = DarkSubEnum.Off;

        [Category("DigitalColor")]
        public SortEnum SortSelect { get => _SortSelect; set => SetProperty(ref _SortSelect, value); }
        private SortEnum _SortSelect = SortEnum.StaggerL;

        [Category("Digital")]
        public int bitshift { get => _bitshift; set => SetProperty(ref _bitshift, value); }
        private int _bitshift  = 0;

        [Category("Digital")]
        public int bitshiftsub { get => _bitshiftsub; set => SetProperty(ref _bitshiftsub, value); }
        private int _bitshiftsub  = -1;

        [Category("Digital")]
        public int DarkOffset { get => _DarkOffset; set => SetProperty(ref _DarkOffset, value); }
        private int _DarkOffset  = 0;

        [Category("DigitalColor")]
        public SoftHOBEnum SoftHOBSelect { get => _SoftHOBSelect; set => SetProperty(ref _SoftHOBSelect, value); }
        private SoftHOBEnum _SoftHOBSelect = SoftHOBEnum.Linear;

        [Category("Digital")]
        public int HOBLStart { get; private set; } = 12;
        [Category("Digital")]
        public int HOBLEnd { get; private set; } = 128 + 12;
        [Category("Digital")]
        public int HOBRStart { get; private set; } = 2116;
        [Category("Digital")]
        public int HOBREnd { get; private set; } = 2116 + 128;

        [Category("Sampling")]
        public int Sampling_X
        {
            get => _Sampling_X;
            set
            {
                if (_Sampling_X == value)
                    return;
                if (value < 0) return;
                if (value + Sampling_Width > Width) return;
                _Sampling_X = value;
                SetShape();
                RaisePropertyChanged();
            }
        }
        [Category("Sampling")]
        public int Sampling_Y
        {
            get => _Sampling_Y;
            set
            {
                if (_Sampling_Y == value)
                    return;
                if (value < 0) return;
                if (value + Sampling_Height > Height) return;
                _Sampling_Y = value;
                SetShape();
                RaisePropertyChanged();
            }
        }
        [Category("Sampling")]
        public int Sampling_Width
        {
            get
            { return _Sampling_Width; }
            set
            {
                if (_Sampling_Width == value)
                    return;
                if (value < 0) return;
                if (value + Sampling_X > Width) return;
                _Sampling_Width = value;
                SetShape();
                RaisePropertyChanged();
            }
        }
        [Category("Sampling")]
        public int Sampling_Height
        {
            get => _Sampling_Height;
            set
            {
                if (_Sampling_Height == value)
                    return;
                if (value < 0) return;
                if (value + Sampling_Y > Height) return;
                _Sampling_Height = value;
                SetShape();
                RaisePropertyChanged();
            }
        }
        [Category("Sampling")]
        public bool SamplingMark
        {
            get => _SamplingMark;
            set
            {
                if (_SamplingMark == value)
                    return;
                _SamplingMark = value;
                SetShape();
                RaisePropertyChanged();
            }
        }

        private int _Sampling_X  = 0;
        private int _Sampling_Y  = 0;
        private int _Sampling_Width = 0;
        private int _Sampling_Height = 0;
        private bool _SamplingMark = false;


        /*MainProcess*/
        [Category("DigitalColor")]
        public ColorConversionCodes bayerpattern { get => _bayerpattern; set => SetProperty(ref _bayerpattern, value); }

        private ColorConversionCodes _bayerpattern = ColorConversionCodes.BayerGB2BGR;

        private double RGain = 1;
        private double GGain = 1;
        private double BGain = 1;

        [Category("DigitalColor"), DisplayName("Gain RGain")]
        public double RGain { get => param.RGain; set => SetProperty(ref param.RGain, value); }
        [Category("DigitalColor"), DisplayName("Gain GGain")]
        public double GGain { get => param.GGain; set => SetProperty(ref param.GGain, value); }
        [Category("DigitalColor"), DisplayName("Gain BGain")]
        public double BGain { get => param.BGain; set => SetProperty(ref param.BGain, value); }

        [Category("DigitalColor")]
        public float[] Matrix { get => _Matrix; set => _Matrix = value; }
        private float[] _Matrix = new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f };

        private ushort[] lut;



        [Category("DigitalColor"), Description("off : <= 1")]
        public int MedianBlur
        {
            get
            { return param.MedianBlur; }
            set
            {
                if (param.MedianBlur == value)
                    return;
                if (value % 2 != 1) return;
                param.MedianBlur = value;
                RaisePropertyChanged();
            }
        }
        private int MedianBlur= 1;
        [Category("DigitalColor"), Description("off : <= 0")]
        public double GaussianBlur
        {
            get
            { return param.GaussianBlur; }
            set
            {
                if (param.GaussianBlur == value)
                    return;
                param.GaussianBlur = value;
                RaisePropertyChanged();
            }
        }
        private double GaussianBlur  = 0;



        
        public int Unfolding = -1;
        [Category("Digital")]
        public int Unfolding { get => param.Unfolding; set => SetProperty(ref param.Unfolding, value); }

        /*PostProcess*/


        [Category("View")]
        public int Canvas_X
        {
            get => _Canvas_X;
            set
            {
                if (_Canvas_X == value)
                    return;
                if (value < 0) return;
                if (value + Canvas_Width > Width) return;
                _Canvas_X = value;
                RaisePropertyChanged("Canvas_X");
            }
        }
        [Category("View")]
        public int Canvas_Y
        {
            get => _Canvas_Y;
            set
            {
                if (_Canvas_Y == value)
                    return;
                if (value < 0) return;
                if (value + Canvas_Height > Height) return;
                _Canvas_Y = value;
                RaisePropertyChanged();
            }
        }

    }

    public class CaptureParam
    {
        public float[] ave = null;
        public float[] dev = null;
        public int[] sgl = null;
        public float[] ave_HOB = null;
        public float[] dev_HOB = null;

        public string savepath;
        public int Num = 0;
        List<(string Key, bool raw1, bool raw2, bool bmp, bool add1, bool add2, bool result)> list 
            = new List<(string Key, bool raw1, bool raw2, bool bmp, bool add1, bool add2, bool result)>();

        public bool enable1 { get => list.Count > 0 ? list[0].raw1 : false; }
        public bool enable2 { get => list.Count > 0 ? list[0].raw2 : false; }
        public bool enableBMP { get => list.Count > 0 ? list[0].bmp : false; }
        public bool enableadd1 { get => list.Count > 0 ? list[0].add1 : false; }
        public bool enableadd2 { get => list.Count > 0 ? list[0].add2 : false; }
        public bool enableresult { get => list.Count > 0 ? list[0].result : false; }
        public string filename { get => list.Count > 0 ? String.Format(list[0].Key, DateTime.Now.ToString("HHmmss_fff")) : "err"; }

        public int Count { get => Num - list.Count; }

        public void Add_raw(int n, string s)
        {
            list.Clear();
            Num = n;
            for (int i = 0; i < n; i++)
            {
                list.Add((@"{0}", true, false, false, false, false, false));
            }
            savepath = s;
        }
        public void Add_hob(int n, string s)
        {
            list.Clear();
            Num = n;
            for (int i = 0; i < n; i++)
            {
                list.Add(($"{i:000}", false, true, false, false, false, false));
            }
            savepath = s;
        }
        public void Add_ave(int n, string s)
        {
            list.Clear();
            Num = n;
            list.Add(($"{0:000}", true, false, false, true, true, false));
            for (int i = 1; i < n; i++)
            {
                list.Add(($"{i:000}", false, false, false, true, true, false));
            }
            list.Add(($"{n:000}", false, false, false, false, false, true));
            savepath = s;
        }
        public void AddBMP(int n, string s)
        {
            list.Clear();
            Num = n;
            for (int i = 0; i < n; i++)
            {

                list.Add((@"{0}", false, false, true, false, false, false));
            }
            savepath = s;
        }


        public bool RemoveFirst()
        {
            if (list.Count <= 0)return false;
            list.RemoveAt(0);
            return true;
        }

        public void Add(int[] src)
        {
            for (int i = 0; i < ave.Length; i++)
            {
                ave[i] += src[i];
                dev[i] += (float)Math.Pow(src[i], 2); //int * intオーバーフローしがち
            }
        }
        public void GetResult()
        {
            for (int i = 0; i < ave.Length; i++)
            {
                double buf = ave[i] / Num;
                ave[i] = (float)buf;
                dev[i] = (float)Math.Sqrt((dev[i] / (Num - 1)) - (buf * buf));
                //n-1でないとNaNでるね
            }
        }

        public string GetStr()
        {
            return null;
        }
    }



    public static class DigitalProcessExtensions
    {
        public static void SaveBMP(this WriteableBitmap src, string path, string filename)
        {
            using (var r = new FileStream(Path.Combine(path, filename), FileMode.Create, FileAccess.Write))
            {
                var img = src;
                BitmapFrame bmpFrame = BitmapFrame.Create(img);
                PngBitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(bmpFrame);
                enc.Save(r);
            }
        }
    }

    public static class ImageFileStreamExtensions
    {
        public static void SaveRawFloat(this float[] src, string path, string filename)
        {
            using (var r = new FileStream(Path.Combine(path, filename), FileMode.Create, FileAccess.Write))
            {
                for (int n = 0; n < src.Length; n++)
                {
                    r.Write(BitConverter.GetBytes(src[n]), 0, sizeof(float));
                }
            }
        }
        public static void SaveRaw(this int[] src, string path, string filename)
        {
            using (var r = new FileStream(Path.Combine(path, filename), FileMode.Create, FileAccess.Write))
            {
                for (int n = 0; n < src.Length; n++)
                {
                    r.Write(BitConverter.GetBytes(src[n]), 0, sizeof(int));
                }
            }
        }

    }
}


//else if(CaptureNum != 0)
//{
//    /*結果の計算*/
//    using (var sw = new StreamWriter(Path.Combine(savepath, "result.txt"), true, Encoding.GetEncoding("shift_jis")))
//    {
//        sw.WriteLine("setting");

//        //reg,tg
//        sw.WriteLine(GetSettingStrings());

//        //degital
//        //DigitalProcess
//        sw.WriteLine(BTYaml.Serialize(DigitalProcess));

//        sw.WriteLine("result");

//        if (a != null)
//        {
//            double buf = 0;

//            sw.WriteLine($"a Count:{a.count}");

//            buf = 0;
//            foreach (var n in a.ave)
//                buf += n;
//            buf /= a.ave.Length;

//            sw.WriteLine($"a Ave:{buf}");

//            buf = 0;
//            foreach (var n in a.dev)
//                buf += n;
//            buf /= a.dev.Length;

//            sw.WriteLine($"a Dev:{buf}");
//        }

//        if (b != null)
//        {
//            double buf = 0;

//            sw.WriteLine($"a Count:{b.count}");

//            buf = 0;
//            foreach (var n in b.ave)
//                buf += n;
//            buf /= b.ave.Length;

//            sw.WriteLine($"b Ave:{buf}");

//            buf = 0;
//            foreach (var n in b.dev)
//                buf += n;
//            buf /= b.dev.Length;

//            sw.WriteLine($"b Dev:{buf} , {b.dev[500+500*Width]}");
//        }
//    }


//    /*後処理*/
//    //a = null;
//    //b = null;
//    CaptureNum = 0;
//    CapCount = 0;
//}