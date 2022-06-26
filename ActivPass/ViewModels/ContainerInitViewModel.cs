﻿#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ActivPass.Localization;
using ActivPass.Models;
using ActivPass.Crypto;

namespace ActivPass.ViewModels
{
    public class ContainerInitViewModel : ViewModel
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        private string _containerName = "";
        public string ContainerName
        {
            get => _containerName;
            set {
                SetProperty(ref _containerName, value);
                ValidateInput();
            }
        }

        private PasswordBox MasterPasswordBox;
        private PasswordBox RepeatPasswordBox;

        public bool ValidInput
        {
            get => ContainerName != String.Empty &&
                    MasterPasswordBox.Password != String.Empty &&
                    RepeatPasswordBox.Password != String.Empty &&
                    MasterPasswordBox.Password == RepeatPasswordBox.Password;
        }

        /// <summary>
        /// Close the passed window instance.
        /// </summary>
        public ICommand Close { get; set; }

        /// <summary>
        /// Create a new container from the view.
        /// </summary>
        public ICommand InitContainer { get; set; }

        /// <summary>
        /// Create a new view model instance
        /// for the contianer init view.
        /// </summary>
        /// <param name="masterPasswordBox">Main master password box</param>
        /// <param name="repeatPasswordBox">Repeat master password box</param>
        public ContainerInitViewModel(PasswordBox MasterPasswordBox, PasswordBox RepeatPasswordBox)
        {
            this.MasterPasswordBox = MasterPasswordBox;
            this.RepeatPasswordBox = RepeatPasswordBox;

            this.Close = new RelayCommand<Window>(CloseWindow);
            this.InitContainer = new RelayCommand<Window>(CreateContainer);
        }

        /// <summary>
        /// Update the valid input state to
        /// check the container init dialog.
        /// </summary>
        public void ValidateInput()
        {
            NotifyPropertyChanged(nameof(ValidInput));
        }

        /// <summary>
        /// Create a new container with the input
        /// paramaters of the current view model.
        /// This will also close the dialog window.
        /// </summary>
        /// <param name="window">Instance to close</param>
        private void CreateContainer(Window window)
        {
            string[] containerList = ContainerStorage.ContainerProvider.ListContainers();

            //Check if a container already exists with the target name
            if (containerList.Any(container => container.Equals(ContainerName))) {
                MessageBox.Show(Localize["ContainerNameExists"], Localize["ContainerCreateFail"], MessageBoxButton.OK, MessageBoxImage.Error);
                
            } else {
                //Create a new empty container
                PasswordContainer emptyContainer = new PasswordContainer(ContainerName, Environment.UserName, true);

                //Store the new container
                bool containerCreated = ContainerStorage.ContainerProvider.SaveContainer(emptyContainer, MasterPasswordBox.Password);

                if (containerCreated) {
                    CloseWindow(window);

                } else {
                    MessageBox.Show(Localize["ContainerStoreFail"], Localize["ContainerCreateFail"], MessageBoxButton.OK, MessageBoxImage.Error);
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
