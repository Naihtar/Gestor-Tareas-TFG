using TFG.ViewModels;
using Wpf.Ui.Controls;
using TFG.Services.NavigationServices;
using System.Windows;
using TFG.Services.DatabaseServices;
using TFG.Services.AuthentificationServices;
using TFG.Database;

namespace TFG {
    public partial class MainWindowView : FluentWindow {
        public MainWindowView() {
            InitializeComponent();
            IConnectionManager connectionManager = new ConnectionManager();
            IDatabaseConnection databaseConnection = new DatabaseConnection(connectionManager);
            INavigationService navigationService = new NavigationService(MainFrame);
            IDatabaseService databaseService = new DatabaseService(databaseConnection);
            IAuthenticationService authenticationService = new AuthenticationService(databaseService);
            DataContext = new MainWindowViewModel(navigationService, databaseService, authenticationService);

            //Fijar tamaño pantalla al iniciar.
            WindowState = WindowState.Normal;
        }
    }
}
