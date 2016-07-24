using NSubstitute;
using Papyrus.Business;

namespace Papyrus.Tests {
    internal static class TestExtensionMethodsForDocumentDto {
        public static Document AsDocument(this DocumentDto documentDto) {
            return Arg.Is<Document>(d => 
                d.Title == documentDto.Title &&
                d.Description == documentDto.Description &&
                d.Content == documentDto.Content &&
                d.Language == documentDto.Language
                );
        }
    }
}