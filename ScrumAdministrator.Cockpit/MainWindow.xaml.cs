using System.Windows;
using ScrumAdministrator.Cockpit.ViewModel;
using FirstFloor.ModernUI.Windows.Controls;

namespace ScrumAdministrator.Cockpit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
            //Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}