namespace AuroraRgb.Profiles.Slime_Rancher.GSI.Nodes {
    public class MailNode : AutoJsonNode<MailNode>
    {

        [AutoJsonPropertyName("new_mail")]
        public bool NewMail;

        internal MailNode(string json) : base(json) { }
    }
}
