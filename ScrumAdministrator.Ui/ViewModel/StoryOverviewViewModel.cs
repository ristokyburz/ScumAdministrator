using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ScrumAdministrator.Server.DataAccess;
using System.Collections.ObjectModel;

namespace ScrumAdministrator.Ui.ViewModel
{
    public class StoryOverviewViewModel : ViewModelBase
    {
        private JiraRepository _jiraRepository;

        public StoryOverviewViewModel()
        {
            Stories = new ObservableCollection<StoryViewModel>();
            PrintCommand = new RelayCommand(ExecutePrintCommand, CanExecutePrintCommand);
            _jiraRepository = new JiraRepository();

            
        }

        public void Initialize()
        {
            AddNewStory();
            AddNewStory();
        }

        public string Test
        {
            get
            {
                return "asdfasdfieieieie";
            }
        }


        public RelayCommand PrintCommand { get; private set; }

        public ObservableCollection<StoryViewModel> Stories { get; set; }

        public void AddNewStory()
        {
            var storyViewModel = new StoryViewModel();
            storyViewModel.Initialize(this, 0, 0, false, false);
            Stories.Add(storyViewModel);
        }

        private void ExecutePrintCommand()
        {
            //char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            //string[] storyNumberStrings = StoryNumbers.Split(delimiterChars);
            //var storyNumbers = new List<int>();

            //foreach (string storyNumberString in storyNumberStrings)
            //{
            //    storyNumbers.Add(int.Parse(storyNumberString));
            //}

            //var stories = _jiraRepository.GetStories(storyNumbers);
            //_jiraRepository.GetStoryAsPdf(stories);
        }

        private bool CanExecutePrintCommand()
        {
            return true;
        }
    }
}
