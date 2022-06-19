#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
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
        /// Container item name search text box
        /// </summary>
        public TextBox SearchBox { get; private set; }

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

        private string _seachText;
        public string SearchText
        {
            get => _seachText;
            set
            {
                if (_seachText != value) {
                    _seachText = value;
                    NotifyPropertyChanged(nameof(SearchText));

                    if (PasswordItems != null) {
                        PasswordItemsView.Refresh();
                    }
                }
            }
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

        public ICollectionView PasswordItemsView
        {
            get {
                ICollectionView view = CollectionViewSource.GetDefaultView(PasswordItems);
                view.Filter = SearchPasswordFilter;
                return view;
            }
        }
        
        public ICommand ShowMainMenu { get; set; }
        public ICommand ExitApp { get; set; }
        public ICommand ContainerLogin { get; set; }
        public ICommand ContainerLogout { get; set; }
        public ICommand CreateContainer { get; set; }
        public ICommand EditContainer { get; set; }
        public ICommand OpenPasswordItem { get; set; }
        public ICommand AddPasswordItem { get; set; }
        public ICommand DeletePasswordItem { get; set; }
        public ICommand UsernameToClipboard { get; set; }
        public ICommand PasswordToClipboard { get; set; }
        public ICommand OpenSettings { get; set; }
        public ICommand ExportContainer { get; set; }

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
        /// Check if a password item passes the name seach.
        /// Returns true for items which are not password items.
        /// </summary>
        /// <param name="item">Password item to check</param>
        /// <returns></returns>
        private bool SearchPasswordFilter(object item)
        {
            PasswordItemViewModel passwordItem = item as PasswordItemViewModel;

            if (item == null || SearchText == "") {
                return true;

            } else {
                return passwordItem.Name.ToLower().Contains(SearchText.ToLower());
            }
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

            //Get all available container names
            string[] availableContainer = ContainerStorage.ContainerProvider.ListContainers();
            
            //Display the list of available containers
            if (availableContainer.Length > 0) {
                ContainerNames = new ObservableCollection<string>(availableContainer);
                SelectedContainer = ContainerNames[0];
            }
        }

        /// <summary>
        /// Show the container config editor window.
        /// </summary>
        private void EditCurrentContainer()
        {
            ContainerEditor containerEditor = new ContainerEditor();
            containerEditor.ShowDialog();
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
                    this.SearchBox.Focus();
                    NotifyPropertyChanged(nameof(PasswordItemsView));

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
                PasswordItems[itemIndex].Url = editorItem.Url;
                PasswordItems[itemIndex].Notes = editorItem.Notes;

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
            PasswordItem editorItem = new PasswordItem("", "", "", "", "");

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

                //Empty container info
                if (PasswordItems.Count > 0) {
                    this.EmptyContainerInfo = Visibility.Hidden;
                }

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
            //Confirm the delete operation
            QuestionBox deleteDialog = new QuestionBox(
                Localize["DeleteQuestionPart1"] + " " + item.Name + " " + Localize["DeleteQuestionPart2"],
                Localize["DeleteDialogTitle"]);

            //Wait for the question box dialog result
            deleteDialog.ShowDialog();

            //Check the delete operation should be performed
            if (deleteDialog.ConfirmResult) {

                //Get the index of the item to delete
                int itemIndex = PasswordItems.IndexOf(item);

                //Create a new item array with decreased size
                PasswordItem[] newItems = new PasswordItem[Container.Items.Length - 1];

                //Copy all password items to the new collection
                for (int i = 0, j = 0; i < Container.Items.Length; i++) {

                    //Skip the item for deletion
                    if (Container.Items[i] != item.Proxy) {
                        newItems[j] = Container.Items[i];
                        j++;
                    }
                }

                //Overwrite the container item collection
                Container.Items = newItems;
                PasswordItems.Remove(item);

                //Empty container info
                if (PasswordItems.Count == 0) {
                    this.EmptyContainerInfo = Visibility.Visible;
                }

                //Save the container with the current storage provider
                if (!ContainerStorage.ContainerProvider.SaveContainer(Container, MasterPasswordBox.Password)) {
                    MessageBox.Show("Failed to delete the container!", "Container deletion failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Copy the username of a password item to the clipboard.
        /// </summary>
        /// <param name="item">Copy from this view model instance</param>
        private void CopyUsernameToClipboard(PasswordItemViewModel item)
        {
            Clipboard.SetText(item.Username, TextDataFormat.UnicodeText);
        }

        /// <summary>
        /// Copy the password of a password item to the clipboard.
        /// </summary>
        /// <param name="item">Copy from this view model instance</param>
        private void CopyPasswordToClipboard(PasswordItemViewModel item)
        {
            Clipboard.SetText(item.Password, TextDataFormat.UnicodeText);
        }

        /// <summary>
        /// Show the config editor to change the application settings.
        /// </summary>
        private void ShowConfigEditor()
        {
            ConfigEditor configEditor = new ConfigEditor(Config);
            configEditor.ShowDialog();

            //Apply the new language value
            SetLanguage(Config.Language);
        }

        /// <summary>
        /// Show the export dialog of the current container.
        /// </summary>
        private void ContainerExport()
        {
            //Create the save file dialog
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                FileName = Container.ContainerName,
                DefaultExt = ".csv",
                Filter = "Comma-separated values (.csv)|*.csv"
            };

            //Show the dialog and save the dialog result
            bool? dialogResult = saveDialog.ShowDialog();

            //Create the csv file when the dialog result is save
            if (dialogResult == true) {
                string separator = ";";
                string filename = saveDialog.FileName;
                string[] csvLines = new string[Container.Items.Length + 1];

                //CSV Headlines
                csvLines[0] = "Name" + separator + "Username" + separator + "Password" + separator + "Url" + separator + "Notes";

                //CSV item data
                for (int i = 0; i < Container.Items.Length; i++) {
                    PasswordItem item = Container.Items[i];

                    //Replace line brakes in notes
                    string pureNotes = item.Notes is string ? item.Notes.Replace("\r\n", " ") : "";
                    csvLines[i + 1] = item.Name + separator + item.Username + separator + item.Password + separator + item.Url + separator + pureNotes;
                }
                
                try {
                    File.WriteAllLines(filename, csvLines);

                } catch (Exception err) {
                    MessageBox.Show("Failed to save the csv file: " + err.Message, "CSV file export error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Create a new view model instance
        /// for the activ pass main window.
        /// </summary>
        /// <param name="MainMenu">Container burger context menu</param>
        /// <param name="MasterPasswordBox">Login master password box</param>
        /// <param name="SearchBox">TextBox for searching password items</param>
        public MainViewModel(ContextMenu MainMenu, PasswordBox MasterPasswordBox, TextBox SearchBox)
        {
            //Constructor paramater storage
            this.MainMenu = MainMenu;
            this.MasterPasswordBox = MasterPasswordBox;
            this.SearchBox = SearchBox;

            //Create a default config when no config exists
            if (!ConfigProvider.ConfigExists) {
                ConfigProvider.SaveConfig(ConfigData.DefaultConfig);
            }

            //Load the config
            Config = ConfigProvider.LoadConfig();

            //Apply the config values
            SetLanguage(Config.Language);

            //Command bindings
            this.ShowMainMenu = new RelayCommand<UIElement>(DisplayMainMenu);
            this.ExitApp = new RelayCommand(() => Application.Current.Shutdown());
            this.ContainerLogin = new RelayCommand(() => Application.Current.Shutdown());
            this.ContainerLogout = new RelayCommand(LockContainer);
            this.CreateContainer = new RelayCommand(CreateNewContainer);
            this.EditContainer = new RelayCommand(EditCurrentContainer);
            this.AddPasswordItem = new RelayCommand(CreatePasswordItem);
            this.OpenSettings = new RelayCommand(ShowConfigEditor);
            this.ExportContainer = new RelayCommand(ContainerExport);
            this.OpenPasswordItem = new RelayCommand<PasswordItemViewModel>(ShowPasswordItemDetails);
            this.DeletePasswordItem = new RelayCommand<PasswordItemViewModel>(ShowDeleteItemDialog);
            this.UsernameToClipboard = new RelayCommand<PasswordItemViewModel>(CopyUsernameToClipboard);
            this.PasswordToClipboard = new RelayCommand<PasswordItemViewModel>(CopyPasswordToClipboard);

            //Default values
            this.Login = false;
            this.SearchText = string.Empty;
            this.LoginInfo = Localize["LoginFailed"];
            this.LoginInfoVisibility = Visibility.Hidden;
            this.EmptyContainerInfo = Visibility.Hidden;
            this.PasswordItems = new ObservableCollection<PasswordItemViewModel>();

            //Get all available container names
            string[] availableContainer = ContainerStorage.ContainerProvider.ListContainers();

            if (availableContainer.Length == 0) {
                //Show the init container view
                ContainerInit containerInitView = new ContainerInit();
                containerInitView.ShowDialog();

                //Reload the container list
                availableContainer = ContainerStorage.ContainerProvider.ListContainers();
            }

            //Display the list of available containers
            if (availableContainer.Length > 0) {
                ContainerNames = new ObservableCollection<string>(availableContainer);
                SelectedContainer = ContainerNames[0];
            }
        }
    }
}