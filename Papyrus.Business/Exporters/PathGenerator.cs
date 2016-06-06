namespace Papyrus.Business.Exporters {
    public class PathGenerator {
        private string Language;
        private string Product;
        private string Version;

        public virtual PathGenerator ForLanguage(string language) {
            Language = language;
            return this;
        }

        public virtual PathGenerator ForProduct(string product) {
            Product = product;
            return this;
        }

        public virtual PathGenerator ForVersion(string version) {
            Version = version;
            return this;
        }

        public virtual string GenerateMkdocsPath() {
            const string separator = "/";
            return Version + separator + Product + separator + Language;
        }
    }
}