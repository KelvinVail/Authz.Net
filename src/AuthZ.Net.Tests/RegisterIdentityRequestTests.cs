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
        public void RegisterIdentityShouldThrowIfFirstNameIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("firstName", () =>
                new RegisterIdentityRequest(null, "X", "X", "X", "X", this.claimsPrincipal));
            Assert.Throws<ArgumentNullException>("firstName", () =>
                new RegisterIdentityRequest(string.Empty, "X", "X", "X", "X", this.claimsPrincipal));
        }

        [Theory]
        [InlineData("Bob")]
        [InlineData("Katie")]
        public void RegisterIdentitySetsFirstName(string firstName)
        {
            var req = new RegisterIdentityRequest(firstName, "X", "X@X", "X", "X", this.claimsPrincipal);
            Assert.Equal(firstName, req.FirstName);
        }

        [Fact]
        public void RegisterIdentityShouldThrowIfLastNameIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("lastName", () =>
                new RegisterIdentityRequest("X", null, "X@X", "X", "X", this.claimsPrincipal));
            Assert.Throws<ArgumentNullException>("lastName", () =>
                new RegisterIdentityRequest("X", string.Empty, "X@X", "X", "X", this.claimsPrincipal));
        }

        [Theory]
        [InlineData("Smith")]
        [InlineData("Jones")]
        public void RegisterIdentitySetsLastName(string lastName)
        {
            var req = new RegisterIdentityRequest("X", lastName, "X@X", "X", "X", this.claimsPrincipal);
            Assert.Equal(lastName, req.LastName);
        }

        [Fact]
        public void RegisterIdentityShouldThrowIfEmailIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("email", () =>
                new RegisterIdentityRequest("X", "X", null, "X", "X", this.claimsPrincipal));
            Assert.Throws<ArgumentNullException>("email", () =>
                new RegisterIdentityRequest("X", "X", string.Empty, "X", "X", this.claimsPrincipal));
        }

        [Theory]
        [InlineData("bob.smith.com")]
        [InlineData("katie.jones")]
        public void CreateUserShouldThrowIfEmailIsInvalid(string email)
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
                new RegisterIdentityRequest("X", "X", email, "X", "X", this.claimsPrincipal));
            Assert.Equal($"{email} is not a valid email address.", ex.Message);
        }

        [Theory]
        [InlineData("bob@smith.com")]
        [InlineData("katie@jones.com")]
        public void RegisterIdentitySetsEmail(string email)
        {
            var req = new RegisterIdentityRequest("X", "X", email, "X", "X", this.claimsPrincipal);
            Assert.Equal(email, req.Email);
        }

        [Fact]
        public void RegisterIdentityShouldThrowIfCountryIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("countryCode", () =>
                new RegisterIdentityRequest("X", "X", "X@X", null, "X", this.claimsPrincipal));
            Assert.Throws<ArgumentNullException>("countryCode", () =>
                new RegisterIdentityRequest("X", "X", "X@X", string.Empty, "X", this.claimsPrincipal));
        }

        [Theory]
        [InlineData("UK")]
        [InlineData("US")]
        public void RegisterIdentitySetsCountryCode(string countryCode)
        {
            var req = new RegisterIdentityRequest("X", "X", "X@X", countryCode, "X", this.claimsPrincipal);
            Assert.Equal(countryCode, req.CountryCode);
        }

        [Fact]
        public void RegisterIdentityShouldThrowIfOrgIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("organisationName", () =>
                new RegisterIdentityRequest("X", "X", "X@X", "X", null, this.claimsPrincipal));
            Assert.Throws<ArgumentNullException>("organisationName", () =>
                new RegisterIdentityRequest("X", "X", "X@X", "X", string.Empty, this.claimsPrincipal));
        }

        [Theory]
        [InlineData("Bob Smith Ltd")]
        [InlineData("Jones Corp")]
        public void RegisterIdentitySetsOrgName(string orgName)
        {
            var req = new RegisterIdentityRequest("X", "X", "X@X", "X", orgName, this.claimsPrincipal);
            Assert.Equal(orgName, req.OrganisationName);
        }

        [Fact]
        public void RegisterIdentityShouldThrowIfClaimsPrincipalIsNull()
        {
            Assert.Throws<ArgumentNullException>("claimsPrincipal", () =>
                new RegisterIdentityRequest("X", "X", "X@X", "X", "X", null));
        }

        [Fact]
        public void RegisterIdentityShouldThrowIfClaimsPrincipalDoesNotContainNameIdentifier()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new RegisterIdentityRequest("X", "X", "X@X", "X", "X", new ClaimsPrincipal()));
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
                new RegisterIdentityRequest("X", "X", "X@X", "X", "X", cp));
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

            var req = new RegisterIdentityRequest("X", "X", "X@X", "X", "X", cp);

            Assert.Equal(id, req.Id);
        }
    }
}