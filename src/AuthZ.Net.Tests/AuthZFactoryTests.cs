namespace AuthZ.Net.Tests
{
    using AuthZ.Net.Tests.Helpers;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class AuthZFactoryTests
    {
        private readonly IdentityRepositorySpy repo = new IdentityRepositorySpy();

        private readonly AuditSpy audit = new AuditSpy();

        private readonly AuthZFactory factory;

        public AuthZFactoryTests()
        {
            this.factory = new AuthZFactory(this.repo, this.audit);
        }

        [Fact]
        public void PrincipleIsNotAuthenticatedGetSessionReturnAnonymousSession()
        {
            var p = new ClaimsPrincipleBuilder().Build();
            var session = this.factory.GetSession(p);
            Assert.IsAssignableFrom<AnonymousSession>(session);
        }

        [Fact(Skip = "Not Implemented")]
        public void PrincipleIsOrgAdminGetSessionReturnOrgAdminSession()
        {
            var p = new ClaimsPrincipleBuilder()
                .IsAuthenticated()
                .Build();

            var session = this.factory.GetSession(p);
            Assert.IsAssignableFrom<OrgAdminSession>(session);
        }
    }
}
