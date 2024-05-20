using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG.Services.DatabaseServices;
using TFG.Services.NavigationServices;

namespace TFG.ViewModels.Access {
    class SignUpViewModel {

        private readonly IDatabaseService _databaseService;
        private readonly NavigationService _navigationService;
        public SignUpViewModel(IDatabaseService db, NavigationService nav) {

            _databaseService = db;
            _navigationService = nav;

        }
    }
}
