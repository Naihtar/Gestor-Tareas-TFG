using System.Windows.Controls;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.Views.Pages {
    public partial class WorkSpacePage : Page {
        public WorkSpacePage(User user, MainWindowViewModel mainWindowViewModel, NavigationService nav) {
            InitializeComponent();
            DataContext = new WorkSpaceViewModel(user, mainWindowViewModel, nav);
        }
    }
}
