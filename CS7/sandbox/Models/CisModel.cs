using BTCV.Models.DSP;
using BTUSB.BT200C;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using BTUSB.Yamls;
using System.Collections.ObjectModel;

namespace BTCV.Models
{

    //var dddd = yaml["dsp"].Serialize();
    //param = yaml["test"].Deserialize<DSPParam>();
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CISRegs : BindableBase
    {
        Dictionary<string, string> _a;
        public Dictionary<string, string> a { get => _a; set { } }
        public IReadOnlyDictionary<string, string> v { get => _a; set { } }
        public string ReadOnlyRegs { get => string.Join(",", a.Select(x => $"{x.Key} {x.Value}").ToArray()); }

        private bool visible = true;
        [DefaultValue(true)]
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public CISRegs()
        {
            _a = new Dictionary<string, string>()
            {
                ["1"] = "A",
                ["2"] = "B",
            };

            
        }


    }


    public abstract class CisModel : BindableBase, IDisposable
    {
        public CISRegs test { get; set; } = new CISRegs();

        /* コンストラクタ・初期化 ****************************** */

        public CisModel() { }

        public void InitImage(WriteableBitmap bitmap)
        {
            digitalprocess = new DigitalProcess(bitmap);
            Width = (int)digitalprocess.img.Width;
            Height = (int)digitalprocess.img.Height;
            Size = Width * Height;
            Canvas_Width = Width;
            Canvas_Height = Height;

            RaisePropertyChanged("img");
            //RaisePropertyChanged("ScalingMode");//img = nullの時設定できないので生成時にraise
        }

        protected virtual void Dispose(bool disposing)
        {
            _tokenSourceIO.Dispose();
            _tokenSourceDigi.Dispose();
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /* プロパティ・コマンド ****************************** */

        /*変数*/

        DigitalProcess digitalprocess;
        public DSPParam param = new DSPParam();

        /*画像*/

        [Browsable(false)]
        public WriteableBitmap img { get => digitalprocess?.img ?? null; }
        [Category("View")]
        public int Width { get => _Width; protected set => SetProperty(ref _Width, value); }
        private int _Width;
        [Category("View")]
        public int Height { get => _Height; protected set => SetProperty(ref _Height, value); }
        private int _Height;
        [Category("View")]
        public int Size { get => _Size; protected set => SetProperty(ref _Size, value); }
        private int _Size;
        [Category("View")]
        public int Canvas_Width { get => _Canvas_Width; protected set => SetProperty(ref _Canvas_Width, value); }
        private int _Canvas_Width;
        [Category("View")]
        public int Canvas_Height { get => _Canvas_Height; protected set => SetProperty(ref _Canvas_Height, value); }
        private int _Canvas_Height;

        [Category("Canvas"), Description("Quality: Fant > Linear, Unspecified = Linear")]
        public BitmapScalingMode ScalingMode { get => _ScalingMode; set => SetProperty(ref _ScalingMode, value); }
        private BitmapScalingMode _ScalingMode;

        /*速度*/

        [Category("View")]
        public int wait_io { get; set; }
        [Category("View")]
        public int wait_digi { get; set; }
        [Browsable(false)]
        public double fps_io { get; protected set; }
        [Browsable(false)]
        public double fps_digi { get; protected set; }

        /*状態*/

        public enum StatusEnum
        {
            Ready,
            Busy,
            Close,
            Run,
            Capture,
            Burst
        }
        [Category("View")]
        public StatusEnum Status { get => _Status; protected set => SetProperty(ref _Status, value); }
        protected StatusEnum _Status = StatusEnum.Close;

        public bool isRun { get => isRunIO & isRunDecorder; }

        private bool _isRunIO;
        public bool isRunIO { get => _tokenSourceIO?.Token?.CanBeCanceled ?? false; }
        public bool isRunDecorder { get => _tokenSourceDigi?.Token?.CanBeCanceled ?? false; }

        public bool isCapture { get; }


        /* -------------------- */


        public bool Reset()
        {
            try
            {
                /*FPGA初期化*/
                _Reset();

                /*レジスタ標準設定*/
                _ResetReg();
                _ResetTG();

                /*FPGA設定との前後注意*/
                //サイズ情報は後でないととれない
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ResetDSP();
                }), DispatcherPriority.Background, new object[] { });
                RaisePropertyChanged("img");
                Status = StatusEnum.Ready;
                return true;
            }
            catch (Exception e)
            {
                Status = StatusEnum.Close;
                throw new IOException(e.ToString());
            }
        }
        protected abstract bool _Reset();
        protected abstract bool _ResetReg();
        protected abstract bool _ResetTG();


        public bool WriteSetting(string path)
        {
            var buf = Status;
            Status = StatusEnum.Busy;
            var hoge = _WriteSetting(path);
            Status = buf;
            return hoge;
        }
        protected abstract bool _WriteSetting(string path);


        public bool Run()
        {
            /* 準備 */
            if (Status == StatusEnum.Ready)
            {
                Status = StatusEnum.Busy;
                Status = _Start() ? StatusEnum.Run : Status = StatusEnum.Close;
            }
            else
            {
                return false;
            }

            //じゅんび
            digitalprocess = new DigitalProcess(Width, Height, Canvas_Width, Canvas_Height);

            if (_tokenSourceIO == null) _tokenSourceIO = new CancellationTokenSource();
            var token_io = _tokenSourceIO.Token;

            img_uint2int = ("", new int[Size]);

            if (_tokenSourceDigi == null) _tokenSourceDigi = new CancellationTokenSource();
            var token_digi = _tokenSourceDigi.Token;

            DateTime DTio = DateTime.Now;
            DateTime DTdec = DateTime.Now;

            double GetFPS(ref DateTime v)
            {
                var hoge = DateTime.Now;
                var ts = hoge.Subtract(v);
                v = hoge;
                return 1000 / ts.Milliseconds;
            }

            int[] src = new int[Size];

            /* IO2Buf and Buf2WriteableBitmap */
            var t1 = Task.Factory.StartNew(() =>
            {
                for (;;)
                {
                    if (token_io.IsCancellationRequested) return;
                    Thread.Sleep(wait_io);
                    A();
                    fps_io = GetFPS(ref DTio);
                }
            }, token_io, TaskCreationOptions.LongRunning, TaskScheduler.Default).ContinueWith(t =>
            {
                //あとしまつ
                _tokenSourceIO.Dispose();
                _tokenSourceIO = null;
                _Stop();
            });
            var t2 = Task.Factory.StartNew(() =>
            {
                string flagbuf = "";
                for (;;)
                {
                    if (token_digi.IsCancellationRequested) return;
                    Thread.Sleep(wait_digi);
                    B(src, flagbuf);
                    fps_digi = GetFPS(ref DTdec);
                    RaisePropertyChanged("img");
                }
            }, token_digi, TaskCreationOptions.LongRunning, TaskScheduler.Default)
            .ContinueWith(t =>
            {
                //あとしまつ
                _tokenSourceDigi.Dispose();
                _tokenSourceDigi = null;
                src = null;
            });

            return true;
        }
        public bool Stop()
        {
            if (Status == StatusEnum.Run)
            {
                _tokenSourceIO?.Cancel();
                _tokenSourceDigi?.Cancel();

                Status = StatusEnum.Ready;
                return true;
            }

            return false;
        }

        protected abstract bool _Start();  //初期化
        protected abstract bool io_grab();    //1fの読み出し動作
        protected abstract void io_transfer(int[] dst);

        protected abstract bool _Stop();





        /* -------------------- */

        // CapStart -> count set
        //          -> _CaptureStart(rec)

        // Capture  ->


        protected CaptureEnum CapMode = CaptureEnum.BurstBMP;
        [Browsable(false)]
        public int CapCount { get => _CapCount; protected set => SetProperty(ref _CapCount, value); }
        private int _CapCount = 0;

        /*!!!*/
        CaptureParam cappara = new CaptureParam();
        public bool CaptureStart(string path, int n, CaptureEnum mode)
        {
            CapMode = mode;

            switch (mode)
            {
                case CaptureEnum.BurstBMP:
                    Status = StatusEnum.Burst;
                    cappara.AddBMP(n, path);
                    CapCount = 0;
                    break;
                case CaptureEnum.BurstRaw:
                    Status = StatusEnum.Burst;
                    cappara.Add_raw(n, path);
                    CapCount = 0;
                    break;
                case CaptureEnum.RecRaw:
                    Stop();
                    Status = StatusEnum.Capture;
                    if (_CaptureStart(n))
                    {
                        cappara.Add_raw(n, path);
                        CapCount = 0;
                    }
                    break;
                case CaptureEnum.RecRawAveraging:
                    Stop();
                    Status = StatusEnum.Capture;
                    if (_CaptureStart(n))
                    {
                        cappara.sgl = new int[Size];
                        cappara.ave = new float[Size];
                        cappara.dev = new float[Size];
                        cappara.Add_ave(n, path);
                        CapCount = 0;
                    }
                    break;
                default:
                    return false;

            }

            return true;
        }
        public bool Capture(int count, bool averaging)
        {
            /* 準備 */
            //if (Status == StatusEnum.Capture) Status = StatusEnum.Busy;
            //else throw new IOException("Capture Err");
            //string flagbuf = "";

            /*画像の順次取り込み*/
            var t = Task.Run(() =>
            {
                int[] buf = new int[Size];
                while (_Capture())
                {

                    //flagbuf = imgbufflag;
                    //lock (img_uint2int)
                    //{
                    //    decflag = true;
                    //    Buffer.BlockCopy(img_uint2int, 0, buf, 0, 4 * img_uint2int.Length);
                    //    decflag = false;
                    //}

                    //Dispatcher(buf);
                }

                //平均データの出力, bufはなんでもいい（上書きされる
                if (CapMode == CaptureEnum.RecRawAveraging)
                    //Dispatcher(null);


                //可否判定（decored内でやらないと破綻するね
                Status = StatusEnum.Ready;
            });

            //Status = StatusEnum.Ready;
            /*!!!*/

            return true;
        }


        protected abstract bool _CaptureStart(int n);
        protected abstract bool _Capture();

        /* -------------------- */

        [Category("Register")]
        public int Iris { get => _Iris; set => SetProperty(ref _Iris, value, () => _SetIris(value)); }
        private int _Iris = 0;
        protected virtual void _SetIris(int value) { }

        [Category("Register")]
        public List<string> GainList { get => _GainList; protected set => SetProperty(ref _GainList, value); }
        private List<string> _GainList;

        [Category("Register")]
        public string GainSelect { get => _GainSelect; set => SetProperty(ref _GainSelect, value,()=> _SetGain(value)); }
        private string _GainSelect = "";
        protected virtual void _SetGain(string s) { }


        [Category("View")]
        public double Scale { get => _Scale; set => SetProperty(ref _Scale, value); }
        private double _Scale = 1;

        /* -------------------- */


        #region LUT
        public enum LUTEnum
        {
            off = 0,
            gamma0_45_bit16,
            gamma2_20_bit16,
            gamma2_50_bit16,
            gamma5_00_bit16,
            gamma2_20_bit12,
            gamma2_20_bit8,
            custom
        }


        private LUTEnum _LUTSelect = LUTEnum.off;
        [Category("DigitalColor"), Description("HDR")]
        public LUTEnum LUTSelect
        {
            get
            { return _LUTSelect; }
            set
            {
                if (_LUTSelect == value)
                    return;
                _LUTSelect = value;

                switch (value)
                {
                    case LUTEnum.gamma5_00_bit16:
                        param.lut = makelut(65535, 1.0 / 5.0);
                        break;
                    case LUTEnum.gamma2_50_bit16:
                        param.lut = makelut(65535, 1.0 / 2.5);
                        break;
                    case LUTEnum.gamma2_20_bit16:
                        param.lut = makelut(65535, 1.0 / 2.2);
                        break;
                    case LUTEnum.gamma0_45_bit16:
                        param.lut = makelut(65535, 1.0 / 0.45);
                        break;
                    case LUTEnum.gamma2_20_bit12:
                        param.lut = makelut(4095, 1.0 / 2.2);
                        break;
                    case LUTEnum.gamma2_20_bit8:
                        param.lut = makelut(255, 1.0 / 2.2);
                        break;
                    case LUTEnum.off:
                    case LUTEnum.custom:
                    default:
                        param.lut = null;
                        break;
                }
                RaisePropertyChanged();

                ushort[] makelut(int max, double gamma)
                {
                    var buf = new ushort[ushort.MaxValue + 1];

                    for (int i = 0; i < max; i++)
                    {
                        buf[i] = (ushort)(255 * Math.Pow((double)i / max, gamma));
                    }
                    for (int i = max; i < buf.Length; i++)
                    {
                        buf[i] = 255;
                    }
                    return buf;
                }
            }
        }

        #endregion




        [Browsable(false)]
        public int[] SamplingValue { get; set; } = new int[4];

        /*落書き要素*/



        [Category("Sampling")]
        public List<string> Shape { get => _Shape; protected set => SetProperty(ref _Shape, value); }
        private List<string> _Shape;
        private void SetShape()
        {
            if (SamplingMark == true)
            {
                var i = new List<string>();
                i.Add($"Rect,{Sampling_X - Canvas_X},{Sampling_Y - Canvas_Y},{Sampling_Width},{Sampling_Height}");
                //i.Add($"Circle,{Sampling_X - Canvas_X},{Sampling_Y - Canvas_Y},{Sampling_Width / 2}");
                i.Add($"Text,{Sampling_X - Canvas_X},{Sampling_Y - Canvas_Y},18,SamplingArea");
                Shape = i;
            }
            else
            {
                Shape = null;
            }
        }



        /* レジスタ */


        /* コマンド ****************************** */

        public virtual void WriteReg(int addr, int data0, int data1, int data2, int data3) { }
        public virtual void WriteReg(int addr, int data) { }
        public virtual uint ReadReg(int addr) => 0;

        /* メソッド ****************************** */

        public virtual string GetSettingStrings() => "";

        private void ResetDSP()
        {
            BTYaml yaml = App.Container.Resolve<BTYaml>("Config");

            //画像サイズ
            //FPGAから読み出しで上書きされるので
            //仮値
            Width = yaml["width"].Parse(Width);
            Height = yaml["height"].Parse(Height);

            //ウインドウサイズ
            Canvas_Width = yaml["canvas"]["width"].Parse(Width);
            Canvas_Height = yaml["canvas"]["height"].Parse(Height);
            Canvas_X = yaml["canvas"]["x"].Parse(Canvas_X);
            Canvas_Y = yaml["canvas"]["y"].Parse(Canvas_Y);

            //初期化
            digitalprocess = new DigitalProcess(Width, Height, Canvas_Width, Canvas_Height);

            //機能
            param.bitshift = yaml["dsp"]["bitshift"].Parse(bitshift);
            param.bitshiftsub = yaml["dsp"]["bitshiftsub"].Parse(bitshiftsub);
            Scale = yaml["dsp"]["scale"].Parse(Scale);

            param.DarkOffset = yaml["dsp"]["offset"].Parse(DarkOffset);
            param.DarkSubSelect = yaml["dsp"]["darksub"].Parse<DarkSubEnum>(DarkSubSelect);
            param.SoftHOBSelect = yaml["dsp"]["softhob"].Parse<SoftHOBEnum>(SoftHOBSelect);
            param.SortSelect = yaml["dsp"]["sort"].Parse<SortEnum>(SortSelect);
            param.DemosaicSelect = yaml["dsp"]["demosaic"].Parse<DemosaicEnum>(DemosaicSelect);

            //色

            param.RGain = yaml["dsp"]["rgain"].Parse(RGain);
            param.GGain = yaml["dsp"]["ggain"].Parse(GGain);
            param.BGain = yaml["dsp"]["bgain"].Parse(BGain);

            //matrix

            param.bayerpattern = yaml["dsp"]["bayerpattern"].Parse<ColorConversionCodes>(bayerpattern);


            //スレッド違い？エラーはくのでraiseしない
            _ScalingMode = yaml["dsp"]["scalingmode"].Parse<BitmapScalingMode>(_ScalingMode);

            param.Unfolding = yaml["dsp"]["unfolding"].Parse(Unfolding);
            //DarkSub

            imgdarksub = new int[Size];
            if (System.IO.File.Exists("dark.bin"))
            {
                byte[] buf = File.ReadAllBytes("dark.bin");
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        int i = x + y * Width;
                        imgdarksub[i] = (int)BitConverter.ToSingle(buf, i * 4);
                    }
                }
            }
        }

        protected bool WriteDSPFromYaml(string path)
        {
            var yaml = new BTYaml(path);
            //matrix

            //機能
            param.bitshift = yaml["dsp"]["bitshift"].Parse(bitshift);
            param.bitshiftsub = yaml["dsp"]["bitshiftsub"].Parse(bitshiftsub);
            param.Scale = yaml["dsp"]["scale"].Parse(Scale);

            param.DarkOffset = yaml["dsp"]["offset"].Parse(DarkOffset);
            param.DarkSubSelect = yaml["dsp"]["darksub"].Parse<DarkSubEnum>(DarkSubSelect);
            param.SoftHOBSelect = yaml["dsp"]["softhob"].Parse<SoftHOBEnum>(SoftHOBSelect);
            param.SortSelect = yaml["dsp"]["sort"].Parse<SortEnum>(SortSelect);
            param.DemosaicSelect = yaml["dsp"]["demosaic"].Parse<DemosaicEnum>(DemosaicSelect);

            //色

            param.RGain = yaml["dsp"]["rgain"].Parse(RGain);
            param.GGain = yaml["dsp"]["ggain"].Parse(GGain);
            param.BGain = yaml["dsp"]["bgain"].Parse(BGain);

            //matrix

            param.bayerpattern = yaml["dsp"]["bayerpattern"].Parse<ColorConversionCodes>(bayerpattern);


            //スレッド違い？エラーはくのでraiseしない
            _ScalingMode = yaml["dsp"]["scalingmode"].Parse<BitmapScalingMode>(_ScalingMode);

            param.Unfolding = yaml["dsp"]["unfolding"].Parse(Unfolding);

            return true;
        }

        /*************************/
        //非同期
        private CancellationTokenSource _tokenSourceIO;
        private CancellationTokenSource _tokenSourceDigi;
        private static object lockobj = new object();

        private (string name, int[] img) img_uint2int;
        int[] imgdarksub;
        protected bool decflag = false;


        private void A()
        {
            io_grab();
            lock (lockobj)
            {
                if (!decflag)
                {
                    io_transfer(img_uint2int.img);
                    img_uint2int.name = System.DateTime.Now.ToString("HHmmssfff");
                }
            }
            
        }
        private void B(int[] src, string flagbuf)
        {
            while (flagbuf == img_uint2int.name) continue;

            decflag = true;
            lock (lockobj)
            {
                flagbuf = img_uint2int.name;
                Buffer.BlockCopy(img_uint2int.img, 0, src, 0, 4 * img_uint2int.img.Length);
            }
            decflag = false;

            //src 32bit -> BurstRaw
            //          -> DarkSub
            //          -> HOB 
            //          -> Stagger
            //          -> Sampling
            //          -> BitShift and Offset
            //          -> Trim and Convert16bit
            //          -> Demosaic and Convert8bit
            //          -> ConvertWriteableBitmap
            //          -> BurstBMP
            CapCount = cappara.Count;

            digitalprocess.param = param.Clone();
            digitalprocess.Run(src, imgdarksub, out int[] buf, cappara);
            SamplingValue = buf;


            //if (!cappara.RemoveFirst())
            //{
            //    CapCount = -1;
            //    Status = Status == StatusEnum.Burst ? StatusEnum.Run
            //               : Status == StatusEnum.Capture ? StatusEnum.Ready
            //               : Status;
            //}
        }

    }
}


//一方向、更新どうすんの
//this.hogehoge2 = ReactiveProperty.FromObject(
//    hoge.hoge, x => x.hoge,
//    convert: x => x,
//    convertBack: x => x);