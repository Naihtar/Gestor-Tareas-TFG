using MongoDB.Bson;
using System.Threading.Tasks;
using TFG.Models;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Base;

namespace TFG.ViewModels.Workspace.Tasks {
    class AppTaskEditViewModel : AppTaskBaseViewModel {

        public CommandViewModel SaveTaskCommand { get; }

        public AppTaskEditViewModel(AppTask? task, AppContainer container, AppUser user, INavigationService navigationService, IDatabaseService db, IAuthenticationService auth)
            : base(task, container, user, navigationService, db, auth) {

            SaveTaskCommand = new CommandViewModel(async (obj) => await SaveTaskAsyncWrapper());
        }

        private bool CheckFieldName() {
            return string.IsNullOrEmpty(EditableTask.NombreTarea);
        }

        private string _tagsTask;
        public string TagsTask {
            get => _tagsTask;
            set {
                _tagsTask = value;
                OnPropertyChanged(nameof(TagsTask));
            }
        }

        private void UpdateTagsFromText() {
            // Separar las etiquetas por comas
            var tags = TagsTask.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(tag => tag.Trim())
                               .Take(3)  // Tomar solo las primeras tres etiquetas
                               .ToArray();

            // Actualizar la lista de etiquetas en el modelo
            EditableTask.EtiquetasTarea = tags;

            // Actualizar el texto de etiquetas para reflejar las etiquetas actuales (opcional)
            TagsTask = string.Join(", ", tags);
        }

        protected override async Task SaveTaskAsyncWrapper() {
            // Verificar que el campo de nombre no esté vacío
            bool fieldName = CheckFieldName();

            if (fieldName) {
                ErrorMessage = "Rellene el campo del nombre.";
                return;
            }

            // Actualizar las etiquetas desde el texto
            UpdateTagsFromText();

            // Guardar los cambios
            await SaveEditChangesAsync();
        }
    }
}
