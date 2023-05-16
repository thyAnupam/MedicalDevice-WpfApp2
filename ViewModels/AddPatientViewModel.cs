using System;
using System.Windows.Input;
using WpfApp2.Repository;
using WpfApp2.Repository.Models;
using WpfApp2.Resources;
using WpfApp2.ViewModels.Services;
using WpfApp2.Views;

namespace WpfApp2.ViewModels
{
    public class AddPatientViewModel:ViewModelBase
    {
        private string firstName = "";
        private string lastName = "";
        private DateTime dob=DateTime.MinValue;
        
        private int height=0;
        private string errmsg;

        private bool[] _modeArray = new bool[] { true, false}; //FOR Gender
        public bool[] ModeArray
        {
            get { return _modeArray; } 
        }
        public int SelectedMode
        {
            get { return Array.IndexOf(_modeArray, true); } //FOR Gender
        }


        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }
        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }
        public DateTime DOB
        {
            get => dob;
            set => SetProperty(ref dob, value);
        }

        

        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
                
            }
        }

        public string ErrorMessage
        {
            get => errmsg;
            set => SetProperty(ref errmsg, value);
        }




        private INavigationService _navigationService;
        public AddPatientViewModel(INavigationService s) 
        {
            _navigationService = s;
            AddPatientCommand = new RelayCommand(AddPatient);

        }

        public ICommand AddPatientCommand { get; }

        private bool AllFieldsNotNull()
        {
            if (FirstName != "" && LastName!="" && DOB != DateTime.MinValue && Height !=0)
                return true;


            return false;
        }

        public void AddPatient()
        {
            //Add patient to context
            
            if (AllFieldsNotNull())
            {
                Patient p = new Patient {Firstname=FirstName, Lastname=LastName, Dob=DOB, Gender=SelectedMode+1, Height=Height};
                //Add p to context

                using(var context = new MoDbContext())
                {
                    context.Patients.Add(p);
                    context.SaveChanges();
                }

                ErrorMessage = Resource1.PatientAdded;
                _navigationService.NavigateTo(new AddStudySeriesView(_navigationService));
            }

            else
            {
                ErrorMessage = Resource1.ProvideAllPatientDetails;
            }
           

            

        }
    }
}
