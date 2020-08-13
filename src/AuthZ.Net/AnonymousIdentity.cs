namespace AuthZ.Net
{
    using AuthZ.Net.Interfaces;

    public class AnonymousIdentity : IIdentity
    {
        public string Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string CountryCode { get; }

        public string OrganisationName { get; }
    }
}
