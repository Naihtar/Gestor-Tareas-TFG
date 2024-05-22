using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels;
using TFG.ViewModels.User;

namespace TFG.Views.Pages.User {
    /// <summary>
    /// Interaction logic for UserProfileDeletePage.xaml
    /// </summary>
    public partial class UserProfileDeletePage : Page {
        public UserProfileDeletePage(AppUser user, INavigationService nav, IDatabaseService db, IAuthenticationService auth) {
            InitializeComponent();
            DataContext = new UserProfileDeleteViewModel(user, nav, db, auth);
        }
    }
}
