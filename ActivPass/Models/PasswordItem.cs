﻿#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;

namespace ActivPass.Models
{
    [Serializable]
    public class PasswordItem : ICloneable
    {
        public string Name { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }

        public PasswordItem(string name, string username, string password, string url)
        {
            Name = name;
            Username = username;
            Password = password;
            Url = url;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
