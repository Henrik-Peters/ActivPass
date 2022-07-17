#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ActivPass.Localization;
using ActivPass.Models;
using ActivPass.Crypto;
using ActivPass.Views;


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

        private static string[] INACTIVITY_TIME_OPTIONS = new string[] { "1 min", "2 min", "5 min", "15 min", "30 min" };

        private ObservableCollection<string> _inactivityTimes;
        public ObservableCollection<string> InactivityTimes
        {
            get => _inactivityTimes;
            set => SetProperty(ref _inactivityTimes, value);
        }

        private string _selectedInactivityTime;
        public string SelectedInactivityTime
        {
            get => _selectedInactivityTime;
            set => SetProperty(ref _selectedInactivityTime, value);
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
        /// Textbox reference of the container editor view.
        /// </summary>
        public PasswordBox ChangePasswordBox { get; set; }

        /// <summary>
        /// Close the passed window instance.
        /// </summary>
        public ICommand Close { get; set; }

        /// <summary>
        /// Rename the current container.
        /// </summary>
        public ICommand RenameContainer { get; set; }

        /// <summary>
        /// Change the master password of the container.
        /// </summary>
        public ICommand ChangeMasterPassword { get; set; }

        /// <summary>
        /// Delete the current container.
        /// </summary>
        public ICommand DeleteContainer { get; set; }

        public ContainerEditorViewModel() {
            //Default props
            this.LockContainer = false;
            
            this.InactivityTimes = new ObservableCollection<string>(INACTIVITY_TIME_OPTIONS);
            this.SelectedInactivityTime = INACTIVITY_TIME_OPTIONS[3];

            //Command bindings
            this.Close = new RelayCommand<Window>(CloseWindow);
            this.RenameContainer = new RelayCommand(RenameCurrentContainer);

            //Danger zone command bindings
            this.ChangeMasterPassword = new RelayCommand(ChangeCurrentMasterPassword);
            this.DeleteContainer = new RelayCommand<Window>(DeleteCurrentContainer);
        }

        /// <summary>
        /// Rename the current container
        /// </summary>
        private void RenameCurrentContainer()
        {
            //Store the old container name for deleting
            string oldContainerName = Container.ContainerName;

            //Set the new name in the container model
            Container.ContainerName = this.ContainerName;

            //Save the container with the current storage provider
            if (!ContainerStorage.ContainerProvider.SaveContainer(Container, MasterPasswordBox.Password)) {
                MessageBox.Show("Failed to store the new container!", "Container store failed", MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                //Delete the old container
                if (!ContainerStorage.ContainerProvider.DeleteContainer(oldContainerName)) {
                    MessageBox.Show("Failed to delete the old container!", "Container delete failed", MessageBoxButton.OK, MessageBoxImage.Error);
                } else {
                    MessageBox.Show(Localize["ContainerRenameDetails"], Localize["ContainerRenameHeader"], MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            //Lock the container after closing the dialog
            this.LockContainer = true;
        }

        private void ChangeCurrentMasterPassword()
        {
            //Store back the new password
            MasterPasswordBox.Password = ChangePasswordBox.Password;

            //Save the container with the current storage provider
            if (!ContainerStorage.ContainerProvider.SaveContainer(Container, ChangePasswordBox.Password)) {
                MessageBox.Show("Failed to store the new container!", "Container store failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else {
                MessageBox.Show(Localize["MasterPasswordChanged"], Localize["MasterPassword"], MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Delete the current container and
        /// close the passed window after deleting.
        /// </summary>
        /// <param name="window">Close this window</param>
        private void DeleteCurrentContainer(Window window)
        {
            //Confirm the delete operation
            QuestionBox deleteDialog = new QuestionBox(
                Localize["DeleteQuestionPart1"] + " " + Container.ContainerName + " " + Localize["DeleteQuestionPart2"],
                Localize["DeleteDialogTitle"]);

            //Wait for the question box dialog result
            deleteDialog.ShowDialog();

            //Check the delete operation should be performed
            if (deleteDialog.ConfirmResult) {
                //Delete the container
                if (!ContainerStorage.ContainerProvider.DeleteContainer(Container.ContainerName)) {
                    MessageBox.Show("Failed to delete the container!", "Container delete failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else {
                    MessageBox.Show(Localize["DeleteContainerSuccess"], Localize["DeleteContainer"], MessageBoxButton.OK, MessageBoxImage.Information);
                    this.LockContainer = true;
                    this.Container = null;

                    //Close the window because it makes no sense to edit a not existing container
                    if (window != null) {
                        window.Close();
                    }
                }

            }
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
