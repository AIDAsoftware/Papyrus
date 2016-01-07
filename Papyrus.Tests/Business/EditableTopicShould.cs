using System;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class EditableTopicShould
    {
        [Test]
        public void convert_itself_to_a_topic()
        {
            var opportunity = new DisplayableProduct
            {
                ProductId = "OpportunityId",
                ProductName = "Opportunity"
            };
            var editableVersionRange = new EditableVersionRange
            {
                FromVersion = new ProductVersion("FirstVersionId", "1.0", DateTime.Today.AddDays(-2)),
                ToVersion = new ProductVersion("SecondVersionId", "2.0", DateTime.Today),
            };
            var editableDocument = new EditableDocument
            {
                Title = "Título",
                Description = "Descripción",
                Content = "Contenido",
                Language = "es-ES"
            };
            editableVersionRange.Documents.Add(editableDocument);
            var editableVersionRanges = new ObservableCollection<EditableVersionRange>
            {
                editableVersionRange
            };
            var editableTopic = new EditableTopic
            {
                Product = opportunity,
                TopicId = "TopicId",
                VersionRanges = editableVersionRanges
            };

            var topic = editableTopic.ToTopic();

            topic.ProductId.Should().Be("OpportunityId");
            topic.TopicId.Should().Be("TopicId");
            topic.VersionRanges.Should().HaveCount(1);
            var versionRange = topic.VersionRanges.First();
            versionRange.FromVersionId.Should().Be(editableVersionRange.FromVersion.VersionId);
            versionRange.ToVersionId.Should().Be(editableVersionRange.ToVersion.VersionId);
            versionRange.Documents.Should().HaveCount(1);
            versionRange.Documents["es-ES"].ShouldBeEquivalentTo(editableDocument, options => options.Excluding(d => d.DocumentId));
        }
    }
}