using System.Windows.Controls;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.Views.Pages {
    /// <summary>
    /// Interaction logic for UserProfileEditPasswordPage.xaml
    /// </summary>
    public partial class UserProfileEditPasswordPage : Page
    {
        public UserProfileEditPasswordPage(AppUser user, NavigationService nav)
        {
            InitializeComponent();

            DataContext = new UserProfileEditPasswordViewModel(user, nav);
        }
    }
}
