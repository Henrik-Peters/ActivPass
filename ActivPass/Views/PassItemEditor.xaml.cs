#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using System.Windows.Input;
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
        public PassItemEditorViewModel vm;

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

            //Check if the inital input is valid
            this.ValidateEditorItem();
        }

        private void PassTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            PassBox.Password = this.vm.Password;
            this.vm.UpdatePasswordScore();
            ValidateEditorItem();
        }

        private void PassBox_KeyUp(object sender, KeyEventArgs e)
        {
            this.vm.Password = PassBox.Password;
            this.vm.UpdatePasswordScore();
            ValidateEditorItem();
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

        private void GeneratePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            //Show the password generator view
            PassGenerator passGenerator = new();
            passGenerator.ShowDialog();

            //Get the view model of the dialog
            var dialogVm = passGenerator.DataContext as PassGeneratorViewModel;

            if (dialogVm.ApplyResult) {
                this.vm.Password = dialogVm.Password;

                //Sync the view model password with the password box
                PassBox.Password = this.vm.Password;
                this.vm.UpdatePasswordScore();
                ValidateEditorItem();
            }
        }

        private void UsernameBox_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateEditorItem();
        }

        private void NameBox_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateEditorItem();
        }

        private void UrlBox_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateEditorItem();
        }

        private void NotesBox_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateEditorItem();
        }

        /// <summary>
        /// Check if the current item could be stored as a valid
        /// item and enable or disable the the save button.
        /// </summary>
        private void ValidateEditorItem()
        {
            //Check if all text boxes are non empty
            if (NameBox.Text == string.Empty || UsernameBox.Text == string.Empty || PassBox.Password == string.Empty) {
                this.SaveButton.IsEnabled = false;
            } else {
                this.SaveButton.IsEnabled = true;
            }
        }

        private void UrlBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.vm.UpdateUrlWarning();
        }
    }
}
