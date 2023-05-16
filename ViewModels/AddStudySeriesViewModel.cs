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

        private ObservableCollection<string> _series = new ObservableCollection<string>();
        public ObservableCollection<string> Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged(nameof(Series));
            }
        }
        private MoDbContext _context;

        public AddStudySeriesViewModel(int PatientId, INavigationService navigationService)
        {
            _context = new MoDbContext();
            LoadStudies(PatientId);
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
            var st = new Study { StudyName = studyName, Created = DateTime.Today, PatientId = 1 };
            _context.Studies.Add(st);
            _context.SaveChanges();
            LoadStudies(1);

        }

        public void AddSeries()
        {
            var st = new Series { SeriesName=seriesName, Created=DateTime.Today, StudyId=5};
            _context.Series.Add(st);
            _context.SaveChanges();
            LoadSeries(5);

        }


        public void GoToData()
        {
            _navigationService.NavigateTo(new AddDataToPatientView());
        }




    }
}
