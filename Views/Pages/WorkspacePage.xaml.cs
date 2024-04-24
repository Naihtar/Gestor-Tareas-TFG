using System.Windows;
using System.Windows.Controls;
using TFG.ViewModels;
using TFGDesktopApp.Models;

namespace TFG.Views.Pages {
    public partial class WorkSpacePage : Page {
        private readonly User _user;

        public WorkSpacePage(User user) {
            InitializeComponent();
            _user = user;
            DataContext = _user;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            // Regresar a la página LoginPage
            NavigationService?.GoBack();
        }

        private void HomeMenuItem_Click(object sender, RoutedEventArgs e) {
            // Mostrar la información del usuario
            ShowUserInfo();
        }

        private void ShowUserInfo() {
            // Puedes mostrar la información del usuario en un StackPanel u otro control
            // Por ejemplo, aquí se muestra en un StackPanel llamado UserInfoPanel

            UserInfoPanel.Children.Clear();

            var nombreUsuarioTextBlock = new TextBlock { Text = _user.NombreUsuario, Margin = new Thickness(0, 0, 0, 10), FontSize = 16 };
            var apellido1UsuarioTextBlock = new TextBlock { Text = _user.Apellido1Usuario, Margin = new Thickness(0, 0, 0, 10), FontSize = 16 };
            var apellido2UsuarioTextBlock = new TextBlock { Text = _user.Apellido2Usuario, Margin = new Thickness(0, 0, 0, 10), FontSize = 16 };
            var emailUsuarioTextBlock = new TextBlock { Text = _user.EmailUsuario, Margin = new Thickness(0, 0, 0, 10), FontSize = 16 };

            UserInfoPanel.Children.Add(nombreUsuarioTextBlock);
            UserInfoPanel.Children.Add(apellido1UsuarioTextBlock);
            UserInfoPanel.Children.Add(apellido2UsuarioTextBlock);
            UserInfoPanel.Children.Add(emailUsuarioTextBlock);
        }
    }
}
