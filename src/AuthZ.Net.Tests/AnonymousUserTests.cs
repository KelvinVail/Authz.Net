namespace AuthZ.Net.Tests
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net;
    using AuthZ.Net.Roles;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class AnonymousUserTests
    {
        private readonly AuthZClient authZClient;

        private readonly IdentityRepositorySpy identityRepo = new IdentityRepositorySpy();

        private readonly AnonymousRole loggedInRole = new AnonymousRole();

        public AnonymousUserTests()
        {
            this.authZClient = new AuthZClient(this.identityRepo, this.loggedInRole);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("ABC")]
        public async Task AnonymousUserCannotGetUsers(string userId)
        {
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => this.authZClient.Identity(userId));
            Assert.Equal(
                $"You are not allowed to access information about user {userId}",
                ex.Message);
            Assert.False(this.identityRepo.GetUserCalled);
        }
    }
}
