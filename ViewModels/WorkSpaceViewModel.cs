using System.Collections.ObjectModel;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    public class WorkSpaceViewModel : BaseViewModel {
        private User _user;
        private MainWindowViewModel _mainWindowViewModel;

        public ObservableCollection<string> NombreContenedores { get; }


        public CommandViewModel MyCommand { get; }

        public WorkSpaceViewModel(User user, MainWindowViewModel mainWindowViewModel) {

            NombreContenedores = new ObservableCollection<string>();
            _user = user;
            _mainWindowViewModel = mainWindowViewModel;
            MyCommand = new CommandViewModel(ExecuteMyCommand);
        }

        private void ExecuteMyCommand(object parameter) {
            // Cambia IsValueTrue a true cuando se ejecuta el comando.
            _mainWindowViewModel.IsValueTrue = true;
        }
    }
}
