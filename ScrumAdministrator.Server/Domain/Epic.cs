using System.Collections.Generic;
using System.Linq;

namespace ScrumAdministrator.Server.Domain
{
    public class Epic : Story
    {
        public Epic()
        {
            Stories = new List<Story>();
        }

        public double StoryPointsTotal
        {
            get
            {
                return Stories
                    .Where(x =>
                        x.JiraStory != null)
                            .Sum(x =>
                                x.JiraStory.CustomFields["Story Points"] != null ?
                                    double.Parse(x.JiraStory.CustomFields["Story Points"].Values.First()) : 0);
            }
        }

        public double StoryPointsOpen
        {
            get
            {
                return Stories
                    .Where(x =>
                        x.JiraStory != null &&
                        x.JiraStory.Status.Id == "1" || x.JiraStory.Status.Id == "12214")
                            .Sum(x =>
                                x.JiraStory.CustomFields["Story Points"] != null ?
                                    double.Parse(x.JiraStory.CustomFields["Story Points"].Values.First()) : 0);
            }
        }

        public double StoryPointsInProgress
        {
            get
            {
                return Stories
                    .Where(x =>
                        x.JiraStory != null &&
                        x.JiraStory.Status.Id == "10826")
                            .Sum(x =>
                                x.JiraStory.CustomFields["Story Points"] != null ?
                                    double.Parse(x.JiraStory.CustomFields["Story Points"].Values.First()) : 0);
            }
        }

        public double StoryPointsDone
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

        public List<Story> Stories { get; set; }
    }
}
