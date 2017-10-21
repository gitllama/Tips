using Microsoft.Practices.Unity;
using Prism.Unity;
using PixelPrismUnity.Views;
using System.Windows;

namespace PixelPrismUnity
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
