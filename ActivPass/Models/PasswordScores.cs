#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Linq;

namespace ActivPass.Models
{
    public static class PasswordScores
    {
        /// <summary>
        /// Rate the strength of a password and calculate
        /// the password strength score enum value
        /// </summary>
        /// <param name="password">Rate this password</param>
        /// <returns>Score of the password</returns>
        public static PasswordStrength GetScore(string password)
        {
            //Handle empty passwords
            if (password == null || password.Length == 0) {
                return PasswordStrength.NONE;

            } else if (password.Length <= 6) {
                //Short passwords with 6 chars or less are always very weak
                return PasswordStrength.VERY_WEAK;

            } else {
                //Start with weak because length is 7 or above
                int score = 2;

                //1 score point for long passwords
                if (password.Length >= 20) {
                    score++;
                }

                //1 score point for mixed lower and uppercase
                if (ContainsLowercase(password) && ContainsUppercase(password)) {
                    score++;
                }

                //1 score point for numbers and symbols
                if (ContainsNumbers(password) && ContainsSymbols(password)) {
                    score++;
                }

                //1 score point for complexity
                if (HasComplexScore(password)) {
                    score++;
                }

                //Max score is 6
                return (PasswordStrength)score;
            }
        }

        /// <summary>
        /// Check if a password passes the high requirement
        /// check to get all points for extremly strong
        /// </summary>
        /// <param name="password">Check this password</param>
        /// <returns>True when the password is very complex</returns>
        private static bool HasComplexScore(string password)
        {
            //Check the length first
            if (password.Length < 64){
                return false;
            } else {
                //Check that it contains chars from all groups
                bool differentCharGroups =
                    ContainsLowercase(password) &&
                    ContainsUppercase(password) &&
                    ContainsNumbers(password) &&
                    ContainsSymbols(password);

                if (!differentCharGroups) {
                    return false;
                } else{
                    //Count the amount of distinct charts
                    int distinctChars = password.Distinct().Count();
                    return distinctChars >= 40;
                }
            }
        }

        /// <summary>
        /// Check if the password has chars which are lowercase
        /// </summary>
        /// <param name="password">Check this password</param>
        /// <returns>True when lowercase chars were found</returns>
        private static bool ContainsLowercase(string password)
        {
            return password.Any(char.IsLower);
        }

        /// <summary>
        /// Check if the password has chars which are uppercase
        /// </summary>
        /// <param name="password">Check this password</param>
        /// <returns>True when uppercase chars were found</returns>
        private static bool ContainsUppercase(string password)
        {
            return password.Any(char.IsUpper);
        }

        /// <summary>
        /// Check if the password has chars which are numbers
        /// </summary>
        /// <param name="password">Check this password</param>
        /// <returns>True when numbers chars were found</returns>
        private static bool ContainsNumbers(string password)
        {
            return password.Any(char.IsNumber);
        }

        /// <summary>
        /// Check if the password has symbols which are numbers
        /// </summary>
        /// <param name="password">Check this password</param>
        /// <returns>True when symbols chars were found</returns>
        private static bool ContainsSymbols(string password)
        {
            return password.Any(c => char.IsSymbol(c) || char.IsPunctuation(c));
        }
    }
}
