using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using ScrumAdministrator.Ui.ViewModel;
using ScrumAdministrator.Server.DataAccess;

namespace ScrumAdministrator.Ui
{
    public class MainViewModel : ViewModelBase
    {
        private string _storyNumbers;
        private JiraRepository _jiraRepository;
        private StoryOverviewViewModel _storyOverview;

        public MainViewModel()
        {
            PrintCommand = new RelayCommand(ExecutePrintCommand, CanExecutePrintCommand);
            _jiraRepository = new JiraRepository();
            _storyOverview = new StoryOverviewViewModel();
            StoryOverview = new StoryOverviewViewModel();
            StoryOverview.Initialize();
        }

        private void ExecutePrintCommand()
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] storyNumberStrings = StoryNumbers.Split(delimiterChars);
            var storyNumbers = new List<int>();

            foreach (string storyNumberString in storyNumberStrings)
            {
                storyNumbers.Add(int.Parse(storyNumberString));
            }

            //var stories = _jiraRepository.GetStories(storyNumbers);
            //_jiraRepository.GetStoryAsPdf(stories);
        }

        private bool CanExecutePrintCommand()
        {
            return true;
        }

        StoryOverviewViewModel StoryOverview
        {
            get { return _storyOverview; }
            set
            {
                if (_storyOverview != value)
                {
                    _storyOverview = value;
                    RaisePropertyChanged("StoryOverview");
                }
            }
        }

        public RelayCommand PrintCommand { get; private set; }

        public string StoryNumbers
        {
            get { return _storyNumbers; }
            set
            {
                if (_storyNumbers != value)
                {
                    _storyNumbers = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
