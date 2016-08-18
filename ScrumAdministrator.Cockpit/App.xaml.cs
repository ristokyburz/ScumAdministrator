using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace ScrumAdministrator.Cockpit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
