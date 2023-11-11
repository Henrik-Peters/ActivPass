#region License
// MIT License
// Copyright 2023 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using ActivPass.Models;

namespace ActivPass.ViewModels
{
    public class PassReportViewModel : ViewModel
    {
        private PasswordItem _proxy;

        /// <summary>
        /// The represented password item model.
        /// </summary>
        public PasswordItem Proxy
        {
            get => _proxy;
            set => SetProperty(ref _proxy, value);
        }

        /// <summary>
        /// Name of the password item.
        /// </summary>
        public string Name
        {
            get => _proxy.Name;
            set
            {
                if (_proxy.Name != value) {
                    _proxy.Name = value;
                    NotifyPropertyChanged(nameof(_proxy.Name));
                }
            }
        }

        /// <summary>
        /// Username of the password item.
        /// </summary>
        public string Username
        {
            get => _proxy.Username;
            set
            {
                if (_proxy.Username != value) {
                    _proxy.Username = value;
                    NotifyPropertyChanged(nameof(_proxy.Username));
                }
            }
        }

        /// <summary>
        /// Password of the password item.
        /// </summary>
        public string Password
        {
            get => _proxy.Password;
            set
            {
                if (_proxy.Password != value) {
                    _proxy.Password = value;
                    NotifyPropertyChanged(nameof(_proxy.Password));
                }
            }
        }

        /// <summary>
        /// Url of the password item.
        /// </summary>
        public string Url
        {
            get => _proxy.Url;
            set
            {
                if (_proxy.Url != value) {
                    _proxy.Url = value;
                    NotifyPropertyChanged(nameof(_proxy.Url));
                }
            }
        }

        /// <summary>
        /// Create a new view model from a password item.
        /// </summary>
        /// <param name="proxy">Represented password item</param>
        public PassReportViewModel(PasswordItem proxy)
        {
            this._proxy = proxy;
        }
    }
}
