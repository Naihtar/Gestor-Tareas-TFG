using System.Windows.Controls;
using TFG.Views.Pages;
using TFGDesktopApp.Models;

namespace TFG.Services.NavigationServices {
    public class NavigationService : INavigationService {
        private readonly Frame _frame;

        public NavigationService(Frame frame) {
            _frame = frame;
        }

        public void NavigateToLogin() {
            _frame.Navigate(new LoginPage(_frame));
        }

        public void NavigateToWorkSpace(User user) {
            _frame.Navigate(new WorkSpacePage(user));
        }
    }
}

