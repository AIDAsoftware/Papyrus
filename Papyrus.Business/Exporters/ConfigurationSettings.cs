namespace Papyrus.Business.Exporters
{
    public class ConfigurationSettings {
        public string ExportationPath { get; }
        public string ImagesFolder { get; }
        public string SiteDir { get; }

        public ConfigurationSettings(string exportationPath, string imagesFolder, string siteDir = "") {
            ExportationPath = exportationPath;
            ImagesFolder = imagesFolder;
            SiteDir = siteDir;
        }

        protected bool Equals(ConfigurationSettings other)
        {
            return string.Equals(ExportationPath, other.ExportationPath) && 
                   string.Equals(ImagesFolder, other.ImagesFolder) && 
                   string.Equals(SiteDir, other.SiteDir);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ConfigurationSettings) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ExportationPath?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (ImagesFolder?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (SiteDir?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}