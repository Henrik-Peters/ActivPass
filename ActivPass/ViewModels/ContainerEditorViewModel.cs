#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using ActivPass.Localization;
using ActivPass.Models;
using ActivPass.Crypto;

namespace ActivPass.ViewModels
{
    public class ContainerEditorViewModel : ViewModel
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        private string _containerName;
        public string ContainerName
        {
            get => _containerName;
            set => SetProperty(ref _containerName, value);
        }

        private bool _lockContainer;
        public bool LockContainer
        {
            get => _lockContainer;
            set => SetProperty(ref _lockContainer, value);
        }

        private PasswordContainer _container;
        public PasswordContainer Container
        {
            get => _container;
            set => SetProperty(ref _container, value);
        }

        /// <summary>
        /// Textbox reference of the main viewmodel.
        /// </summary>
        public PasswordBox MasterPasswordBox { get; set; }

        /// <summary>
        /// Close the passed window instance.
        /// </summary>
        public ICommand Close { get; set; }

        /// <summary>
        /// Rename the current container.
        /// </summary>
        public ICommand RenameContainer { get; set; }

        public ContainerEditorViewModel() {
            //Default props
            this.LockContainer = false;

            //Command bindings
            this.Close = new RelayCommand<Window>(CloseWindow);
            this.RenameContainer = new RelayCommand(RenameCurrentContainer);
        }

        /// <summary>
        /// Rename the current container
        /// </summary>
        private void RenameCurrentContainer()
        {
            //Set the new name in the container model
            Container.ContainerName = this.ContainerName;

            //Save the container with the current storage provider
            if (!ContainerStorage.ContainerProvider.SaveContainer(Container, MasterPasswordBox.Password)) {
                MessageBox.Show("Failed to store the container!", "Container store failed", MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                MessageBox.Show("Container as been renamed", "Container renamed", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //Lock the container after closing the dialog
            this.LockContainer = true;
        }

        /// <summary>
        /// Close the passed window instance.
        /// </summary>
        /// <param name="window">Instance to close</param>
        private void CloseWindow(Window window)
        {
            if (window != null) {
                window.Close();
            }
        }
    }
}
