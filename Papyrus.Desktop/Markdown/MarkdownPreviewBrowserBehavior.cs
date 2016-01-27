using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommonMark;

namespace Papyrus.Desktop.Markdown {
    public static class MarkdownPreviewBrowserBehavior {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html",
            typeof(string),
            typeof(MarkdownPreviewBrowserBehavior),
            new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d) {
            return (string)d.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value) {
            d.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e) {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) {
                return;
            }

            var webBrowser = dependencyObject as WebBrowser;
            if (webBrowser == null) return;
            var markdown = e.NewValue as string;
            var html = ConvertMarkdown2Html(markdown);
            webBrowser.NavigateToString(html);
        }

        private static string ConvertMarkdown2Html(string markdown) {
            var settings = CommonMarkSettings.Default.Clone();
            settings.RenderSoftLineBreaksAsLineBreaks = true;
            var htmlResult = CommonMarkConverter.Convert(markdown, settings);
            var imagesContainerPath = GetImagesContainerPath();
            htmlResult = string.Format(@"
                                            <!DOCTYPE html>
                                            <html>
                                                <head>
                                                    <base href='{0}'>
                                                    <meta charset='UTF-8'/>
                                                </head>
                                                <script type='text/javascript'>
                                                        function setVerticalScrollPosition(position) {{window.scrollTo(position, position);}}
                                                </script>
                                                <body>
                                                    {1}
                                                </body>
                                            </html>", imagesContainerPath.AbsoluteUri, htmlResult);
            return htmlResult;
        }

        private static Uri GetImagesContainerPath() {
            var imagesFolder = ConfigurationManager.AppSettings["ImagesFolder"];
            var imagesFolderParent = Directory.GetParent(imagesFolder).FullName;
            var imagesUri = new Uri(imagesFolderParent);
            return imagesUri;
        }
    }
}
