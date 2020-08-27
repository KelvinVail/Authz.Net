namespace AuthZ.Net.Tests
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;
    using AuthZ.Net.Interfaces;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class AnonymousSessionTests
    {
        private readonly IdentityRepositorySpy identityRepo = new IdentityRepositorySpy();

        private readonly AuditSpy audit = new AuditSpy();

        private readonly ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();

        private readonly AnonymousSession session;

        public AnonymousSessionTests()
        {
            this.session = new AnonymousSession(this.identityRepo, this.audit);
            var claim = new Claim(ClaimTypes.NameIdentifier, "1");
            var claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(claim);

            this.claimsPrincipal.AddIdentity(claimIdentity);
        }

        [Fact]
        public async Task AnonymousUserCanRegisterAnIdentity()
        {
            var request = new RegisterIdentityRequest(this.claimsPrincipal);
            await this.session.Register(request);

            Assert.Equal("1", this.identityRepo.LastIdentityRegistered.Id);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("123")]
        [InlineData("ABC")]
        public async Task AnAuditTrailIsLeftWhenAnIdentityIsRegistered(string id)
        {
            var c = new Claim(ClaimTypes.NameIdentifier, id);
            var ci = new ClaimsIdentity();
            ci.AddClaim(c);
            var cp = new ClaimsPrincipal();
            cp.AddIdentity(ci);

            var req = new RegisterIdentityRequest(cp);
            await this.session.Register(req);

            Assert.Equal("Register Identity", this.audit.LastEvent);
            Assert.Equal("Anon", this.audit.LastExecutor);
            Assert.Equal(id, this.audit.LastTarget);
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
        }

        // Identity(identityId).SuspendThenDelete(TimeSpan deleteDelay)
        // Identity(identityId).Suspend()
        // Identity(identityId).Reinstate()
        // Identity(identityId).SuspendAndResendValidationEmail()
        // Identity(identityId).ResendVerificationEmail()
    }
}
