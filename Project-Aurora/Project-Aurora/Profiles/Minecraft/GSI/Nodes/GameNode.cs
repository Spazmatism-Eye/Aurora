namespace AuroraRgb.Profiles.Minecraft.GSI.Nodes {

    public class GameNode : AutoJsonNode<GameNode> {

        [AutoJsonPropertyName("keys")] public MinecraftKeyBinding[] KeyBindings;
        public bool ControlsGuiOpen;
        public bool ChatGuiOpen;

        internal GameNode() : base() { }
        internal GameNode(string json) : base(json) { }
    }
}
