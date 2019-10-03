#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using ActivPass.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivPass.ViewModels
{
    public class ContainerInitViewModel : ViewModel
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        public ContainerInitViewModel()
        {

        }
    }
}
