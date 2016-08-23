using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using ScrumAdministrator.Server.DataAccess;
using ScrumAdministrator.Server.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ScrumAdministrator.Server.Service
{
    public class JiraService
    {
        private readonly JiraRepository _jiraRepository;

        public JiraService()
        {
            _jiraRepository = new JiraRepository();
        }

        public Art GetCurrentArt()
        {
            return _jiraRepository.GetCurrentArt();
        }

        public Story GetStory(string id)
        {
            return _jiraRepository.GetStory(id);
        }

        public void PrintStories(List<Story> stories)
        {
            _jiraRepository.LoadIssuesOfStories(stories);
            Print(stories);
        }

        public void UpdateStory(Story story)
        {
            _jiraRepository.MoveStory(story, 0);
        }

        private void Print(List<Story> stories)
        {
            var document = new PdfDocument();
            PdfPage page = null;
            XGraphics gfx = null;

            if (stories.Any())
            {
                for (int i = 0; i < stories.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        DrawRectangles(gfx, stories[i], false);
                    }
                    else
                    {
                        DrawRectangles(gfx, stories[i], true);
                    }
                }

                string filename = string.Format("Stories_{0}.pdf", Guid.NewGuid().ToString());
                document.Save(filename);
                Process.Start(filename);
            }
        }

        private void DrawRectangles(XGraphics gfx, Story story, bool includeOffset)
        {
            int offset = 0;
            if (includeOffset)
            {
                offset = 400;
            }

            var tf = new XTextFormatter(gfx);

            XPen pen = new XPen(XColors.Black, 2);

            string storyPoints = string.Empty;
            if (story.JiraStory.CustomFields["Story Points"] != null && story.JiraStory.CustomFields["Story Points"].Values.First() != "0")
            {
                storyPoints = story.JiraStory.CustomFields["Story Points"].Values.First();

                if (storyPoints == "0.5")
                {
                    storyPoints = ".5";
                }
            }

            var xRectStoryPoint = new XRect(20, 30 + offset, 100, 100);
            var xRectKey = new XRect(130, 30 + offset, 440, 100);
            var xRectKeyText = new XRect(135, 35 + offset, 415, 95);
            var xRectKeyEpic = new XRect(20, 30 + offset, 550, 100);
            var xRectKeyEpicText = new XRect(80, 30 + offset, 525, 95);
            var xRectSummery = new XRect(20, 140 + offset, 550, 270);
            var xRectSummeryText = new XRect(30, 150 + offset, 520, 210);
            var xRectDescription = new XRect(20, 180 + offset, 550, 220);
            var xRectDescriptionText = new XRect(30, 190 + offset, 520, 180);
            var xRectPriority = new XRect(440, 370 + offset, 130, 40);
            gfx.DrawRectangle(pen, XBrushes.LightGray, xRectPriority);

            if (story.JiraStory.Type.Name == "Epic")
            {
                gfx.DrawRectangle(pen, story.StoryColor.ColorBrush, xRectKeyEpic);
                var fontKey = new XFont("Verdana", 80, XFontStyle.Bold);
                tf.DrawString(story.JiraStory.Key.ToString(), fontKey, XBrushes.Black, xRectKeyEpicText, XStringFormats.TopLeft);
            }
            else
            {
                gfx.DrawRectangle(pen, xRectStoryPoint);
                var fontStoryPoint = new XFont("Verdana", 80, XFontStyle.Bold);
                gfx.DrawString(storyPoints, fontStoryPoint, XBrushes.Black, xRectStoryPoint, XStringFormats.Center);

                gfx.DrawRectangle(pen, story.StoryColor.ColorBrush, xRectKey);
                var fontKey = new XFont("Verdana", 67, XFontStyle.Bold);
                tf.DrawString(story.JiraStory.Key.ToString(), fontKey, XBrushes.Black, xRectKeyText, XStringFormats.TopLeft);
            }

            gfx.DrawRectangle(pen, xRectSummery);

            var fontPriority = new XFont("Verdana", 35, XFontStyle.Bold);
            gfx.DrawString(string.Format("#{0}", story.Priority.ToString()), fontPriority, XBrushes.Black, xRectPriority, XStringFormats.Center);

            if (story.IsAdditionalStory)
            {
                var image = XImage.FromFile("Images/star.png");
                gfx.DrawImage(image, 395, 365 + offset);
            }

            if (story.JiraStory.Summary != null)
            {
                if (!story.IsWithDetails)
                {
                    if (story.JiraStory.Type.Name == "Epic")
                    {
                        var fontSummery = new XFont("Verdana", 50, XFontStyle.Bold);
                        tf.DrawString(story.JiraStory.Summary, fontSummery, XBrushes.Black, xRectSummeryText, XStringFormats.TopLeft);
                    }
                    else
                    {
                        var fontSummery = new XFont("Verdana", 32);
                        tf.DrawString(story.JiraStory.Summary, fontSummery, XBrushes.Black, xRectSummeryText, XStringFormats.TopLeft);
                    }
                }
                else
                {
                    var fontSummery = new XFont("Verdana", 16);
                    tf.DrawString(story.JiraStory.Summary, fontSummery, XBrushes.Black, xRectSummeryText, XStringFormats.TopLeft);

                    if (story.JiraStory.Description != null)
                    {
                        var description = story.JiraStory.Description;

                        if (description.Length > 650)
                        {
                            description = string.Format("{0} ...", description.Substring(0, 650));
                        }

                        var fontDescription = new XFont("Verdana", 10);
                        tf.DrawString(description, fontDescription, XBrushes.Black, xRectDescriptionText, XStringFormats.TopLeft);
                    }
                }
            }
        }

        //private XSolidBrush GetColor(double storyPoint)
        //{
        //    if (storyPoint)

        //    switch (storyPoint)
        //    {
        //        case 1:
        //            return new XSolidBrush(XColor.FromArgb(255, 194, 232, 195));

        //        case 2:
        //            return new XSolidBrush(XColor.FromArgb(255, 131, 207, 133));

        //        case 3:
        //            return new XSolidBrush(XColor.FromArgb(255, 221, 216, 117));

        //        case 5:
        //            return new XSolidBrush(XColor.FromArgb(255, 246, 180, 92));

        //        case 8:
        //            return new XSolidBrush(XColor.FromArgb(255, 228, 138, 110));

        //        default:
        //            return new XSolidBrush(XColor.FromArgb(255, 255, 255, 255));

        //    }
        //}
    }
}
