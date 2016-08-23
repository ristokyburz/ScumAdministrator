using System.Collections.Generic;
using System.Linq;

namespace ScrumAdministrator.Server.Domain
{
    public class Art
    {
        public Art()
        {
            ActiveSprints = new List<ActivSprint>();
            ClosedSprints = new List<ClosedSprint>();
            Epics = new List<Epic>();
        }

        public string ArtDescription { get; set; }

        public int NumberStoriesDone
        {
            get
            {
                return ActiveSprints.Sum(x => x.NumberStoriesDone);
            }
        }

        public int NumberStoriesDvlInWork
        {
            get
            {
                return ActiveSprints.Sum(x => x.NumberStoriesDvlInWork);
            }
        }

        public int NumberStoriesDvlOpen
        {
            get
            {
                return ActiveSprints.Sum(x => x.NumberStoriesDvlOpen);
            }
        }

        public int NumberStoriesTotal
        {
            get
            {
                return NumberStoriesDvlOpen + NumberStoriesDvlInWork + NumberStoriesDone;
            }
        }

        public double NumberStoryPointsDone
        {
            get
            {
                return ActiveSprints.Sum(x => x.NumberStoryPointsDone);
            }
        }

        public double NumberStoryPointsDvlInWork
        {
            get
            {
                return ActiveSprints.Sum(x => x.NumberStoryPointsDvlInWork);
            }
        }

        public double NumberStoryPointsDvlOpen
        {
            get
            {
                return ActiveSprints.Sum(x => x.NumberStoryPointsDvlOpen);
            }
        }

        public double NumberStoryPointsTotal
        {
            get
            {
                return NumberStoryPointsDone + NumberStoryPointsDvlOpen + NumberStoryPointsDvlInWork;
            }
        }

        public double WeightedAverageStoryPoints
        {
            get
            {
                return ClosedSprints.Sum(x => x.CummulatedStoryPointsFromRatio);
            }
        }

        public double CapacityFactor
        {
            get
            {
                if (ActiveSprints.Count > 0)
                {
                    return WeightedAverageStoryPoints / ActiveSprints.Single(x => x.IsCurrent).NumberStoryPointsDone;
                }

                return 0;
            }
        }

        public double AverageStoryPoints
        {
            get
            {
                return ClosedSprints.Sum(x => x.NumberStoryPointsDone) / ClosedSprints.Count;
            }
        }

        public List<ActivSprint> ActiveSprints { get; set; }

        public List<ClosedSprint> ClosedSprints { get; set; }

        public List<Epic> Epics { get; set; }
    }
}
