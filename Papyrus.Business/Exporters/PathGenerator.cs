namespace Papyrus.Business.Exporters {
    public abstract class PathGenerator {
        protected string Language;
        protected string Product;
        protected string Version;

        public virtual PathGenerator ForLanguage(string language) {
            this.Language = language;
            return this;
        }

        public virtual PathGenerator ForProduct(string product) {
            this.Product = product;
            return this;
        }

        public virtual PathGenerator ForVersion(string version) {
            this.Version = version;
            return this;
        }

        public abstract string GenerateMkdocsPath();

        public abstract string GenerateDocumentRoute();
    }
}