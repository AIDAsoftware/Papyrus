using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Topics;

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
                FromVersionId = "FirstVersionId",
                ToVersionId = "SecondVersionId"
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
            versionRange.ShouldBeEquivalentTo(editableVersionRange, options => options.Excluding(vr => vr.VersionRangeId));
            versionRange.Documents.Should().HaveCount(1);
            versionRange.Documents["es-ES"].ShouldBeEquivalentTo(editableDocument, options => options.Excluding(d => d.DocumentId));
        }
    }
}