﻿using ScrumAdministrator.Cockpit.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace ScrumAdministrator.Cockpit
{
    /// <summary>
    /// Description for StoryOverviewView.
    /// </summary>
    public partial class StoryOverviewView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the StoryOverviewView class.
        /// </summary>
        public StoryOverviewView()
        {
            var storyOverViewModel = new StoryOverviewViewModel();
            DataContext = storyOverViewModel;
            InitializeComponent();
            storyOverViewModel.Initialize();
        }
    }
}