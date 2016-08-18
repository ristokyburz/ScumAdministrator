using System.Collections.Generic;
using System.Linq;

namespace ScrumAdministrator.Server.Domain
{
    public class Sprint
    {
        public Sprint()
        {
            Stories = new List<Story>();
        }

        public int Id { get; set; }

        public bool IsCurrent { get; set; }

        public int SortedInternalId { get; set; }

        public List<Story> Stories { get; set; }

        public double NumberStoryPointsDone
        {
            get
            {
                return Stories
                    .Where(x =>
                        x.JiraStory != null &&
                        x.JiraStory.Status.Id == "5")
                            .Sum(x =>
                                x.JiraStory.CustomFields["Story Points"] != null ?
                                    double.Parse(x.JiraStory.CustomFields["Story Points"].Values.First()) : 0);
            }
        }
    }
}
