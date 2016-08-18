using GalaSoft.MvvmLight;
using ScrumAdministrator.Cockpit.Model;

namespace ScrumAdministrator.Cockpit.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MainViewModel()
        {
            StoryOverviewViewModel = new StoryOverviewViewModel();
        }

        private readonly IDataService _dataService;

        private StoryOverviewViewModel _storyOverview;

        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        return;
                    }

                });
        }

        public StoryOverviewViewModel StoryOverviewViewModel
        {
            get { return _storyOverview; }
            set
            {
                if (_storyOverview != value)
                {
                    _storyOverview = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}