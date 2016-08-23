using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using ScrumAdministrator.Server.Domain;
using System.Collections.Generic;
using ScrumAdministrator.Server.Service;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace ScrumAdministrator.Cockpit.ViewModel
{
    public class StoryOverviewViewModel : ViewModelBase
    {
        private JiraService _jiraService;
        private ActivSprint _selectedSprint;
        private bool _isWithDetailsForAll;
        private StoryColor _selectedStoryColorForAll;
        private StoryColor _selectedEpicColorForAll;
        private Visibility _busyIndicatorVisibility;
        private bool _isToryToPrintForAll;
        private bool _isEpicToPrintForAll;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StoryOverviewViewModel()
        {
            Sprints = new ObservableCollection<ActivSprint>();
            Epics = new ObservableCollection<EpicViewModel>();
            Stories = new ObservableCollection<StoryViewModel>();
            ChartViewModel = new ChartViewModel();
            BusyIndicatorVisibility = Visibility.Hidden;
            PrintStoriesCommand = new RelayCommand(ExecutePrintStoriesCommand, CanExecutePrintStoriesCommand);
            PrintEpicsCommand = new RelayCommand(ExecutePrintEpicsCommand, CanExecutePrintEpicsCommand);
            _jiraService = new JiraService();
            CurrentArt = new Art();
            GetSprints();

            StoryColorsForAll = new List<StoryColor>
            {
                new StoryColor { Description = "Jira" },
                new StoryColor { Description = "Yellow" },
                new StoryColor { Description = "Blue" },
                new StoryColor { Description = "Green" },
                new StoryColor { Description = "Red" },
                new StoryColor { Description = "White" }
            };

            SelectedStoryColorForAll = StoryColorsForAll.Single(x => x.Description == "White");

            EpicColorsForAll = new List<StoryColor>
            {
                new StoryColor { Description = "Jira" },
                new StoryColor { Description = "Yellow" },
                new StoryColor { Description = "Blue" },
                new StoryColor { Description = "Green" },
                new StoryColor { Description = "Red" },
                new StoryColor { Description = "White" }
            };

            SelectedEpicColorForAll = EpicColorsForAll.Single(x => x.Description == "White");

            IsStoryToPrintForAll = true;
            IsEpicToPrintForAll = true;
        }

        public RelayCommand PrintStoriesCommand { get; private set; }

        public RelayCommand PrintEpicsCommand { get; private set; }

        public ObservableCollection<ActivSprint> Sprints { get; set; }

        public ObservableCollection<EpicViewModel> Epics { get; set; }

        public ObservableCollection<StoryViewModel> Stories { get; set; }

        public ChartViewModel ChartViewModel { get; set; }

        public ActivSprint SelectedSprint
        {
            get { return _selectedSprint; }
            set
            {
                if (_selectedSprint != value)
                {
                    _selectedSprint = value;

                    Stories.Clear();

                    IsAdditionalStoryForAll = false;
                    IsWithDetailsForAll = false;

                    foreach (var story in SelectedSprint.Stories)
                    {
                        var storyViewModel = new StoryViewModel();
                        storyViewModel.Initialize(this, story, false, false);
                        Stories.Add(storyViewModel);
                    }

                    RaisePropertyChanged();
                }
            }
        }

        public Art CurrentArt { get; set; }

        public bool IsAdditionalStoryForAll
        {
            get { return _isWithDetailsForAll; }
            set
            {
                if (_isWithDetailsForAll != value)
                {
                    _isWithDetailsForAll = value;
                    foreach (var story in Stories)
                    {
                        story.IsAdditionalStory = _isWithDetailsForAll;
                    }
                }

                RaisePropertyChanged();
            }
        }

        public bool IsWithDetailsForAll
        {
            get { return _isWithDetailsForAll; }
            set
            {
                if (_isWithDetailsForAll != value)
                {
                    _isWithDetailsForAll = value;
                    foreach (var story in Stories)
                    {
                        story.IsWithDetails = _isWithDetailsForAll;
                    }
                }

                RaisePropertyChanged();
            }
        }

        public bool IsStoryToPrintForAll
        {
            get { return _isToryToPrintForAll; }
            set
            {
                if (_isToryToPrintForAll != value)
                {
                    _isToryToPrintForAll = value;
                    foreach (var story in Stories)
                    {
                        story.IsStoryToPrint = _isToryToPrintForAll;
                    }
                }

                RaisePropertyChanged();
            }
        }

        public bool IsEpicToPrintForAll
        {
            get { return _isEpicToPrintForAll; }
            set
            {
                if (_isEpicToPrintForAll != value)
                {
                    _isEpicToPrintForAll = value;
                    foreach (var epic in Epics)
                    {
                        epic.IsStoryToPrint = _isEpicToPrintForAll;
                    }
                }

                RaisePropertyChanged();
            }
        }

        public Visibility BusyIndicatorVisibility
        {
            get { return _busyIndicatorVisibility; }
            set
            {
                if (_busyIndicatorVisibility != value)
                {
                    _busyIndicatorVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public List<StoryColor> StoryColorsForAll { get; set; }

        public StoryColor SelectedStoryColorForAll
        {
            get { return _selectedStoryColorForAll; }
            set
            {
                if (_selectedStoryColorForAll != value)
                {
                    _selectedStoryColorForAll = value;
                    RaisePropertyChanged();

                    foreach (var storyVM in Stories)
                    {
                        storyVM.SelectedStoryColor = storyVM.StoryColors.Single(x => x.Description == SelectedStoryColorForAll.Description);
                        storyVM.UpdateItems();
                    }
                }
            }
        }

        public List<StoryColor> EpicColorsForAll { get; set; }

        public StoryColor SelectedEpicColorForAll
        {
            get { return _selectedEpicColorForAll; }
            set
            {
                if (_selectedEpicColorForAll != value)
                {
                    _selectedEpicColorForAll = value;
                    RaisePropertyChanged();

                    foreach (var epicVM in Epics)
                    {
                        epicVM.SelectedStoryColor = epicVM.StoryColors.Single(x => x.Description == SelectedEpicColorForAll.Description);
                        epicVM.UpdateItems();
                    }
                }
            }
        }

        public void AddNewStory()
        {
            var storyViewModel = new StoryViewModel();
            storyViewModel.Initialize(this, new Story { Id = "EONE-", Priority = 0 }, false, false);
            Stories.Add(storyViewModel);

            _jiraService.UpdateStory(Stories.Last().Story);

            foreach (var storyVM in Stories)
            {
                storyVM.UpdateItems();
            }
        }

        private void ExecutePrintStoriesCommand()
        {
            PrintStoryCards();
        }

        private bool CanExecutePrintStoriesCommand()
        {
            return true;
        }

        private void ExecutePrintEpicsCommand()
        {
            PrintEpicCards();
        }

        private bool CanExecutePrintEpicsCommand()
        {
            return true;
        }

        private async void GetSprints()
        {
            var sprints = new List<ActivSprint>();
            BusyIndicatorVisibility = Visibility.Visible;
            await Task.Factory.StartNew(() => CurrentArt = _jiraService.GetCurrentArt());
            AddEpics();
            CurrentArt.ActiveSprints.ForEach(x => Sprints.Add(x));
            SelectedSprint = Sprints.First();
            ChartViewModel.Initialize(CurrentArt);
            RaisePropertyChanged("CurrentArt");
            RaisePropertyChanged("ChartViewModel");
            ChartViewModel.Update();
            BusyIndicatorVisibility = Visibility.Hidden;
        }

        private void AddEpics()
        {
            Epics.Clear();

            foreach (var epic in CurrentArt.Epics)
            {
                var epicViewModel = new EpicViewModel();
                epicViewModel.Initialize(this, epic);
                Epics.Add(epicViewModel);
            }
        }

        private async void PrintStoryCards()
        {
            BusyIndicatorVisibility = Visibility.Visible;
            var stories = new List<Story>();
            foreach (var storyViewModel in Stories.Where(x => x.IsStoryToPrint))
            {
                stories.Add(
                    new Story
                    {
                        Id = storyViewModel.Id,
                        Priority = storyViewModel.Priority,
                        IsAdditionalStory = storyViewModel.IsAdditionalStory,
                        IsWithDetails = storyViewModel.IsWithDetails,
                        StoryColor = storyViewModel.SelectedStoryColor
                    });
            }

            await Task.Factory.StartNew(() => _jiraService.PrintStories(stories));
            BusyIndicatorVisibility = Visibility.Hidden;
        }

        private async void PrintEpicCards()
        {
            BusyIndicatorVisibility = Visibility.Visible;
            var stories = new List<Story>();
            foreach (var epicViewModel in Epics.Where(x => x.IsStoryToPrint))
            {
                stories.Add(
                    new Story
                    {
                        Id = epicViewModel.Id,
                        Priority = epicViewModel.Priority,
                        StoryColor = epicViewModel.SelectedStoryColor
                    });
            }

            await Task.Factory.StartNew(() => _jiraService.PrintStories(stories));
            BusyIndicatorVisibility = Visibility.Hidden;
        }
    }
}