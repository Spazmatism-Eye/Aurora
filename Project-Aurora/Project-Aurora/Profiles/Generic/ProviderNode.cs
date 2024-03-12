namespace AuroraRgb.Profiles.Generic {
    
    public class ProviderNode : AutoJsonNode<ProviderNode> {

        public string Name;
        public int AppID;

        internal ProviderNode(string json) : base(json) { }
    }
}
