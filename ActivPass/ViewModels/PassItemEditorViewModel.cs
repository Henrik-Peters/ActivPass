﻿#region License
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
                }
            }
        }
        
        private bool _saveEditorItem;
        public bool SaveEditorItem
        {
            get => _saveEditorItem;
            set => SetProperty(ref _saveEditorItem, value);
        }

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
        /// Create a new view model instance
        /// for the password item editor.
        /// </summary>
        /// <param name="item">Init with these values</param>
        public PassItemEditorViewModel(PasswordItem item)
        {
            this._item = item;

            //Inital values
            this.SaveEditorItem = false;

            //Command bindings
            this.Close = new RelayCommand<Window>(CloseWindow);
            this.SaveItem = new RelayCommand<Window>(SaveItemAndClose);
            this.CopyToClipboard = new RelayCommand<string>(SetClipboardText);
        }

        /// <summary>
        /// Set a new text in the clipboard content.
        /// The data text format will be set to unicode text.
        /// </summary>
        /// <param name="text">Copy this text to the clipboard</param>
        private void SetClipboardText(string text)
        {
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
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
