using System.ComponentModel;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace TFG.ViewModels.Base {
    public abstract class BaseViewModel : INotifyPropertyChanged {

        private string? _errorMessage;

        public string? ErrorMessage {
            get { return _errorMessage; }
            set {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName) {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public static bool IsValidEmail(string email) {
            try {
                var addr = new MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
        }


    }
}
