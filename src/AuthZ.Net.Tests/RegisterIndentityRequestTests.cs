namespace AuthZ.Net.Tests
{
    using System;
    using AuthZ.Net.Interfaces;
    using Xunit;

    public class RegisterIdentityTests
    {
        [Fact]
        public void CreateUserShouldThrowIfFirstNameIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("firstName", () => new RegisterIdentityRequest(null, "X", "X", "X", "X"));
            Assert.Throws<ArgumentNullException>("firstName", () => new RegisterIdentityRequest(string.Empty, "X", "X", "X", "X"));
        }

        [Fact]
        public void CreateUserShouldThrowIfLastNameIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("lastName", () => new RegisterIdentityRequest("X", null, "X@X", "X", "X"));
            Assert.Throws<ArgumentNullException>("lastName", () => new RegisterIdentityRequest("X", string.Empty, "X@X", "X", "X"));
        }

        [Fact]
        public void CreateUserShouldThrowIfEmailIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("email", () => new RegisterIdentityRequest("X", "X", null, "X", "X"));
            Assert.Throws<ArgumentNullException>("email", () => new RegisterIdentityRequest("X", "X", string.Empty, "X", "X"));
        }

        [Fact]
        public void CreateUserShouldThrowIfCountryIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("countryCode", () => new RegisterIdentityRequest("X", "X", "X@X", null, "X"));
            Assert.Throws<ArgumentNullException>("countryCode", () => new RegisterIdentityRequest("X", "X", "X@X", string.Empty, "X"));
        }

        [Fact]
        public void CreateUserShouldThrowIfOrgIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>("organisationName", () => new RegisterIdentityRequest("X", "X", "X@X", "X", null));
            Assert.Throws<ArgumentNullException>("organisationName", () => new RegisterIdentityRequest("X", "X", "X@X", "X", string.Empty));
        }

        [Theory]
        [InlineData("bob.smith.com")]
        [InlineData("katie.jones")]
        public void CreateUserShouldThrowIfEmailIsInvalid(string email)
        {
            var ex = Assert.Throws<InvalidOperationException>(() => new RegisterIdentityRequest("X", "X", email, "X", "X"));
            Assert.Equal($"{email} is not a valid email address.", ex.Message);
        }
    }
}