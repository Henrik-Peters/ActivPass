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
            ["ScoreNone"] = new List<string> { "Score: None", "Stärke: Keine" },
            ["ScoreVeryWeak"] = new List<string> { "Score: Very weak", "Stärke: Sehr schwach" },
            ["ScoreWeak"] = new List<string> { "Score: Weak", "Stärke: Schwach" },
            ["ScoreMedium"] = new List<string> { "Score: Medium", "Stärke: Mittel" },
            ["ScoreStrong"] = new List<string> { "Score: Strong", "Stärke: Sehr stark" },
            ["ScoreVeryStrong"] = new List<string> { "Score: Very strong", "Stärke: Sehr stark" },
            ["ScoreExtremeStrong"] = new List<string> { "Score: Extremely strong", "Stärke: Extrem stark" },
            ["MasterPasswordChanged"] = new List<string> { "Master Password was changed", "Master Passwort wurde geändert" },
            ["ContainerRenameDetails"] = new List<string> { "The container name was successfully changed", "Der Container Name wurde erfolgreich geändert" },
            ["ContainerRenameHeader"] = new List<string> { "Rename container", "Container umbenennen" },
            ["EditContainerHeader"] = new List<string> { "Edit container settings", "Container Einstellungen bearbeiten" },
            ["DeleteContainer"] = new List<string> { "Delete Container", "Container löschen" },
            ["DeleteContainerSuccess"] = new List<string> { "The container was successfully deleted", "Der Container wurde erfolgreich gelöscht" },
            ["DeleteContainerDetails"] = new List<string> { "All stored container data will be permanently deleted", "Alle gespeicherten Daten werden permanent gelöscht" },
            ["ChangeMasterPassword"] = new List<string> { "If the master password is lost, access to the container is no longer possible", "Beim Verlust des Master Passworts ist kein Zugriff mehr auf den Container möglich" },
            ["AutoLock"] = new List<string> { "Automatically lock container in case of inactivity", "Container bei Inaktivität automatisch sperren" },
            ["Change"] = new List<string> { "Change", "Ändern" },
            ["Close"] = new List<string> { "Close", "Schließen" },
            ["Rename"] = new List<string> { "Rename", "Umbenennen" },
            ["ContainerName"] = new List<string> { "Container name", "Containername" },
            ["Notes"] = new List<string> { "Notes", "Notizen" },
            ["Url"] = new List<string> { "Url", "Url" },
            ["Menu"] = new List<string> { "Menu", "Menü" },
            ["AddSecret"] = new List<string> { "Add Secret", "Eintrag erstellen" },
            ["Search"] = new List<string> { "Search", "Suchen" },
            ["Open"] = new List<string> { "Open", "Öffnen" },
            ["Delete"] = new List<string> { "Delete", "Löschen" },
            ["Lock"] = new List<string> { "Lock", "Sperren" },
            ["Export"] = new List<string> { "Export", "Exportieren" },
            ["Import"] = new List<string> { "Import", "Importieren" },
            ["Exit"] = new List<string> { "Exit", "Beenden" },
            ["OverwriteDialogTitle"] = new List<string> { "Overwrite item", "Eintrag überschreiben" },
            ["OverwriteQuestionPart1"] = new List<string> { "Should the entry", "Soll der Eintrag" },
            ["OverwriteQuestionPart2"] = new List<string> { "be overwritten?", "überschrieben werden?" },
            ["ContainerInitTitle"] = new List<string> { "ActivPass - Create container", "ActivPass - Neuen Container anlegen" },
            ["EmptyContainerInfo"] = new List<string> { "There are no passwords in this container", "Dieser Container enthält keine Passwörter" },
            ["Cancel"] = new List<string> { "Cancel", "Abbrechen" },
            ["Login"] = new List<string> { "Login", "Anmelden" },
            ["Name"] = new List<string> { "Name", "Name" },
            ["Yes"] = new List<string> { "Yes", "Ja" },
            ["No"] = new List<string> { "No", "Nein" },
            ["Username"] = new List<string> { "Username", "Benutzername" },
            ["Save"] = new List<string> { "Save", "Speichern" },
            ["Settings"] = new List<string> { "Settings", "Einstellungen" },
            ["ContainerSelect"] = new List<string> { "Container", "Container" },
            ["LoginFailed"] = new List<string> { "Container master password incorrect", "Fehlerhaftes Container Master Passwort" },
            ["LoginContainerFailed"] = new List<string> { "Invalid container name", "Ungültiger Containername" },
            ["CreateContainer"] = new List<string> { "Create container", "Container erstellen" },
            ["NewContainer"] = new List<string> { "New container", "Container erstellen" },
            ["EditContainer"] = new List<string> { "Edit container", "Container bearbeiten" },
            ["ContainerName"] = new List<string> { "Container name", "Containername" },
            ["MasterPassword"] = new List<string> { "Master password", "Master Passwort" },
            ["Password"] = new List<string> { "Password", "Passwort" },
            ["ShowPassword"] = new List<string> { "Show password", "Zeige Passwort" },
            ["MasterPasswordRepeat"] = new List<string> { "Repeat master password", "Master Passwort wiederholen" },
            ["CreateContainerInfo"] = new List<string> { "Create a new password box and secure it with a master password.\nThe storage of the box will be enrypted with the master password.", "Erstellen Sie eine neue Passwortbox und sichern diese mit dem\nMaster Passwort ab. Der Speicher wird mit damit verschlüsselt." },
            ["ContainerCreateFail"] = new List<string> { "Failed to create new container", "Container konnte nicht erstellt werden" },
            ["ContainerNameExists"] = new List<string> { "Container name already exists", "Ein Container mit diesem Namen existiert bereits" },
            ["ContainerStoreFail"] = new List<string> { "Failed to store the container", "Container konnte nicht gespeichert werden" },
            ["CopyToClipboard"] = new List<string> { "Copy to clipboard", "In Zwischenablage kopieren" },
            ["DeleteDialogTitle"] = new List<string> { "Delete confirmation", "Löschbestätigung" },
            ["DeleteQuestionPart1"] = new List<string> { "Are you sure that", "Soll" },
            ["DeleteQuestionPart2"] = new List<string> { "should be deleted?", "wirklich gelöscht werden?" },
            ["CopyUsernameToClipboard"] = new List<string> { "Copy username", "Benutzernamen kopieren" },
            ["CopyPasswordToClipboard"] = new List<string> { "Copy password", "Passwort kopieren" },
            ["ConfigEditorHeadline"] = new List<string> { "Edit the application configuration", "Anwendungskonfiguration bearbeiten" },
            ["MaximizeWindowConfig"] = new List<string> { "Maximize window on startup", "Fenster beim Start maximieren" },
            ["Language"] = new List<string> { "Language", "Sprache" }
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
