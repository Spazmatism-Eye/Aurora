namespace AuroraRgb.Profiles.Minecraft.GSI.Nodes {
    public class WorldNode : AutoJsonNode<WorldNode> {

        public long WorldTime;
        public bool IsDayTime;
        public bool IsRaining;
        public float RainStrength;
        public int DimensionID;

        internal WorldNode(string json) : base(json) { }
    }
}
