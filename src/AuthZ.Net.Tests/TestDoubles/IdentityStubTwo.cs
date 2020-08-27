namespace AuthZ.Net.Tests.TestDoubles
{
    using AuthZ.Net.Interfaces;

    public class IdentityStubTwo : IIdentity
    {
        public IdentityStubTwo(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
