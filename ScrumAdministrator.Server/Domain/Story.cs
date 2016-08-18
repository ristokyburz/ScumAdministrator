using Atlassian.Jira;
using PdfSharp.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace ScrumAdministrator.Server.Domain
{
    public class Story
    {
        public Story()
        {
            JiraTasks = new List<Issue>();
        }

        public string Id { get; set; }

        public int Priority { get; set; }

        public bool IsAdditionalStory { get; set; }

        public bool IsWithDetails { get; set; }

        public double StoryPoints
        {
            get
            {
                if (JiraStory != null && JiraStory.CustomFields["Story Points"] != null && JiraStory.CustomFields["Story Points"].Values.First() != "0")
                {
                    return double.Parse(JiraStory.CustomFields["Story Points"].Values.First());
                }

                return 0;
            }
        }

        public string StoryJiraUrl
        {
            get
            {
                return string.Format(@"https://dev.finnova.ch/jira/browse/{0}", Id);
            }
        }

        public Issue JiraStory { get; set; }

        public List<Issue> JiraTasks { get; set; }

        public StoryColor StoryColor { get; set; }

        public StoryStatusType StoryStatus
        {
            get
            {
                if (JiraStory != null)
                {
                    switch (JiraStory.Status.Id)
                    {
                        case "1": 
                        case "12214":
                            return StoryStatusType.NotStarted;

                        case "10826":
                            return StoryStatusType.InProgress;

                        case "5":
                            return StoryStatusType.Done;

                        case "12213":
                            return StoryStatusType.Rejected;
                        case "12209":
                            return StoryStatusType.ReadyForRefinement;

                        default:
                            return StoryStatusType.Unknown;
                    }
                }

                return StoryStatusType.Unknown;
            }
        }

    }

    public class StoryColor
    {
        public string Description { get; set; }

        public XSolidBrush ColorBrush { get; set; }
    }

    public enum StoryStatusType
    {
        Unknown,
        NotStarted,
        InProgress,
        Rejected,
        ReadyForRefinement,
        Done
    }
}
