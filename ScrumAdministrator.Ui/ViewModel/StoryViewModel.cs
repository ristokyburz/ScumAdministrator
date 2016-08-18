using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ScrumAdministrator.Ui.ViewModel
{
    public class StoryViewModel : ViewModelBase
    {
        private StoryOverviewViewModel _parentViewModel;
        private int _id;
        private int _priority;
        private bool _isAdditionalStory;
        private bool _isWithDetails;

        public StoryViewModel()
        {
            AddStoryCommand = new RelayCommand(ExecuteAddStory, CanExecuteAddStory);
        }

        public RelayCommand AddStoryCommand { get; private set; }

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    RaisePropertyChanged();
                }
            }
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

        public void Initialize(
            StoryOverviewViewModel parentViewModel, 
            int id,
            int priority,
            bool isAdditionalStory,
            bool isWithDetails)
        {
            _parentViewModel = parentViewModel;
            Id = id;
            Priority = priority;
            IsAdditionalStory = isAdditionalStory;
            IsWithDetails = isWithDetails;
        }

        private bool CanExecuteAddStory()
        {
            return true;
        }

        private void ExecuteAddStory()
        {
            _parentViewModel.AddNewStory();
        }
    }
}
