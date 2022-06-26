#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using ActivPass.ViewModels;

namespace ActivPass
{
    /// <summary>
    /// Viewlogic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// View model instance for this window.
        /// </summary>
        private MainViewModel vm;

        /// <summary>
        /// The current item view model from the mouse hover.
        /// </summary>
        private PasswordItemViewModel mouseOverItem;

        public MainWindow()
        {
            InitializeComponent();

            //View model initialisation
            this.vm = new MainViewModel(this.FindResource("MainMenu") as ContextMenu, this.MasterPassword, this.SearchBox);
            this.DataContext = this.vm;
            this.mouseOverItem = null;

            //Window maximize on startup
            if (MainViewModel.Config.StartupLayout == Configuration.WindowStartupState.Maximised) {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ContainerLogin();
        }

        private void MasterPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                ContainerLogin();
                e.Handled = true;
            }
        }

        private void ContainerLogin()
        {
            if (this.ContainerSelector.SelectedItem is string containerName) {

                //Forward the login event to the view model
                vm.OpenContainer(containerName, this.MasterPassword.Password);
            }
        }

        private void PassItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //Store the current data context to allow copying item data later
            if (sender is Border senderElement && senderElement.DataContext is PasswordItemViewModel itemViewModel) {
                mouseOverItem = itemViewModel;
            }
        }

        private void PassItem_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseOverItem = null;
        }

        private void ActivPassWindow_KeyUp(object sender, KeyEventArgs e)
        {
            //Reset inactivity
            vm.ResetInactivityTimer();

            //Copy the password to clipboard from the current hovered password item
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control && mouseOverItem != null) {
                Clipboard.SetText(mouseOverItem.Password, TextDataFormat.UnicodeText);
            }
        }

        private void ActivPassWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            vm.ResetInactivityTimer();
        }

        private void ActivPassWindow_MouseMove(object sender, MouseEventArgs e)
        {
            vm.ResetInactivityTimer();
        }
    }
}
