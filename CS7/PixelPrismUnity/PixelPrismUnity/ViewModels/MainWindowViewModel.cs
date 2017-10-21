using Pixels;
using Pixels.Stream;
using Prism.Mvvm;
using System.IO;
using YamlDotNet;
using YamlDotNet.Serialization;
using System;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using Pixels2Extend;
using System.Threading;

namespace PixelPrismUnity.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Unity Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private WriteableBitmap _img;
        public WriteableBitmap img
        {
            get { return _img; }
            set { SetProperty(ref _img, value); }
        }

        public MainWindowViewModel()
        {
            var p = (new Deserializer())
                    .Deserialize<Pixel<int>>(
                    File.ReadAllText("config.yaml")).Create();
            CancellationTokenSource token = new CancellationTokenSource();

            p.Cancellation(token);

            p = p.Read(@"D:\Aki\Desktop\000.bin")["HOB"].Average();
            //p["Full"].Average();
            //p["Active"];
            img =p["Test"].BitShiftR(8)["Full"].StaggerL().ToColorGR();


            var o = (new Serializer())
                    .Serialize(p);


            var y = Observable.Range(1, 10)
                .Where(x => x % 2 == 0)
                .Select(x => x)
                .Subscribe(x => Debug.WriteLine($"{x}"), ()=> Debug.WriteLine("finish"));



            y.Dispose();
        }
    }



}
