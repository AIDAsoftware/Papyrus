using NSubstitute;
using Papyrus.Business.Domain.Documents;
using Papyrus.Business.Domain.Products;

namespace Papyrus.Tests.Business {
    internal static class TestExtensionMethodsForDocumentDto {
        public static Document Equivalent(this DocumentDto documentDto) {
            return Arg.Is<Document>(d => 
                d.Id.Value == documentDto.Id &&
                d.Title == documentDto.Title &&
                d.Description == documentDto.Description &&
                d.Content == documentDto.Content &&
                d.Language == documentDto.Language &&
                d.VersionIdentifier.Equals(new VersionIdentifier(documentDto.ProductId, documentDto.VersionId))
            );
        }
    }
}