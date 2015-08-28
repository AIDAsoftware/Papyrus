using System;

namespace Papyrus.Infrastructure.Core {
    public static class ValidationExtensions {

            public static void ThrowExceptionIfArgumentIsNull(this object value, string paramName, string additionalMessage = null) {
                if (value != null) return;
                if (string.IsNullOrEmpty(additionalMessage)) throw new ArgumentException(paramName);
                throw new ArgumentException(additionalMessage, paramName);
            }
    }
}
