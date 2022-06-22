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
    /// Viewlogic for ContainerEditor.xaml
    /// </summary>
    public partial class ContainerEditor : Window
    {
        public ContainerEditor()
        {
            InitializeComponent();

            //Forward the change master password box to the view model
            var vm = this.DataContext as ContainerEditorViewModel;
            vm.ChangePasswordBox = this.ChangePasswordBox;
        }

        private void ChangePasswordBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Disable changing to empty passwords
            if (ChangePasswordBox.Password == "") {
                ChangePasswordButton.IsEnabled = false;
            } else {
                ChangePasswordButton.IsEnabled = true;
            }
        }
    }
}
