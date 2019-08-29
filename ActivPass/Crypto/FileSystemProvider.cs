#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivPass.Configuration;
using ActivPass.Models;

namespace ActivPass.Crypto
{
    public class FileSystemProvider : IContainerProvider
    {
        /// <summary>
        /// Containers will be stored in the
        /// file system in this directory path.
        /// </summary>
        private string StoragePath
        {
            get => ConfigProvider.AppDataPath;
        }

        public PasswordContainer LoadContainer(string containerName, string masterKey)
        {
            return null;
        }

        public bool SaveContainer(PasswordContainer container, string masterKey)
        {
            if (!Directory.Exists(StoragePath)) {
                Directory.CreateDirectory(StoragePath);
            }

            return true;
        }
    }
}
