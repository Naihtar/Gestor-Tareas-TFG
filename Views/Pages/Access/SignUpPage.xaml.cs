﻿using System.Windows.Controls;
using TFG.Services.AuthentificationServices;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;
using TFG.ViewModels.Access;

namespace TFG.Views.Pages.Access {
    public partial class SignUpPage : Page {
        public SignUpPage(IDatabaseService databaseService, IAuthenticationService authenticationService, INavigationService navigationService) {
            InitializeComponent();
            DataContext = new SignUpViewModel(databaseService, authenticationService, navigationService);
        }
    }
}
