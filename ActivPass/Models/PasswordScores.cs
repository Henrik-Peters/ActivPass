#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivPass.Models
{
    public static class PasswordScores
    {
        /// <summary>
        /// Rate the strength of a password and calculate
        /// the password strength enum value
        /// </summary>
        /// <param name="password">Rate this password</param>
        /// <returns>Score of the password</returns>
        public static PasswordStrength GetScore(string password)
        {
            switch (password.Length) {
                case 0:
                    return PasswordStrength.NONE;

                case 1:
                    return PasswordStrength.VERY_WEAK;

                case 2:
                    return PasswordStrength.WEAK;

                case 3:
                    return PasswordStrength.MEDIUM;

                case 4:
                    return PasswordStrength.STRONG;

                case 5:
                    return PasswordStrength.VERY_STRONG;

                case 6:
                    return PasswordStrength.EXTREME_STRONG;

                default:
                    return PasswordStrength.NONE;
            }
        }
    }
}
