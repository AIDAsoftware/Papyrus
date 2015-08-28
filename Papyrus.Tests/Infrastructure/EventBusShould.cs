using System;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Tests.Infrastructure {
    [TestFixture]
    public class EventBusShould {

        [SetUp]
        public void SetUp() {
            EventBus.Clean();
        }

        [Test]
        public void subscribe_to_event() {
            var domainEventListenerTest = new SubscriberTest();
            EventBus.Subscribe(domainEventListenerTest);

            EventBus.Raise(new DomainEventTest());

            domainEventListenerTest.Handled.Should().BeTrue();
        }

        [Test]
        public void subscribe_to_event_only_once() {
            var domainEventListenerTest = new SubscriberTest();
            EventBus.Subscribe(domainEventListenerTest);
            EventBus.Subscribe(domainEventListenerTest);

            EventBus.Raise(new DomainEventTest());
            EventBus.Raise(new OtherDomainEventTest());

            domainEventListenerTest.HandledCount.Should().Be(1);
            domainEventListenerTest.HandledCountOtherDomain.Should().Be(1);
        }

        [Test]
        public void allow_multiple_subscribers_to_event() {
            var domainEventListenerTest = new SubscriberTest();
            var otherDomainEventListenerTest = new OtherSubscriberTest();
            EventBus.Subscribe(domainEventListenerTest);
            EventBus.Subscribe(otherDomainEventListenerTest);

            EventBus.Raise(new DomainEventTest());

            domainEventListenerTest.Handled.Should().BeTrue();
            otherDomainEventListenerTest.Handled.Should().BeTrue();
        }

        [Test]
        public void allow_same_listener_to_differents_event() {
            var domainEventListenerTest = new SubscriberTest();
            EventBus.Subscribe(domainEventListenerTest);

            EventBus.Raise(new DomainEventTest());
            EventBus.Raise(new OtherDomainEventTest());

            domainEventListenerTest.Handled.Should().BeTrue();
            domainEventListenerTest.HandledOtherDomainTest.Should().BeTrue();
        }

        [Test]
        public void avoid_error_on_publish_event_with_no_subscribers() {
            EventBus.Raise(new DomainEventTest());
        }

        [Test]
        public void not_stop_calling_listener_when_one_throws_exception() {
            var domainEventListenerTest = new SubscriberTest();
            EventBus.Subscribe(
                (Action<DomainEventTest>)(evt => {
                    throw new NotImplementedException();
                }));
            EventBus.Subscribe(domainEventListenerTest);
            var thrown = false;
            try {
                EventBus.Raise(new DomainEventTest());
            } catch (ListenerException ex) {
                thrown = true;
                ex.InnerException.GetType().Should().Be(typeof(NotImplementedException));
            } finally {
                thrown.Should().BeTrue();
                domainEventListenerTest.Handled.Should().BeTrue();
            }
        }

        [Test]
        public void accept_actions_as_listeners() {
            var called = false;
            DomainEventTest expected = null;
            var sentEvent = new DomainEventTest();
            EventBus.Subscribe((Action<DomainEventTest>)(evt => {
                called = true;
                expected = evt;
            }));

            EventBus.Raise(sentEvent);

            called.Should().BeTrue();
            sentEvent.Should().BeSameAs(expected);
        }


        public class DomainEventTest { }

        public class OtherDomainEventTest { }

        public class SubscriberTest : Subscriber<DomainEventTest>, Subscriber<OtherDomainEventTest> {
            public bool Handled { get; set; }
            public int HandledCount = 0;
            public bool HandledOtherDomainTest { get; set; }
            public int HandledCountOtherDomain = 0;

            public void Handle(DomainEventTest @event) {
                Handled = true;
                ++HandledCount;
            }

            public void Handle(OtherDomainEventTest @event) {
                HandledOtherDomainTest = true;
                ++HandledCountOtherDomain;
            }
        }

        public class OtherSubscriberTest : Subscriber<DomainEventTest> {
            public bool Handled { get; set; }

            public void Handle(DomainEventTest @event) {
                Handled = true;
            }
        }
    }
}
