using System.IO;

namespace Papyrus.Business.Exporters {
    public class MkdocsFileNameConverter {
        const string InvalidLetters = "áàäéèëíìïóòöúùuÁÀÄÉÈËÍÌÏÓÒÖÚÙÜçÇñ";
        const string ValidLetters = "aaaeeeiiiooouuuAAAEEEIIIOOOUUUcCn";
        private const string ValidChar = "-";

        public static string ConvertToValidFileName(string title) {
            var exportableTitle = ReplaceInvalidCharactersForASystemFile(title);
            exportableTitle = ReplaceInvalidCharactersForMkdocsFile(exportableTitle);
            return exportableTitle;
        }

        private static string ReplaceInvalidCharactersForMkdocsFile(string title) {
            for (var i = 0; i < InvalidLetters.Length; i++) {
                title = title.Replace(InvalidLetters[i], ValidLetters[i]);
            }
            return title;
        }

        private static string ReplaceInvalidCharactersForASystemFile(string title) {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var invalidChar in invalidChars) {
                title = title.Replace(invalidChar.ToString(), ValidChar);
            }
            return title;
        }
    }
}