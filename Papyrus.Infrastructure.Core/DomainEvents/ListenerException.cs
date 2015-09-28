using System;

namespace Papyrus.Infrastructure.Core.DomainEvents {
    public class ListenerException : Exception {
        public ListenerException(Exception innerException) : base("", innerException) { }
    }
}
