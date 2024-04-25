using TFG.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using TFG.Services.NavigationServices;
using System.Windows.Media.Imaging;

namespace TFG {
    public partial class MainWindowView : FluentWindow {
        public MainWindowView() {
            InitializeComponent();
            var navigationService = new NavigationService(MainFrame);
            DataContext = new MainWindowViewModel(navigationService);
        }
    }
}