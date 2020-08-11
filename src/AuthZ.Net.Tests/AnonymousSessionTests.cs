namespace AuthZ.Net.Tests
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class AnonymousSessionTests
    {
        private readonly IdentityRepositorySpy identityRepo = new IdentityRepositorySpy();

        private readonly AnonymousSession session;

        public AnonymousSessionTests()
        {
            this.session = new AnonymousSession(this.identityRepo);
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
            var request = new RegisterIdentityRequest(firstName, lastName, email, countryCode, orgName);
            await this.session.Register(request);

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

        // LoggedInIdentity
        // Identity(identityId).SuspendThenDelete(TimeSpan deleteDelay)
        // Identity(identityId).Suspend()
        // Identity(identityId).Reinstate()
        // Identity(identityId).SuspendAndResendValidationEmail()
        // Identity(identityId).ResendVerificationEmail()
    }
}
