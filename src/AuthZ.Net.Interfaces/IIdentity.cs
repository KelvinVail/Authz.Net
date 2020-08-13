namespace AuthZ.Net.Interfaces
{
    public interface IIdentity
    {
        public string Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string CountryCode { get; }

        public string OrganisationName { get; }
    }
}