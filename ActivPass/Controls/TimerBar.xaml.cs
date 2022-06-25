#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ActivPass.Controls
{
    /// <summary>
    /// Interaktionslogik für TimerBar.xaml
    /// </summary>
    public partial class TimerBar : UserControl
    {
        #region DependencyProperties

        /// <summary>
        /// Identifies the TextProperty dependency property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(TimerBar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the text to be displayed
        /// next to the time icon
        /// </summary>
        [Bindable(true)]
        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        #endregion
        public TimerBar()
        {
            InitializeComponent();
        }
    }
}
