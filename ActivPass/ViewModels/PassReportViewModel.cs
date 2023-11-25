#region License
// MIT License
// Copyright 2023 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using ActivPass.Localization;
using ActivPass.Models;
using ActivPass.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace ActivPass.ViewModels
{
    public class PassReportViewModel : ViewModel
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        private PasswordItem _proxy;

        /// <summary>
        /// The represented password item model.
        /// </summary>
        public PasswordItem Proxy
        {
            get => _proxy;
            set => SetProperty(ref _proxy, value);
        }

        /// <summary>
        /// Name of the password item.
        /// </summary>
        public string Name
        {
            get => _proxy.Name;
            set
            {
                if (_proxy.Name != value) {
                    _proxy.Name = value;
                    NotifyPropertyChanged(nameof(_proxy.Name));
                }
            }
        }

        /// <summary>
        /// Username of the password item.
        /// </summary>
        public string Username
        {
            get => _proxy.Username;
            set
            {
                if (_proxy.Username != value) {
                    _proxy.Username = value;
                    NotifyPropertyChanged(nameof(_proxy.Username));
                }
            }
        }

        /// <summary>
        /// Password of the password item.
        /// </summary>
        public string Password
        {
            get => _proxy.Password;
            set
            {
                if (_proxy.Password != value) {
                    _proxy.Password = value;
                    NotifyPropertyChanged(nameof(_proxy.Password));
                }
            }
        }

        /// <summary>
        /// Url of the password item.
        /// </summary>
        public string Url
        {
            get => _proxy.Url;
            set 
            {
                if (_proxy.Url != value) {
                    _proxy.Url = value;
                    NotifyPropertyChanged(nameof(_proxy.Url));
                    NotifyPropertyChanged(nameof(ShowOpenBtn));
                }
            }
        }

        /// <summary>
        /// Notes of the password item.
        /// </summary>
        public string Notes
        {
            get => _proxy.Notes;
            set {
                if (_proxy.Notes != value) {
                    _proxy.Notes = value;
                    NotifyPropertyChanged(nameof(_proxy.Notes));
                }
            }
        }

        private string _warningText;
        public string WarningText
        {
            get => _warningText;
            set => SetProperty(ref _warningText, value);
        }

        private string[] _duplicateNames;
        public string[] DuplicateNames
        {
            get => _duplicateNames;
            set => SetProperty(ref _duplicateNames, value);
        }

        private PasswordStrength _passwordStrength;
        public PasswordStrength PasswordStrength
        {
            get => _passwordStrength;
            set
            {
                if (_passwordStrength != value)
                {
                    _passwordStrength = value;
                    NotifyPropertyChanged(nameof(PasswordStrength));
                }
            }
        }

        /// <summary>
        /// If the open url button should be visible.
        /// </summary>
        public Visibility ShowOpenBtn
        {
            get {
                if (this._proxy.HasBrowsableUrl()) {
                    return Visibility.Visible;
                } else {
                    return Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// If the warning icon should be visible
        /// </summary>
        public Visibility WarningVisibility
        {
            get {
                if (this.WarningText != string.Empty) {
                    return Visibility.Visible;
                } else {
                    return Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Open the argument as a new process.
        /// </summary>
        public ICommand OpenUrl { get; set; }

        /// <summary>
        /// Open the warning text dialog box.
        /// </summary>
        public ICommand ShowWarningText { get; set; }

        /// <summary>
        /// Create a new view model from a password item.
        /// </summary>
        /// <param name="proxy">Represented password item</param>
        public PassReportViewModel(PasswordItem proxy)
        {
            this._proxy = proxy;

            //Init values
            this.DuplicateNames = Array.Empty<string>();

            //Init report values
            this.UpdatePasswordScore();
            this.UpdateWarnings();

            //Command bindings
            this.OpenUrl = new RelayCommand<string>(OpenBrowserUrl);
            this.ShowWarningText = new RelayCommand(OpenWarningTextDialog);
        }

        /// <summary>
        /// Update the score rating of the current password
        /// </summary>
        public void UpdatePasswordScore()
        {
            //Calculate and apply the score
            PasswordStrength score = PasswordScores.GetScore(this.Password);
            this.PasswordStrength = score;
        }

        /// <summary>
        /// Update the warning status and warning text
        /// </summary>
        public void UpdateWarnings()
        {
            List<string> warnings = new();

            //Password duplicates
            if (this.DuplicateNames.Length > 0) {
                warnings.Add(Localize["PasswordDuplicat"] + ": " + string.Join(", ", this.DuplicateNames));
            }

            //Url warning
            if (!this._proxy.HasEncryptedTrafficUrl()) {
                warnings.Add(Localize["NonEncryptionTrafficUrl"]);
            }

            //Apply the warning text
            if (warnings.Count > 0) {
                this.WarningText = string.Join("; ", warnings);
            } else {
                this.WarningText = string.Empty;
            }
        }

        /// <summary>
        /// Open the url as a new browser process when
        /// the url is valid to launch a new process.
        /// </summary>
        /// <param name="url">Opent this url</param>
        private void OpenBrowserUrl(string url)
        {
            if (this._proxy.HasBrowsableUrl()) {
                var startInfo = new System.Diagnostics.ProcessStartInfo { FileName = @url, UseShellExecute = true };
                System.Diagnostics.Process.Start(startInfo);
            }
        }

        /// <summary>
        /// Show the dialog box to display the warning text
        /// </summary>
        private void OpenWarningTextDialog()
        {
            //Prepare the question box dialog
            QuestionBox warnTextDialog = new QuestionBox(
                this.WarningText,
                Localize["Warnings"] + " - " + this.Name);

            //Set dialog props
            warnTextDialog.contentPresenter.Content = "!";
            warnTextDialog.ConfirmButton.Visibility = Visibility.Hidden;
            warnTextDialog.CancelButton.Content = Localize["Close"];

            //Wait for the question box dialog result
            warnTextDialog.ShowDialog();
        }
    }
}
