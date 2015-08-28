namespace Papyrus.Infrastructure.Core.SharedDomain.Validation {
    public class ValidationError {
        public string PropertyName { get; set; }
        public string MessageKey { get; set; }
        public string DisplayMessage { get; set; }
    }
}
