#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ActivPass.Localization;

namespace ActivPass.Controls
{
    public partial class StrengthReportBarLegend : UserControl
    {
        #region DependencyProperties

        /// <summary>
        /// Identifies the legend visibility dependency property
        /// </summary>
        public static readonly DependencyProperty ExtremeStrongVisibilityProperty = DependencyProperty.Register(
            "ExtremeStrongVisibility", typeof(Visibility), typeof(StrengthReportBarLegend), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Gets or sets the visibility for the current legend item
        /// </summary>
        [Bindable(true)]
        public Visibility ExtremeStrongVisibility
        {
            get => (Visibility)this.GetValue(ExtremeStrongVisibilityProperty);
            set
            {
                this.SetValue(ExtremeStrongVisibilityProperty, value);
                this.UpdateLegendVisibility();
            }
        }

        /// <summary>
        /// Identifies the legend visibility dependency property
        /// </summary>
        public static readonly DependencyProperty VeryStrongVisibilityProperty = DependencyProperty.Register(
            "VeryStrongVisibility", typeof(Visibility), typeof(StrengthReportBarLegend), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Gets or sets the visibility for the current legend item
        /// </summary>
        [Bindable(true)]
        public Visibility VeryStrongVisibility
        {
            get => (Visibility)this.GetValue(VeryStrongVisibilityProperty);
            set
            {
                this.SetValue(VeryStrongVisibilityProperty, value);
                this.UpdateLegendVisibility();
            }
        }

        /// <summary>
        /// Identifies the legend visibility dependency property
        /// </summary>
        public static readonly DependencyProperty StrongVisibilityProperty = DependencyProperty.Register(
            "StrongVisibility", typeof(Visibility), typeof(StrengthReportBarLegend), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Gets or sets the visibility for the current legend item
        /// </summary>
        [Bindable(true)]
        public Visibility StrongVisibility
        {
            get => (Visibility)this.GetValue(StrongVisibilityProperty);
            set
            {
                this.SetValue(StrongVisibilityProperty, value);
                this.UpdateLegendVisibility();
            }
        }

        /// <summary>
        /// Identifies the legend visibility dependency property
        /// </summary>
        public static readonly DependencyProperty MediumVisibilityProperty = DependencyProperty.Register(
            "MediumVisibility", typeof(Visibility), typeof(StrengthReportBarLegend), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Gets or sets the visibility for the current legend item
        /// </summary>
        [Bindable(true)]
        public Visibility MediumVisibility
        {
            get => (Visibility)this.GetValue(MediumVisibilityProperty);
            set
            {
                this.SetValue(MediumVisibilityProperty, value);
                this.UpdateLegendVisibility();
            }
        }

        /// <summary>
        /// Identifies the legend visibility dependency property
        /// </summary>
        public static readonly DependencyProperty WeakVisibilityProperty = DependencyProperty.Register(
            "WeakVisibility", typeof(Visibility), typeof(StrengthReportBarLegend), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Gets or sets the visibility for the current legend item
        /// </summary>
        [Bindable(true)]
        public Visibility WeakVisibility
        {
            get => (Visibility)this.GetValue(WeakVisibilityProperty);
            set
            {
                this.SetValue(WeakVisibilityProperty, value);
                this.UpdateLegendVisibility();
            }
        }

        /// <summary>
        /// Identifies the legend visibility dependency property
        /// </summary>
        public static readonly DependencyProperty VeryWeakVisibilityProperty = DependencyProperty.Register(
            "VeryWeakVisibility", typeof(Visibility), typeof(StrengthReportBarLegend), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Gets or sets the visibility for the current legend item
        /// </summary>
        [Bindable(true)]
        public Visibility VeryWeakVisibility
        {
            get => (Visibility)this.GetValue(VeryWeakVisibilityProperty);
            set
            {
                this.SetValue(VeryWeakVisibilityProperty, value);
                this.UpdateLegendVisibility();
            }
        }

        #endregion

        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public static TranslateManager Localize => TranslateManager.GetTranslateManager();

        public StrengthReportBarLegend()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the visibility of all legend items
        /// </summary>
        private void UpdateLegendVisibility()
        {
            //Extreme strong
            this.BorderExtremeStrong.Visibility = ExtremeStrongVisibility;
            this.LabelExtremeStrong.Visibility = ExtremeStrongVisibility;

            //Very strong
            this.BorderVeryStrong.Visibility = VeryStrongVisibility;
            this.LabelVeryStrong.Visibility = VeryStrongVisibility;

            //Strong
            this.BorderStrong.Visibility = StrongVisibility;
            this.LabelStrong.Visibility = StrongVisibility;

            //Medium
            this.BorderMedium.Visibility = MediumVisibility;
            this.LabelMedium.Visibility = MediumVisibility;

            //Weak
            this.BorderWeak.Visibility = WeakVisibility;
            this.LabelWeak.Visibility = WeakVisibility;

            //Very weak
            this.BorderVeryWeak.Visibility = VeryWeakVisibility;
            this.LabelVeryWeak.Visibility = VeryWeakVisibility;
        }
    }
}
