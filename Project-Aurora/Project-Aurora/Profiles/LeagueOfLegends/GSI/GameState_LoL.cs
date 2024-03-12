using AuroraRgb.Profiles.LeagueOfLegends.GSI.Nodes;

namespace AuroraRgb.Profiles.LeagueOfLegends.GSI
{
    public class GameState_LoL : GameState
    {
        private PlayerNode player;
        public PlayerNode Player => player ?? (player = new PlayerNode());

        private MatchNode match;
        public MatchNode Match => match ?? (match = new MatchNode());

        public GameState_LoL() : base()
        {

        }

        public GameState_LoL(string json_data) : base(json_data)
        {

        }
    }
}
