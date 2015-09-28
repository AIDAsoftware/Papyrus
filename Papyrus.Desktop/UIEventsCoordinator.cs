using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Papyrus.Business.Documents;
using Papyrus.Desktop.Features.Documents.Events;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop {
    public class UIEventsCoordinator {
        readonly List<object> services = new List<object>();

        public void SubscribeToDocumentEvents() {
            var documentService = GetService<DocumentService>();
            
            EventBus.Subscribe((Action<CreateNewDocument>)(async evt => {
                try {
                    await documentService.Create(evt.Document);
                }
                catch (Exception ex) {
                    EventBus.Raise(new ApplicationErrorOcurred(ex));
                }
            }));

        }
        public void AddService<T>(T service) {
            services.Add(service);
        }

        private T GetService<T>() where T : class {
            return (services.FirstOrDefault(service => (service is T)) as T);
        }
    }

    public class ApplicationErrorOcurred {
        public ApplicationErrorOcurred(Exception exception) {
            Exception = exception;
        }

        public Exception Exception { get; private set; }
    }
}
