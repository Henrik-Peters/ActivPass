#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using System.Windows.Input;
using ActivPass.Localization;
using ActivPass.Configuration;

namespace ActivPass.ViewModels
{
    public class ConfigEditorViewModel : ViewModel
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        private int _selectedLanguageIndex;
        public int SelectedLanguageIndex
        {
            get => _selectedLanguageIndex;
            set => SetProperty(ref _selectedLanguageIndex, value);
        }

        private bool _maximizeStartupWindow;
        public bool MaximizeStartupWindow
        {
            get => _maximizeStartupWindow;
            set => SetProperty(ref _maximizeStartupWindow, value);
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
            //Config values
            Language language;
            WindowStartupState startupState;

            //Select the language based in the selector index
            switch (SelectedLanguageIndex) {
                case 0:
                    language = Language.English;
                    break;

                case 1:
                    language = Language.German;
                    break;

                default:
                    language = Language.English;
                    break;
            }

            //Select the window startup state based on the checkbox value
            if (MaximizeStartupWindow) {
                startupState = WindowStartupState.Maximised;
            } else {
                startupState = WindowStartupState.Default;
            }

            //Create the new config instance
            ConfigData newConfig = new ConfigData(language, startupState);

            //Apply and save the new config
            MainViewModel.Config = newConfig;
            ConfigProvider.SaveConfig(newConfig);
            Localize.Language = newConfig.Language;

            //Close the window
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
