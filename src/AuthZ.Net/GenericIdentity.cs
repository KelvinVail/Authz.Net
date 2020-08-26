namespace AuthZ.Net
{
    using AuthZ.Net.Interfaces;

    public sealed class GenericIdentity : IIdentity
    {
        public GenericIdentity(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
