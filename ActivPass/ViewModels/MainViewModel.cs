#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ActivPass.ViewModels
{
    /// <summary>
    /// ViewModel implementation for the main application.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        public ContextMenu MainMenu { private get; set; }
        public ICommand ShowMainMenu { get; set; }

        /// <summary>
        /// Show the application main menu which
        /// should be binded to the MainMenu property.
        /// </summary>
        /// <param name="placementTarget">Target for the menu</param>
        private void DisplayMainMenu(UIElement placementTarget)
        {
            this.MainMenu.PlacementTarget = placementTarget;
            this.MainMenu.Placement = PlacementMode.Bottom;
            this.MainMenu.IsOpen = true;
        }

        public MainViewModel()
        {
            this.ShowMainMenu = new RelayCommand<UIElement>(DisplayMainMenu);
        }
    }
}
