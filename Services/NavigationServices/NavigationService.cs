using System.Windows.Controls;
using TFG.ViewModels;
using TFG.Views.Pages;
using TFGDesktopApp.Models;

namespace TFG.Services.NavigationServices {
    public class NavigationService : INavigationService {
        private readonly Frame _frame;

        public NavigationService(Frame frame) {
            _frame = frame;
        }

        public void NavigateToLogin(MainWindowViewModel mainWindowViewModel) {
            _frame.Navigate(new LoginPage(_frame, mainWindowViewModel));
        }


        public void NavigateToWorkSpace(User user, MainWindowViewModel mainWindowViewModel) {
            _frame.Navigate(new WorkSpacePage(user, mainWindowViewModel));
        }
    }
}

