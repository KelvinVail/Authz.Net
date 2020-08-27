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

        private readonly SessionFactory factory;

        public AnonymousSessionTests()
        {
            this.factory = new SessionFactory(this.identityRepo, this.audit);
            var claim = new Claim(ClaimTypes.NameIdentifier, "1");
            var claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(claim);

            this.claimsPrincipal.AddIdentity(claimIdentity);
        }

        [Fact]
        public async Task AnonymousUserCannotRegisterAnIdentity()
        {
            var session = await this.factory.GetSession(this.claimsPrincipal);
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                session.Register(new IdentityStub("1")));
            Assert.Equal(
                "You are not authorized to perform this action.",
                ex.Message);
            Assert.False(this.identityRepo.RegisterCalled);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("123")]
        [InlineData("ABC")]
        public async Task AnAuditTrailIsLeftWhenAnIdentityIsRegistered(string id)
        {
            var session = await this.factory.GetSession(this.claimsPrincipal);
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                session.Register(new IdentityStub(id)));

            Assert.Equal("Register Identity Attempt", this.audit.LastEvent);
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
            var session = await this.factory.GetSession(this.claimsPrincipal);
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => session.Identity(identityId));
            Assert.Equal(
                "You are not authorized to access this information.",
                ex.Message);
            Assert.False(this.identityRepo.GetUserCalled);
        }

        [Fact]
        public async Task AnonymousUserCannotGetIdentities()
        {
            var session = await this.factory.GetSession(this.claimsPrincipal);
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => session.Identities());
            Assert.Equal(
                "You are not authorized to access this information.",
                ex.Message);
            Assert.False(this.identityRepo.GetUserCalled);
        }

        [Fact]
        public async Task AnonymousUserIsLoggedInAsAnonymousIdentity()
        {
            var session = await this.factory.GetSession(this.claimsPrincipal);
            var identity = session.LoggedInIdentity();
            Assert.IsType<AnonymousIdentity>(identity);
            Assert.IsAssignableFrom<IIdentity>(identity);

            Assert.Equal("Anon", identity.Id);
        }

        // Identity(identityId).SuspendThenDelete(TimeSpan deleteDelay)
        // Identity(identityId).Suspend()
        // Identity(identityId).Reinstate()
        // Identity(identityId).SuspendAndResendValidationEmail()
        // Identity(identityId).ResendVerificationEmail()
    }
}
