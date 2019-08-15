#region License
// MIT License
// Copyright 2019 Henrik Peters
// See LICENSE file in the project root for full license information
#endregion
using System;
using System.Windows.Input;

namespace ActivPass.ViewModels
{
    /// <summary>
    /// This class can be used to link a command property to
    /// an executable action in the viewModel. This allows a
    /// simple forward execution. No parameters are used.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action _targetMethod;
        private Func<bool> _executeValidator;

        /// <summary>
        /// Is raised when <see cref="canExecute(object)"/> has changed.
        /// The CommandManager is not connected to save performance.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { /*CommandManager.RequerySuggested += value;*/ }
            remove { /*CommandManager.RequerySuggested -= value;*/ }
        }

        /// <summary>
        /// Create a new relay command that will run the passed
        /// action when <see cref="Execute(object)"/> is called.
        /// </summary>
        /// <param name="execute">Action for relaying to</param>
        /// <param name="canExecute">Function to call for canExecute</param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this._targetMethod = execute;
            this._executeValidator = canExecute;
        }

        /// <summary>
        /// Check if this command can be executed by the
        /// canExecute function (passed by the constructor).
        /// </summary>
        /// <param name="parameter">Parameters are not used</param>
        /// <returns>True when the command can execute</returns>
        public bool CanExecute(object parameter)
        {
            return _executeValidator == null ? true : _executeValidator.Invoke();
        }

        /// <summary>
        /// Execute the action passed by the constructor.
        /// </summary>
        /// <param name="parameter">Parameters are not used</param>
        public void Execute(object parameter)
        {
            _targetMethod?.Invoke();
        }
    }

    /// <summary>
    /// This class can be used to link a command property to
    /// an executable action in the viewModel. This allows a
    /// simple forward execution. It uses one parameter.
    /// </summary>
    /// <typeparam name="T">Type of the the parameter</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _targetMethod;
        private readonly Predicate<T> _canExecute;

        /// <summary>
        /// Is raised when <see cref="canExecute(object)"/> has changed.
        /// The CommandManager is not connected to save performance.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { /*CommandManager.RequerySuggested += value;*/ }
            remove { /*CommandManager.RequerySuggested -= value;*/ }
        }

        /// <summary>
        /// Create a new relay command that will run the passed
        /// action when <see cref="Execute(object)"/> is called.
        /// </summary>
        /// <param name="execute">Action for relaying to</param>
        /// <param name="canExecute">Predicate to call for canExecute</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            this._targetMethod = execute;
            this._canExecute = canExecute;
        }

        /// <summary>
        /// Check if this command can be executed by the
        /// canExecute function (passed by the constructor).
        /// </summary>
        /// <param name="parameter">Has to be of the type T</param>
        /// <returns>True when the command can execute</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute.Invoke((T)parameter);
        }

        /// <summary>
        /// Execute the action passed by the constructor.
        /// </summary>
        /// <param name="parameter">Has to be of the type T</param>
        public void Execute(object parameter)
        {
            _targetMethod?.Invoke((T)parameter);
        }
    }
}
