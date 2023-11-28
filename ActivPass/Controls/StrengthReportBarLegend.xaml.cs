#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows.Controls;
using ActivPass.Localization;

namespace ActivPass.Controls
{
    public partial class StrengthReportBarLegend : UserControl
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public static TranslateManager Localize => TranslateManager.GetTranslateManager();

        public StrengthReportBarLegend()
        {
            InitializeComponent();
        }
    }
}
