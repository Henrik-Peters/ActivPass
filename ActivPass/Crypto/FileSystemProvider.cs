#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
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

        /// <summary>
        /// Instance for handling encryption keys
        /// </summary>
        private EncryptionKeys EncryptionKeys { get; }

        public FileSystemProvider()
        {
            this.EncryptionKeys = new EncryptionKeys(null, null);
        }

        /// <summary>
        /// Get the file path for a specific container name.
        /// </summary>
        /// <param name="containerName">Name of the container</param>
        /// <returns></returns>
        private string GetContainerFilePath(string containerName)
        {
            return this.StoragePath + "\\" + containerName + ".bin";
        }

        public string[] ListContainers()
        {
            if (!Directory.Exists(StoragePath)) {
                return new string[0];

            } else {
                return Directory.GetFiles(StoragePath, "*.bin", SearchOption.TopDirectoryOnly)
                        .Select(Path.GetFileNameWithoutExtension)
                        .ToArray();
            }
        }

        public PasswordContainer LoadContainer(string containerName, string masterKey)
        {
            try {

                //Create the container file path based on the name
                string containerFilePath = GetContainerFilePath(containerName);

                PasswordContainer container;
                byte[] key = EncryptionKeys.GenerateKey(masterKey);
                byte[] iv = EncryptionKeys.GenerateIV(masterKey);

                using (Aes aes = Aes.Create()) {
                    aes.Mode = CipherMode.CBC;
                    aes.KeySize = 256;

                    aes.Key = key;
                    aes.IV = iv;

                    //Create an decryptor to perform the stream transform
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    
                    using (var fstream = new FileStream(containerFilePath, FileMode.Open)) {
                        using (CryptoStream csDecrypt = new CryptoStream(fstream, decryptor, CryptoStreamMode.Read)) {

                            container = JsonSerializer.Deserialize<PasswordContainer>(csDecrypt);
                        }
                    }
                }

                return container;

            } catch {
                return null;
            }
        }

        public bool DeleteContainer(string containerName)
        {
            string filePath = GetContainerFilePath(containerName);

            try {
                //Check if the file exists and delete
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                else
                {
                    return false;
                }
            } catch {
                return false;
            }
        }

        public bool SaveContainer(PasswordContainer container, string masterKey)
        {
            if (!Directory.Exists(StoragePath)) {
                Directory.CreateDirectory(StoragePath);
            }

            return this.SaveContainer(container, masterKey, GetContainerFilePath(container.ContainerName));
        }

        public bool SaveContainer(PasswordContainer container, string masterKey, string containerFilePath)
        {
            byte[] key = EncryptionKeys.GenerateKey(masterKey);
            byte[] iv = EncryptionKeys.GenerateIV(masterKey);

            try {
                using (Aes aes = Aes.Create()) {
                    aes.Mode = CipherMode.CBC;
                    aes.KeySize = 256;

                    aes.Key = key;
                    aes.IV = iv;

                    //Create an encryptor to perform the stream transform
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (var fstream = new FileStream(containerFilePath, FileMode.Create)) {
                        using (CryptoStream csEncrypt = new CryptoStream(fstream, encryptor, CryptoStreamMode.Write)) {

                            JsonSerializer.Serialize(csEncrypt, container);
                        }
                    }
                }

                return true;
            } catch {
                return false;
            }
        }
    }
}
