﻿using System.Windows.Controls;
using TFG.Models;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Workspace.Container;

namespace TFG.Views.Pages.Workspace.Container {
    public partial class AppContainerPage : Page {
        public AppContainerPage(IDatabaseService databaseService, INavigationService navigationService, AppUser appUser, AppContainer appContainer) {
            InitializeComponent();

            DataContext = new AppContainerViewModel(databaseService, navigationService, appUser, appContainer);
        }
    }
}
