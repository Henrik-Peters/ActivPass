#region License
// MIT License
// Copyright 2023 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace ActivPass.Crypto
{
    public class EncryptionKeys
    {
        /// <summary>
        /// Default salt to use for first hash calculation
        /// </summary>
        private readonly byte[] STATIC_HASH_SALT = [
            0xE2, 0xE3, 0xCD, 0xB5, 0x9A, 0xE7, 0x1F, 0xC3,
            0x98, 0x8E, 0x1B, 0x74, 0x59, 0xF7, 0x0C, 0x2F,
            0x54, 0xAC, 0xEB, 0x27, 0x9E, 0x64, 0xBD, 0x21,
            0xA5, 0x4E, 0x53, 0x61, 0xA9, 0xA7, 0x9E, 0xA4
        ];

        /// <summary>
        /// Default salt to use during argon hashing
        /// </summary>
        private readonly byte[] STATIC_ARGON_SALT = [
            0xA1, 0xB3, 0xF1, 0xB2, 0x3B, 0xB6, 0x1A, 0x2A,
            0x87, 0x50, 0x75, 0x26, 0xFA, 0xF8, 0xDA, 0x1C,
            0xAB, 0xAE, 0x62, 0xA1, 0xF8, 0x48, 0x91, 0x3E,
            0x39, 0xEC, 0x92, 0x23, 0xC9, 0xB3, 0xE3, 0xA5
        ];

        private readonly byte[] hashSalt;
        private readonly byte[] argonSalt;

        public EncryptionKeys(byte[] hashSalt, byte[] argonSalt)
        {
            this.hashSalt = hashSalt ?? STATIC_HASH_SALT;
            this.argonSalt = argonSalt ?? STATIC_ARGON_SALT;
        }

        /// <summary>
        /// Generate the bytes for the IV of the container
        /// encryption based on the master password
        /// </summary>
        /// <param name="masterKey">Use this master key</param>
        /// <returns>Byte array with 16 bytes of hash data</returns>
        public byte[] GenerateIV(string masterKey)
        {
            byte[] iv = new byte[16];
            byte[] masterKeyBytes = Encoding.UTF32.GetBytes(masterKey);
            byte[] inputBytes = (byte[])[.. this.hashSalt, .. masterKeyBytes];

            using (SHA256 sha256 = SHA256.Create()) {
                byte[] masterKeyHash = sha256.ComputeHash(inputBytes);

                for (int i = 0; i < iv.Length; i++) {
                    iv[i] = masterKeyHash[(i * 2) % masterKeyHash.Length];
                }
            }

            return iv;
        }

        /// <summary>
        /// Generate the bytes for the encryption key of
        /// the container based on the master password
        /// </summary>
        /// <param name="masterKey">Use this master key</param>
        /// <returns>Byte array with 32 bytes of hash data</returns>
        public byte[] GenerateKey(string masterKey)
        {
            //Prepare the hash input
            byte[] keyInput = this.GeneratePreHash(masterKey);

            //Perform argon2 hashing
            var argon2 = new Argon2i(keyInput) {
                DegreeOfParallelism = 32,
                MemorySize = 8192,
                Iterations = 100,
                Salt = this.argonSalt
            };

            //Get bytes from the hash
            var hash = argon2.GetBytes(32);
            return hash;
        }

        private byte[] GeneratePreHash(string masterKey)
        {
            byte[] key = new byte[32];
            byte[] masterKeyBytes = Encoding.UTF32.GetBytes(masterKey);
            byte[] inputBytes = (byte[])[.. this.hashSalt, .. masterKeyBytes];

            using (SHA256 sha256 = SHA256.Create()) {
                byte[] masterKeyHash = sha256.ComputeHash(inputBytes);

                for (int i = 0; i < key.Length; i++) {
                    key[i] = masterKeyHash[i % masterKeyHash.Length];
                }
            }

            return key;
        }
    }
}
