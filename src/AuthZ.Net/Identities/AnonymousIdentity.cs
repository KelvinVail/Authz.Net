namespace AuthZ.Net.Identities
{
    using AuthZ.Net.Interfaces;

    public class AnonymousIdentity : IIdentity
    {
        public string Id => "Anon";
    }
}
