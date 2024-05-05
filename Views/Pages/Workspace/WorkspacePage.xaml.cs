using System.Windows.Controls;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.Views.Pages {
    public partial class WorkSpacePage : Page {
        public WorkSpacePage(AppUser user, NavigationService nav) {
            InitializeComponent();
            DataContext = new WorkSpaceViewModel(user, nav);
        }
    }
}
