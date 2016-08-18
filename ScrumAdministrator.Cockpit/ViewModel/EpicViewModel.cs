using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PdfSharp.Drawing;
using ScrumAdministrator.Server.Domain;
using ScrumAdministrator.Server.Service;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ScrumAdministrator.Cockpit.ViewModel
{
    public class EpicViewModel : ViewModelBase
    {
        private JiraService _jiraService;
        private StoryOverviewViewModel _parentViewModel;
        private StoryColor _selectedStoryColor;
        private bool _isStoryToPrint;

        public EpicViewModel()
        {
            OpenUrlCommand = new RelayCommand(ExecuteOpenUrl);
            StoryColors = new List<StoryColor>
            {
                new StoryColor { Description = "Jira", ColorBrush = new XSolidBrush(XColor.FromArgb(255, 255, 255)) },
                new StoryColor { Description = "Yellow", ColorBrush = new XSolidBrush(XColor.FromArgb(200, 255, 225, 65)) },
                new StoryColor { Description = "Blue", ColorBrush = new XSolidBrush(XColor.FromArgb(200, 71, 136, 222)) },
                new StoryColor { Description = "Green", ColorBrush = new XSolidBrush(XColor.FromArgb(200, 119, 181, 119)) },
                new StoryColor { Description = "Red", ColorBrush = new XSolidBrush(XColor.FromArgb(200, 242, 100, 90)) },
                new StoryColor { Description = "White", ColorBrush = new XSolidBrush(XColor.FromArgb(255, 255, 255)) }
            };

            SelectedStoryColor = StoryColors.Single(x => x.Description == "White");
            _jiraService = new JiraService();
            IsStoryToPrint = true;
        }

        public RelayCommand OpenUrlCommand { get; private set; }

        public void Initialize(
            StoryOverviewViewModel parentViewModel,
            Story story)
        {
            Story = story;
            SetJiraColor();
            _parentViewModel = parentViewModel;
        }

        private void SetJiraColor()
        {
            if (Story.JiraStory.CustomFields["Epic Color"] != null)
            {
                string epicColor = Story.JiraStory.CustomFields["Epic Color"].Values.First();
                switch (epicColor)
                {
                    case "ghx-label-1":
                        StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(200, 129, 91, 58));
                        break;
                    case "ghx-label-3":
                        StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(200, 211, 156, 63));
                        break;
                    case "ghx-label-5":
                        StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(200, 74, 103, 133));
                        break;
                    case "ghx-label-6":
                        StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(200, 142, 176, 33));
                        break;
                    case "ghx-label-8":
                        StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(200, 101, 73, 130));
                        break;
                    case "ghx-label-9":
                        StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(200, 241, 92, 117));
                        break;
                    default:
                        StoryColors.Single(x => x.Description == "Jira").ColorBrush = new XSolidBrush(XColor.FromArgb(255, 255, 255));
                        break;
                }
            }
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

        public void UpdateItems()
        {
            RaisePropertyChanged("AddStoryVisibility");
            RaisePropertyChanged("SelectedStoryColor");
            RaisePropertyChanged("Status");
            RaisePropertyChanged("StoryPoints");
            RaisePropertyChanged("Title");
            RaisePropertyChanged("Url");
        }

        private void ExecuteAddStory()
        {
            _parentViewModel.AddNewStory();
        }

        private bool CanExecuteAddStory()
        {
            return true;
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
