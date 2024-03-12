namespace AuroraRgb.Profiles.StardewValley.GSI.Nodes {

    public class JournalNode : AutoJsonNode<JournalNode>
    {
        public bool QuestAvailable;
        public bool NewQuestAvailable;
        public int QuestCount;

        internal JournalNode(string json) : base(json) { }
    }
}