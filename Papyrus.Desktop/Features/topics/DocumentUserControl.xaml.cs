using System;
using System.Windows;
using System.Windows.Controls;

namespace Papyrus.Desktop.Features.Topics
{
    public partial class DocumentUserControl : UserControl
    {
        public DocumentUserControl()
        {
            InitializeComponent();
        }

        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e) {
            try {
                //this.MarkdownPreview.InvokeScript("setVerticalScrollPosition", ((TextBox)sender).VerticalOffset);
            } catch (Exception exn) {
                Console.WriteLine(exn.Message);
            }
        }
    }
}
