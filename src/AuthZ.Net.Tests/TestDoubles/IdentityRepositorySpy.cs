namespace AuthZ.Net.Tests.TestDoubles
{
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public class IdentityRepositorySpy : IIdentityRepository
    {
        public IIdentity LastIdentityRegistered { get; private set; }

        public bool GetUserCalled { get; private set; }

        public async Task Register(IIdentity identity)
        {
            await Task.CompletedTask;
            this.LastIdentityRegistered = identity;
        }

        public async Task<IIdentity> GetIdentity(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
