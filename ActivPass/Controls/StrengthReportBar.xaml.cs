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
    public partial class StrengthReportBar : UserControl
    {
        #region DependencyProperties

        /// <summary>
        /// Identifies the grid column width dependency property
        /// </summary>
        public static readonly DependencyProperty Width0Property = DependencyProperty.Register(
            "Width0", typeof(GridLength), typeof(StrengthReportBar), new PropertyMetadata(new GridLength(0.3, GridUnitType.Star)));

        /// <summary>
        /// Gets or sets the width of the grid column
        /// </summary>
        [Bindable(true)]
        public GridLength Width0
        {
            get => (GridLength)this.GetValue(Width0Property);
            set
            {
                this.SetValue(Width0Property, value);
                this.UpdateGridLayout();
            }
        }

        /// <summary>
        /// Identifies the grid column width dependency property
        /// </summary>
        public static readonly DependencyProperty Width1Property = DependencyProperty.Register(
            "Width1", typeof(GridLength), typeof(StrengthReportBar), new PropertyMetadata(new GridLength(0.3, GridUnitType.Star)));

        /// <summary>
        /// Gets or sets the width of the grid column
        /// </summary>
        [Bindable(true)]
        public GridLength Width1
        {
            get => (GridLength)this.GetValue(Width1Property);
            set
            {
                this.SetValue(Width1Property, value);
                this.UpdateGridLayout();
            }
        }

        /// <summary>
        /// Identifies the grid column width dependency property
        /// </summary>
        public static readonly DependencyProperty Width2Property = DependencyProperty.Register(
            "Width2", typeof(GridLength), typeof(StrengthReportBar), new PropertyMetadata(new GridLength(0.1, GridUnitType.Star)));

        /// <summary>
        /// Gets or sets the width of the grid column
        /// </summary>
        [Bindable(true)]
        public GridLength Width2
        {
            get => (GridLength)this.GetValue(Width2Property);
            set
            {
                this.SetValue(Width2Property, value);
                this.UpdateGridLayout();
            }
        }

        /// <summary>
        /// Identifies the grid column width dependency property
        /// </summary>
        public static readonly DependencyProperty Width3Property = DependencyProperty.Register(
            "Width3", typeof(GridLength), typeof(StrengthReportBar), new PropertyMetadata(new GridLength(0.15, GridUnitType.Star)));

        /// <summary>
        /// Gets or sets the width of the grid column
        /// </summary>
        [Bindable(true)]
        public GridLength Width3
        {
            get => (GridLength)this.GetValue(Width3Property);
            set
            {
                this.SetValue(Width3Property, value);
                this.UpdateGridLayout();
            }
        }

        /// <summary>
        /// Identifies the grid column width dependency property
        /// </summary>
        public static readonly DependencyProperty Width4Property = DependencyProperty.Register(
            "Width4", typeof(GridLength), typeof(StrengthReportBar), new PropertyMetadata(new GridLength(0.1, GridUnitType.Star)));

        /// <summary>
        /// Gets or sets the width of the grid column
        /// </summary>
        [Bindable(true)]
        public GridLength Width4
        {
            get => (GridLength)this.GetValue(Width4Property);
            set
            {
                this.SetValue(Width4Property, value);
                this.UpdateGridLayout();
            }
        }

        /// <summary>
        /// Identifies the grid column width dependency property
        /// </summary>
        public static readonly DependencyProperty Width5Property = DependencyProperty.Register(
            "Width5", typeof(GridLength), typeof(StrengthReportBar), new PropertyMetadata(new GridLength(0.05, GridUnitType.Star)));

        /// <summary>
        /// Gets or sets the width of the grid column
        /// </summary>
        [Bindable(true)]
        public GridLength Width5
        {
            get => (GridLength)this.GetValue(Width5Property);
            set
            {
                this.SetValue(Width5Property, value);
                this.UpdateGridLayout();
            }
        }

        #endregion

        public StrengthReportBar()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the size of the grid
        /// </summary>
        private void UpdateGridLayout()
        {
            this.Col0.Width = this.Width0;
            this.Col1.Width = this.Width1;
            this.Col2.Width = this.Width2;
            this.Col3.Width = this.Width3;
            this.Col4.Width = this.Width4;
            this.Col5.Width = this.Width5;
        }
    }
}
