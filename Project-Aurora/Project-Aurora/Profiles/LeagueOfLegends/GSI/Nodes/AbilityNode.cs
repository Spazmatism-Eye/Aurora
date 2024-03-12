using AuroraRgb.Nodes;

namespace AuroraRgb.Profiles.LeagueOfLegends.GSI.Nodes
{
    public class AbilityNode : Node
    {
        public bool Learned => Level != 0;
        public int Level;
        public string Name = "";

        //TODO: there might be additional useful info to add here such as cooldown
    }
}
