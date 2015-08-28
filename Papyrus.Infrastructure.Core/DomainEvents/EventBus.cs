using System;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Infrastructure.Core.DomainEvents {
    public static class EventBus {
        private static ISet<Subscriber> subscribers = new HashSet<Subscriber>();

        public static void Subscribe(Subscriber handler) {
            subscribers.Add(handler);
        }

        public static void Subscribe<T>(Action<T> handler) {
            subscribers.Add(new AnonumousSubscriber<T>(handler));
        }

        public static void Raise<T>(T domainEvent) {
            Exception anyExceptionRaisedByASubscriber = null;
            foreach (var handlerOfT in subscribers.OfType<Subscriber<T>>()) {
                try {
                    handlerOfT.Handle(domainEvent);
                } catch (Exception ex) {
                    anyExceptionRaisedByASubscriber = ex;
                }
            }
            if (anyExceptionRaisedByASubscriber != null) {
                throw new ListenerException(anyExceptionRaisedByASubscriber);
            }
        }

        public static void Clean() {
            subscribers = null;
            subscribers = new HashSet<Subscriber>();
        }

        private class AnonumousSubscriber<T> : Subscriber<T> {
            readonly Action<T> Handler;

            public AnonumousSubscriber(Action<T> handler) {
                Handler = handler;
            }
            public void Handle(T domainEvent) {
                Handler(domainEvent);
            }
        }
    }
}
