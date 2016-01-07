using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Papyrus.Business.Documents;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Features.Topics;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop {
    public partial class App : Application
    {
        public App() {
            var userMessageRequestSubscription = EventBus.AsObservable<OnUserMessageRequest>().Subscribe(Handle);
            var addImageRequestSubcription = EventBus.AsObservable<SelectingImages>().Subscribe(Handle);
            Exit += (sender, args) => userMessageRequestSubscription.Dispose();
            Exit += (sender, args) => addImageRequestSubcription.Dispose();
        }

        private void Handle(SelectingImages request) {
            var initialDirectory = ConfigurationManager.AppSettings["ImagesFolder"];
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Multiselect = true,
                InitialDirectory = initialDirectory
            };
            if (openFileDialog.ShowDialog() == true) {
                EventBus.Send(new SelectedDiskImagesToInsertIn(request.Document, openFileDialog.FileNames));
            }
        }

        public void Handle(OnUserMessageRequest domainEvent)
        {
            MessageBox.Show(domainEvent.Message, "Aviso");
        }
    }

    public class SelectedDiskImagesToInsertIn {
        public EditableDocument Document { get; private set; }
        public IEnumerable<string> FileNames { get; private set; }
 
        public SelectedDiskImagesToInsertIn(EditableDocument document, IEnumerable<string> fileNames) {
            Document = document;
            FileNames = fileNames;
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
