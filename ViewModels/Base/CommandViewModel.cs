using System.Windows.Input;

namespace TFG.ViewModels.Base {
    public class CommandViewModel : ICommand {

        //Atributos
        private readonly Action<object> _executeAction;
        private readonly Predicate<object>? _canExecuteAction;

        //Constructores
        public CommandViewModel(Action<object> executeAction) {
            _executeAction = executeAction;
            _canExecuteAction = null;
        }

        public CommandViewModel(Action<object> executeAction, Predicate<object> canExecuteAction) {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        //Eventos
        public event EventHandler? CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //Methods
        public bool CanExecute(object? parameter) {
            return _canExecuteAction == null ? true : _canExecuteAction(parameter);
        }

        public void Execute(object? parameter) {

            _executeAction(parameter);

        }

        public void RaiseCanExecuteChanged() {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
