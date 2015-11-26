namespace Papyrus.Business.Exporters {
    public class PathByVersionGenerator : PathGenerator {
        public override string GeneratePath() {
            return Language + "/" + Version;
        }
    }
}