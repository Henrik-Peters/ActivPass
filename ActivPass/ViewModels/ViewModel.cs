#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ActivPass.ViewModels
{
    /// <summary>
    /// This class provides an abstraction for the view-models.
    /// It contains the PropertyChanged event and some helper methods.
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Will be raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Perform a setter operation and notify the
        /// PropertyChanged event when the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property (class)</typeparam>
        /// <param name="field">Reference of the field to store the new value</param>
        /// <param name="value">The new value to store</param>
        /// <param name="propertyName">Leave empty to use the callers name</param>
        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] String propertyName = "") where T : class
        {
            if (field == value)
                return;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notify the PropertyChanged event. Should only
        /// be called when the value of the property changed.
        /// </summary>
        /// <param name="propertyName">Leave empty to use the callers name</param>
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region SetProperty overloadings for primitive types

        /// <summary>
        /// Perform a setter operation and notify the
        /// PropertyChanged event when the value has changed.
        /// </summary>
        /// <param name="field">Reference of the field to store the new value</param>
        /// <param name="value">The new value to store</param>
        /// <param name="propertyName">Leave empty to use the callers name</param>
        protected void SetProperty(ref bool field, bool value, [CallerMemberName] String propertyName = "")
        {
            if (field == value)
                return;

            field = value;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Perform a setter operation and notify the
        /// PropertyChanged event when the value has changed.
        /// </summary>
        /// <param name="field">Reference of the field to store the new value</param>
        /// <param name="value">The new value to store</param>
        /// <param name="propertyName">Leave empty to use the callers name</param>
        protected void SetProperty(ref Visibility field, Visibility value, [CallerMemberName] String propertyName = "")
        {
            if (field == value)
                return;

            field = value;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
