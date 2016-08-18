using System.Collections.Generic;
using System.Linq;

namespace ScrumAdministrator.Server.Domain
{
    public class ActivSprint : Sprint
    {
        public string Description { get; set; }

        public int NumberStoriesDone
        {
            get
            {
                return Stories
                    .Where(x => 
                        x.StoryStatus == StoryStatusType.Done)
                    .Count();
            }
        }

        public int NumberStoriesDvlInWork
        {
            get
            {
                return Stories
                    .Where(x => 
                        x.StoryStatus == StoryStatusType.InProgress)
                    .Count();
            }
        }

        public int NumberStoriesDvlOpen
        {
            get
            {
                return Stories
                    .Where(x => 
                        x.StoryStatus == StoryStatusType.NotStarted)
                    .Count();
            }
        }

        public double NumberStoryPointsDvlInWork
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

        public double NumberStoryPointsDvlOpen
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
    }
}
