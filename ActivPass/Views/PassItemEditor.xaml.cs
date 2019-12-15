#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using ActivPass.Models;
using ActivPass.ViewModels;

namespace ActivPass.Views
{
    /// <summary>
    /// Viewlogic for PassItemEditor.xaml
    /// </summary>
    public partial class PassItemEditor : Window
    {
        /// <summary>
        /// View model instance for this window.
        /// </summary>
        private PassItemEditorViewModel vm;

        /// <summary>
        /// If the password is visible as plain text.
        /// </summary>
        private bool passwordIsVisible;

        /// <summary>
        /// Create a new password item editor
        /// and viewer window instance.
        /// </summary>
        /// <param name="item">Init the editor values with this item</param>
        public PassItemEditor(PasswordItem item)
        {
            InitializeComponent();

            //View model initialisation
            this.vm = new PassItemEditorViewModel(item);
            this.DataContext = this.vm;

            //Inital values
            this.passwordIsVisible = false;
            this.PassTextBox.Visibility = Visibility.Hidden;

            //Set the inital password text value
            this.PassBox.Password = this.vm.Password;
        }

        private void PassTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            PassBox.Password = this.vm.Password;
        }

        private void PassBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            this.vm.Password = PassBox.Password;
        }

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            //Toggle the password visible flag
            passwordIsVisible = !passwordIsVisible;

            if (passwordIsVisible) {
                PassBox.Visibility = Visibility.Hidden;
                PassTextBox.Visibility = Visibility.Visible;
                PassTextBox.Focus();
                PassTextBox.Select(PassTextBox.Text.Length, 0);

            } else {
                PassBox.Visibility = Visibility.Visible;
                PassTextBox.Visibility = Visibility.Hidden;
                PassBox.Focus();
            }
        }
    }
}
