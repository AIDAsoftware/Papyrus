using System;

namespace Papyrus.Business.Topics {
    public class CannotCreateDocumentsWithoutTitleException : Exception {
        public CannotCreateDocumentsWithoutTitleException() : 
            base("No pueden existir documentos sin títulos. \n" +
                "Compruebe que todos sus documentos lo tienen.") {
        }
    }
}