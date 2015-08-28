using System;
using System.Diagnostics;
using System.Windows.Input;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Desktop.Util.Command
{
    public class RelayCommand<T> : ICommand {
        private readonly Action<T> myExecute;
        private readonly Predicate<T> myCanExecute;

        public RelayCommand(Action<T> execute)
            : this(execute, null) {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute) {
            execute.ThrowExceptionIfArgumentIsNull("execute");

            myExecute = execute;
            myCanExecute = canExecute;
        }
 
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) {
            return myCanExecute == null || myCanExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) {
            if (parameter == null) {
                myExecute(default(T));
                return;
            }
            myExecute((T)parameter);
        }
    }
}