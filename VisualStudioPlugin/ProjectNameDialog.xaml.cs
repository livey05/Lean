﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using Microsoft.VisualStudio.PlatformUI;
using System.Collections.Generic;
using System;

namespace QuantConnect.VisualStudioPlugin
{
    /// <summary>
    /// Dialog window to select a project in QuantConnect that will be
    /// used to save selected files to
    /// </summary>
    public partial class ProjectNameDialog : DialogWindow
    {
        private bool _projectNameProvided = false;
        private string _selectedProjectName = null;
        private int? _selectedProjectId = 0;

        /// <summary>
        /// Create ProjectNameDialog
        /// </summary>
        /// <param name="projects">List of projects for a user to select from, a project is represented as a tuple with project id and name.</param>
        /// <param name="suggestedProjectName"></param>
        public ProjectNameDialog(List<Tuple<int, string>> projects, string suggestedProjectName)
        {
            InitializeComponent();
            SetProjectNames(projects);
            SetSuggestedProjectName(suggestedProjectName);
        }

        private void SetProjectNames(List<Tuple<int, string>> projects)
        {
            projects.ForEach(p => projectNameBox.Items.Add(new ComboboxItem(p.Item1, p.Item2)));
        }

        private void SetSuggestedProjectName(string suggestedProjectName)
        {
            projectNameBox.Text = suggestedProjectName;
        }

        private void SelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedItem = projectNameBox.SelectedItem as ComboboxItem;

            if (selectedItem != null)
            {
                var projectId = selectedItem.ProjectId;
                var projectName = selectedItem.ProjectName;
                SaveSelectedProjectName(projectId, projectName);
                Close();
            }
            else if (projectNameBox.Text.Length != 0)
            {
                SaveSelectedProjectName(null, projectNameBox.Text);
                Close();
            }
            else
            {
                DisplayProjectNameError();
            }
        }

        private void DisplayProjectNameError()
        {
            projectNameBox.BorderBrush = System.Windows.Media.Brushes.Red;
            projectNameBox.ToolTip = "Error occurred with the data of the control.";
        }

        private void SaveSelectedProjectName(int? projectId, string projectName)
        {
            _projectNameProvided = true;
            _selectedProjectName = projectName;
            _selectedProjectId = projectId;
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Check if user selected a valid project name
        /// </summary>
        /// <returns>True if a valid project name was selected, false otherwise</returns>
        public bool ProjectNameProvided()
        {
            return _projectNameProvided;
        }

        /// <summary>
        /// Get selected project name
        /// </summary>
        /// <returns>Selected project name</returns>
        public string GetSelectedProjectName()
        {
            return _selectedProjectName;
        }

        /// <summary>
        /// Get an id of a selected projected
        /// </summary>
        /// <returns>Id of a selected project if an existing project name was selected, null otherwise</returns>
        public int? GetSelectedProjectId()
        {
            return _selectedProjectId;
        }

        /// <summary>
        /// Item that represents project name and project id in a combo box
        /// </summary>
        private class ComboboxItem {
            public int ProjectId { get; }
            public string ProjectName { get; }

            public ComboboxItem(int projectId, string projectName)
            {
                ProjectId = projectId;
                ProjectName = projectName;
            }

            public override string ToString()
            {
                return ProjectName;
            }
        }
    }
}
