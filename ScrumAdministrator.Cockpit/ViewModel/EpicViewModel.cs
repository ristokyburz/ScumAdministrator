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
using System.Windows.Media;

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
            Epic epic)
        {
            Epic = epic;
            SetJiraColor();
            _parentViewModel = parentViewModel;
        }

        private void SetJiraColor()
        {
            if (Epic.JiraStory.CustomFields["Epic Color"] != null)
            {
                string epicColor = Epic.JiraStory.CustomFields["Epic Color"].Values.First();
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

        public Epic Epic { get; set; }

        public string Id
        {
            get { return Epic.Id; }
            set
            {
                if (Epic.Id != value)
                {
                    Epic.Id = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int Priority
        {
            get { return Epic.Priority; }
            set
            {
                if (Epic.Priority != value)
                {
                    Epic.Priority = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Title
        {
            get
            {
                if (Epic != null && Epic.JiraStory != null)
                {
                    int maxLengthOfSummery = 48;
                    if (Epic.JiraStory.Summary.Length > maxLengthOfSummery)
                    {
                        return string.Format("{0}...", Epic.JiraStory.Summary.Substring(0, maxLengthOfSummery));
                    }
                    else
                    {
                        return Epic.JiraStory.Summary;
                    }

                }

                return string.Empty;
            }
        }

        public string Url
        {
            get { return Epic.StoryJiraUrl; }
        }

        public int StoryPointsTotal
        {
            get { return (int)Epic.StoryPointsTotal; }
        }

        public int StoryPointsOpen
        {
            get { return (int)Epic.StoryPointsOpen; }
        }

        public int StoryPointsInProgress
        {
            get { return (int)Epic.StoryPointsInProgress; }
        }

        public int StoryPointsDone
        {
            get { return (int)Epic.StoryPointsDone; }
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

        public Brush StatusColor
        {
            get
            {
                if (StoryPointsTotal == 0)
                {
                    return new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }

                double part = (double)StoryPointsDone / (double)StoryPointsTotal;

                if (part == 1)
                {
                    return new SolidColorBrush(Color.FromArgb(150, 153, 204, 153));
                }
                else if (part > 0.8)
                {
                    return new SolidColorBrush(Color.FromArgb(150, 227, 234, 90));
                }
                else if (part > 0.4)
                {
                    return new SolidColorBrush(Color.FromArgb(150, 245, 182, 34));
                }
                else if (part > 0.2)
                {
                    return new SolidColorBrush(Color.FromArgb(150, 255, 194, 122));
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(150, 253, 191, 156));
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
            RaisePropertyChanged("StoryPointsTotal");
            RaisePropertyChanged("StoryPointsOpen");
            RaisePropertyChanged("StoryPointsInProgress");
            RaisePropertyChanged("StoryPointsDone");
            RaisePropertyChanged("StatusColor");
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
    }
}
