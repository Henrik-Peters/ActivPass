#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;

namespace ActivPass.Models
{
    [Serializable]
    public class PasswordContainer
    {
        /// <summary>
        /// Unique name to identify the container
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Additional metadata about the username
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Toggle automatic inactivity locking
        /// </summary>
        public bool AutoLock { get; set; }

        /// <summary>
        /// Toggle custom inactivity locking time
        /// </summary>
        public bool CustomAutoLock { get; set; }

        /// <summary>
        /// Custom auto lock time in seconds
        /// </summary>
        public int AutoLockSeconds { get; set; }

        /// <summary>
        /// All stored password entries
        /// </summary>
        public PasswordItem[] Items { get; set; }

        public PasswordContainer(string ContainerName, string Owner, bool autoLock, bool customAutoLock, int autoLockSeconds)
        {
            this.ContainerName = ContainerName;
            this.Owner = Owner;

            this.Items = Array.Empty<PasswordItem>();
            AutoLock = autoLock;
            CustomAutoLock = customAutoLock;
            AutoLockSeconds = autoLockSeconds;  
        }

        public bool CheckPasswordDuplicates(string password, int excludeIndex)
        {
            for (int i = 0; i < this.Items.Length; i++) {
                if (i != excludeIndex && password == this.Items[i].Password) {
                    return true;
                }
            }

            return false;
        }
    }
}
