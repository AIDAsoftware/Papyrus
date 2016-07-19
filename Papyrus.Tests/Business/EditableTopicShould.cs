using System;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;
using Papyrus.Desktop;

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

        [Test]
        public void clone_itself() {
            var topic = AnyTopic();

            var cloned = topic.Clone();
            
            cloned.Should().Be(topic);
            cloned.Should().NotBeSameAs(topic);
        }
        
        [Test]
        public void clone_its_product() {
            var topic = AnyTopic();

            var cloned = topic.Clone();
            
            cloned.Product.Should().Be(topic.Product);
            cloned.Product.Should().NotBeSameAs(topic.Product);
        }

        [Test]
        public void clone_its_version_ranges() {
            var topic = AnyTopic();

            var cloned = topic.Clone();

            cloned.VersionRanges.Should().BeEquivalentTo(topic.VersionRanges);
            cloned.VersionRanges.Should().NotBeSameAs(topic.VersionRanges);
        }
        
        [Test]
        public void clone_its_documents() {
            var versionRange = AnyTopic().VersionRanges.First();

            var cloned = versionRange.Clone();

            cloned.Documents.ShouldBeEquivalentTo(versionRange.Documents);
            cloned.Documents.Should().NotBeSameAs(versionRange.Documents);
        }

        [Test]
        public void clone_its_versions() {
            var versionRange = AnyTopic().VersionRanges.First();

            var cloned = versionRange.Clone();

            cloned.FromVersion.ShouldBeEquivalentTo(versionRange.FromVersion);
            cloned.ToVersion.ShouldBeEquivalentTo(versionRange.ToVersion);
            cloned.FromVersion.Should().NotBeSameAs(versionRange.FromVersion);
            cloned.ToVersion.Should().NotBeSameAs(versionRange.ToVersion);
        }
        
        [Test]
        public void clone_a_document() {
            var versionRange = AnyTopic().VersionRanges.First();
            var oldDocument = versionRange.Documents.First();

            var clonedDocument = versionRange.Clone().Documents.First();

            clonedDocument.ShouldBeEquivalentTo(oldDocument);
            clonedDocument.Should().NotBeSameAs(oldDocument);
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