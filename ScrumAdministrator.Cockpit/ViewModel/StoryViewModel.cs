using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using ScrumAdministrator.Server.Domain;
using System.Collections.Generic;
using PdfSharp.Drawing;
using ScrumAdministrator.Server.Service;
using System.Diagnostics;

namespace ScrumAdministrator.Cockpit.ViewModel
{
    public class StoryViewModel : ViewModelBase
    {
        private JiraService _jiraService;
        private StoryOverviewViewModel _parentViewModel;
        private bool _isAdditionalStory;
        private bool _isWithDetails;
        private StoryColor _selectedStoryColor;
        private bool _isStoryToPrint;

        public StoryViewModel()
        {
            AddStoryCommand = new RelayCommand(ExecuteAddStory, CanExecuteAddStory);
            RemoveStoryCommand = new RelayCommand(ExecuteRemoveStory, CanExecuteRemoveStory);
            OpenUrlCommand = new RelayCommand(ExecuteOpenUrl);
            StoryColors = new List<StoryColor>
            {
                new StoryColor { Description = "Jira", ColorBrush = new XSolidBrush(XColor.FromArgb(255, 255, 255)) },
                new StoryColor { Description = "Yellow", ColorBrush = new XSolidBrush(XColor.FromArgb(255, 253, 165)) },
                new StoryColor { Description = "Blue", ColorBrush = new XSolidBrush(XColor.FromArgb(186, 230, 252)) },
                new StoryColor { Description = "Green", ColorBrush = new XSolidBrush(XColor.FromArgb(181, 237, 208)) },
                new StoryColor { Description = "Red", ColorBrush = new XSolidBrush(XColor.FromArgb(232, 190, 186)) },
                new StoryColor { Description = "White", ColorBrush = new XSolidBrush(XColor.FromArgb(255, 255, 255)) }
            };

            SelectedStoryColor = StoryColors.Single(x => x.Description == "White");
            _jiraService = new JiraService();
            IsStoryToPrint = true;
        }

        public RelayCommand AddStoryCommand { get; private set; }

        public RelayCommand RemoveStoryCommand { get; private set; }

        public RelayCommand OpenUrlCommand { get; private set; }

        public void Initialize(
            StoryOverviewViewModel parentViewModel,
            Story story,
            bool isAdditionalStory,
            bool isWithDetails)
        {
            Story = story;
            _parentViewModel = parentViewModel;
            IsAdditionalStory = isAdditionalStory;
            IsWithDetails = isWithDetails;
            SetJiraColor();
        }

        public Story Story { get; set; }

        public string Id
        {
            get { return Story.Id; }
            set
            {
                if (Story.Id != value)
                {
                    Story.Id = value;
                    RaisePropertyChanged();
                    LoadStory(Story.Id);
                }
            }
        }

        public int Priority
        {
            get { return Story.Priority; }
            set
            {
                if (Story.Priority != value)
                {
                    Story.Priority = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Title
        {
            get
            {
                if (Story != null && Story.JiraStory != null)
                {
                    int maxLengthOfSummery = 48;
                    if (Story.JiraStory.Summary.Length > maxLengthOfSummery)
                    {
                        return string.Format("{0}...", Story.JiraStory.Summary.Substring(0, maxLengthOfSummery));
                    }
                    else
                    {
                        return Story.JiraStory.Summary;
                    }
                    
                }

                return string.Empty;
            }
        }

        public string Url
        {
            get { return Story.StoryJiraUrl; }
        }

        public bool IsAdditionalStory
        {
            get { return _isAdditionalStory; }
            set
            {
                if (_isAdditionalStory != value)
                {
                    _isAdditionalStory = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsWithDetails
        {
            get { return _isWithDetails; }
            set
            {
                if (_isWithDetails != value)
                {
                    _isWithDetails = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsStoryToPrint
        {
            get { return _isStoryToPrint; }
            set
            {
                if (_isStoryToPrint != value)
                {
                    _isStoryToPrint = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Visibility AddStoryVisibility
        {
            get
            {
                if (this == _parentViewModel.Stories.Last())
                {
                    return Visibility.Visible;
                }

                return Visibility.Hidden;
            }
        }

        public List<StoryColor> StoryColors { get; set; }

        public StoryColor SelectedStoryColor
        {
            get { return _selectedStoryColor; }
            set
            {
                if (_selectedStoryColor != value)
                {
                    _selectedStoryColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double StoryPoints
        {
            get
            {
                return Story.StoryPoints;
            }
        }

        public string Status
        {
            get
            {
                switch (Story.StoryStatus)
                {
                    case StoryStatusType.Done:
                        return "Done";

                    case StoryStatusType.NotStarted:
                        return "Not Started";

                    case StoryStatusType.InProgress:
                        return "In Progress";

                    case StoryStatusType.Unknown:
                        return string.Empty;

                    case StoryStatusType.Rejected:
                        return "Rejected";

                    default:
                        return string.Empty;
                }
            }
        }

        public void UpdateItems()
        {
            RaisePropertyChanged("AddStoryVisibility");
            RaisePropertyChanged("SelectedStoryColor");
            RaisePropertyChanged("Status");
            RaisePropertyChanged("StoryPoints");
            RaisePropertyChanged("Title");
            RaisePropertyChanged("Url");
        }

        private void SetJiraColor()
        {
            if (Story.JiraStory != null)
            {
                if (Story.JiraStory.CustomFields["Epic Link"] != null)
                {
                    var epicId = Story.JiraStory.CustomFields["Epic Link"].Values.First();
                    var epic = _parentViewModel.Epics.SingleOrDefault(x => x.Story.Id == epicId);

                    if (epic != null)
                    {
                        if (epic.Story.JiraStory.CustomFields["Epic Color"] != null)
                        {
                            string epicColor = epic.Story.JiraStory.CustomFields["Epic Color"].Values.First();
                            switch (epicColor)
                            {
                                case "ghx-label-1":
                                    StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(150, 129, 91, 58));
                                    break;
                                case "ghx-label-3":
                                    StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(150, 211, 156, 63));
                                    break;
                                case "ghx-label-5":
                                    StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(150, 74, 103, 133));
                                    break;
                                case "ghx-label-6":
                                    StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(150, 142, 176, 33));
                                    break;
                                case "ghx-label-8":
                                    StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(150, 101, 73, 130));
                                    break;
                                case "ghx-label-9":
                                    StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(150, 241, 92, 117));
                                    break;
                                default:
                                    StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(255, 255, 255));
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void ExecuteAddStory()
        {
            _parentViewModel.AddNewStory();
        }

        private bool CanExecuteAddStory()
        {
            return true;
        }

        private void ExecuteRemoveStory()
        {
            _parentViewModel.Stories.Remove(this);

            foreach (var storyVM in _parentViewModel.Stories)
            {
                storyVM.UpdateItems();

            }
        }

        private bool CanExecuteRemoveStory()
        {
            if (_parentViewModel.Stories.Count > 1)
            {
                return true;
            }

            return false;
        }

        public void ExecuteOpenUrl()
        {
            Process.Start(Url);
        }

        private async void LoadStory(string id)
        {
            Story loadedStory = null;
            await Task.Factory.StartNew(() => loadedStory = _jiraService.GetStory(id));

            if (loadedStory != null)
            {
                Story = loadedStory;
            }
            else
            {
                Story = new Story();
            }

            UpdateItems();
        }
    }
}