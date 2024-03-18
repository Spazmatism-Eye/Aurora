using AuroraRgb.Nodes;

namespace AuroraRgb.Profiles.LethalComapny.GSI.Nodes {
    public class PlayerNode : Node {

        public int Health;
        public int Stamina;

        internal PlayerNode(string json) : base(json) {
            Health = GetInt("health");
            Stamina = GetInt("stamina");
        }

    }
}
