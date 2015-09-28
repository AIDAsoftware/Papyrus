using System;

namespace Papyrus.Infrastructure.Core {
    public static class EventExtensions {
        public static void RaiseEvent(this EventHandler eventHandler, object sender, EventArgs e) {
            if (eventHandler != null) eventHandler(sender, e);
        }

        public static void RaiseEvent<T>(this EventHandler<T> eventHandler, object sender, T e)
            where T : EventArgs {
            if (eventHandler != null) eventHandler(sender, e);
        }
    }
}
