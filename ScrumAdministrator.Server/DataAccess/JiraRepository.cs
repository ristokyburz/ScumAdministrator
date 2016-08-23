using Atlassian.Jira;
using ScrumAdministrator.Server.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAdministrator.Server.DataAccess
{
    public class JiraRepository
    {
        Jira _jiraRestClient;

        public JiraRepository()
        {
            string address = ConfigurationManager.AppSettings["JiraAddress"];
            string username = ConfigurationManager.AppSettings["JiraUsername"];
            string password = ConfigurationManager.AppSettings["JiraPassword"];
            _jiraRestClient = Jira.CreateRestClient(address, username, password);
            _jiraRestClient.MaxIssuesPerRequest = 50;
        }

        public Art GetCurrentArt()
        {
            var art = new Art();

            AddActiveSprints(art);
            AddClosedSprints(art);
            AddEpics(art);

            return art;
        }

        public void LoadIssuesOfStories(List<Story> stories)
        {
            var issueKeys = new List<ComparableString>();
            var jiraStories = new List<Issue>();
            var invalidStories = new List<Story>();

            foreach (var story in stories)
            {
                try
                {
                    story.JiraStory = _jiraRestClient.GetIssue(story.Id);
                }
                catch (Exception)
                {
                    invalidStories.Add(story);
                    continue;
                }
            }

            foreach (var invalidStory in invalidStories)
            {
                stories.Remove(invalidStory);
            }
        }

        public void MoveStory(Story story, int newSprintId)
        {
            var issue = _jiraRestClient.GetIssue("EONE-5571");
            issue.CustomFields["Story Points"].Values[0] = "1";
            
            issue.SaveChanges();
            Task.Run<Issue>(() => _jiraRestClient.RestClient.UpdateIssueAsync(issue, new System.Threading.CancellationToken()));
        }

        private void AddActiveSprints(Art art)
        {
            //_jiraRestClient.RestClient
            var sprintIdWithDescriptions = ConfigurationManager.AppSettings["Sprints"].Split('|').ToList();
            int internalId = 1;

            foreach (var sprintIdWithDescription in sprintIdWithDescriptions)
            {
                int sprintId = int.Parse(sprintIdWithDescription.Split(';')[0]);
                string description = sprintIdWithDescription.Split(';')[1];
                bool isCurrent = bool.Parse(sprintIdWithDescription.Split(';')[2]);
                art.ActiveSprints.Add(new ActivSprint { Id = sprintId, Description = description, SortedInternalId = internalId, IsCurrent = isCurrent });
                internalId++;
            }

            foreach (var sprint in art.ActiveSprints)
            {
                var jiraStories = _jiraRestClient.GetIssuesFromJql(string.Format("sprint={0} AND (type=Story OR type=Bug OR type=Kaizen)", sprint.Id));
                int priority = 10;
                foreach (var jiraStory in jiraStories.OrderBy(x => x.CustomFields["Rank"].Values.First()))
                {
                    var story = new Story
                    {
                        Id = jiraStory.Key.ToString(),
                        JiraStory = jiraStory,
                        Priority = priority
                    };

                    sprint.Stories.Add(story);
                    priority += 10;
                }
            }
        }

        private void AddClosedSprints(Art art)
        {
            var sprintIdWitthDistribuationXValues = ConfigurationManager.AppSettings["StatisticSprints"].Split('|').ToList();
            int internalId = 1;

            foreach (var sprintIdWitthDistribuationXValue in sprintIdWitthDistribuationXValues)
            {
                int sprintId = int.Parse(sprintIdWitthDistribuationXValue.Split(';').First());
                double distribuationXValue = double.Parse(sprintIdWitthDistribuationXValue.Split(';').Last());

                var activSprintFromArt = art.ActiveSprints.SingleOrDefault(x => x.Id == sprintId);
                if (activSprintFromArt != null)
                {
                    var closedSprint = new ClosedSprint { Id = sprintId, SortedInternalId = internalId, DistribuationValue = distribuationXValue, ParentArt = art };
                    closedSprint.Stories.AddRange(activSprintFromArt.Stories);
                    art.ClosedSprints.Add(closedSprint);
                    
                }
                else
                {
                    var jiraStories = _jiraRestClient.GetIssuesFromJql(string.Format("sprint={0} AND (type=Story OR type=Bug OR type=Kaizen)", sprintId));
                    var closedSprint = new ClosedSprint { Id = sprintId, SortedInternalId = internalId, DistribuationValue = distribuationXValue, ParentArt = art };
                    foreach (var jiraStory in jiraStories.OrderBy(x => x.CustomFields["Rank"].Values.First()))
                    {
                        var story = new Story
                        {
                            Id = jiraStory.Key.ToString(),
                            JiraStory = jiraStory,
                        };

                        closedSprint.Stories.Add(story);
                    }

                    art.ClosedSprints.Add(closedSprint);
                }

                internalId++;
            }
        }

        private void AddEpics(Art art)
        {
            var epicIds = new List<string>();
            var epicIdsString = string.Empty;
            var allStories = art.ActiveSprints.SelectMany(x => x.Stories);

            foreach (var story in art.ActiveSprints.SelectMany(x => x.Stories))
            {
                if (story.JiraStory.CustomFields["Epic Link"] != null)
                {
                    epicIds.Add(story.JiraStory.CustomFields["Epic Link"].Values.First());
                }
            }

            epicIds = epicIds.Distinct().ToList();

            for (int i = 0; i < epicIds.Count; i++)
            {
                if (i == 0)
                {
                    epicIdsString += string.Format("id in ({0}", epicIds[i]);
                }
                else
                {
                    epicIdsString += string.Format(", {0}", epicIds[i]);
                }
            }

            epicIdsString += ")";

            var jiraIssues = _jiraRestClient.GetIssuesFromJql(epicIdsString);

            int priority = 1;
            foreach (var jiraStory in jiraIssues)
            {
                var story = new Epic
                {
                    Id = jiraStory.Key.ToString(),
                    JiraStory = jiraStory,
                    Priority = priority,
                    Stories = allStories.Where(x => x.JiraStory.CustomFields["Epic Link"] != null && x.JiraStory.CustomFields["Epic Link"].Values.First() == jiraStory.Key.Value).ToList()
                };

                art.Epics.Add(story);
                priority += 1;
            }
        }

        public Story GetStory(string id)
        {
            try
            {
                var issue = _jiraRestClient.GetIssue(id);
                return new Story
                {
                    Id = issue.Key.ToString(),
                    JiraStory = issue,
                    Priority = 0
                };
            }
            catch (Exception)
            {
                return null;
            }
        } 
    }
}