using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Papyrus.Infrastructure.Core.DomainEvents {
    public static class ReactiveEventBus {
        private static readonly Subject<object> messageSubject = new Subject<object>();

        public static void Send<T>(T message) {
            messageSubject.OnNext(message);
        }

        public static IObservable<T> AsObservable<T>() { return messageSubject.OfType<T>(); }
    }
}
