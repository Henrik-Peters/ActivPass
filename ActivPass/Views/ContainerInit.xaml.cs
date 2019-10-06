#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using ActivPass.ViewModels;

namespace ActivPass.Views
{
    /// <summary>
    /// Viewlogic for ContainerInit.xaml
    /// </summary>
    public partial class ContainerInit : Window
    {
        /// <summary>
        /// View model instance for this window.
        /// </summary>
        private ContainerInitViewModel vm;

        public ContainerInit()
        {
            InitializeComponent();

            this.vm = new ContainerInitViewModel(this.FirstPasswordBox, this.SecondPasswordBox);
            this.DataContext = this.vm;
        }

        private void FirstPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.vm.ValidateInput();
        }

        private void SecondPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.vm.ValidateInput();
        }
    }
}
