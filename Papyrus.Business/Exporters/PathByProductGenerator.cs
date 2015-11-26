namespace Papyrus.Business.Exporters {
    public class PathByProductGenerator : PathGenerator {
        public override string GeneratePath() {
            var separator = "/";
            return Version + separator + Product + separator + Language;
        }
    }
}