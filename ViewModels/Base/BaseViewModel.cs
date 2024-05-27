using System.ComponentModel;
using System.Net.Mail;
using System.Windows;
using System.Windows.Threading;

namespace TFG.ViewModels.Base {
    //Implementamos la interfaz INotifyPropertyChanged, 
    //para notificar al programa respecto a los posibles cambios en variables o datos
    public abstract class BaseViewModel : INotifyPropertyChanged {

        //Diccionario con el contenido
        private readonly ResourceDictionary _resourceDictionary = Application.Current.Resources;
        public ResourceDictionary ResourceDictionary {
            get { return _resourceDictionary; }
        }

        private DispatcherTimer? _timer; //Cronómetro
        public event PropertyChangedEventHandler? PropertyChanged; //Eventos

        //Atributos
        private string? _message;
        public string? Message {
            get { return _message; }
            set {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        private string? _errorMessage;
        public string? ErrorMessage {
            get { return _errorMessage; }
            set {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        private string? _successMessage;
        public string? SuccessMessage {
            get { return _successMessage; }
            set {
                _successMessage = value;
                OnPropertyChanged(nameof(SuccessMessage));
            }
        }
        private bool _errorOpen;
        public bool ErrorOpen {
            get { return _errorOpen; }
            set {
                _errorOpen = value;
                OnPropertyChanged(nameof(ErrorOpen));
            }
        }
        private bool _successOpen;
        public bool SuccessOpen {
            get { return _successOpen; }
            set {
                _successOpen = value;
                OnPropertyChanged(nameof(SuccessOpen));
            }
        }

        //Métodos

        //Método para notificar los cambios
        public void OnPropertyChanged(string propertyName) {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        //Método para validar que el correo electrónico tenga la estructura correcta.
        public static bool IsValidEmail(string email) {
            try {
                var addr = new MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
        }

        //Método para iniciar el cronómetro
        public void StartTimer() {
            _timer?.Stop();

            _timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(7)
            };

            _timer.Tick += (s, e) => {
                _timer.Stop();
                CloseNotifications();
            };

            _timer.Start();
        }

        //Cerrar las infobars
        private void CloseNotifications() {
            ErrorOpen = false;
            SuccessOpen = false;
        }

        //Mostrar un mensaje de acción complentada correctamente
        public void ShowSuccessMessage(string message) {
            ErrorOpen = false;
            SuccessOpen = true;
            SuccessMessage = message;
            StartTimer();
        }
    }
}
