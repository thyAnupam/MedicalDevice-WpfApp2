using Microsoft.EntityFrameworkCore;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.Repository;
using WpfApp2.Repository.Models;
using WpfApp2.ViewModels.Services;
using WpfApp2.Views;

namespace WpfApp2.ViewModels
{
    public class AddStudySeriesViewModel: ViewModelBase
    {
        
        private readonly INavigationService _navigationService;
        private ObservableCollection<string> _studies;
        private ObservableCollection<string> _series;

        private string studyName = "";
        private string seriesName = "";
        
        public string StudyName
        {
            get => studyName;
            set
            {
                SetProperty(ref studyName, value);
                OnPropertyChanged(nameof(StudyName));
            }
        }
        public string SeriesName
        {
            get => seriesName;
            set => SetProperty(ref seriesName, value);
        }
        public ObservableCollection<string> Studies
        {
            get => _studies;
            set
            {
                _studies = value;
                OnPropertyChanged(nameof(Studies));
            }
        }

        
        public ObservableCollection<string> Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged(nameof(Series));
            }
        }

        private string _selectedstudy="";
        public string SelectedStudy
        {
            get => _selectedstudy;
            set
            {
                _selectedstudy = value;
                OnPropertyChanged(nameof(SelectedStudy));
            }
        }

        private string errmsg;
        public string ErrorMessage
        {
            get => errmsg;
            set
            {
                SetProperty(ref errmsg, value);
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }



        private MoDbContext _context;

        private int _patientID;
        public AddStudySeriesViewModel(int PatientId, INavigationService navigationService)
        {
            _context = new MoDbContext();
            _patientID = PatientId;
            LoadStudies(_patientID);
            _navigationService = navigationService;
            AddDataCommand = new RelayCommand(GoToData);
            AddSeriesCommand = new RelayCommand(AddSeries);
            AddStudyCommand = new RelayCommand(AddStudy);   

        }

        private async void LoadStudies(int id)
        {
            var list = _context.Studies.Where(x => x.PatientId==id).Select(x => new string(x.StudyName)).ToList();
            Studies = new ObservableCollection<string>(list);
        }

        private async void LoadSeries(int id)
        {
            var list = _context.Series.Where(x => x.StudyId == id).Select(x => new string(x.SeriesName)).ToList();
            Series = new ObservableCollection<string>(list);
        }

        public ICommand AddDataCommand { get; }
        public ICommand AddStudyCommand { get; }
        public ICommand AddSeriesCommand { get; }

        public void AddStudy()
        {
            var st = new Study { StudyName = studyName, Created = DateTime.Today, PatientId = _patientID };
            _context.Studies.Add(st);
            _context.SaveChanges();
            StudyName = "";
            LoadStudies(_patientID);

        }

        public void AddSeries()
        {
            if(SelectedStudy == "")
            {
                ErrorMessage = "Please select a study";
            }
            else
            {
                var st = new Series { SeriesName = seriesName, Created = DateTime.Today, StudyId = GetStudyId(SelectedStudy) };
                _context.Series.Add(st);
                _context.SaveChanges();

                SeriesName = "";
                LoadSeries(GetStudyId(SelectedStudy));
                ErrorMessage = "";

            }
            

        }


        public void GoToData()
        {
            _navigationService.NavigateTo(new AddDataToPatientView(_patientID, 2));
        }

        

        public int GetStudyId(string s)
        {
            int id=-1;
            using(var context = new MoDbContext())
            {
                id = context.Studies.First(x => x.StudyName==s).StudyId;
            }

            return id;

        }

    }
}
