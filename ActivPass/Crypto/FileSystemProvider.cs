#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
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
                string containerFilePath = StoragePath + "\\" + containerName + ".bin";

                PasswordContainer container;
                byte[] key = GenerateKey(masterKey);
                byte[] iv = GenerateIV(masterKey);

                using (Aes aes = Aes.Create()) {
                    aes.Mode = CipherMode.CBC;
                    aes.KeySize = 256;

                    aes.Key = key;
                    aes.IV = iv;

                    //Create an decryptor to perform the stream transform
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    
                    using (var fstream = new FileStream(containerFilePath, FileMode.Open)) {
                        using (CryptoStream csDecrypt = new CryptoStream(fstream, decryptor, CryptoStreamMode.Read)) {

                            BinaryFormatter formatter = new BinaryFormatter();
                            container = (PasswordContainer)formatter.Deserialize(csDecrypt);
                        }
                    }
                }

                return container;

            } catch {
                return null;
            }
        }

        public bool SaveContainer(PasswordContainer container, string masterKey)
        {
            if (!Directory.Exists(StoragePath)) {
                Directory.CreateDirectory(StoragePath);
            }

            //Create the container file path based on the name
            string containerFilePath = StoragePath + "\\" + container.ContainerName + ".bin";

            byte[] key = GenerateKey(masterKey);
            byte[] iv = GenerateIV(masterKey);

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

                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(csEncrypt, container);
                        }
                    }
                }

                return true;
            } catch {
                return false;
            }
        }

        private byte[] GenerateIV(string masterKey)
        {
            byte[] iv = new byte[16];
            byte[] masterKeyBytes = Encoding.UTF8.GetBytes(masterKey);

            using (SHA256 sha256 = SHA256.Create()) {
                byte[] masterKeyHash = sha256.ComputeHash(masterKeyBytes);

                for (int i = 0; i < iv.Length; i++) {
                    iv[i] = masterKeyHash[(i * 2) % masterKeyHash.Length];
                }
            }

            return iv;
        }

        private byte[] GenerateKey(string masterKey)
        {
            byte[] key = new byte[32];
            byte[] masterKeyBytes = Encoding.UTF8.GetBytes(masterKey);

            using (SHA256 sha256 = SHA256.Create()) {
                byte[] masterKeyHash = sha256.ComputeHash(masterKeyBytes);

                for (int i = 0; i < key.Length; i++) {
                    key[i] = masterKeyHash[i % masterKeyHash.Length];
                }
            }

            return key;
        }
    }
}
