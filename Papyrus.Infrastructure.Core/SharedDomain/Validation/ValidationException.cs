using System;
using System.Collections.Generic;

namespace Papyrus.Infrastructure.Core.SharedDomain.Validation {
    public class ValidationException : Exception {
        public IList<ValidationError> ValidationErrors { get; set; }

        public ValidationException(string propertyName = null, string messageKey = null, string displayMessage = null) {
            ValidationErrors = new List<ValidationError> {
                new ValidationError {
                    MessageKey = messageKey, 
                    PropertyName = propertyName,
                    DisplayMessage = displayMessage
                }
            };
        }
    }
}