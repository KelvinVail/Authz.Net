namespace AuthZ.Net.Tests
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AuthZ.Net.Tests.Helpers;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class OrgAdminSessionTests
    {
        private readonly IdentityRepositorySpy identityRepo = new IdentityRepositorySpy();

        private readonly AuditSpy audit = new AuditSpy();

        private readonly ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();

        private readonly OrgAdminSession session;

        public OrgAdminSessionTests()
        {
            this.session = new OrgAdminSession();
            var claim = new Claim(ClaimTypes.NameIdentifier, "1");
            var claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(claim);

            this.claimsPrincipal.AddIdentity(claimIdentity);
        }

        [Fact]
        public void ImplementsISession()
        {
            Assert.True(typeof(ISession).IsAssignableFrom(typeof(OrgAdminSession)));
        }

        [Fact(Skip = "Not Implemented")]
        public async Task AnonymousUserCanRegisterAnIdentity()
        {
            var request = new RegisterIdentityRequest(this.claimsPrincipal);
            await this.session.Register(request);

            Assert.Equal("1", this.identityRepo.LastIdentityRegistered.Id);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData("1")]
        [InlineData("123")]
        [InlineData("ABC")]
        public async Task AnAuditTrailIsLeftWhenAnIdentityIsRegistered(string id)
        {
            var cp = new ClaimsPrincipleBuilder()
                .WithId(id)
                .Build();

            var req = new RegisterIdentityRequest(cp);
            await this.session.Register(req);

            Assert.Equal("Register Identity", this.audit.LastEvent);
            Assert.Equal("Anon", this.audit.LastExecutor);
            Assert.Equal(id, this.audit.LastTarget);
        }
    }
}
