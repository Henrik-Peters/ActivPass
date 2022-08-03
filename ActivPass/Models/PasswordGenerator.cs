#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Text;
using System.Security.Cryptography;

namespace ActivPass.Models
{
    public static class PasswordGenerator
    {
        //Char pool constants
        private readonly static string CAPITAL_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly static string LOWERCASE_LETTERS = "abcdefghijklmnopqrstuvwxyz";
        private readonly static string DIGITS = "0123456789";
        private readonly static string ASCII_SYMBOLS = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        /// <summary>
        /// Generate a random password with the specified
        /// options based on a cryptographic safe random
        /// </summary>
        /// <param name="length">Length of the Password</param>
        /// <param name="options">Use these options for generation</param>
        /// <returns>Generated password text</returns>
        public static string GeneratePassword(uint length, PasswordGeneratorOptions options) {
            //Destruct options
            var (capitalLetters, lowercaseLetters, digits, symbols, spaces) = options;

            //Pool with all chars to use for generation
            string charPool = "";

            if (capitalLetters) {
                charPool += CAPITAL_LETTERS;
            }

            if (lowercaseLetters) {
                charPool += LOWERCASE_LETTERS;
            }

            if (digits) {
                charPool += DIGITS;
            }

            if (symbols) {
                charPool += ASCII_SYMBOLS;
            }

            if (spaces) {
                charPool += " ";
            }

            //Make sure the char pool is not empty
            if (charPool.Length == 0) {
                return "";
            }

            //Create string builder for faster appending
            StringBuilder builder = new();

            for (int i = 0; i < length; i++)
            {
                builder.Append(charPool[RandomNumberGenerator.GetInt32(charPool.Length)]);
            }

            return builder.ToString();
        }
    }
}
