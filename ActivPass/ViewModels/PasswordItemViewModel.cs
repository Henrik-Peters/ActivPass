#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using ActivPass.Models;

namespace ActivPass.ViewModels
{
    /// <summary>
    /// View model implementation for password item controls.
    /// </summary>
    public class PasswordItemViewModel : ViewModel
    {
        private PasswordItem _proxy;

        /// <summary>
        /// The represented password item model.
        /// </summary>
        public PasswordItem Proxy {
            get => _proxy;
            set => SetProperty(ref _proxy, value);
        }

        /// <summary>
        /// Name of the password item.
        /// </summary>
        public string Name
        {
            get => _proxy.Name;
            set {
                if (_proxy.Name != value) {
                    _proxy.Name = value;
                    NotifyPropertyChanged(nameof(_proxy.Name));
                }
            }
        }
        
        /// <summary>
        /// Create a new view model from a password item.
        /// </summary>
        /// <param name="proxy">Represented password item</param>
        public PasswordItemViewModel(PasswordItem proxy)
        {
            this._proxy = proxy;
        }
    }
}
