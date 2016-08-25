using System.Windows;
using ScrumAdministrator.Cockpit.ViewModel;
using FirstFloor.ModernUI.Windows.Controls;
using System.ComponentModel;

namespace ScrumAdministrator.Cockpit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}