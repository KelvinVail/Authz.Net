namespace AuthZ.Net.Tests
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net;
    using AuthZ.Net.Identities;
    using AuthZ.Net.Roles;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class SuspendedRoleTests
    {
        private readonly AuthZClient authZClient;

        private readonly IdentityRepositorySpy identityRepo = new IdentityRepositorySpy();

        private readonly SuspendedRole loggedInRole
            = new SuspendedRole(new ExistingIdentity("1"));

        public SuspendedRoleTests()
        {
            this.authZClient = new AuthZClient(this.identityRepo, this.loggedInRole);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task SuspendedUserCanGetThemselves(string id)
        {
            var registeredUser = new RegisteredRole(new ExistingIdentity(id));
            var sut = new AuthZClient(this.identityRepo, registeredUser);

            var identity = await sut.Identity(id);

            Assert.NotNull(identity);
            Assert.Equal(id, identity.Id);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        public async Task SuspendedUserCannotGetAnyOtherUsers(string userId)
        {
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => this.authZClient.Identity(userId));
            Assert.Equal(
                "You are not authorized to access this information.",
                ex.Message);
            Assert.False(this.identityRepo.GetUserCalled);
        }
    }
}
