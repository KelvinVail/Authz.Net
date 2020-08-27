namespace AuthZ.Net.Tests
{
    using System.Threading.Tasks;
    using AuthZ.Net.Tests.Helpers;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class SessionFactoryTests
    {
        private readonly IdentityRepositorySpy repo = new IdentityRepositorySpy();

        private readonly AuditSpy audit = new AuditSpy();

        private readonly SessionFactory factory;

        public SessionFactoryTests()
        {
            this.factory = new SessionFactory(this.repo, this.audit);
        }

        [Fact]
        public async Task PrincipleIsNotAuthenticatedGetSessionReturnsAnonymousSession()
        {
            var cp = new ClaimsPrincipleBuilder().WithId("1").Build();
            var session = await this.factory.GetSession(cp);
            Assert.IsAssignableFrom<AnonymousSession>(session);
        }

        [Fact]
        public async Task ClaimsPrincipalDoesNotContainNameIdentifierReturnAnonymousSession()
        {
            var cp = new ClaimsPrincipleBuilder().IsAuthenticated().Build();
            var session = await this.factory.GetSession(cp);
            Assert.IsAssignableFrom<AnonymousSession>(session);
        }

        [Fact]
        public async Task ClaimsPrincipalNameIdentifierIsEmptyReturnAnonymousSession()
        {
            var cp = new ClaimsPrincipleBuilder()
                .WithId(string.Empty)
                .IsAuthenticated()
                .Build();

            var session = await this.factory.GetSession(cp);
            Assert.IsAssignableFrom<AnonymousSession>(session);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("123")]
        [InlineData("ABC")]
        public async Task IdFromClaimsPrincipleIsUsedToGetIdentity(string id)
        {
            var cp = new ClaimsPrincipleBuilder()
                .WithId(id)
                .IsAuthenticated()
                .Build();

            await this.factory.GetSession(cp);
            Assert.Equal(id, this.repo.GetIdUsed);
        }

        [Fact(Skip = "Not Implemented")]
        public void PrincipleIsOrgAdminGetSessionReturnOrgAdminSession()
        {
            var p = new ClaimsPrincipleBuilder()
                .IsAuthenticated()
                .Build();

            var session = this.factory.GetSession(p);
        }
    }
}
