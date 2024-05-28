using System.Windows.Controls;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFG.Models;

namespace TFG.Views.Pages {
    public partial class UserProfileEditPage : Page {
        public UserProfileEditPage(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser) {
            InitializeComponent();
            DataContext = new UserProfileEditViewModel(databaseService, navigationService, appUser);
        }

    }
}
