#region License
// MIT License
// Copyright 2023 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
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

        private string _warningsBoxText;
        public string WarningsBoxText
        {
            get => _warningsBoxText;
            set
            {
                if (_warningsBoxText != value)
                {
                    _warningsBoxText = value;
                    NotifyPropertyChanged(nameof(WarningsBoxText));
                }
            }
        }

        private Visibility _warningIconVisibility;
        public Visibility WarningIconVisibility
        {
            get => _warningIconVisibility;
            set
            {
                if (_warningIconVisibility != value) {
                    _warningIconVisibility = value;
                    NotifyPropertyChanged(nameof(WarningIconVisibility));
                }
            }
        }

        private Visibility _okayIconVisibility;
        public Visibility OkayIconVisibility
        {
            get => _okayIconVisibility;
            set
            {
                if (_okayIconVisibility != value) {
                    _okayIconVisibility = value;
                    NotifyPropertyChanged(nameof(OkayIconVisibility));
                }
            }
        }

        private Visibility _emptyContainerInfo;
        public Visibility EmptyContainerInfo
        {
            get => _emptyContainerInfo;
            set
            {
                if (_emptyContainerInfo != value)
                {
                    _emptyContainerInfo = value;
                    NotifyPropertyChanged(nameof(EmptyContainerInfo));
                }
            }
        }

        private GridLength _extremeStrongWidth;
        public GridLength ExtremeStrongWidth
        {
            get => _extremeStrongWidth;
            set
            {
                if (!_extremeStrongWidth.Equals(value)) {
                    _extremeStrongWidth = value;
                    NotifyPropertyChanged(nameof(ExtremeStrongWidth));
                }
            }
        }

        private GridLength _veryStrongWidth;
        public GridLength VeryStrongWidth
        {
            get => _veryStrongWidth;
            set
            {
                if (!_veryStrongWidth.Equals(value))
                {
                    _veryStrongWidth = value;
                    NotifyPropertyChanged(nameof(VeryStrongWidth));
                }
            }
        }

        private GridLength _strongWidth;
        public GridLength StrongWidth
        {
            get => _strongWidth;
            set
            {
                if (!_strongWidth.Equals(value))
                {
                    _strongWidth = value;
                    NotifyPropertyChanged(nameof(StrongWidth));
                }
            }
        }

        private GridLength _mediumWidth;
        public GridLength MediumWidth
        {
            get => _mediumWidth;
            set
            {
                if (!_mediumWidth.Equals(value))
                {
                    _mediumWidth = value;
                    NotifyPropertyChanged(nameof(MediumWidth));
                }
            }
        }

        private GridLength _weakWidth;
        public GridLength WeakWidth
        {
            get => _weakWidth;
            set
            {
                if (!_weakWidth.Equals(value))
                {
                    _weakWidth = value;
                    NotifyPropertyChanged(nameof(WeakWidth));
                }
            }
        }

        private GridLength _veryWeakWidth;
        public GridLength VeryWeakWidth
        {
            get => _veryWeakWidth;
            set
            {
                if (!_veryWeakWidth.Equals(value))
                {
                    _veryWeakWidth = value;
                    NotifyPropertyChanged(nameof(VeryWeakWidth));
                }
            }
        }

        private Visibility _extremeStrongVisibility;
        public Visibility ExtremeStrongVisibility
        {
            get => _extremeStrongVisibility;
            set
            {
                if (_extremeStrongVisibility != value) {
                    _extremeStrongVisibility = value;
                    NotifyPropertyChanged(nameof(ExtremeStrongVisibility));
                }
            }
        }

        private Visibility _veryStrongVisibility;
        public Visibility VeryStrongVisibility
        {
            get => _veryStrongVisibility;
            set
            {
                if (_veryStrongVisibility != value) {
                    _veryStrongVisibility = value;
                    NotifyPropertyChanged(nameof(VeryStrongVisibility));
                }
            }
        }

        private Visibility _strongVisibility;
        public Visibility StrongVisibility
        {
            get => _strongVisibility;
            set
            {
                if (_strongVisibility != value) {
                    _strongVisibility = value;
                    NotifyPropertyChanged(nameof(StrongVisibility));
                }
            }
        }

        private Visibility _mediumVisibility;
        public Visibility MediumVisibility
        {
            get => _mediumVisibility;
            set
            {
                if (_mediumVisibility != value) {
                    _mediumVisibility = value;
                    NotifyPropertyChanged(nameof(MediumVisibility));
                }
            }
        }

        private Visibility _weakVisibility;
        public Visibility WeakVisibility
        {
            get => _weakVisibility;
            set
            {
                if (_weakVisibility != value) {
                    _weakVisibility = value;
                    NotifyPropertyChanged(nameof(WeakVisibility));
                }
            }
        }

        private Visibility _veryWeakVisibility;
        public Visibility VeryWeakVisibility
        {
            get => _veryWeakVisibility;
            set
            {
                if (_veryWeakVisibility != value) {
                    _veryWeakVisibility = value;
                    NotifyPropertyChanged(nameof(VeryWeakVisibility));
                }
            }
        }

        /// <summary>
        /// Automatically delete clipboard after copying secret data
        /// </summary>
        public bool AutoClearClipboard { get; set; }

        /// <summary>
        /// Time before the clipboard is cleared in seconds
        /// </summary>
        public int ClipboardClearSeconds { get; set; }

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
            this.UpdateScoreBar();
            this.UpdateWarningsBox();

            //Clipboard clearing
            this.AutoClearClipboard = container.ClipboardAutoClear;
            this.ClipboardClearSeconds = container.ClipboardClearSeconds;

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

            //Get the index of edited item
            int itemIndex = PasswordItems.IndexOf(item);

            //Show the item editor dialog
            PassItemEditor itemEditor = new PassItemEditor(editorItem, (password) => {

                foreach (var passItem in this.PasswordItems)
                {
                    if (editorItem.Name != passItem.Name && password == passItem.Password) {
                        return true;
                    }
                }

                return false;
            }, AutoClearClipboard, ClipboardClearSeconds);

            //Show editor dialog
            itemEditor.ShowDialog();

            //Check if the editorItem should be stored
            if (itemEditor.vm.SaveEditorItem) {
                //Set the attributes of the item
                PasswordItems[itemIndex].Name = editorItem.Name;
                PasswordItems[itemIndex].Username = editorItem.Username;
                PasswordItems[itemIndex].Password = editorItem.Password;
                PasswordItems[itemIndex].Url = editorItem.Url;
                PasswordItems[itemIndex].Notes = editorItem.Notes;

                //Update container report values
                item.UpdatePasswordScore();
                this.UpdateDuplicateNames();
                this.UpdateScoreBar();
                this.UpdateWarningsBox();

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
        /// Update the width sizing of the score report bar
        /// </summary>
        private void UpdateScoreBar()
        {
            //Get total item amount
            int total = PasswordItems.Count;

            if (total > 0) {
                //Get the amount for each password score
                int extremeStrong = PasswordItems.Where(item => item.PasswordStrength == PasswordStrength.EXTREME_STRONG).Count();
                int veryStrong = PasswordItems.Where(item => item.PasswordStrength == PasswordStrength.VERY_STRONG).Count();
                int strong = PasswordItems.Where(item => item.PasswordStrength == PasswordStrength.STRONG).Count();
                int medium = PasswordItems.Where(item => item.PasswordStrength == PasswordStrength.MEDIUM).Count();
                int weak = PasswordItems.Where(item => item.PasswordStrength == PasswordStrength.WEAK).Count();
                int veryWeak = PasswordItems.Where(item => item.PasswordStrength == PasswordStrength.VERY_WEAK).Count();

                //Calculate percentage values between 0 and 1
                float pExtremeStrongPer = (float)extremeStrong / total;
                float pVeryStrong = (float)veryStrong / total;
                float pStrong = (float)strong / total;
                float pMedium = (float)medium / total;
                float pWeak = (float)weak / total;
                float pVeryWeak = (float)veryWeak / total;

                //Apply grid length sizing
                this.ExtremeStrongWidth = new GridLength(pExtremeStrongPer, GridUnitType.Star);
                this.VeryStrongWidth = new GridLength(pVeryStrong, GridUnitType.Star);
                this.StrongWidth = new GridLength(pStrong, GridUnitType.Star);
                this.MediumWidth = new GridLength(pMedium, GridUnitType.Star);
                this.WeakWidth = new GridLength(pWeak, GridUnitType.Star);
                this.VeryWeakWidth = new GridLength(pVeryWeak, GridUnitType.Star);

                //Apply legend visibility
                this.ExtremeStrongVisibility = extremeStrong > 0 ? Visibility.Visible : Visibility.Collapsed;
                this.VeryStrongVisibility = veryStrong > 0 ? Visibility.Visible : Visibility.Collapsed;
                this.StrongVisibility = strong > 0 ? Visibility.Visible : Visibility.Collapsed;
                this.MediumVisibility = medium > 0 ? Visibility.Visible : Visibility.Collapsed;
                this.WeakVisibility = weak > 0 ? Visibility.Visible : Visibility.Collapsed;
                this.VeryWeakVisibility = veryWeak > 0 ? Visibility.Visible : Visibility.Collapsed;

                //Empty container info
                this.EmptyContainerInfo = Visibility.Collapsed;
            } else {
                //Empty container
                this.ExtremeStrongWidth = new GridLength(1, GridUnitType.Star);
                this.VeryStrongWidth = new GridLength(1, GridUnitType.Star);
                this.StrongWidth = new GridLength(1, GridUnitType.Star);
                this.MediumWidth = new GridLength(1, GridUnitType.Star);
                this.WeakWidth = new GridLength(1, GridUnitType.Star);
                this.VeryWeakWidth = new GridLength(1, GridUnitType.Star);

                //Empty container info
                this.EmptyContainerInfo = Visibility.Visible;
            }
        }

        /// <summary>
        /// Update the warning header box text
        /// </summary>
        private void UpdateWarningsBox()
        {
            //Calculate the amount of items with warnings
            int totalWarnings = PasswordItems.Aggregate(0, (totalWarnings, item) => {
                return item.WarningText != string.Empty ? totalWarnings + 1 : totalWarnings;
            });

            //Update the box text
            this.WarningsBoxText = totalWarnings + " " + Localize["Warnings"];

            //Update warning icons
            if (totalWarnings == 0) {
                this.WarningIconVisibility = Visibility.Collapsed;
                this.OkayIconVisibility = Visibility.Visible;
            } else {
                this.WarningIconVisibility = Visibility.Visible;
                this.OkayIconVisibility = Visibility.Collapsed;
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
