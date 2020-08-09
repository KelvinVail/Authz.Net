namespace AuthZ.Net.Tests
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;
    using AuthZ.Net.Roles;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class RegisterIdentityTests
    {
        private readonly AuthZClient authZClient;

        private readonly IdentityRepositorySpy identityRepo = new IdentityRepositorySpy();

        public RegisterIdentityTests()
        {
            this.authZClient = new AuthZClient(this.identityRepo, new AnonymousRole());
        }

        [Theory]
        [InlineData("Bob", "Smith", "UK", "OrgName", "bob@smith.com")]
        [InlineData("Katie", "Jones", "US", "OrgName2", "katie@jones.com")]
        public async Task RegisterIdentityRequestShouldAddUserToRepo(
            string firstName,
            string lastName,
            string countryCode,
            string orgName,
            string email)
        {
            var request = new RegisterIdentityRequest(firstName, lastName, email, countryCode, orgName);
            await this.authZClient.Register(request);

            Assert.Equal(firstName, this.identityRepo.LastIdentityRegistered.FirstName);
            Assert.Equal(lastName, this.identityRepo.LastIdentityRegistered.LastName);
            Assert.Equal(email, this.identityRepo.LastIdentityRegistered.Email);
            Assert.Equal(countryCode, this.identityRepo.LastIdentityRegistered.CountryCode);
            Assert.Equal(orgName, this.identityRepo.LastIdentityRegistered.OrganisationName);
        }

        [Fact]
        public void CreateUserShouldThrowIfFirstNameIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("firstName", () => new RegisterIdentityRequest(null, "X", "X", "X", "X"));
            Assert.Throws<ArgumentNullException>("firstName", () => new RegisterIdentityRequest(string.Empty, "X", "X", "X", "X"));
        }

        [Fact]
        public void CreateUserShouldThrowIfLastNameIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("lastName", () => new RegisterIdentityRequest("X", null, "X@X", "X", "X"));
            Assert.Throws<ArgumentNullException>("lastName", () => new RegisterIdentityRequest("X", string.Empty, "X@X", "X", "X"));
        }

        [Fact]
        public void CreateUserShouldThrowIfEmailIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("email", () => new RegisterIdentityRequest("X", "X", null, "X", "X"));
            Assert.Throws<ArgumentNullException>("email", () => new RegisterIdentityRequest("X", "X", string.Empty, "X", "X"));
        }

        [Fact]
        public void CreateUserShouldThrowIfCountryIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("countryCode", () => new RegisterIdentityRequest("X", "X", "X@X", null, "X"));
            Assert.Throws<ArgumentNullException>("countryCode", () => new RegisterIdentityRequest("X", "X", "X@X", string.Empty, "X"));
        }

        [Fact]
        public void CreateUserShouldThrowIfOrgIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("organisationName", () => new RegisterIdentityRequest("X", "X", "X@X", "X", null));
            Assert.Throws<ArgumentNullException>("organisationName", () => new RegisterIdentityRequest("X", "X", "X@X", "X", string.Empty));
        }

        [Theory]
        [InlineData("bob.smith.com")]
        [InlineData("katie.jones")]
        public void CreateUserShouldThrowIfEmailIsInvalid(string email)
        {
            var ex = Assert.Throws<InvalidOperationException>(() => new RegisterIdentityRequest("X", "X", email, "X", "X"));
            Assert.Equal($"{email} is not a valid email address.", ex.Message);
        }
    }
}
