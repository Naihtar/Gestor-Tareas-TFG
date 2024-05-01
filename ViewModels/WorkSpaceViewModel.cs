using System.Collections.ObjectModel;
using TFG.Services.DatabaseServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    public class WorkSpaceViewModel : BaseViewModel {
        private User _user;
        private MainWindowViewModel _mainWindowViewModel;
        private readonly DatabaseService _databaseService;

        public ObservableCollection<string> NombreContenedores { get; }


        public CommandViewModel MyCommand { get; }

        public WorkSpaceViewModel(User user, MainWindowViewModel mainWindowViewModel) {

            NombreContenedores = new ObservableCollection<string>();
            _user = user;
            _mainWindowViewModel = mainWindowViewModel;
            _databaseService = new DatabaseService();
            MyCommand = new CommandViewModel(ExecuteMyCommand);
            _ = FillContainerNames();

        }

        private void ExecuteMyCommand(object parameter) {
            // Cambia IsValueTrue a true cuando se ejecuta el comando.
            _mainWindowViewModel.IsValueTrue = true;
        }

        public async Task FillContainerNames() {
            foreach (var containerId in _user.ListaContenedoresUsuario) {
                var containerName = await _databaseService.GetContainerByIdAsync(containerId);
                NombreContenedores.Add(containerName.NombreContenedor);
            }
        }
    }
}
