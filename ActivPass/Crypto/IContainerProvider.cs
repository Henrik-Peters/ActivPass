#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using ActivPass.Models;

namespace ActivPass.Crypto
{
    public interface IContainerProvider
    {
        /// <summary>
        /// Create a list with all available
        /// container names for loading.
        /// </summary>
        /// <returns>Array with container names</returns>
        string[] ListContainers();

        /// <summary>
        /// Load an encrypted container from the storage
        /// and decrypt it with the master key. When the
        /// operation has failed null will be returned.
        /// </summary>
        /// <param name="containerName">Name of the container</param>
        /// <param name="masterKey">Decrypt the container with this key</param>
        /// <returns>Decrypted container or null</returns>
        PasswordContainer LoadContainer(string containerName, string masterKey);

        /// <summary>
        /// Save a container to the storage. Existing container
        /// data will be overwritten. The container will be
        /// encrypted with the master key.
        /// </summary>
        /// <param name="container">Container to encrypt and save</param>
        /// <param name="masterKey">Key for the encryption</param>
        /// <returns>True when the operation was sucessful</returns>
        bool SaveContainer(PasswordContainer container, string masterKey);
    }
}
