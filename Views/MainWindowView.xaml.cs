using System.Windows.Controls;
using TFG.ViewModels;
using TFG.Views;
using Wpf.Ui.Controls;

namespace TFG {
    public partial class MainWindowView : FluentWindow {
        public static Frame MFrame { get; private set; }

        public MainWindowView() {
            InitializeComponent();

            // Asigna el MainFrame a la propiedad estática
            MFrame = this.MainFrame;

            // Crea una instancia del ViewModel y establece el contexto de datos
            DataContext = new MainWindowViewModel();
        }
    }
}
