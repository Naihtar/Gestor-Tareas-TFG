using System.ComponentModel;

namespace TFGDesktopApp.ViewModels {
    public abstract class BaseViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName) {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

    }
}
