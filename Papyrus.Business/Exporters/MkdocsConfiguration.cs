using System;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Exporters {
    internal class MkdocsConfiguration {
        private const string MarkDownExtension = ".md";

        private const string SimaSiteName = "SIMA Documentation";
        private const string DefaultTheme = "readthedocs";
        private const string KeyValueSeparator = ": ";
        private static readonly string NewLine = Environment.NewLine;
        
        public string Theme { get; }
        public string SiteName { get; }
        public string SiteDir { get; }
        private readonly Dictionary<string, ExportableDocument> pages = new Dictionary<string, ExportableDocument>();

        public MkdocsConfiguration(string siteDir) {
            Theme = DefaultTheme;
            SiteName = SimaSiteName;
            SiteDir = siteDir;
        }

        public void AddPage(string pageName, ExportableDocument document)
        {
            pages.Add(pageName, document);
        }

        public override string ToString() {
            var themeLine = "theme" + KeyValueSeparator + Theme + NewLine;
            var siteNameLine = "site_name" + KeyValueSeparator + SiteName + NewLine;
            var siteDir = "site_dir" + KeyValueSeparator + SiteDir + NewLine;
            var pagesLines = "pages" + KeyValueSeparator + NewLine;
            pagesLines = pages
                .Select(k => new Page {
                    FileName = ConvertToValidFileName(k.Value.Title),
                    Title = k.Key,
                    Order = k.Value.Order
                })
                .OrderBy(p => p.Order)
                .Aggregate(pagesLines, (current, page) => current + ToMkdocsPageFormat(page));
            return themeLine + siteNameLine + siteDir + pagesLines;
        }

        private static string ToMkdocsPageFormat(Page page)
        {
            return "- " + "'" + page.Title + "'" + KeyValueSeparator + "'" + page.FileName + "'" + NewLine;
        }

        private static string ConvertToValidFileName(string title)
        {
            return MkdocsFileNameConverter.ConvertToValidFileName(title) + MarkDownExtension;
        }
    }

    internal class Page {
        public string FileName { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}