#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivPass.Localization;

namespace ActivPass.ViewModels
{
    public class ConfigEditorViewModel : ViewModel
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        /// <summary>
        /// Close the passed window instance.
        /// </summary>
        public ICommand Close { get; set; }

        /// <summary>
        /// Save the config and close the passed window instance.
        /// </summary>
        public ICommand Save { get; set; }
        
        /// <summary>
        /// Create a new view model instance for the config editor.
        /// </summary>
        public ConfigEditorViewModel()
        {
            //Command bindings
            this.Close = new RelayCommand<Window>(CloseWindow);
            this.Save = new RelayCommand<Window>(SaveConfig);
        }

        /// <summary>
        /// Save the current config and close the passed window instance.
        /// </summary>
        /// <param name="window">Instance to close</param>
        private void SaveConfig(Window window)
        {
            CloseWindow(window);
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
