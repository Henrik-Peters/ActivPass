#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Reflection;
using System.Xml.Serialization;
using ActivPass.Localization;

namespace ActivPass.Configuration
{
    /// <summary>
    /// Configuration instance data for the main application.
    /// The setters should only be used in the serialisation process.
    /// </summary>
    [Serializable]
    [XmlRoot("ActivPass-Config")]
    public sealed class ConfigData : ICloneable
    {
        /// <summary>
        /// Contains a configuration with default values.
        /// This instance should be used as a fallback.
        /// </summary>
        public static readonly ConfigData DefaultConfig = new ConfigData(
            Language.English,
            WindowStartupState.Default
        );

        /// <summary>
        /// The version of ActivPass when this config was created.
        /// Can be used to detect old config versions on startup.
        /// </summary>
        [XmlAttribute("ActivPass-Version")]
        public string Version { get; set; }

        /// <summary>
        /// Constructor for serialization
        /// </summary>
        private ConfigData() { }

        /// <summary>
        /// Localization language to use for translations.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Options for the size and position
        /// of the main window at startup.
        /// </summary>
        public WindowStartupState StartupLayout { get; set; }

        /// <summary>
        /// Create a new config instance with all config values.
        /// All properties should only be set with this constructor.
        /// </summary>
        /// <param name="Language"></param>
        /// <param name="StartupLayout"></param>
        public ConfigData(Language Language, WindowStartupState StartupLayout)
        {
            this.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Language = Language;
            this.StartupLayout = StartupLayout;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}