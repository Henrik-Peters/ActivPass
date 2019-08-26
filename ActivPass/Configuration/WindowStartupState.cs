#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
namespace ActivPass.Configuration
{
    /// <summary>
    /// Options for the main window
    /// layout at application startup.
    /// </summary>
    public enum WindowStartupState
    {
        /// <summary>
        /// Use the startup options of the main window.
        /// </summary>
        Default,

        /// <summary>
        /// Set the window state to maximised.
        /// </summary>
        Maximised,

        /// <summary>
        /// Use the window size from the last session,
        /// but center the window on the screen.
        /// </summary>
        RestoreSizeCentered,

        /// <summary>
        /// Use the window size and position of the last session.
        /// </summary>
        RestoreAll
    }
}
