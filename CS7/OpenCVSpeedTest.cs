using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using OpenCVSpeedTest.Models;
using System.Windows.Media;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCVSpeedTest.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        //ストップウォッチを開始する

        byte[] dst;
        byte[] r_data;
        byte[] g_data;
        byte[] b_data;
        public async void Initialize()
        {
            //4Kファイルを読み込もう16bit

            r_data = File.ReadAllBytes(@"D:\OneDrive\画像\raw\u10_Ship_4K.r");
            g_data = File.ReadAllBytes(@"D:\OneDrive\画像\raw\u10_Ship_4K.g");
            b_data = File.ReadAllBytes(@"D:\OneDrive\画像\raw\u10_Ship_4K.b");

            dst = new byte[2160 * 3840];

            Converter(r_data, 0, 0, ref dst);
            Converter(g_data, 0, 1, ref dst);
            Converter(g_data, 1, 0, ref dst);
            Converter(b_data, 1, 1, ref dst);

            await Task.Run(a2);

        }

        private Task a()
        {
            sw.Start();
            while (true)
            {
                using (Mat buf = new Mat(2160, 3840, MatType.CV_8UC1, dst))
                using (Mat dstmat = new Mat())
                {
                    //18fps
                    Cv2.CvtColor(buf, dstmat, ColorConversionCodes.BayerBG2BGR);
                    var i = BitmapSourceConverter.ToBitmapSource(dstmat);

                    //35fps
                    //var i = BitmapSourceConverter.ToBitmapSource(buf);

                    //Cv2.CvtColor()

                    //var grayMat = buf.CvtColor(ColorConversionCodes.BayerBG2BGR);
                    //grayMat.Dispose();

                    i.Freeze();
                    //Cv2.ImShow("dst", dstmat);

                    //image.Do
                    image = i;

                    //dstmat.Dispose();
                    //buf.Dispose();

                    fps = (1000 / sw.Elapsed.TotalMilliseconds).ToString();
                    sw.Restart();
                    
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
        }
        private Task b()
        {
            sw.Start();
            while (true)
            {
                using (Mat r = new Mat(2160, 3840, MatType.CV_8UC1, r_data))
                using (Mat g = new Mat(2160, 3840, MatType.CV_8UC1, g_data))
                using (Mat b = new Mat(2160, 3840, MatType.CV_8UC1, b_data))
                using (Mat dstmat = new Mat())
                {
                    //

                    Cv2.Merge(new Mat[] { b , g, r }, dstmat);
                    var i = BitmapSourceConverter.ToBitmapSource(dstmat);

                    i.Freeze();
                    image = i;


                    fps = (1000 / sw.Elapsed.TotalMilliseconds).ToString();
                    sw.Restart();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
        }

        private Task a2()
        {
            sw.Start();
            using (Mat buf = new Mat(2160, 3840, MatType.CV_8UC1, dst))
            using (Mat dstmat = new Mat())
            {
                while (true)
                {
                    //18fps
                    Cv2.CvtColor(buf, dstmat, ColorConversionCodes.BayerBG2BGR);
                    var i = WriteableBitmapConverter.ToWriteableBitmap(dstmat);

                    i.Freeze();
                    image = i;

                    fps = (1000 / sw.Elapsed.TotalMilliseconds).ToString();
                    sw.Restart();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
        }

        private Task b2()
        {
            sw.Start();
            while (true)
            {
                using (Mat r = new Mat(2160, 3840, MatType.CV_8UC1, r_data))
                using (Mat g = new Mat(2160, 3840, MatType.CV_8UC1, g_data))
                using (Mat b = new Mat(2160, 3840, MatType.CV_8UC1, b_data))
                using (Mat dstmat = new Mat())
                {
                    //

                    Cv2.Merge(new Mat[] { b, g, r }, dstmat);
                    var i = BitmapSourceConverter.ToBitmapSource(dstmat);

                    i.Freeze();
                    image = i;


                    fps = (1000 / sw.Elapsed.TotalMilliseconds).ToString();
                    sw.Restart();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
        }

        private void Converter(byte[] src,int r,int c,ref byte[] dst)
        {
            for (int y = r; y < 2160; y+=2)
                for (int x = c; x < 3840; x+=2)
                {
                    dst[x + 3840 * y] = (byte)(BitConverter.ToInt16
                        (
                            new byte[] {src[(x + 3840 * y) * 2 + 1], src[(x + 3840 * y) * 2]},
                            0
                        )>> 4);
                }
        }

        #region image変更通知プロパティ
        private ImageSource _image;

        public ImageSource image
        {
            get
            { return _image; }
            set
            { 
                if (_image == value)
                    return;
                _image = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region fps変更通知プロパティ
        private string _fps;

        public string fps
        {
            get
            { return _fps; }
            set
            { 
                if (_fps == value)
                    return;
                _fps = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
