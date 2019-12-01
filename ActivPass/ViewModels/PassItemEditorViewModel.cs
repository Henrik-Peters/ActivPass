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
        /// Create a new view model instance
        /// for the password item editor.
        /// </summary>
        /// <param name="item">Init with these values</param>
        public PassItemEditorViewModel(PasswordItem item)
        {
            this._item = item;
        }
    }
}
