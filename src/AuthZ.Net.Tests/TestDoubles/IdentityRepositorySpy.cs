namespace AuthZ.Net.Tests.TestDoubles
{
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public class IdentityRepositorySpy : IIdentityRepository
    {
        public IIdentity LastIdentityRegistered { get; private set; }

        public bool GetUserCalled { get; private set; }

        public async Task Register(IIdentity request)
        {
            await Task.CompletedTask;
            this.LastIdentityRegistered = request;
        }
    }
}
