#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows.Media;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using ActivPass.Localization;
using ActivPass.Models;

namespace ActivPass.Controls
{
    /// <summary>
    /// Interaktionslogik für PasswordStrengthBar.xaml
    /// </summary>
    public partial class PasswordStrengthBar : UserControl
    {
        #region DependencyProperties

        /// <summary>
        /// Identifies the TextProperty dependency property
        /// </summary>
        public static readonly DependencyProperty StrengthScoreProperty = DependencyProperty.Register(
            "StrengthScore", typeof(PasswordStrength), typeof(PasswordStrengthBar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the text to be displayed
        /// next to the time icon
        /// </summary>
        [Bindable(true)]
        public PasswordStrength StrengthScore
        {
            get => (PasswordStrength)this.GetValue(StrengthScoreProperty);
            set
            {
                this.SetValue(StrengthScoreProperty, value);
                this.UpdateScore(value);
            }
        }

        #endregion

        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public static TranslateManager Localize => TranslateManager.GetTranslateManager();

        public PasswordStrengthBar()
        {
            InitializeComponent();

            //Update inital score state
            this.UpdateScore(this.StrengthScore);
        }

        /// <summary>
        /// Update the user control state
        /// </summary>
        /// <param name="score">Show this score</param>
        private void UpdateScore(PasswordStrength score)
        {
            switch (score) {
                case PasswordStrength.NONE:
                    this.ScoreLabel.Content = Localize["ScoreNone"];
                    this.ApplyBaseColor(this.Bar0, this.Bar1, this.Bar2, this.Bar3, this.Bar4, this.Bar5);
                    break;

                case PasswordStrength.VERY_WEAK:
                    this.ScoreLabel.Content = Localize["ScoreVeryWeak"];
                    this.ApplyColor(this.Resources["BarColor0"] as Brush, this.Bar0);
                    this.ApplyBaseColor(this.Bar1, this.Bar2, this.Bar3, this.Bar4, this.Bar5);
                    break;

                case PasswordStrength.WEAK:
                    this.ScoreLabel.Content = Localize["ScoreWeak"];
                    this.ApplyColor(this.Resources["BarColor1"] as Brush, this.Bar0, this.Bar1);
                    this.ApplyBaseColor(this.Bar2, this.Bar3, this.Bar4, this.Bar5);
                    break;

                case PasswordStrength.MEDIUM:
                    this.ScoreLabel.Content = Localize["ScoreMedium"];
                    this.ApplyColor(this.Resources["BarColor2"] as Brush, this.Bar0, this.Bar1, this.Bar2);
                    this.ApplyBaseColor(this.Bar3, this.Bar4, this.Bar5);
                    break;

                case PasswordStrength.STRONG:
                    this.ScoreLabel.Content = Localize["ScoreStrong"];
                    this.ApplyColor(this.Resources["BarColor3"] as Brush, this.Bar0, this.Bar1, this.Bar2, this.Bar3);
                    this.ApplyBaseColor(this.Bar4, this.Bar5);
                    break;

                case PasswordStrength.VERY_STRONG:
                    this.ScoreLabel.Content = Localize["ScoreVeryStrong"];
                    this.ApplyColor(this.Resources["BarColor4"] as Brush, this.Bar0, this.Bar1, this.Bar2, this.Bar3, this.Bar4);
                    this.ApplyBaseColor(this.Bar5);
                    break;

                case PasswordStrength.EXTREME_STRONG:
                    this.ScoreLabel.Content = Localize["ScoreExtremeStrong"];
                    this.ApplyColor(this.Resources["BarColor5"] as Brush, this.Bar0, this.Bar1, this.Bar2, this.Bar3, this.Bar4, this.Bar5);
                    break;
            }
        }

        /// <summary>
        /// Apply the empty base color to the specified rectangles
        /// </summary>
        /// <param name="components">Apply to these components</param>
        private void ApplyBaseColor(params Rectangle[] components)
        {
            //Lookup the base color from resources
            var baseColor = this.Resources["BarColorBase"] as Brush;
            this.ApplyColor(baseColor, components);
        }

        /// <summary>
        /// Apply background color to the specified rectangles
        /// </summary>
        /// <param name="color">Apply this background color</param>
        /// <param name="components">Apply to these components</param>
        private void ApplyColor(Brush color, params Rectangle[] components)
        {
            foreach (var component in components)
            {
                component.Fill = color;
            }
        }
    }
}
