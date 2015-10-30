using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommonMark;
using MarkdownSharp;

namespace Papyrus.Business.Topics
{
    public class EditableDocument : INotifyPropertyChanged
    {
        public string Language { get; set; }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        private string content;

        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                CommonMarkSettings settings = CommonMarkSettings.Default.Clone();
                settings.RenderSoftLineBreaksAsLineBreaks = true;
                var htmlResult = CommonMark.CommonMarkConverter.Convert(value, settings);
                //MarkdownOptions mkOptions = new MarkdownOptions() {AutoNewlines = true};
                //MarkdownSharp.Markdown mkParser = new Markdown(mkOptions);
                //var htmlResult = mkParser.Transform(value);
                htmlResult = string.Format(@"
                                            <!DOCTYPE html>
                                            <html>
                                                <head>
                                                    <base href='file://c:/'>
                                                    <meta charset='UTF-8'/>
                                                </head>
                                                <script type='text/javascript'>
                                                        function setVerticalScrollPosition(position) {{alert('xxx');document.body.scrollTop = position;}}
                                                </script>
                                                <body>
                                                    {0}
                                                </body>
                                            </html>", htmlResult);
                ContentAsHtml = htmlResult;
                OnPropertyChanged("Content");
            }
        }

        private string contentAsHtml;
        public string ContentAsHtml {
            get { return contentAsHtml; }
            set
            {
                contentAsHtml = value;
                OnPropertyChanged("ContentAsHtml");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ToDocument(VersionRange versionRange)
        {
            versionRange.AddDocument(new Document(
                title: Title,
                description: Description,
                content: Content,
                language: Language
                ));
        }
    }
}