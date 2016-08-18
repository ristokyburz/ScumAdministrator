using GalaSoft.MvvmLight;
using ScrumAdministrator.Server.Domain;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ScrumAdministrator.Cockpit.ViewModel
{
    public class ChartViewModel : ViewModelBase
    {
        private const int FOKUSFACTOR = 2;
        private const int STROKETHIKNESS = 1;
        private const int YAXISDELTA = 25;

        public ChartViewModel()
        {

        }

        public void Initialize(Art currentArt)
        {
            CurrentArt = currentArt;
        }

        public void Update()
        {
            RaisePropertyChanged("MaxStoryPointsYAxis");
            RaisePropertyChanged("StoryPointsYAxis12");
            RaisePropertyChanged("StoryPointsYAxis11");
            RaisePropertyChanged("StoryPointsYAxis10");
            RaisePropertyChanged("StoryPointsYAxis9");
            RaisePropertyChanged("StoryPointsYAxis8");
            RaisePropertyChanged("StoryPointsYAxis7");
            RaisePropertyChanged("StoryPointsYAxis6");
            RaisePropertyChanged("StoryPointsYAxis5");
            RaisePropertyChanged("StoryPointsYAxis4");
            RaisePropertyChanged("StoryPointsYAxis3");
            RaisePropertyChanged("StoryPointsYAxis2");
            RaisePropertyChanged("StoryPointsYAxis1");
            RaisePropertyChanged("StoryPointsDonePoints");
            RaisePropertyChanged("StoryPointsInProgressPoints");
            RaisePropertyChanged("StoryPointsNotStartedPoints");
            RaisePropertyChanged("MaxStoryPointsXAxisScaled");
        }

        public Art CurrentArt { get; set; }

        public int MaxStoryPointsYAxis
        {
            get
            {
                if (CurrentArt != null)
                {
                    return 500;
                    // return (int)Math.Round(CurrentArt.NumberStoryPointsTotal / 10, MidpointRounding.AwayFromZero) * 10;
                }

                return 0;
            }
        }

        public double MaxStoryPointsXAxisScaled
        {
            get
            {
                if (CurrentArt != null)
                {
                    return CurrentArt.NumberStoryPointsTotal * FOKUSFACTOR;
                }

                return 0;
            }
        }

        public int StoryPointsYAxis12
        {
            get
            {
                return MaxStoryPointsYAxis - YAXISDELTA;
            }
        }

        public int StoryPointsYAxis11
        {
            get
            {
                return MaxStoryPointsYAxis - (2 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis10
        {
            get
            {
                return MaxStoryPointsYAxis - (3 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis9
        {
            get
            {
                return MaxStoryPointsYAxis - (4 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis8
        {
            get
            {
                return MaxStoryPointsYAxis - (5 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis7
        {
            get
            {
                return MaxStoryPointsYAxis - (6 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis6
        {
            get
            {
                return MaxStoryPointsYAxis - (7 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis5
        {
            get
            {
                return MaxStoryPointsYAxis - (8 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis4
        {
            get
            {
                return MaxStoryPointsYAxis - (9 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis3
        {
            get
            {
                return MaxStoryPointsYAxis - (10 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis2
        {
            get
            {
                return MaxStoryPointsYAxis - (11 * YAXISDELTA);
            }
        }

        public int StoryPointsYAxis1
        {
            get
            {
                return MaxStoryPointsYAxis - (12 * YAXISDELTA);
            }
        }

        public PointCollection StoryPointsDonePoints
        {
            get
            {
                if (CurrentArt != null)
                {
                    return new PointCollection(
                        new[]
                        {
                            new Point(0, 0),
                            new Point(100, StoryPointsDoneSprint1 * FOKUSFACTOR),
                            new Point(200, StoryPointsDoneSprint2 * FOKUSFACTOR),
                            new Point(300, StoryPointsDoneSprint3 * FOKUSFACTOR),
                            new Point(400, StoryPointsDoneSprint4 * FOKUSFACTOR),
                            new Point(500, StoryPointsDoneSprint5 * FOKUSFACTOR),
                            new Point(600, StoryPointsDoneSprint6 * FOKUSFACTOR),
                            new Point(600, 0),
                            new Point(0, 0),
                        });
                }

                return new PointCollection();
            }
        }

        public PointCollection StoryPointsInProgressPoints
        {
            get
            {
                if (CurrentArt != null)
                {
                    return new PointCollection(
                        new[]
                        {
                            new Point(0, 0),
                            new Point(100, (StoryPointsDoneSprint1 + StoryPointsWipSprint1) * FOKUSFACTOR),
                            new Point(200, (StoryPointsDoneSprint2 + StoryPointsWipSprint2) * FOKUSFACTOR),
                            new Point(300, (StoryPointsDoneSprint3 + StoryPointsWipSprint3) * FOKUSFACTOR),
                            new Point(400, (StoryPointsDoneSprint4 + StoryPointsWipSprint4) * FOKUSFACTOR),
                            new Point(500, (StoryPointsDoneSprint5 + StoryPointsWipSprint5) * FOKUSFACTOR),
                            new Point(600, (StoryPointsDoneSprint6 + StoryPointsWipSprint6) * FOKUSFACTOR),
                            new Point(600, (StoryPointsDoneSprint6 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(500, (StoryPointsDoneSprint5 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(400, (StoryPointsDoneSprint4 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(300, (StoryPointsDoneSprint3 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(200, (StoryPointsDoneSprint2 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(100, (StoryPointsDoneSprint1 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(0, 0)
                        });
                }

                return new PointCollection();
            }
        }

        public PointCollection StoryPointsNotStartedPoints
        {
            get
            {
                if (CurrentArt != null)
                {
                    return new PointCollection(
                        new[]
                        {
                            new Point(0, 0),
                            new Point(0, CurrentArt.NumberStoryPointsTotal * FOKUSFACTOR),
                            new Point(600, CurrentArt.NumberStoryPointsTotal * FOKUSFACTOR),
                            new Point(600, (StoryPointsDoneSprint6 + StoryPointsWipSprint6 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(500, (StoryPointsDoneSprint5 + StoryPointsWipSprint5 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(400, (StoryPointsDoneSprint4 + StoryPointsWipSprint4 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(300, (StoryPointsDoneSprint3 + StoryPointsWipSprint3 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(200, (StoryPointsDoneSprint2 + StoryPointsWipSprint2 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(100, (StoryPointsDoneSprint1 + StoryPointsWipSprint1 + STROKETHIKNESS) * FOKUSFACTOR),
                            new Point(0, 0)
                        });
                }

                return new PointCollection();
            }
        }

        public int StoryPointsDoneSprint1
        {
            get
            {
                return (int)CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 1).NumberStoryPointsDone;
            }
        }

        public int StoryPointsDoneSprint2
        {
            get
            {
                return
                    (int)(StoryPointsDoneSprint1 + 
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 2).NumberStoryPointsDone);
            }
        }

        public int StoryPointsDoneSprint3
        {
            get
            {
                return
                    (int)(StoryPointsDoneSprint2 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 3).NumberStoryPointsDone);
            }
        }

        public int StoryPointsDoneSprint4
        {
            get
            {
                return
                    (int)(StoryPointsDoneSprint3 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 4).NumberStoryPointsDone);
            }
        }

        public int StoryPointsDoneSprint5
        {
            get
            {
                return
                    (int)(StoryPointsDoneSprint4 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 5).NumberStoryPointsDone);
            }
        }

        public int StoryPointsDoneSprint6
        {
            get
            {
                return
                    (int)(StoryPointsDoneSprint5 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 6).NumberStoryPointsDone);
            }
        }

        public int StoryPointsWipSprint1
        {
            get
            {
                return (int)CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 1).NumberStoryPointsDvlInWork;
            }
        }

        public int StoryPointsWipSprint2
        {
            get
            {
                return
                    (int)(StoryPointsWipSprint1 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 2).NumberStoryPointsDvlInWork);
            }
        }

        public int StoryPointsWipSprint3
        {
            get
            {
                return
                    (int)(StoryPointsWipSprint2 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 3).NumberStoryPointsDvlInWork);
            }
        }

        public int StoryPointsWipSprint4
        {
            get
            {
                return
                    (int)(StoryPointsWipSprint3 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 4).NumberStoryPointsDvlInWork);
            }
        }

        public int StoryPointsWipSprint5
        {
            get
            {
                return
                    (int)(StoryPointsWipSprint4 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 5).NumberStoryPointsDvlInWork);
            }
        }

        public int StoryPointsWipSprint6
        {
            get
            {
                return
                    (int)(StoryPointsWipSprint5 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 6).NumberStoryPointsDvlInWork);
            }
        }

        public int StoryPointsNotStartedSprint1
        {
            get
            {
                return (int)CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 1).NumberStoryPointsDvlOpen;
            }
        }

        public int StoryPointsNotStartedSprint2
        {
            get
            {
                return
                    (int)(StoryPointsNotStartedSprint1 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 2).NumberStoryPointsDvlOpen);
            }
        }

        public int StoryPointsNotStartedSprint3
        {
            get
            {
                return
                    (int)(StoryPointsNotStartedSprint2 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 3).NumberStoryPointsDvlOpen);
            }
        }

        public int StoryPointsNotStartedSprint4
        {
            get
            {
                return
                    (int)(StoryPointsNotStartedSprint3 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 4).NumberStoryPointsDvlOpen);
            }
        }

        public int StoryPointsNotStartedSprint5
        {
            get
            {
                return
                    (int)(StoryPointsNotStartedSprint4 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 5).NumberStoryPointsDvlOpen);
            }
        }

        public int StoryPointsNotStartedSprint6
        {
            get
            {
                return
                    (int)(StoryPointsNotStartedSprint5 +
                    CurrentArt.ActiveSprints.Single(x => x.SortedInternalId == 6).NumberStoryPointsDvlOpen);
            }
        }
    }
}
