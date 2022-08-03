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

        private bool _capitalLetters;
        public bool CapitalLetters
        {
            get => _capitalLetters;
            set {
                if (_capitalLetters != value) {
                    _capitalLetters = value;
                    NotifyPropertyChanged(nameof(CapitalLetters));
                    this.GeneratePassword();
                }
            }
        }

        private bool _lowercaseLetters;
        public bool LowercaseLetters
        {
            get => _lowercaseLetters;
            set {
                if (_lowercaseLetters != value) {
                    _lowercaseLetters = value;
                    NotifyPropertyChanged(nameof(LowercaseLetters));
                    this.GeneratePassword();
                }
            }
        }

        private bool _digits;
        public bool Digits
        {
            get => _digits;
            set {
                if (_digits != value) {
                    _digits = value;
                    NotifyPropertyChanged(nameof(Digits));
                    this.GeneratePassword();
                }
            }
        }

        private bool _symbols;
        public bool Symbols
        {
            get => _symbols;
            set {
                if (_symbols != value) {
                    _symbols = value;
                    NotifyPropertyChanged(nameof(Symbols));
                    this.GeneratePassword();
                }
            }
        }

        private bool _spaces;
        public bool Spaces
        {
            get => _spaces;
            set {
                if (_spaces != value) {
                    _spaces = value;
                    NotifyPropertyChanged(nameof(Spaces));
                    this.GeneratePassword();
                }
            }
        }

        private Visibility _cancelButtonVisible;
        public Visibility CancelButtonVisible
        {
            get => _cancelButtonVisible;
            set => SetProperty(ref _cancelButtonVisible, value);
        }

        private string _dialogButtonText;
        public string DialogButtonText
        {
            get => _dialogButtonText;
            set => SetProperty(ref _dialogButtonText, value);
        }

        private bool _applyResult;
        public bool ApplyResult
        {
            get => _applyResult;
            set => SetProperty(ref _applyResult, value);
        }

        /// <summary>
        /// Close the passed window instance.
        /// </summary>
        public ICommand Close { get; set; }

        /// <summary>
        /// Set the apply state to true and
        /// close the passed window instance.
        /// </summary>
        public ICommand Apply { get; set; }

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

            //Inital option values
            this.CapitalLetters = true;
            this.LowercaseLetters = true;
            this.Digits = true;
            this.Symbols = true;
            this.Spaces = false;

            //Inital button values
            this.CancelButtonVisible = Visibility.Visible;
            this.DialogButtonText = Localize["Apply"];
            this.ApplyResult = false;

            //Command bindings
            this.Close = new RelayCommand<Window>(CloseWindow);
            this.Apply = new RelayCommand<Window>(ApplyWindow);
            this.CopyToClipboard = new RelayCommand<string>(SetClipboardText);
            this.GenerateNextPassword = new RelayCommand(GeneratePassword);
        }

        /// <summary>
        /// This will hide the apply button and rename
        /// cancel to close for standalone dialog mode
        /// </summary>
        public void EnableStandaloneMode()
        {
            this.CancelButtonVisible = Visibility.Collapsed;
            this.DialogButtonText = Localize["Close"];
        }

        /// <summary>
        /// Generate the next random password with
        /// the current options in the view model
        /// </summary>
        private void GeneratePassword()
        {
            //Generate random password
            string password = PasswordGenerator.GeneratePassword((uint)this.Length,
                new(this.CapitalLetters, this.LowercaseLetters, this.Digits, this.Symbols, this.Spaces)
            );

            this.Password = password;
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
        /// Set the apply state and close the window instance.
        /// </summary>
        /// <param name="window">Instance to close</param>
        private void ApplyWindow(Window window)
        {
            this.ApplyResult = true;
            this.CloseWindow(window);
        }

        /// <summary>
        /// Close the passed window instance.
        /// </summary>
        /// <param name="window">Instance to close</param>
        private void CloseWindow(Window window)
        {
            if (window != null) {
                window.Close();
            }
        }
    }
}
