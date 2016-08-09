using NSubstitute;
using Papyrus.Business.Domain.Documents;

namespace Papyrus.Tests.Business {
    internal static class TestExtensionMethodsForDocumentDto {
        // TODO : name
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