namespace AuthZ.Net
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;
    using AuthZ.Net.Interfaces;

    public class RegisterIdentityRequest : IIdentity
    {
        public RegisterIdentityRequest(
            string firstName,
            string lastName,
            string email,
            string countryCode,
            string organisationName,
            ClaimsPrincipal claimsPrincipal)
        {
            this.Id = ValidateId(claimsPrincipal);
            this.FirstName = Validate(firstName, nameof(firstName));
            this.LastName = Validate(lastName, nameof(lastName));
            this.Email = ValidateEmail(Validate(email, nameof(email)));
            this.CountryCode = Validate(countryCode, nameof(countryCode));
            this.OrganisationName = Validate(organisationName, nameof(organisationName));
        }

        public string Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string CountryCode { get; }

        public string OrganisationName { get; }

        private static string ValidateId(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) throw new ArgumentNullException(nameof(claimsPrincipal));
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(claim?.Value))
                throw new InvalidOperationException();
            return claim.Value;
        }

        private static string ValidateEmail(string email)
        {
            if (!new EmailAddressAttribute().IsValid(email))
                throw new InvalidOperationException($"{email} is not a valid email address.");

            return email;
        }

        private static string Validate(string input, string name)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(name);
            return input;
        }
    }
}
