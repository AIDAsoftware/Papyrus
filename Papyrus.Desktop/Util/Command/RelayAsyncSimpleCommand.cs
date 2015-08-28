﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Desktop.Util.Command
{
    public class RelayAsyncSimpleCommand<TResult> : RelayAsyncCommandBase, INotifyPropertyChanged {
        private readonly Func<CancellationToken, Task<TResult>> _execute;
        private readonly CancelAsyncCommand _cancelCommand;
        private NotifyTaskCompletion<TResult> _execution;
        private readonly Func<bool> canExecute;

        public RelayAsyncSimpleCommand(Func<CancellationToken, Task<TResult>> execute, Func<bool> canExecute = null)
        {
            execute.ThrowExceptionIfArgumentIsNull("execute");

            _execute = execute;
            _cancelCommand = new CancelAsyncCommand();
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) {
            return (Execution == null || Execution.IsCompleted) && (canExecute == null || canExecute());
        }

        public override async Task ExecuteAsync(object parameter) {
            _cancelCommand.NotifyCommandStarting();
            Execution = new NotifyTaskCompletion<TResult>(_execute(_cancelCommand.Token));
            RaiseCanExecuteChanged();

            if (Execution.TaskCompletion != null) {
                await Execution.TaskCompletion;
            }
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        public ICommand CancelCommand {
            get { return _cancelCommand; }
        }

        public NotifyTaskCompletion<TResult> Execution {
            get { return _execution; }
            private set
            {
                _execution = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand {
            private CancellationTokenSource _cts = new CancellationTokenSource();
            private bool _commandExecuting;

            public CancellationToken Token { get { return _cts.Token; } }

            public void NotifyCommandStarting() {
                _commandExecuting = true;
                if (!_cts.IsCancellationRequested) return;
                _cts = new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished() {
                _commandExecuting = false;
                RaiseCanExecuteChanged();
            }

            bool ICommand.CanExecute(object parameter) {
                return _commandExecuting && !_cts.IsCancellationRequested;
            }

            void ICommand.Execute(object parameter) {
                _cts.Cancel();
                RaiseCanExecuteChanged();
            }

            public event EventHandler CanExecuteChanged {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            private void RaiseCanExecuteChanged() {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    public static class RelayAsyncSimpleCommand {
        public static RelayAsyncSimpleCommand<object> Create(Func<Task> command, Func<bool> canExecute) {
            return new RelayAsyncSimpleCommand<object>(async _ => { await command(); return null; }, canExecute);
        }

        public static RelayAsyncSimpleCommand<TResult> Create<TResult>(Func<Task<TResult>> command, Func<bool> canExecute) {
            return new RelayAsyncSimpleCommand<TResult>(_ => command(), canExecute);
        }

        public static RelayAsyncSimpleCommand<object> Create(Func<CancellationToken, Task> command, Func<bool> canExecute) {
            return new RelayAsyncSimpleCommand<object>(async token => { await command(token); return null; }, canExecute);
        }

        public static RelayAsyncSimpleCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command, Func<bool> canExecute) {
            return new RelayAsyncSimpleCommand<TResult>(command, canExecute);
        }
    }
}
