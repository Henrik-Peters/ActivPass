#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using ActivPass.Localization;
using ActivPass.Configuration;
using ActivPass.Crypto;
using ActivPass.Views;

namespace ActivPass.ViewModels
{
    /// <summary>
    /// ViewModel implementation for the main application.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        public static ConfigData Config { get; set; }

        public ContextMenu MainMenu { private get; set; }

        private ObservableCollection<string> _containerNames;
        public ObservableCollection<string> ContainerNames {
            get => _containerNames;
            set => SetProperty(ref _containerNames, value);
        }

        private bool _login;
        public bool Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public ICommand ShowMainMenu { get; set; }
        public ICommand ExitApp { get; set; }
        public ICommand ContainerLogin { get; set; }
        public ICommand ContainerLogout { get; set; }

        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        /// <summary>
        /// Store a new language setting in the
        /// translate manager singleton and update
        /// the translations bindings of the view.
        /// </summary>
        /// <param name="language">New language</param>
        private void SetLanguage(Language language)
        {
            Localize.Language = language;
            NotifyPropertyChanged(nameof(Localize));
        }

        /// <summary>
        /// Show the application main menu which
        /// should be binded to the MainMenu property.
        /// </summary>
        /// <param name="placementTarget">Target for the menu</param>
        private void DisplayMainMenu(UIElement placementTarget)
        {
            this.MainMenu.PlacementTarget = placementTarget;
            this.MainMenu.Placement = PlacementMode.Bottom;
            this.MainMenu.IsOpen = true;
        }

        private void OpenContainer(string containerName, string masterPassword)
        {

        }

        public MainViewModel()
        {
            //Create a default config when no config exists
            if (!ConfigProvider.ConfigExists) {
                ConfigProvider.SaveConfig(ConfigData.DefaultConfig);
            }

            Config = ConfigProvider.LoadConfig();

            //Command bindings
            this.ShowMainMenu = new RelayCommand<UIElement>(DisplayMainMenu);
            this.ExitApp = new RelayCommand(() => Application.Current.Shutdown());
            this.ContainerLogin = new RelayCommand(() => Application.Current.Shutdown());
            this.ContainerLogout = new RelayCommand(() => Login = false);

            //Default values
            this.Login = false;

            //Get all available container names
            string[] availableContainer = ContainerStorage.ContainerProvider.ListContainers();

            if (availableContainer.Length == 0) {
                //Show the init container view
                ContainerInit containerInitView = new ContainerInit();
                containerInitView.ShowDialog();

            } else {
                //Display the list of available containers
                ContainerNames = new ObservableCollection<string>(availableContainer);
            }
        }
    }
}