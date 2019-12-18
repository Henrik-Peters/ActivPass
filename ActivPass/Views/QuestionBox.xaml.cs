#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;

namespace ActivPass.Views
{
    /// <summary>
    /// Viewlogic for QuestionBox.xaml
    /// </summary>
    public partial class QuestionBox : Window
    {
        /// <summary>
        /// If the confirm button was pressed.
        /// </summary>
        public bool ConfirmResult = false;

        /// <summary>
        /// Create a new question box dialog with custom content.
        /// </summary>
        /// <param name="message">Main message to display</param>
        /// <param name="title">Title of the dialog</param>
        public QuestionBox(string message, string title)
        {
            InitializeComponent();

            //Set the dialog texts
            this.Title = title;
            this.QuestionLabel.Content = message;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmResult = false;
            this.Close();
        }
    }
}
