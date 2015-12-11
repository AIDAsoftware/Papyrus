namespace Papyrus.Business.Exporters {
    public class PathByProductGenerator : PathGenerator {
        public override string GenerateMkdocsPath() {
            var separator = "/";
            return Version + separator + Product + separator + Language;
        }

        public override string GenerateDocumentRoute() {
            return string.Empty;
        }
    }
}