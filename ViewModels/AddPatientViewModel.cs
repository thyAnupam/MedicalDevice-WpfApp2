using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.ViewModels.Services;
using WpfApp2.Views;

namespace WpfApp2.ViewModels
{
    public class AddPatientViewModel:ViewModelBase
    {
        private INavigationService _navigationService;
        public AddPatientViewModel(INavigationService s) 
        {
            _navigationService = s;
            AddPatientCommand = new RelayCommand(AddPatient);

        }

        public ICommand AddPatientCommand { get; }

        public void AddPatient()
        {
            //Add patient to context
            _navigationService.NavigateTo(new AddDataToPatientUserControl());

        }
    }
}
