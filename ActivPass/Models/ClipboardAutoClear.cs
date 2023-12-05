#region License
// MIT License
// Copyright 2023 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Windows;
using System.Windows.Threading;

namespace ActivPass.Models
{
    public static class ClipboardAutoClear
    {
        private static DispatcherTimer ClipboardCleanupTimer = null;
        private static string ClipboardText = string.Empty;

        /// <summary>
        /// Start the timer to perform a clipboard cleanup.
        /// The cleanup will be performed after timeSpan.
        /// </summary>
        /// <param name="timeSpan">Wait this time before cleanup</param>
        /// <param name="clipboardText">Only cleanup with this text in clipboard</param>
        public static void ScheduleTimer(TimeSpan timeSpan, string clipboardText)
        {
            ClipboardText = (string)clipboardText.Clone();
            ClipboardCleanupTimer?.Stop();

            //Start the cleanup timer
            ClipboardCleanupTimer = new DispatcherTimer(
                timeSpan,
                DispatcherPriority.Normal,
                new EventHandler(ClipboardCleanupTimer_Tick),
                Application.Current.Dispatcher
            );
        }

        private static void ClipboardCleanupTimer_Tick(object sender, EventArgs e)
        {
            ClipboardCleanupTimer?.Stop();
            CleanupClipboard();
        }

        private static void CleanupClipboard()
        {
            string text = Clipboard.GetText(TextDataFormat.UnicodeText);

            if (text.Equals(ClipboardText) && ClipboardText != string.Empty) {
                Clipboard.Clear();
            }

            //Delete possible secret data from static memory
            ClipboardText = string.Empty;
            ClipboardCleanupTimer = null;
        }
    }
}
