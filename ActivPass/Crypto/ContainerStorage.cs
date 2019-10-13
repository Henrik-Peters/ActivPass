#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
namespace ActivPass.Crypto
{
    /// <summary>
    /// Singleton for providing the
    /// container storage implementation.
    /// </summary>
    public class ContainerStorage
    {
        private IContainerProvider StorageImplementation { get; set; }

        public static IContainerProvider ContainerProvider => instance.StorageImplementation;
        
        /// <summary>
        /// Singleton instance with default constructor
        /// </summary>
        private static ContainerStorage instance = new ContainerStorage(new FileSystemProvider());

        private ContainerStorage(IContainerProvider containerProvider)
        {
            this.StorageImplementation = containerProvider;
        }
    }
}
