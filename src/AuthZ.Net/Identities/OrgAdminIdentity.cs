namespace AuthZ.Net.Identities
{
    using AuthZ.Net.Interfaces;

    public class OrgAdminIdentity : IIdentity
    {
        public OrgAdminIdentity(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
