using System;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Exporters {
    internal class MkdocsConfiguration {
        private const string SimaSiteName = "SIMA Documentation";
        private const string DefaultTheme = "readthedocs";
        private const string KeyValueSeparator = ": ";
        public string Theme { get; set; }
        public string SiteName { get; set; }
        private readonly Dictionary<string, string> pages = new Dictionary<string, string>();

        public MkdocsConfiguration() {
            Theme = DefaultTheme;
            SiteName = SimaSiteName;
        }

        public void AddPage(string pageName, string fileName) {
            pages.Add(pageName, fileName);
        }

        public override string ToString() {
            var themeLine = "theme" + KeyValueSeparator + Theme + Environment.NewLine;
            var siteNameLine = "site_name" + KeyValueSeparator + SiteName + Environment.NewLine;
            var pagesLines = "pages" + KeyValueSeparator + Environment.NewLine;
            pagesLines = pages.Aggregate(pagesLines, (current, page) => current + ToMkdocsPageFormat(page));
            return themeLine + siteNameLine + pagesLines;
        }

        private static string ToMkdocsPageFormat(KeyValuePair<string, string> page) {
            return "- " + "'" + page.Key + "'" + KeyValueSeparator + "'" + page.Value + "'" + Environment.NewLine;
        }
    }
}