#region License
// MIT License
// Copyright 2023 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using ActivPass.ViewModels;
using System.Windows;

namespace ActivPass.Views
{
    /// <summary>
    /// Interaktionslogik für ContainerReport.xaml
    /// </summary>
    public partial class ContainerReport : Window
    {
        public ContainerReport()
        {
            InitializeComponent();

            //Init view model
            this.DataContext = new ContainerReportViewModel();
        }
    }
}
