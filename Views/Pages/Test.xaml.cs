using System.Windows;
using System.Windows.Controls;

namespace TFG.Views.Pages {
    public partial class Test : Page {
        private readonly Frame _mainFrame;
        public Test() {
            InitializeComponent();
            _mainFrame = MainWindowView.MFrame;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            // Regresar a la página LoginPage
            _mainFrame.GoBack();
        }
    }
}
