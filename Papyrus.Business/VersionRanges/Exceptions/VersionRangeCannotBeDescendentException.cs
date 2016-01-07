using System;

namespace Papyrus.Business.VersionRanges.Exceptions {
    public class VersionRangeCannotBeDescendentException : Exception{
        public VersionRangeCannotBeDescendentException()
            : base("No pueden existir rangos de versiones descendientes. \n" +
                "Compruebe que todos los rangos para este topic son ascendientes.") {
        }
    }
}