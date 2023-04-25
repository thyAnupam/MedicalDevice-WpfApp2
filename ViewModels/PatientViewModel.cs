using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using WpfApp2.Models;

namespace WpfApp2.ViewModels
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PatientService patientService;

        public PatientViewModel()
        {
            patientService = new PatientService();
            LoadData();
        }

        private List<Patient> patientList;
        public List<Patient> PatientList
        {
            get { return patientList; }
            set { patientList = value; OnPropertyChanged("PatientList"); }

        }

        private void LoadData()
        {
            PatientList = patientService.GetPatients();
        }
    }
}
