#region License
// MIT License
// Copyright 2023 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ActivPass.Localization;
using ActivPass.Models;
using ActivPass.Views;

namespace ActivPass.ViewModels
{
    public class ContainerReportViewModel : ViewModel
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        private ObservableCollection<PassReportViewModel> _passwordItems;
        public ObservableCollection<PassReportViewModel> PasswordItems
        {
            get => _passwordItems;
            set => SetProperty(ref _passwordItems, value);
        }

        public ICollectionView PasswordItemsView
        {
            get
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(PasswordItems);

                //Add sort description
                view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                return view;
            }
        }

        /// <summary>
        /// Close the passed window instance
        /// </summary>
        public ICommand Close { get; set; }

        /// <summary>
        /// Show the password item editor
        /// </summary>
        public ICommand OpenPasswordItem { get; set; }

        /// <summary>
        /// Save the container after items have been modified
        /// </summary>
        public ICommand SaveContainer { get; set; }

        public ContainerReportViewModel()
        {
            //Command bindings
            this.Close = new RelayCommand<Window>(CloseWindow);
            this.OpenPasswordItem = new RelayCommand<PassReportViewModel>(ShowPasswordItemDetails);

            //Initial values
            PasswordItems = new ObservableCollection<PassReportViewModel>();
        }

        /// <summary>
        /// Initialize all report data for a given
        /// password container and all the items
        /// </summary>
        /// <param name="container">Generate report for this container</param>
        public void InitReportData(PasswordContainer container, ICommand SaveContainerCallback)
        {
            //Create the view model instances from the password item collection
            PasswordItems = new ObservableCollection<PassReportViewModel>(
                container.Items.Select(item => new PassReportViewModel(item))
            );

            //Update duplicate name info
            this.UpdateDuplicateNames();

            //Notify view change
            NotifyPropertyChanged(nameof(PasswordItemsView));

            this.SaveContainer = SaveContainerCallback;
        }

        /// <summary>
        /// Show detailed information with the password
        /// item dialog for a specific password item.
        /// </summary>
        /// <param name="item">Password item to display</param>
        private void ShowPasswordItemDetails(PassReportViewModel item)
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

                //Calculate the password score when the password has changed
                item.UpdatePasswordScore();

                //Save the container with the current storage provider
                SaveContainer.Execute(null);
            }
        }

        /// <summary>
        /// Update the duplicate names of all items
        /// </summary>
        private void UpdateDuplicateNames()
        {
            foreach (var passItem in this.PasswordItems) {
                List<string> currDuplicates = new();

                //Compare with all other items
                foreach (var cmpItem in this.PasswordItems) {
                    if (passItem.Password == cmpItem.Password && passItem.Name != cmpItem.Name) {
                        currDuplicates.Add(cmpItem.Name);
                    }
                }

                //Set the duplicate names in the item view model
                passItem.DuplicateNames = currDuplicates.ToArray();
                passItem.UpdateWarnings();
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
