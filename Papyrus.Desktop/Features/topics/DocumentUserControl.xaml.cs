using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop.Features.Topics
{
    public partial class DocumentUserControl : UserControl
    {
        public EditableDocument ViewModel
        {
            get { return DataContext as EditableDocument; }
        }

        public DocumentUserControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) {
                return;
            }

            var addImageSubscriber = EventBus.AsObservable<SelectedDiskImagesToInsertIn>()
                .Where(evt => ReferenceEquals(evt.Document, ViewModel))
                .Subscribe(evt => InserImages(evt.FileNames));
            Unloaded += (sender, args) => addImageSubscriber.Dispose();
        }

        private void InserImages(IEnumerable<string> fileNames) {
            var selectionPosition = ContentTextBox.SelectionStart;
            foreach (var fileName in fileNames) {
                ContentTextBox.Text = ContentTextBox.Text.Insert(selectionPosition, MardownForImage(fileName));
            }
            ContentTextBox.SelectionStart = selectionPosition;
        }

        private string MardownForImage(string fileName) {
            return String.Format("\r\n![]({0})\r\n", CalculateRelativePath(fileName));
        }

        private static string CalculateRelativePath(string fileName) {
            var basePath = ConfigurationManager.AppSettings["ImagesFolder"];
            var baseUri = new Uri(basePath);
            var absoluteUri = new Uri(fileName);
            var relativeUri = baseUri.MakeRelativeUri(absoluteUri);
            return relativeUri.ToString();
        }

        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e) {
            try {
                this.MarkdownPreview.InvokeScript("setVerticalScrollPosition", ((TextBox)sender).VerticalOffset);
            } catch (Exception exn) {
                Console.WriteLine(exn.Message);
            }
        }

        private void InsertImage(object sender, RoutedEventArgs e) {
            EventBus.Send(new SelectingImages(ViewModel));
        }
    }

    public class SelectingImages {
        public EditableDocument Document { get; private set; }
        public SelectingImages(EditableDocument editableDocument) {
            Document = editableDocument;
        }
    }
}
