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
        public string ContainerName { get; set; }
        public string Owner { get; set; }
        public bool AutoLock { get; set; }

        public PasswordItem[] Items { get; set; }

        public PasswordContainer(string ContainerName, string Owner, bool autoLock)
        {
            this.ContainerName = ContainerName;
            this.Owner = Owner;

            this.Items = new PasswordItem[0];
            AutoLock = autoLock;
        }
    }
}
