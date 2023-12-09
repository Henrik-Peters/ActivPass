#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ActivPass.Models;
using ActivPass.Localization;
using System.ComponentModel;

namespace ActivPass.ViewModels
{
    public class PassItemEditorViewModel : ViewModel
    {
        /// <summary>
        /// Current item data of the editor
        /// </summary>
        private PasswordItem _item;

        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        /// <summary>
        /// Displayname of the password item
        /// </summary>
        public string Name
        {
            get => _item.Name;
            set {
                if (_item.Name != value) {
                    _item.Name = value;
                    NotifyPropertyChanged(nameof(_item.Name));
                }
            }
        }

        /// <summary>
        /// Username of the password item
        /// </summary>
        public string Username
        {
            get => _item.Username;
            set
            {
                if (_item.Username != value) {
                    _item.Username = value;
                    NotifyPropertyChanged(nameof(_item.Username));
                }
            }
        }

        /// <summary>
        /// Url of the password item
        /// </summary>
        public string Url
        {
            get => _item.Url;
            set
            {
                if (_item.Url != value)
                {
                    _item.Url = value;
                    NotifyPropertyChanged(nameof(_item.Url));
                    NotifyPropertyChanged(nameof(ShowOpenBtn));

                    //Update unsafe url warning
                    this.UpdateUrlWarning();
                }
            }
        }

        /// <summary>
        /// Url of the notes text
        /// </summary>
        public string Notes
        {
            get => _item.Notes;
            set
            {
                if (_item.Notes != value)
                {
                    _item.Notes = value;
                    NotifyPropertyChanged(nameof(_item.Notes));
                }
            }
        }

        /// <summary>
        /// Password of the password item
        /// </summary>
        public string Password
        {
            get => _item.Password;
            set
            {
                if (_item.Password != value) {
                    _item.Password = value;
                    NotifyPropertyChanged(nameof(_item.Password));

                    //Update multi usage password warning
                    this.UpdateMultiPasswordWarning();
                }
            }
        }
        
        private bool _saveEditorItem;
        public bool SaveEditorItem
        {
            get => _saveEditorItem;
            set => SetProperty(ref _saveEditorItem, value);
        }

        private PasswordStrength _passwordStrength;
        public PasswordStrength PasswordStrength
        {
            get => _passwordStrength;
            set
            {
                if (_passwordStrength != value) {
                    _passwordStrength = value;
                    NotifyPropertyChanged(nameof(PasswordStrength));
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
        /// Close the passed window instance.
        /// </summary>
        public ICommand Close { get; set; }

        /// <summary>
        /// Save the current item and close the window instance.
        /// </summary>
        public ICommand SaveItem { get; set; }

        /// <summary>
        /// Copy the argument as unicode text to the clipboard.
        /// </summary>
        public ICommand CopyToClipboard { get; set; }

        /// <summary>
        /// Open the argument as a new process.
        /// </summary>
        public ICommand OpenUrl { get; set; }

        /// <summary>
        /// Callback to check for password duplicates
        /// </summary>
        private Predicate<string> CheckPasswordDuplicates { get; set; }

        /// <summary>
        /// If the open url button should be visible.
        /// </summary>
        public Visibility ShowOpenBtn
        {
            get {
                if (this._item.HasBrowsableUrl()) {
                    return Visibility.Visible;
                } else {
                    return Visibility.Hidden;
                }
            }
        }

        private Visibility _showUnsafeUrlWarning;
        /// <summary>
        /// If the unsafe url warning text should be visible.
        /// </summary>
        public Visibility ShowUnsafeUrlWarning
        {
            get => _showUnsafeUrlWarning;
            set
            {
                if (_showUnsafeUrlWarning != value) {
                    _showUnsafeUrlWarning = value;
                    NotifyPropertyChanged(nameof(ShowUnsafeUrlWarning));
                }
            }
        }

        private Visibility _showMultiPasswordWarning;
        /// <summary>
        /// If the multi usage password warning should be visible.
        /// </summary>
        public Visibility ShowMultiPasswordWarning
        {
            get => _showMultiPasswordWarning;
            set
            {
                if (_showMultiPasswordWarning != value) {
                    _showMultiPasswordWarning = value;
                    NotifyPropertyChanged(nameof(ShowMultiPasswordWarning));
                }
            }
        }

        /// <summary>
        /// Create a new view model instance
        /// for the password item editor.
        /// </summary>
        /// <param name="item">Init with these values</param>
        /// <param name="checkPasswordDuplicatesCallback">Callback for multi usage passwords</param>
        public PassItemEditorViewModel(PasswordItem item, Predicate<string> checkPasswordDuplicatesCallback,
            bool autoClearClipboard, int clipboardClearSeconds)
        {
            this._item = item;

            //Initial values
            this.SaveEditorItem = false;
            this.CheckPasswordDuplicates = checkPasswordDuplicatesCallback;
            this.AutoClearClipboard = autoClearClipboard;
            this.ClipboardClearSeconds = clipboardClearSeconds;

            //Init calculated values
            this.UpdatePasswordScore();
            this.UpdateMultiPasswordWarning();
            this.UpdateUrlWarning();
            
            //Command bindings
            this.Close = new RelayCommand<Window>(CloseWindow);
            this.SaveItem = new RelayCommand<Window>(SaveItemAndClose);
            this.CopyToClipboard = new RelayCommand<string>(SetClipboardText);
            this.OpenUrl = new RelayCommand<string>(OpenBrowserUrl);
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
        /// Update the state of the unsafe url warning text
        /// </summary>
        public void UpdateUrlWarning()
        {
            if (Url != string.Empty && !PasswordItem.UrlGrantsEncryptedTraffic(Url)) {
                ShowUnsafeUrlWarning = Visibility.Visible;
            } else {
                ShowUnsafeUrlWarning = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Update the state of the multi usage password warning
        /// </summary>
        private void UpdateMultiPasswordWarning()
        {
            if (this.Password != string.Empty && this.CheckPasswordDuplicates(this.Password)) {
                ShowMultiPasswordWarning = Visibility.Visible;
            } else {
                ShowMultiPasswordWarning = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Set a new text in the clipboard content.
        /// The data text format will be set to unicode text.
        /// </summary>
        /// <param name="text">Copy this text to the clipboard</param>
        private void SetClipboardText(string text)
        {
            Clipboard.SetText(text, TextDataFormat.UnicodeText);

            //Launch clipboard auto clear
            if (AutoClearClipboard) {
                ClipboardAutoClear.ScheduleTimer(TimeSpan.FromSeconds(ClipboardClearSeconds), text);
            }
        }

        /// <summary>
        /// Open the url as a new browser process when
        /// the url is valid to launch a new process.
        /// </summary>
        /// <param name="url">Opent this url</param>
        private void OpenBrowserUrl(string url)
        {
            if (this._item.HasBrowsableUrl())
            {
                var startInfo = new System.Diagnostics.ProcessStartInfo { FileName = @url, UseShellExecute = true };
                System.Diagnostics.Process.Start(startInfo);
            }
        }

        /// <summary>
        /// Set the flag to store the current
        /// password item from the editor dialog.
        /// </summary>
        /// <param name="window">Instance to close</param>
        private void SaveItemAndClose(Window window)
        {
            //Check that all fields have values
            if (Name != string.Empty && Username != string.Empty && Password != string.Empty) {
                this.SaveEditorItem = true;
                this.CloseWindow(window);
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
