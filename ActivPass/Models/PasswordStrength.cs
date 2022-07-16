#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
namespace ActivPass.Models
{
    /// <summary>
    /// Values for password ratings
    /// </summary>
    public enum PasswordStrength
    {
        VERY_WEAK = 0,
        WEAK = 1,
        MEDIUM = 2,
        STRONG = 3,
        VERY_STRONG = 4,
        EXTREME_STRONG = 5
    }
}
