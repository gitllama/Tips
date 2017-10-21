using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

using BTUSB;
using BTUSB.BT200C;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using BTUSB.Yamls;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace BTCV.Models.BT200C
{
    public class BT200C60 : CisModel
    {
        /* コンストラクタ・初期化 ****************************** */

        public BT200C60() : base() { }

        /* プロパティ・コマンド ****************************** */

        /*本体*/

        protected USBInterface USBIF;

        //参照範囲の整理必要
        RegWritter rw = new RegWritter();

        protected int RX_BUF_SIZE { get; set; }
        protected int PIXEL_BYTE { get; set; } = 3;

        protected int RegisterSize { get; set; }
        protected uint[] img_io2uint;
        /*状態*/



        /*機能*/


        /* -------------------- */


        protected override bool _Reset()
        {
            if (Status == StatusEnum.Close)
            {
                USBIF = USBinterfaceExtensions200C.CreateUSB();

                if (!USBIF.Open()) throw new IOException("can't open device");              //オープン・初期化動作
                if (!USBIF.isUSB3()) System.Windows.MessageBox.Show("connected USB 2.0");   //USB3.0の確認             
                if (!USBIF.Reset()) throw new IOException("can't reset device");            //USBのリセット動作

                rw.data = USBIF.CISReg;　//<-!!!意味がないというかただの配列サイズ初期化
                ResetGain(); //ここでいい？
                ResetIris();

                /****/

                //サイズの初期化

                USBIFSTATUS sTATUS;
                uint ysize = 0u;
                uint xsize = 0u;

                sTATUS = USBIF.ReadReg(10u, ref xsize, 0u);
                Width = (int)(sTATUS != USBIFSTATUS.E_OK ? 2256u : xsize * 2u);
                sTATUS = USBIF.ReadReg(3u, ref ysize, 0u);
                Height = (int)(sTATUS != USBIFSTATUS.E_OK ? 1178u : ysize * 2u);

                Size = Width * Height;
                RX_BUF_SIZE = btc_GetImageSize();

                img_io2uint = new uint[RX_BUF_SIZE];
            }
            else if (Status == StatusEnum.Run)
            {
                Stop();
                if (!USBIF.Open()) return false;
            }
            else
            {
                //初期化動作
                if (!USBIF.Open()) return false;
            }


            return true;
        }

        protected override bool _ResetReg() => USBIF.ResetReg(App.Container.Resolve<string>("ConfigFilePath"));
        protected override bool _ResetTG() => USBIF.ResetTG(App.Container.Resolve<string>("ConfigFilePath"));
        protected virtual int btc_GetImageSize() => Width * Height * PIXEL_BYTE / 4;

        protected override bool _WriteSetting(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".CMOS":
                    return USBIF.WriteRegFromCMOS(path);
                case ".TG":
                    return USBIF.WriteTgFromTG(path);
                case ".yaml":
                case ".yml":
                case ".conf":
                    if (!USBIF.WriteRegFromYaml(path)) return false;
                    if (!USBIF.WriteTgFromYaml(path)) return false;
                    if (!WriteDSPFromYaml(path)) return false;
                    return true;
                default:
                    return false;
            }
        }

        protected override bool _Start()
        {
            USBIFSTATUS sTATUS = USBIF.SendCommand(0x07, new uint[] { 0u, (uint)RX_BUF_SIZE, 0 }, 3);
            if (sTATUS != USBIFSTATUS.E_OK) throw new IOException(sTATUS.ToString());
            if (!USBIF.Reset()) throw new IOException("can't reset device");

            var result = btc_SetStartDMA();

            return true;
        }
        protected override bool io_grab()
        {
            /*IO2uint*/
            while (btc_CapSet() != USBIFSTATUS.E_OK) USBIF.Open();
            return true;
        }
        protected override void io_transfer(int[] dst)
        {
            int c = 0;
            for (int y = 0; y < 1178; y += 2)
                for (int x = 0; x < 2256; x += 2)
                {
                    int h = x + y * 2256;

                    dst[h] = ((int)(img_io2uint[c] & 0xFFFFFF00)) >> 8;
                    dst[h + 1] = ((int)(((img_io2uint[c] & 0x000000FF) << 24) | ((img_io2uint[c + 1] & 0xFFFF0000) >> 8))) >> 8;
                    dst[h + 2256] = ((int)(((img_io2uint[c + 1] & 0x0000FFFF) << 16) | ((img_io2uint[c + 2] & 0xFF000000) >> 16))) >> 8;
                    dst[h + 1 + 2256] = ((int)(img_io2uint[c + 2] & 0x00FFFFFF) << 8) >> 8;
                    c += 3;
                }
        }


        protected override bool _Stop() => btc_SetStopDMA();

        protected virtual bool btc_SetStartDMA() => USBIF.DmaWriteReg(0, 1, 0) == USBIFSTATUS.E_OK ? true : false;
        protected virtual bool btc_SetStopDMA() => USBIF.DmaWriteReg(0, 0, 0) == USBIFSTATUS.E_OK ? true : false;

        /* -------------------- */

        //かぶってる
        protected int capnum = 0;
        protected DateTime captime;
        protected override bool _CaptureStart(int num)
        {
            capnum = num;
            captime = DateTime.Now;

            uint[] array = new uint[] { 1u, (uint)RX_BUF_SIZE, (uint)num };

            USBIFSTATUS sTATUS = USBIF.SendCommand(7u, array, array.Length);
            if (sTATUS != USBIFSTATUS.E_OK) throw new IOException(sTATUS.ToString());

            if (!USBIF.Reset()) throw new IOException("can't reset device");

            USBIF.WriteReg(0x8000, 1u, 0u); //0x8000 = 32768u
            while ((array[0] & 1u) != 0u)
            {
                USBIF.ReadReg(0x8000, ref array[0], 0u); //0x8000 = 32768u
                Thread.Sleep(100);
            }

            return true;
        }
        protected override bool _Capture()
        {
            if (capnum <= 0) return false;
            capnum--;

            USBIFSTATUS sTATUS = btc_CapSet();

            if (sTATUS != USBIFSTATUS.E_OK) throw new IOException($"Capture Err {sTATUS}");

            if (!decflag)
            {
                //lock (lockobj) decodeIO();
                //imgbufflag = System.DateTime.Now.ToString("HHmmssfff");
            }

            return true;
        }
        //protected override void _SaveRaw(string s) => img_uint2int.SaveRaw(savepath, s);

        /* -------------------- */



        protected override void _SetIris(int value)
        {
            if (value < 0)
            {
                //シャッタOFF
            }
            else
            {
                var i = value;
                i = i < 0 ? 0 : i;

                //var reg = ((USBWin7)USBIF).GetCISReg();
                //((USBWin7)USBIF).WriteCISReg(36, (byte)((i & 0xFF00) >> 8), (byte)(i & 0xFF), reg[38], reg[39]);

                rw.Set("iris", (uint)value);
            }
        }

        /*Gain*/



        public bool ResetGain()
        {
            BTYaml yaml = App.Container.Resolve<BTYaml>("Config");
            GainList = new List<string>();
            foreach (var i in yaml["gain"].Keys)
            {
                /*かぶるとき修正*/
                rw.Add(i, ToData(yaml["gain"][i]));
                GainList.Add(i);
            }
            rw.WriteAction = (addr, data) => USBIF.WriteCISReg(addr, data);
            return true;

            Dictionary<byte, string> ToData(BTYaml v)
            {
                var hoge = new Dictionary<byte, string>();
                foreach (var i in v.Keys)
                {
                    var addr = Convert.ToByte(i, 16);
                    var data = v[i].Parse<string>();
                    hoge.Add(addr, data);
                }
                return hoge;
            }

        }
        public bool ResetIris()
        {
            BTYaml yaml = App.Container.Resolve<BTYaml>("Config");

            var hoge = new Dictionary<byte, int[]>();
            foreach (var i in yaml["iris"].Keys)
            {
                var addr = Convert.ToByte(i, 16);
                var data = yaml["iris"][i].Values;
                hoge.Add(addr, data.ConvertAll(x => int.Parse(x)).ToArray());
            }
            rw.Add("iris", hoge);

            return true;
        }


        protected override void _SetGain(string s)
        {
            rw.Set(s);
            /*
            foreach (var i in GainSetList[s])
            {
                ((USBWin7)USBIF).WriteCISReg(i.Key,i.Value);
            }
            */
            //return 0;
        }

        /* -------------------- */

        protected virtual USBIFSTATUS btc_CapSet() => USBIF.CaptureImage(img_io2uint, img_io2uint.Length, 0u, 0);


        public override string GetSettingStrings()
        {
            var dst = $"{string.Join(",", USBIF.CISReg)}\r\n";

            foreach (var i in USBIF.CISTG)
            {
                dst += $"{i.Key}:{string.Join("-", i.Value)} ";
            }
            return dst;
        }


        public override void WriteReg(int addr, int data0, int data1, int data2, int data3)
        {
            USBIF.WriteCISReg((uint)addr, new byte[] { (byte)data0, (byte)data1, (byte)data2, (byte)data3 });
            //base.WriteReg(addr, data);
        }
        public override void WriteReg(int addr, int data)
        {
            USBIF.WriteCISReg((uint)addr, new byte[] { (byte)data });
            //base.WriteReg(addr, data);
        }
        public override uint ReadReg(int addr)
        {
            USBIF.ReadCISReg((uint)addr, out uint data);
            return data;
        }
    }
}
