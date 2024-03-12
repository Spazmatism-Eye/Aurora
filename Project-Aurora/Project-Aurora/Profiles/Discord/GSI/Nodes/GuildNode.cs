namespace AuroraRgb.Profiles.Discord.GSI.Nodes {
    public class GuildNode : AutoJsonNode<GuildNode> {
        public long Id;
        public string Name;

        internal GuildNode(string json) : base(json) { }
    }
}
