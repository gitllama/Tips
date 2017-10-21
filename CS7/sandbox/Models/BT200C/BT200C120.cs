using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

using BTUSB;
using System.Windows.Media.Imaging;

namespace BTCV.Models.BT200C
{
    public class BT200C120 : BT200C60
    {

        /* コンストラクタ・初期化 ****************************** */

        public BT200C120() : base() { }

        /* プロパティ・コマンド ****************************** */

        /*本体*/


        /*状態*/


        /*機能*/

        /* -------------------- */


        protected override bool _Reset()
        {
            base._Reset();

            var result = Check120fps();

            return true;
        }

        private bool Check120fps()
        {
            uint[] data = new uint[1];
            uint hoge = 0;

            if (USBIF.ReceiveCommand(0x0B, 0, (uint)data.Length, data) != USBIFSTATUS.E_OK)
            {
                throw new PlatformNotSupportedException("120fps Open Err");
                //return false;
            }

            /*!!!*/
            if((data[0] & 0x20) != 0)
            {
                //WDR = on
            }



            if (USBIF.ReadReg(0x6C, ref hoge, 0) != USBIFSTATUS.E_OK)
            {
                throw new PlatformNotSupportedException("120fps Open Err");
                //return false;
            }
            if (hoge != 0 && (data[0] & 0x1F) == 0x12)
            {
                //120 On
            }
            return true;
        }

        /* -------------------- */

        protected override int btc_GetImageSize() => Width * Height  / 4 * 7 /12 * 4;
        protected override bool btc_SetStartDMA() => USBIF.DmaWriteReg(0, 1, 2) == USBIFSTATUS.E_OK ? true : false;
        protected override bool btc_SetStopDMA() => USBIF.DmaWriteReg(0, 0, 2) == USBIFSTATUS.E_OK ? true : false;

        protected override USBIFSTATUS btc_CapSet() => USBIF.CaptureImage(img_io2uint, img_io2uint.Length, 0u, 1);

        protected override void io_transfer(int[] dst)
        {
            Action<int, int> func = (a, b) =>
            {
                var oddLine = a;
                var evnLine = a + Width;

                dst[oddLine + 0] = ((int)(
                                                    ((img_io2uint[b + 0] & 0xFFFFFC000) >> 0)
                                                )) >> 14;
                dst[oddLine + 1] = ((int)(
                                                    ((img_io2uint[b + 0] & 0x00003FFF) << 18) |
                                                    ((img_io2uint[b + 1] & 0xF0000000) >> 14)
                                                )) >> 14;
                dst[evnLine + 0] = ((int)(
                                                    ((img_io2uint[b + 1] & 0x0FFFFC00) << 4)
                                                )) >> 14;
                dst[evnLine + 1] = ((int)(
                                                    ((img_io2uint[b + 1] & 0x00003FFF) << 22) |
                                                    ((img_io2uint[b + 2] & 0xFF000000) >> 10)
                                                )) >> 14;

                dst[oddLine + 2] = ((int)(
                                                    ((img_io2uint[b + 2] & 0x00FFFFC0) << 8)
                                                )) >> 14;
                dst[oddLine + 3] = ((int)(
                                                    ((img_io2uint[b + 2] & 0x0000003F) << 26) |
                                                    ((img_io2uint[b + 3] & 0xFFF00000) >> 6)
                                                )) >> 14;
                dst[evnLine + 2] = ((int)(
                                                    ((img_io2uint[b + 3] & 0x000FFFFC) << 12)
                                                )) >> 14;
                dst[evnLine + 3] = ((int)(
                                                    ((img_io2uint[b + 3] & 0x00000003) << 30) |
                                                    ((img_io2uint[b + 4] & 0xFFFF0000) >> 2)
                                                )) >> 14;

                dst[oddLine + 4] = ((int)(
                                                    ((img_io2uint[b + 4] & 0x0000FFFF) << 16) |
                                                    ((img_io2uint[b + 5] & 0xC0000000) >> 16)
                                                )) >> 14;
                dst[oddLine + 5] = ((int)(
                                                    ((img_io2uint[b + 5] & 0x3FFFF000) << 2)
                                                )) >> 14;
                dst[evnLine + 4] = ((int)(
                                                    ((img_io2uint[b + 5] & 0x00000FFF) << 20) |
                                                    ((img_io2uint[b + 6] & 0xFC000000) >> 12)
                                                )) >> 14;
                dst[evnLine + 5 ] = ((int)(
                                                    ((img_io2uint[b + 6] & 0x03FFFF00) << 6)
                                                )) >> 14;

                dst[oddLine + 6] = ((int)(
                                                    ((img_io2uint[b + 6] & 0x000000FF) << 24) |
                                                    ((img_io2uint[b + 7] & 0xFFC00000) >> 8)
                                                )) >> 14;
                dst[oddLine + 7] = ((int)(
                                                    ((img_io2uint[b + 7] & 0x003FFFF0) << 10)
                                                )) >> 14;
                dst[evnLine + 6 ] = ((int)(
                                                    ((img_io2uint[b + 7] & 0x0000000F) << 28) |
                                                    ((img_io2uint[b + 8] & 0xFFFC0000) >> 4)
                                                )) >> 14;
                dst[evnLine + 7] = ((int)(
                                                    ((img_io2uint[b + 8] & 0x0003FFFF) << 14)
                                                )) >> 14;
            };

            int i_num = (Size / 48) >> 1;

            //int o_idx_cnt = 0;
            //int o_idx0 = 0;
            //int c = 0;
            //for (int i = 0; i < i_num; i++)
            //{
            //    func(o_idx0 + 0, c + 0);                        //odd bank
            //    func(o_idx0 + 8, c + 9);                        //odd bank
            //    func(o_idx0 + 16, c + 18);                      //odd bank

            //    func(o_idx0 + 0 + 24, c + 0 + i_num * 28);      //even bank
            //    func(o_idx0 + 8 + 24, c + 9 + i_num * 28);      //even bank
            //    func(o_idx0 + 16 + 24, c + 18 + i_num * 28);    //even bank

            //    c += 28;
            //    o_idx0 += 48;

            //    o_idx_cnt += 48;

            //    if (o_idx_cnt == Width)
            //    {
            //        o_idx_cnt = 0;
            //        o_idx0 += Width;
            //    }
            //}

            int count = 0;
            int cn = 0;
            for(int y=0; y< Height; y+=2)
            {
                for(int x = 0;x < Width; x+=48)
                {
                    func(count + 0, cn + 0);                        //odd bank
                    func(count + 8, cn + 9);                        //odd bank
                    func(count + 16, cn + 18);                      //odd bank

                    func(count + 0 + 24, cn + 0 + i_num * 28);      //even bank
                    func(count + 8 + 24, cn + 9 + i_num * 28);      //even bank
                    func(count + 16 + 24, cn + 18 + i_num * 28);    //even bank

                    cn += 28;
                    count += 48;
                }
                count += Width;
            }
        }

    }
}

