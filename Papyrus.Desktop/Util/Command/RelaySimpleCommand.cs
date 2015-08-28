using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Papyrus.Infrastructure.Core;
using Papyrus.Infrastructure.Core.SharedDomain.Validation;

namespace Papyrus.Desktop.Util.Command
{
    public class RelaySimpleCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;
        private readonly Action<IList<ValidationError>> onValidationError;
        private readonly Action<Exception> onExecutionError;

        public RelaySimpleCommand(Action execute, Func<bool> canExecute = null, Action<IList<ValidationError>> onValidationError = null, Action<Exception> onExecutionError = null) {
            execute.ThrowExceptionIfArgumentIsNull("execute");

            this.execute = execute;
            this.canExecute = canExecute;
            this.onValidationError = onValidationError;
            this.onExecutionError = onExecutionError;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter) {
            return canExecute == null || canExecute();
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) {
            try {
                execute();
            }
            catch (ValidationException ex) {
                if (onValidationError != null) {
                    onValidationError(ex.ValidationErrors);
                }
            }
            catch (Exception ex) {
                if (onExecutionError != null) {
                    onExecutionError(ex);
                }
            }
        }
    }
}
