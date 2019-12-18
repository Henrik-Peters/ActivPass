#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using ActivPass.Localization;

namespace ActivPass.ViewModels
{
    public class QuestionBoxViewModel : ViewModel
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        public QuestionBoxViewModel(){ }
    }
}
