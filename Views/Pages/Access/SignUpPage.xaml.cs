using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Access;

namespace TFG.Views.Pages.Access {
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page {
        public SignUpPage(IDatabaseService db, INavigationService nav, IAuthenticationService auth) {
            InitializeComponent();
            DataContext = new SignUpViewModel(db, nav, auth);
        }
    }
}
