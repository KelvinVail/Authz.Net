namespace AuthZ.Net.Tests.TestDoubles
{
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public class IdentityRepositorySpy : IIdentityRepository
    {
        public IIdentity LastIdentityRegistered { get; private set; }

        public bool RegisterCalled { get; private set; }

        public bool GetUserCalled { get; private set; }

        public string GetIdUsed { get; private set; }

        public async Task Register(IIdentity identity)
        {
            await Task.CompletedTask;
            this.RegisterCalled = true;
            this.LastIdentityRegistered = identity;
        }

        public async Task<IIdentity> GetIdentity(string id)
        {
            await Task.CompletedTask;
            this.GetUserCalled = true;
            this.GetIdUsed = id;
            return default;
        }
    }
}
