using System.Windows.Controls;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFG.Models;

namespace TFG.Views.Pages {
    public partial class WorkSpacePage : Page {
        public WorkSpacePage(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth, AppContainer? container) {
            InitializeComponent();
            InitializeViewModel(user, nav, db, auth, container);
        }

        private async void InitializeViewModel(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth, AppContainer? container) {
            DataContext = await WorkSpaceViewModel.CreateAsync(user, nav, db, auth, container);
        }
    }

}
