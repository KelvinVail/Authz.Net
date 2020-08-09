namespace AuthZ.Net.Interfaces
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RegisterIdentityRequest
    {
        public RegisterIdentityRequest(
            string firstName,
            string lastName,
            string email,
            string countryCode,
            string organisationName)
        {
            this.FirstName = Validate(firstName, nameof(firstName));
            this.LastName = Validate(lastName, nameof(lastName));
            this.Email = ValidateEmail(Validate(email, nameof(email)));
            this.CountryCode = Validate(countryCode, nameof(countryCode));
            this.OrganisationName = Validate(organisationName, nameof(organisationName));
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string CountryCode { get; }

        public string OrganisationName { get; }

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
