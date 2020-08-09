namespace AuthZ.Net.Identities
{
    public class ExistingIdentity : IIdentity
    {
        public ExistingIdentity(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
