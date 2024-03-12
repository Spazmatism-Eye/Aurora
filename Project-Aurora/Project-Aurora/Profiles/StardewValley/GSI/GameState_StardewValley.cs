using AuroraRgb.Profiles.Generic;
using AuroraRgb.Profiles.StardewValley.GSI.Nodes;

namespace AuroraRgb.Profiles.StardewValley.GSI {
    public class GameState_StardewValley : GameState {
        public ProviderNode Provider => NodeFor<ProviderNode>("Provider");

        public WorldNode World => NodeFor<WorldNode>("World");
        public PlayerNode Player => NodeFor<PlayerNode>("Player");
        public InventoryNode Inventory => NodeFor<InventoryNode>("Inventory");

        public JournalNode Journal => NodeFor<JournalNode>("Journal");
        public GameStatusNode Game => NodeFor<GameStatusNode>("Game");

        public GameState_StardewValley() : base() { }

        /// <summary>
        /// Creates a GameState_StardewValley instance based on the passed JSON data.
        /// </summary>
        /// <param name="JSONstring"></param>
        public GameState_StardewValley(string JSONstring) : base(JSONstring) { }
    }
}
