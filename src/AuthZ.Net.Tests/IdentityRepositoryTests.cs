namespace AuthZ.Net.Tests
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;
    using AuthZ.Net.Interfaces;
    using AuthZ.Net.Tests.Helpers;
    using Xunit;

    public class IdentityRepositoryTests
    {
        private readonly IIdentityRepository repo = new InMemoryIdentityRepository();

        [Fact]
        public async Task CanRegisterAnIdentity()
        {
            await this.repo.Register(Identity("1"));
        }

        [Fact]
        public async Task IdentityIsAnonymousThrow()
        {
            var anon = new AnonymousIdentity();
            await Assert.ThrowsAsync<InvalidOperationException>(() => this.repo.Register(anon));
        }

        [Fact]
        public async Task IdentityIsNullThrow()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => this.repo.Register(null));
            Assert.Equal("identity", ex.ParamName);
        }

        [Fact]
        public async Task IdentityDoesNotExistReturnNull()
        {
            var i = await this.repo.GetIdentity("1");
            Assert.Null(i);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("ABC")]
        [InlineData("ABC-123")]
        public async Task IdentityCanBeRetrieved(string id)
        {
            await this.repo.Register(Identity(id));

            var i = await this.repo.GetIdentity(id);

            Assert.Equal(id, i.Id);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("ABC")]
        [InlineData("ABC-123")]
        public async Task IdentitySubTypeIsRegisteredSubTypeIsReturned(string id)
        {
            var orgAdmin = new OrgAdminIdentity(id);
            await this.repo.Register(orgAdmin);

            var i = await this.repo.GetIdentity(id);
            Assert.IsAssignableFrom<OrgAdminIdentity>(i);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("ABC")]
        [InlineData("ABC-123")]
        public async Task IdentityCanBeUpdated(string id)
        {
            var gen = new GenericIdentity(id);
            await this.repo.Register(gen);

            var i = await this.repo.GetIdentity(id);
            Assert.IsAssignableFrom<GenericIdentity>(i);

            var orgAdmin = new OrgAdminIdentity(id);
            await this.repo.Register(orgAdmin);

            var updated = await this.repo.GetIdentity(id);
            Assert.IsAssignableFrom<OrgAdminIdentity>(updated);
        }

        private static RegisterIdentityRequest Identity(string id)
        {
            var p = new ClaimsPrincipleBuilder()
                .IsAuthenticated()
                .WithId(id)
                .Build();

            return new RegisterIdentityRequest(p);
        }
    }
}
