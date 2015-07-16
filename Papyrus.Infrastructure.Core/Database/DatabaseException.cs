namespace Papyrus.Infrastructure.Core.Database {
    using System;

    public class DatabaseException : Exception {
        public DatabaseException(string message) : base(message) {}
    }
}
