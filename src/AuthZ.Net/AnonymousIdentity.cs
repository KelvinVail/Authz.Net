namespace AuthZ.Net
{
    using AuthZ.Net.Interfaces;

    public class AnonymousIdentity : IIdentity
    {
        public string Id { get; }
    }
}
