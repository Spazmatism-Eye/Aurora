namespace AuroraRgb.Profiles.StardewValley.GSI.Nodes {

    public class InventoryNode : AutoJsonNode<InventoryNode>
    {
        public SelectedSlotNode SelectedSlot => NodeFor<SelectedSlotNode>("SelectedSlot");

        internal InventoryNode(string json) : base(json) { }
    }
    public class SelectedSlotNode : AutoJsonNode<SelectedSlotNode>
    {
        public int Number;
        public string ItemName;
        public SelectedSlotColorNode CategoryColor => NodeFor<SelectedSlotColorNode>("CategoryColor");

        internal SelectedSlotNode(string JSON) : base(JSON) { }
    }

    public class SelectedSlotColorNode : AutoJsonNode<SelectedSlotColorNode>
    {
        public float Red;
        public float Green;
        public float Blue;

        internal SelectedSlotColorNode(string JSON) : base(JSON) { }
    }
}