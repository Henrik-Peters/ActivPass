#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
namespace ActivPass.Models
{
    public class PasswordGeneratorOptions
    {
        private bool CapitalLetters { get; }
        private bool LowercaseLetters { get; }
        private bool Digits { get; }
        private bool Symbols { get; }
        private bool Spaces { get; }

        public PasswordGeneratorOptions(bool capitalLetters, bool lowercaseLetters, bool digits, bool symbols, bool spaces)
        {
            this.CapitalLetters = capitalLetters;
            this.LowercaseLetters = lowercaseLetters;
            this.Digits = digits;
            this.Symbols = symbols;
            this.Spaces = spaces;
        }

        public void Deconstruct(out bool capitalLetters, out bool lowercaseLetters, out bool digits, out bool symbols, out bool spaces)
        {
            capitalLetters = this.CapitalLetters;
            lowercaseLetters = this.LowercaseLetters;
            digits = this.Digits;
            symbols = this.Symbols;
            spaces = this.Spaces;
        }
    }
}
