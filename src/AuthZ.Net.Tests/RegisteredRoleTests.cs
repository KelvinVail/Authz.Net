namespace AuthZ.Net.Tests
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net;
    using AuthZ.Net.Identities;
    using AuthZ.Net.Roles;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class RegisteredRoleTests
    {
        private readonly AuthZClient authZClient;

        private readonly IdentityRepositorySpy identityReport = new IdentityRepositorySpy();

        private readonly RegisteredRole loggedInRole
            = new RegisteredRole(new ExistingIdentity("1"));

        public RegisteredRoleTests()
        {
            this.authZClient = new AuthZClient(this.identityReport, this.loggedInRole);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task RegisteredUserCanGetThemselves(string id)
        {
            var registeredUser = new RegisteredRole(new ExistingIdentity(id));
            var sut = new AuthZClient(this.identityReport, registeredUser);

            var identity = await sut.Identity(id);

            Assert.NotNull(identity);
            Assert.Equal(id, identity.Id);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        public async Task RegisteredUserCannotGetAnyOtherUsers(string userId)
        {
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => this.authZClient.Identity(userId));
            Assert.Equal(
                $"You are not authorized to access this information.",
                ex.Message);
            Assert.False(this.identityReport.GetUserCalled);
        }
    }
}
