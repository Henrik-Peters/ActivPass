#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ActivPass.Models
{
    public static class PasswordGenerator
    {
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

            var nextRan = RandomNumberGenerator.GetInt32(10);

            return nextRan.ToString();
        }
    }
}
