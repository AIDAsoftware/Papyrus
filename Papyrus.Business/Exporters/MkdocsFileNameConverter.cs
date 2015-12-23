using System.IO;

namespace Papyrus.Business.Exporters {
    public class MkdocsFileNameConverter {
        public static string ConvertToValidFileName(string title) {
            var invalidChars = Path.GetInvalidFileNameChars();
            var exportableTitle = title;
            foreach (var invalidChar in invalidChars) {
                exportableTitle = exportableTitle.Replace(invalidChar.ToString(), "-");
            }
            var invalidLetters = "áàäéèëíìïóòöúùuÁÀÄÉÈËÍÌÏÓÒÖÚÙÜçÇñ";
            var validLetters = "aaaeeeiiiooouuuAAAEEEIIIOOOUUUcCn";
            for (int i = 0; i < invalidLetters.Length; i++) {
                exportableTitle = exportableTitle.Replace(invalidLetters[i], validLetters[i]);
            }
            return exportableTitle;
        }
    }
}