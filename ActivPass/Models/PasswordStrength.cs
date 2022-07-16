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
        NONE = 0,
        VERY_WEAK = 1,
        WEAK = 2,
        MEDIUM = 3,
        STRONG = 4,
        VERY_STRONG = 5,
        EXTREME_STRONG = 6
    }
}
