using Aurora.Nodes;

namespace Aurora.Profiles.LethalCompany.GSI.Nodes {
    public class GameStateNode : Node {

        public GameStateEnum GameState;

        internal GameStateNode(string json) : base(json) {
            GameState = GetEnum<GameStateEnum>("game_state");
        }
    }

    public enum GameStateEnum
    {
        InGame,
        Loading,
        InMenu
    }
}
