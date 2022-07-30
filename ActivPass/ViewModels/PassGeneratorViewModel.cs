#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Windows;
using System.Windows.Input;
using ActivPass.Localization;
using ActivPass.Models;

namespace ActivPass.ViewModels
{
    public class PassGeneratorViewModel : ViewModel
    {
        /// <summary>
        /// Reference for bindings to the translate singleton.
        /// </summary>
        public TranslateManager Localize => TranslateManager.GetTranslateManager();

        private string _password;
        public string Password
        {
            get => _password;
            set {
                if (_password != value) {
                    //Apply the new value
                    _password = value;
                    NotifyPropertyChanged(nameof(Password));

                    //Update the password score
                    this.UpdatePasswordScore();
                }
            }
        }

        private PasswordStrength _passwordStrength;
        public PasswordStrength PasswordStrength
        {
            get => _passwordStrength;
            set {
                if (_passwordStrength != value) {
                    _passwordStrength = value;
                    NotifyPropertyChanged(nameof(PasswordStrength));
                }
            }
        }

        private int _length;
        public int Length
        {
            get => _length;
            set {
                if (_length != value && value > 0) {
                    //Apply the new value
                    _length = value;
                    NotifyPropertyChanged(nameof(Length));

                    //Generate new password
                    this.GeneratePassword();
                }
            }
        }

        /// <summary>
        /// Close the passed window instance.
        /// </summary>
        public ICommand Close { get; set; }

        /// <summary>
        /// Copy the argument as unicode text to the clipboard.
        /// </summary>
        public ICommand CopyToClipboard { get; set; }

        /// <summary>
        /// Generate a new random password with the current options.
        /// </summary>
        public ICommand GenerateNextPassword { get; set; }

        public PassGeneratorViewModel()
        {
            //Initial values
            this.Length = 32;
            this.GeneratePassword();

            //Command bindings
            this.Close = new RelayCommand<Window>(CloseWindow);
            this.CopyToClipboard = new RelayCommand<string>(SetClipboardText);
            this.GenerateNextPassword = new RelayCommand(GeneratePassword);
        }

        /// <summary>
        /// Generate the next random password with
        /// the current options in the view model
        /// </summary>
        private void GeneratePassword()
        {
            string pass = "";

            for (int i = 0; i < this.Length; i++) {
                pass += "a";
            }

            this.Password = pass;
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
        /// Set a new text in the clipboard content.
        /// The data text format will be set to unicode text.
        /// </summary>
        /// <param name="text">Copy this text to the clipboard</param>
        private void SetClipboardText(string text)
        {
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
        }

        /// <summary>
        /// Close the passed window instance.
        /// </summary>
        /// <param name="window">Instance to close</param>
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
