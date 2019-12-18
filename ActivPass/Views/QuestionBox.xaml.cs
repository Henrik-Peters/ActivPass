#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ActivPass.Views
{
    /// <summary>
    /// Viewlogic for QuestionBox.xaml
    /// </summary>
    public partial class QuestionBox : Window
    {
        public QuestionBox(string message, string title)
        {
            InitializeComponent();

            //Set the dialog texts
            this.Title = title;
            this.QuestionLabel.Content = message;
        }
    }
}
