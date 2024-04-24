using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TFG.Services.DatabaseServices;
using TFG.ViewModels.Base;
using TFGDesktopApp.Models;

namespace TFG.ViewModels {
    internal class WorkSpaceViewModel : BaseViewModel {

        private readonly User _user;
        private readonly DatabaseService _databaseService;

        public WorkSpaceViewModel(User user) {
            _user = user;
            _databaseService = new DatabaseService();
        }

        public async Task<ObservableCollection<string>> LoadContainersAsync() {
            ObservableCollection<string> nombresContenedores = new ObservableCollection<string>();

            foreach (ObjectId containerId in _user.ListaContenedoresUsuario) {
                var container = await _databaseService.GetContainerByIdAsync(containerId);
                if (container != null) {
                    nombresContenedores.Add(container.NombreContenedor);
                }
            }

            return nombresContenedores;
        }
    }
}
