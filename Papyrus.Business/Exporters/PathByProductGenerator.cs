namespace Papyrus.Business.Exporters {
    public class PathByProductGenerator {
        private string language;
        private string product;
        private string version;

        public PathByProductGenerator ForLanguage(string language) {
            this.language = language;
            return this;
        }

        public PathByProductGenerator ForProduct(string product) {
            this.product = product;
            return this;
        }

        public PathByProductGenerator ForVersion(string version) {
            this.version = version;
            return this;
        }

        public string GeneratePath() {
            var separator = "/";
            return version + separator + product + separator + language;
        }
    }
}