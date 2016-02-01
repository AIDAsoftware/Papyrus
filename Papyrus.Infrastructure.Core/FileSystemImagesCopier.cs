using System;
using System.IO;

namespace Papyrus.Infrastructure.Core {
    public class FileSystemImagesCopier {
        public virtual void CopyFolder(string sourceFolder, string destinationFolder) {
            var dir = new DirectoryInfo(sourceFolder);

            if (!dir.Exists) {
                throw new DirectoryNotFoundException(
                    string.Format("Source directory does not exist or could not be found: {0}", sourceFolder));
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destinationFolder)) {
                Directory.CreateDirectory(destinationFolder);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string temppath = Path.Combine(destinationFolder, file.Name);
                file.CopyTo(temppath, false);
            }

            foreach (DirectoryInfo subdir in dirs) {
                string temppath = Path.Combine(destinationFolder, subdir.Name);
                CopyFolder(subdir.FullName, temppath);
            }
        }
   
    }
}