namespace AuthZ.Net.Tests.TestDoubles
{
    using AuthZ.Net.Interfaces;

    public class IdentityStub : IIdentity
    {
        public IdentityStub(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
