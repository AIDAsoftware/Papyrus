using System;

namespace Papyrus.Business.Documents {
    public class CannotCreateDocumentsWithoutTitleException : Exception {
        public CannotCreateDocumentsWithoutTitleException() :
            base("Debe facilitar un título para todos los documentos de este version range.") {
        }
    }
}