#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using System.Windows.Input;
using ActivPass.ViewModels;

namespace ActivPass
{
    /// <summary>
    /// Viewlogic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            //Check if all login data are available
            if (this.DataContext is MainViewModel vm &&
                this.ContainerSelector.SelectedItem is string containerName) {

                //Forward the login event to the view model
                vm.OpenContainer(containerName, this.MasterPassword.Password);
            }
        }
    }
}
