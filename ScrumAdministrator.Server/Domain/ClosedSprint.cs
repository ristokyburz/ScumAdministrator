using System;
using System.Linq;

namespace ScrumAdministrator.Server.Domain
{
    public class ClosedSprint : Sprint
    {
        public Art ParentArt { get; set; }

        public double DistribuationValue { get; set; }

        public double ExpDistribuationValue
        {
            get
            {
                return Math.Exp(DistribuationValue);
            }
        }

        public double CummulatedStoryPointsFromRatio
        {
            get
            {
                double totalExpDistribuationValue = ParentArt.ClosedSprints.Sum(x => x.ExpDistribuationValue);
                double calculationRatio = ExpDistribuationValue / totalExpDistribuationValue;

                return NumberStoryPointsDone * calculationRatio;
            }
        }
    }
}
