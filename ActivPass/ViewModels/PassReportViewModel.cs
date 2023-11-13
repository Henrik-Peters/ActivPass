#region License
// MIT License
// Copyright 2023 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using ActivPass.Models;
using System.Windows;
using System.Windows.Input;

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
                    NotifyPropertyChanged(nameof(ShowOpenBtn));
                }
            }
        }

        private PasswordStrength _passwordStrength;
        public PasswordStrength PasswordStrength
        {
            get => _passwordStrength;
            set
            {
                if (_passwordStrength != value)
                {
                    _passwordStrength = value;
                    NotifyPropertyChanged(nameof(PasswordStrength));
                }
            }
        }

        /// <summary>
        /// If the open url button should be visible.
        /// </summary>
        public Visibility ShowOpenBtn
        {
            get {
                if (this._proxy.HasBrowsableUrl()) {
                    return Visibility.Visible;
                } else {
                    return Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Open the argument as a new process.
        /// </summary>
        public ICommand OpenUrl { get; set; }

        /// <summary>
        /// Create a new view model from a password item.
        /// </summary>
        /// <param name="proxy">Represented password item</param>
        public PassReportViewModel(PasswordItem proxy)
        {
            this._proxy = proxy;

            //Init report values
            this.UpdatePasswordScore();

            //Command bindings
            this.OpenUrl = new RelayCommand<string>(OpenBrowserUrl);
        }

        /// <summary>
        /// Update the score rating of the current password
        /// </summary>
        public void UpdatePasswordScore()
        {
            //Calculate and apply the score
            PasswordStrength score = PasswordScores.GetScore(this.Password);
            this.PasswordStrength = score;
        }

        /// <summary>
        /// Open the url as a new browser process when
        /// the url is valid to launch a new process.
        /// </summary>
        /// <param name="url">Opent this url</param>
        private void OpenBrowserUrl(string url)
        {
            if (this._proxy.HasBrowsableUrl())
            {
                var startInfo = new System.Diagnostics.ProcessStartInfo { FileName = @url, UseShellExecute = true };
                System.Diagnostics.Process.Start(startInfo);
            }
        }
    }
}
