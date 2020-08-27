namespace AuthZ.Net.Identities
{
    using AuthZ.Net.Interfaces;

    public sealed class GlobalAdminIdentity : IIdentity
    {
        internal GlobalAdminIdentity(IIdentity identity)
        {
            this.Id = identity.Id;
        }

        public string Id { get; }
    }
}
