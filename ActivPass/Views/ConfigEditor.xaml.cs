#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using ActivPass.Configuration;
using ActivPass.ViewModels;

namespace ActivPass.Views
{
    /// <summary>
    /// Viewlogic for ConfigEditor.xaml
    /// </summary>
    public partial class ConfigEditor : Window
    {
        public ConfigEditor(ConfigData config)
        {
            InitializeComponent();

            //Config init
            ConfigEditorViewModel vm = this.DataContext as ConfigEditorViewModel;

            //Set the current language as selected item
            switch (config.Language) {
                case Localization.Language.English:
                    vm.SelectedLanguageIndex = 0;
                    break;

                case Localization.Language.German:
                    vm.SelectedLanguageIndex = 1;
                    break;
            }

            //Checked value for the maximize on startup checkbox
            vm.MaximizeStartupWindow = (config.StartupLayout == WindowStartupState.Maximised);
        }
    }
}
