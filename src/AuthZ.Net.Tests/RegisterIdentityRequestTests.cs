namespace AuthZ.Net.Tests
{
    using System;
    using System.Security.Claims;
    using AuthZ.Net.Interfaces;
    using Xunit;

    public class RegisterIdentityRequestTests
    {
        private readonly ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();

        public RegisterIdentityRequestTests()
        {
            var claim = new Claim(ClaimTypes.NameIdentifier, "1");
            var claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaim(claim);

            this.claimsPrincipal.AddIdentity(claimIdentity);
        }

        [Fact]
        public void ImplementsIIdentity()
        {
            Assert.True(typeof(IIdentity).IsAssignableFrom(typeof(RegisterIdentityRequest)));
        }

        [Fact]
        public void RegisterIdentityShouldThrowIfClaimsPrincipalDoesNotContainNameIdentifier()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new RegisterIdentityRequest(new ClaimsPrincipal()));
        }

        [Fact]
        public void RegisterIdentityShouldThrowIfClaimsPrincipalNameIdentifierIsEmpty()
        {
            var c = new Claim(ClaimTypes.NameIdentifier, string.Empty);
            var i = new ClaimsIdentity();
            i.AddClaim(c);
            var cp = new ClaimsPrincipal();
            cp.AddIdentity(i);

            Assert.Throws<InvalidOperationException>(() =>
                new RegisterIdentityRequest(cp));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("123")]
        [InlineData("ABC")]
        public void RegisterIdentityShouldGetIdFromNameIdentifier(string id)
        {
            var c = new Claim(ClaimTypes.NameIdentifier, id);
            var i = new ClaimsIdentity();
            i.AddClaim(c);
            var cp = new ClaimsPrincipal();
            cp.AddIdentity(i);

            var req = new RegisterIdentityRequest(cp);

            Assert.Equal(id, req.Id);
        }

        // PrincipleIsNotAuthenticatedThrow
    }
}