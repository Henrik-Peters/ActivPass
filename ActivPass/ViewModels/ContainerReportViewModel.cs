#region License
// MIT License
// Copyright 2023 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ActivPass.Localization;
using ActivPass.Models;

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
        /// Close the passed window instance.
        /// </summary>
        public ICommand Close { get; set; }

        public ContainerReportViewModel()
        {
            //Command bindings
            this.Close = new RelayCommand<Window>(CloseWindow);

            PasswordItems = new ObservableCollection<PassReportViewModel>();
        }

        /// <summary>
        /// Initialize all report data for a given
        /// password container and all the items
        /// </summary>
        /// <param name="container">Generate report for this container</param>
        public void InitReportData(PasswordContainer container)
        {
            //Create the view model instances from the password item collection
            PasswordItems = new ObservableCollection<PassReportViewModel>(
                container.Items.Select(item => new PassReportViewModel(item))
            );

            //Notify view change
            NotifyPropertyChanged(nameof(PasswordItemsView));
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
