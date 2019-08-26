#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.IO;
using System.Xml.Serialization;

namespace ActivPass.Configuration
{
    /// <summary>
    /// Provides loading and saving functions for
    /// the configuration data by using an XMLSerializer.
    /// </summary>
    public static class ConfigProvider
    {
        private const string ApplicationFolderName = "ActivPass";
        private const string ConfigFileName = "Configuration.xml";

        /// <summary>
        /// Get the path for the config data. This will be the
        /// ApplicationFolderName constant in the users AppData folder.
        /// </summary>
        public static string AppDataPath
        {
            get => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + ApplicationFolderName;
        }

        /// <summary>
        /// Get the path of the configuration file.
        /// </summary>
        public static string ConfigPath
        {
            get => AppDataPath + "\\" + ConfigFileName;
        }

        /// <summary>
        /// Will be true when the config exists in the file system.
        /// </summary>
        public static bool ConfigExists
        {
            get => File.Exists(ConfigPath);
        }

        /// <summary>
        /// Load the configuration from the file system. The file will be 
        /// deserialized by using an XMLSerializer. The default config will
        /// be returned when the loading from the file system fails.
        /// </summary>
        /// <returns>Loaded config data or default config</returns>
        public static ConfigData LoadConfig()
        {
            ConfigData config = ConfigData.DefaultConfig;

            if (ConfigExists) {
                try {
                    using (var stream = new FileStream(ConfigPath, FileMode.Open)) {
                        var XML = new XmlSerializer(typeof(ConfigData));
                        config = (ConfigData)XML.Deserialize(stream);
                    }
                } catch { }
            }

            return config;
        }

        /// <summary>
        /// Save a configuration to the file system. The file will be
        /// created by using an XMLSerializer. When the application folder
        /// does not exist, it will be created.
        /// </summary>
        /// <param name="config">Configuration to store in the file</param>
        /// <returns>True when the config was written to disk successfully</returns>
        public static bool SaveConfig(ConfigData config)
        {
            if (!Directory.Exists(AppDataPath)) {
                Directory.CreateDirectory(AppDataPath);
            }

            try {
                using (var stream = new FileStream(ConfigPath, FileMode.Create)) {
                    var XML = new XmlSerializer(typeof(ConfigData));
                    XML.Serialize(stream, config);
                }

                return true;
            } catch {
                return false;
            }
        }
    }
}
