namespace AuthZ.Net.Tests
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;
    using AuthZ.Net.Interfaces;
    using AuthZ.Net.Tests.TestDoubles;
    using Xunit;

    public class IdentityRepositoryTests
    {
        private readonly IIdentityRepository repo = new InMemoryIdentityRepository();

        [Fact]
        public async Task CanRegisterAnIdentity()
        {
            await this.repo.Register(new IdentityStub("1"));
        }

        [Fact]
        public async Task FirstIdentityRegisteredIsSetAsGlobalAdmin()
        {
            await this.repo.Register(new IdentityStub("1"));

            var i = await this.repo.GetIdentity("1");
            Assert.IsAssignableFrom<GlobalAdminIdentity>(i);
        }

        [Fact]
        public async Task CannotRegisterAnonymousIdentity()
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
            await this.repo.Register(new IdentityStub(id));

            var i = await this.repo.GetIdentity(id);

            Assert.Equal(id, i.Id);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("ABC")]
        [InlineData("ABC-123")]
        public async Task IdentitySubTypeIsRegisteredSameSubTypeIsReturned(string id)
        {
            await this.repo.Register(new IdentityStub("GlobalAdmin"));

            var orgAdmin = new IdentityStub(id);
            await this.repo.Register(orgAdmin);

            var i = await this.repo.GetIdentity(id);
            Assert.IsAssignableFrom<IdentityStub>(i);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("ABC")]
        [InlineData("ABC-123")]
        public async Task IdentityCanBeUpdated(string id)
        {
            await this.repo.Register(new IdentityStub("GlobalAdmin"));
            var one = new IdentityStub(id);
            await this.repo.Register(one);

            var two = new IdentityStubTwo(id);
            await this.repo.Register(two);

            var updated = await this.repo.GetIdentity(id);
            Assert.IsAssignableFrom<IdentityStubTwo>(updated);
        }
    }
}
