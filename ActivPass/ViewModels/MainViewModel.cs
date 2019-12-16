#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using ActivPass.Localization;
using ActivPass.Configuration;
using ActivPass.Models;
using ActivPass.Crypto;
using ActivPass.Views;

namespace ActivPass.ViewModels
{
    /// <summary>
    /// ViewModel implementation for the main application.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        /// <summary>
        /// Global application config
        /// </summary>
        public static ConfigData Config { get; set; }

        /// <summary>
        /// Container main conext menu to allow
        /// custom context menu placement logic
        /// </summary>
        public ContextMenu MainMenu { get; private set; }

        /// <summary>
        /// Textbox of the master password
        /// for the container login.
        /// </summary>
        public PasswordBox MasterPasswordBox { get; private set; }

        private ObservableCollection<string> _containerNames;
        public ObservableCollection<string> ContainerNames {
            get => _containerNames;
            set => SetProperty(ref _containerNames, value);
        }
        
        private string _selectedContainer;
        public string SelectedContainer
        {
            get => _selectedContainer;
            set => SetProperty(ref _selectedContainer, value);
        }

        private string _loginInfo;
        public string LoginInfo
        {
            get => _loginInfo;
            set => SetProperty(ref _loginInfo, value);
        }

        private Visibility _loginInfoVisibility;
        public Visibility LoginInfoVisibility
        {
            get => _loginInfoVisibility;
            set => SetProperty(ref _loginInfoVisibility, value);
        }

        private Visibility _emptyContainerInfo;
        public Visibility EmptyContainerInfo
        {
            get => _emptyContainerInfo;
            set => SetProperty(ref _emptyContainerInfo, value);
        }

        private bool _login;
        public bool Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        private PasswordContainer _container;
        public PasswordContainer Container
        {
            get => _container;
            set => SetProperty(ref _container, value);
        }

        private ObservableCollection<PasswordItemViewModel> _passwordItems;
        public ObservableCollection<PasswordItemViewModel> PasswordItems
        {
            get => _passwordItems;
            set => SetProperty(ref _passwordItems, value);
        }

        public ICommand ShowMainMenu { get; set; }
        public ICommand ExitApp { get; set; }
        public ICommand ContainerLogin { get; set; }
        public ICommand ContainerLogout { get; set; }
        public ICommand CreateContainer { get; set; }
        public ICommand OpenPasswordItem { get; set; }
        public ICommand AddPasswordItem { get; set; }
        public ICommand DeletePasswordItem { get; set; }

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

        /// <summary>
        /// Show the password container init view
        /// to create a new container instance.
        /// </summary>
        private void CreateNewContainer()
        {
            //Show the init container view
            ContainerInit containerInitView = new ContainerInit();
            containerInitView.ShowDialog();
        }

        /// <summary>
        /// Try to perform a container login. The Name and
        /// master password will be checked before performing
        /// a valid container login.
        /// </summary>
        /// <param name="containerName">Target container name</param>
        /// <param name="masterPassword">Master password of the container</param>
        public void OpenContainer(string containerName, string masterPassword)
        {
            string[] availableContainer = ContainerStorage.ContainerProvider.ListContainers();

            if (availableContainer.Contains(containerName)) {
                PasswordContainer container = ContainerStorage.ContainerProvider.LoadContainer(containerName, masterPassword);

                if (container == null) {
                    //Decrypting or deserializing has failed
                    this.LoginInfo = Localize["LoginFailed"];
                    this.LoginInfoVisibility = Visibility.Visible;

                } else {
                    //Create the view model instances from the password item collection
                    PasswordItems = new ObservableCollection<PasswordItemViewModel>(
                        container.Items.Select(item => new PasswordItemViewModel(item))
                    );

                    //Sucessful login
                    this.Login = true;
                    this.Container = container;
                    this.LoginInfoVisibility = Visibility.Hidden;

                    //Empty container info
                    if (PasswordItems.Count == 0) {
                        this.EmptyContainerInfo = Visibility.Visible;
                    }
                }

            } else {
                this.LoginInfo = Localize["LoginContainerFailed"];
                this.LoginInfoVisibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Logout off the current container
        /// and show the login grid dialog.
        /// </summary>
        private void LockContainer()
        {
            //Reset the login password
            this.MasterPasswordBox.Password = "";

            //Change to login mode
            Login = false;
            Container = null;
            PasswordItems.Clear();
            MasterPasswordBox.Focus();

            //Hide the empty container info
            this.EmptyContainerInfo = Visibility.Hidden;
        }

        /// <summary>
        /// Show detailed information with the password
        /// item dialog for a specific password item.
        /// </summary>
        /// <param name="item">Password item to display</param>
        private void ShowPasswordItemDetails(PasswordItemViewModel item)
        {
            //Create a clone of the item
            PasswordItem editorItem = item.Proxy.Clone() as PasswordItem;

            //Show the item editor dialog
            PassItemEditor itemEditor = new PassItemEditor(editorItem);
            itemEditor.ShowDialog();

            //Check if the editorItem should be stored
            if (itemEditor.vm.SaveEditorItem) {

                //Get the index of edited item
                int itemIndex = PasswordItems.IndexOf(item);

                //Set the attributes of the item
                PasswordItems[itemIndex].Name = editorItem.Name;
                PasswordItems[itemIndex].Username = editorItem.Username;
                PasswordItems[itemIndex].Password = editorItem.Password;

                //Save the container with the current storage provider
                if (!ContainerStorage.ContainerProvider.SaveContainer(Container, MasterPasswordBox.Password)) {
                    MessageBox.Show("Failed to store the container!", "Container store failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Show the dialog to create a new password item
        /// and store the result in the current container.
        /// </summary>
        private void CreatePasswordItem()
        {
            //Create an empty default item
            PasswordItem editorItem = new PasswordItem("", "", "");

            //Show the item editor dialog
            PassItemEditor itemEditor = new PassItemEditor(editorItem);
            itemEditor.ShowDialog();

            //Check if the editorItem should be created
            if (itemEditor.vm.SaveEditorItem) {

                //Create a new item array with increased size
                PasswordItem[] newItems = new PasswordItem[Container.Items.Length + 1];

                //Copy all password items to the new collection
                for (int i = 0; i < Container.Items.Length; i++) {
                    newItems[i] = Container.Items[i];
                }

                //Store the new item
                newItems[newItems.Length - 1] = editorItem;

                //Overwrite the container item collection
                Container.Items = newItems;
                PasswordItems.Add(new PasswordItemViewModel(editorItem));

                //Save the container with the current storage provider
                if (!ContainerStorage.ContainerProvider.SaveContainer(Container, MasterPasswordBox.Password)) {
                    MessageBox.Show("Failed to create the container!", "Container creation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Show the dialog for deleting a password
        /// item before the operation is performed.
        /// </summary>
        /// <param name="item">Password itme to delete</param>
        private void ShowDeleteItemDialog(PasswordItemViewModel item)
        {

        }

        /// <summary>
        /// Create a new view model instance
        /// for the activ pass main window.
        /// </summary>
        /// <param name="MainMenu">Container burger context menu</param>
        /// <param name="MasterPasswordBox">Login master password box</param>
        public MainViewModel(ContextMenu MainMenu, PasswordBox MasterPasswordBox)
        {
            //Constructor paramater storage
            this.MainMenu = MainMenu;
            this.MasterPasswordBox = MasterPasswordBox;

            //Create a default config when no config exists
            if (!ConfigProvider.ConfigExists) {
                ConfigProvider.SaveConfig(ConfigData.DefaultConfig);
            }

            Config = ConfigProvider.LoadConfig();

            //Command bindings
            this.ShowMainMenu = new RelayCommand<UIElement>(DisplayMainMenu);
            this.ExitApp = new RelayCommand(() => Application.Current.Shutdown());
            this.ContainerLogin = new RelayCommand(() => Application.Current.Shutdown());
            this.ContainerLogout = new RelayCommand(LockContainer);
            this.CreateContainer = new RelayCommand(CreateNewContainer);
            this.AddPasswordItem = new RelayCommand(CreatePasswordItem);
            this.OpenPasswordItem = new RelayCommand<PasswordItemViewModel>(ShowPasswordItemDetails);
            this.DeletePasswordItem = new RelayCommand<PasswordItemViewModel>(ShowDeleteItemDialog);

            //Default values
            this.Login = false;
            this.LoginInfo = Localize["LoginFailed"];
            this.LoginInfoVisibility = Visibility.Hidden;
            this.EmptyContainerInfo = Visibility.Hidden;

            //Get all available container names
            string[] availableContainer = ContainerStorage.ContainerProvider.ListContainers();

            if (availableContainer.Length == 0) {
                //Show the init container view
                ContainerInit containerInitView = new ContainerInit();
                containerInitView.ShowDialog();

            } else {
                //Display the list of available containers
                ContainerNames = new ObservableCollection<string>(availableContainer);
                SelectedContainer = ContainerNames[0];
            }
        }
    }
}