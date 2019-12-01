#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using ActivPass.Models;
using ActivPass.ViewModels;

namespace ActivPass.Views
{
    /// <summary>
    /// Viewlogic for PassItemEditor.xaml
    /// </summary>
    public partial class PassItemEditor : Window
    {
        /// <summary>
        /// View model instance for this window.
        /// </summary>
        private PassItemEditorViewModel vm;

        /// <summary>
        /// Create a new password item editor
        /// and viewer window instance.
        /// </summary>
        /// <param name="item">Init the editor values with this item</param>
        public PassItemEditor(PasswordItem item)
        {
            InitializeComponent();

            //View model initialisation
            this.vm = new PassItemEditorViewModel(item);
            this.DataContext = this.vm;
        }
    }
}
