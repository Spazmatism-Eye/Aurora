using Aurora.Profiles;
using Aurora.Profiles.Generic.GSI.Nodes;
using Aurora.Profiles.LethalCompany.GSI.Nodes;
using AuroraRgb.Profiles.LethalComapny.GSI.Nodes;
using AuroraRgb.Profiles.LethalCompany.GSI.Nodes;

namespace Aurora.Profiles.LethalCompany.GSI
{

    public class GameState_LethalCompany : GameState
    {

        private ProviderNode _Provider;
        private GameStateNode _GameState;
        private WorldNode _World;
        private PlayerNode _Player;

        /// <summary>
        /// Provider node provides information about the data source so that Aurora can update the correct gamestate.
        /// </summary>
        public ProviderNode Provider
        {
            get
            {
                if (_Provider == null)
                    _Provider = new ProviderNode(_ParsedData["provider"]?.ToString() ?? "");
                return _Provider;
            }
        }

        /// <summary>
        /// Game node provides information about the GameState (InMenu/loading/InGame) source so that Aurora can update the correct gamestate.
        /// </summary>
        public GameStateNode GameState
        {
            get
            {
                if (_GameState == null)
                    _GameState = new GameStateNode(_ParsedData["game_state"]?.ToString() ?? "");
                return _GameState;
            }
        }

        /// <summary>
        /// World node provides information about the world (e.g. time).
        /// </summary>
        public WorldNode World
        {
            get
            {
                if (_World == null)
                    _World = new WorldNode(_ParsedData["world"]?.ToString() ?? "");
                return _World;
            }
        }

        /// <summary>
        /// Player node provides information about the player (e.g. health and hunger).
        /// </summary>
        public PlayerNode Player
        {
            get
            {
                if (_Player == null)
                    _Player = new PlayerNode(_ParsedData["player"]?.ToString() ?? "");
                return _Player;
            }
        }

        /// <summary>
        /// Creates a default GameState_LethalCompany instance.
        /// </summary>
        public GameState_LethalCompany() : base() { }

        /// <summary>
        /// Creates a GameState_LethalCompany instance based on the passed JSON data.
        /// </summary>
        /// <param name="JSONstring"></param>
        public GameState_LethalCompany(string JSONstring) : base(JSONstring) { }

    }
}
