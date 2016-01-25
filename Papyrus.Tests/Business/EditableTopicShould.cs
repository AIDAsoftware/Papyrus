using System;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Documents;
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

        private readonly static ProductVersion product2 = new ProductVersion("first", "1.0", DateTime.MaxValue);
        private readonly static ProductVersion product1 = new ProductVersion("first", "1.0", DateTime.MinValue);

        [Test]
        public void equals_comparition() {
            var topic = AnyTopic();
            var topic2 = AnyTopic();
            topic.Should().Be(topic2);
        }

        private static EditableTopic AnyTopic() {
            return new EditableTopic {
                Product = new DisplayableProduct { ProductId = "Any", ProductName = "Any" },
                TopicId = "Any",
                VersionRanges = new ObservableCollection<EditableVersionRange> {
                    new EditableVersionRange {
                        FromVersion = product1,
                        ToVersion = product2,
                        Documents = new ObservableCollection<EditableDocument> {
                            new EditableDocument {
                                Title = "Any",
                                Content = "Any",
                                Description = "Any",
                                Language = "Any"
                            }
                        }
                    }
                }
            };
        }
    }
}