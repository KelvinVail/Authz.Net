namespace AuthZ.Net.Tests
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class AnonymousSessionTests
    {
        private readonly IdentityRepositorySpy identityRepo = new IdentityRepositorySpy();

        private readonly ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();

        private readonly AnonymousSession session;

        public AnonymousSessionTests()
        {
            this.session = new AnonymousSession(this.identityRepo);
            var claim = new Claim(ClaimTypes.NameIdentifier, "1");
            var claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(claim);

            this.claimsPrincipal.AddIdentity(claimIdentity);
        }

        [Theory]
        [InlineData("Bob", "Smith", "UK", "OrgName", "bob@smith.com")]
        [InlineData("Katie", "Jones", "US", "OrgName2", "katie@jones.com")]
        public async Task AnonymousUserCanRegisterAnIdentity(
            string firstName,
            string lastName,
            string countryCode,
            string orgName,
            string email)
        {
            var request = new RegisterIdentityRequest(
                firstName,
                lastName,
                email,
                countryCode,
                orgName,
                this.claimsPrincipal);
            await this.session.Register(request);

            Assert.Equal("1", this.identityRepo.LastIdentityRegistered.Id);
            Assert.Equal(firstName, this.identityRepo.LastIdentityRegistered.FirstName);
            Assert.Equal(lastName, this.identityRepo.LastIdentityRegistered.LastName);
            Assert.Equal(email, this.identityRepo.LastIdentityRegistered.Email);
            Assert.Equal(countryCode, this.identityRepo.LastIdentityRegistered.CountryCode);
            Assert.Equal(orgName, this.identityRepo.LastIdentityRegistered.OrganisationName);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("ABC")]
        [InlineData("null")]
        [InlineData("")]
        public async Task AnonymousUserCannotGetIdentity(string identityId)
        {
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => this.session.Identity(identityId));
            Assert.Equal(
                "You are not authorized to access this information.",
                ex.Message);
            Assert.False(this.identityRepo.GetUserCalled);
        }

        [Fact]
        public async Task AnonymousUserCannotGetIdentities()
        {
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => this.session.Identities());
            Assert.Equal(
                "You are not authorized to access this information.",
                ex.Message);
            Assert.False(this.identityRepo.GetUserCalled);
        }

        [Fact]
        public void AnonymousUserIsLoggedInAsAnonymousIdentity()
        {
            var identity = this.session.LoggedInIdentity();
            Assert.IsType<AnonymousIdentity>(identity);
            Assert.IsAssignableFrom<IIdentity>(identity);

            Assert.Null(identity.Id);
            Assert.Null(identity.FirstName);
            Assert.Null(identity.LastName);
            Assert.Null(identity.Email);
            Assert.Null(identity.CountryCode);
            Assert.Null(identity.OrganisationName);
        }

        // Identity(identityId).SuspendThenDelete(TimeSpan deleteDelay)
        // Identity(identityId).Suspend()
        // Identity(identityId).Reinstate()
        // Identity(identityId).SuspendAndResendValidationEmail()
        // Identity(identityId).ResendVerificationEmail()
    }
}
