using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop {
    public partial class App : Application
    {
        public App() {
            var userMessageRequestSubscription = EventBus.AsObservable<OnUserMessageRequest>().Subscribe(Handle);
            Exit += (sender, args) => userMessageRequestSubscription.Dispose();
        }

        public void Handle(OnUserMessageRequest domainEvent)
        {
            MessageBox.Show(domainEvent.Message, "Aviso");
        }
    }

    public class OnUserMessageRequest
    {
        public string Message { get; private set; }

        public OnUserMessageRequest(string message)
        {
            Message = message;
        }
    }
}
