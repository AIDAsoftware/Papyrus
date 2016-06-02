using System;
using System.Collections.Generic;

namespace Papyrus.Business.Exporters {
    internal class MkdocsConfiguration {
        private const string SimaSiteName = "SIMA Documentation";
        private const string DefaultTheme = "readthedocs";
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
            var themeLine = "theme: " + Theme + Environment.NewLine;
            var siteNameLine = "site_name: " + SiteName + Environment.NewLine;
            var pagesLines = "pages:" + Environment.NewLine;
            foreach (var page in pages) {
                pagesLines += "- '" + page.Key + "': " + "'" + page.Value + "'" + Environment.NewLine; 
            }
            return themeLine + siteNameLine + pagesLines;
        }
    }
}