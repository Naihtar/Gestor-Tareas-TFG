using TFG.ViewModels;
using Wpf.Ui.Controls;
using TFG.Services.NavigationServices;
using System.Windows;
using TFG.Services.DatabaseServices;
using TFG.Services.AuthentificationServices;
using TFG.Database;
using TFG.Services;

namespace TFG {
    public partial class MainWindowView : FluentWindow {
        public MainWindowView() {
            InitializeComponent();

            //Inicialización de las dependencias

            //Obteniendo la clave de conexión
            IConnectionManager connectionManager = new ConnectionManager();
            
            //Conectandose a la base de datos
            IDatabaseConnection databaseConnection = new DatabaseConnection(connectionManager);
            IDatabaseService databaseService = new DatabaseService(databaseConnection);
            IAuthenticationService authenticationService = new AuthenticationService(databaseService);

            //Iniciando la traducción predefinida o de la sesión anterior
            ILocalizationService localizationService = new LocalizationService();

            //Iniciando el servicio de navegación junto a las dependencias inicializadas.
            INavigationService navigationService = new NavigationService(MainFrame, databaseService, authenticationService, localizationService);
            
            //Asignando el viewmodel al contexto de la vista junto a llas dependencias que usara.
            DataContext = new MainWindowViewModel(navigationService, localizationService);

            //Fijar tamaño pantalla al iniciar.
            WindowState = WindowState.Normal;
        }
    }
}
