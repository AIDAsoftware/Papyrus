using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters {
    public class MkDocsExporter {
        private TopicRepository repository;

        public MkDocsExporter(TopicRepository repository) {
            this.repository = repository;
        }

        public async Task ExportDocumentsForProductToFolder(string papyrusid, DirectoryInfo targetDirectory) {
            targetDirectory.CreateSubdirectory("Español");
            targetDirectory.CreateSubdirectory("English");
        }
        
//        public async Task ExportDocumentsForProductToFolder(string papyrusid, DirectoryInfo targetDirectory) {
//            if (!targetDirectory.Exists) targetDirectory.Create();
//            var topicSummaries = await repository.GetAllTopicsSummaries();
//            foreach (var topicSummary in topicSummaries) {
//                await ExportTopic(topicSummary, targetDirectory);
//            }
//        }
//
//        private async Task ExportTopic(TopicSummary topic, DirectoryInfo targetDirectory) {
//            //var fileName = topic.Title + ".md";
//            //var filePath = Path.Combine(targetDirectory.FullName, fileName);
//            //await WriteTextAsync(filePath, topic.Content);
//        }
//
//        private async Task WriteTextAsync(string filePath, string text) {
//            byte[] encodedText = Encoding.UTF8.GetBytes(text);
//
//            using (FileStream sourceStream = new FileStream(filePath,
//                FileMode.Append, FileAccess.Write, FileShare.None,
//                bufferSize: 4096, useAsync: true)) {
//                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
//            };
//        }
    
    }
}
