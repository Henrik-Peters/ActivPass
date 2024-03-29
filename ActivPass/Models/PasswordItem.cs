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
        public string Notes { get; set; }

        public PasswordItem(string name, string username, string password, string url, string notes)
        {
            Name = name;
            Username = username;
            Password = password;
            Url = url;
            Notes = notes;
        }

        public bool HasBrowsableUrl()
        {
            return this.Url.StartsWith("http://") || this.Url.StartsWith("https://") || this.Url.StartsWith("www.");
        }

        public bool HasEncryptedTrafficUrl()
        {
            return UrlGrantsEncryptedTraffic(this.Url);
        }

        public static bool UrlGrantsEncryptedTraffic(string url)
        {
            return url.StartsWith("https://") ||
                url.StartsWith("sftp://") ||
                url.StartsWith("wss://") ||
                url.StartsWith("ldaps://") ||
                url.StartsWith("ftps://") ||
                url.StartsWith("ssh://") ||
                url.StartsWith("smtps://");
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
