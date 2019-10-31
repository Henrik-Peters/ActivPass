#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System.Collections.Generic;

namespace ActivPass.Localization
{
    public class TranslateManager
    {
        /// <summary>
        /// The current target language for the translation.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Lookup map for language translations.
        /// </summary>
        static IDictionary<string, List<string>> TranslateTable = new Dictionary<string, List<string>>
        {
            ["Menu"] = new List<string> { "Menu", "Menü" },
            ["AddSecret"] = new List<string> { "Add Secret", "Eintrag erstellen" },
            ["Search"] = new List<string> { "Search", "Suchen" },
            ["Lock"] = new List<string> { "Lock", "Sperren" },
            ["Export"] = new List<string> { "Export", "Exportieren" },
            ["Exit"] = new List<string> { "Exit", "Beenden" },
            ["ContainerInitTitle"] = new List<string> { "ActivPass - Create container", "ActivPass - Neuen Container anlegen" },
            ["Cancel"] = new List<string> { "Cancel", "Abbrechen" },
            ["Login"] = new List<string> { "Login", "Anmelden" },
            ["ContainerSelect"] = new List<string> { "Container", "Container" },
            ["LoginFailed"] = new List<string> { "Container master password incorrect", "Fehlerhaftes Container Master Passwort" },
            ["CreateContainer"] = new List<string> { "Create container", "Container erstellen" },
            ["ContainerName"] = new List<string> { "Container name", "Containername" },
            ["MasterPassword"] = new List<string> { "Master password", "Master Passwort" },
            ["MasterPasswordRepeat"] = new List<string> { "Repeat master password", "Master Passwort wiederholen" },
            ["CreateContainerInfo"] = new List<string> { "Create a new password box and secure it with a master password.\nThe storage of the box will be enrypted with the master password.", "Erstellen Sie eine neue Passwortbox und sichern diese mit dem\nMaster Passwort ab. Der Speicher wird mit damit verschlüsselt." },
            ["ContainerCreateFail"] = new List<string> { "Failed to create new container", "Container konnte nicht erstellt werden" },
            ["ContainerNameExists"] = new List<string> { "Container name already exists", "Ein Container mit diesem Namen existiert bereits" },
            ["ContainerStoreFail"] = new List<string> { "Failed to store the container", "Container konnte nicht gespeichert werden" }
        };

        /// <summary>
        /// Get the translation for a specific key.
        /// </summary>
        /// <exception cref="KeyNotFoundException">When the key is not in the translation table</exception>
        /// <exception cref="ArgumentOutOfRangeException">When no translation is available</exception>
        /// <param name="key">Key to use for translation</param>
        /// <returns>Translated string for the key</returns>
        public string this[string key]
        {
            get => TranslateTable[key][(int)Language];
        }

        /// <summary>
        /// Singleton instance for the global translate manager.
        /// </summary>
        private static TranslateManager translateManagerInstance = new TranslateManager(Language.English);

        /// <summary>
        /// Getter for the translate manager singleton.
        /// </summary>
        /// <returns>Global translation manager instance</returns>
        public static TranslateManager GetTranslateManager() => translateManagerInstance;

        /// <summary>
        /// Singleton constructor with a default language.
        /// </summary>
        /// <param name="language">Default to this language</param>
        private TranslateManager(Language language) {
            Language = language;
        }
    }
}
