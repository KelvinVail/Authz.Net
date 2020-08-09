namespace AuthZ.Net.Tests.TestDoubles
{
    using System.Threading.Tasks;
    using global::AuthZ.Net.Interfaces;

    public class IdentityRepositorySpy : IIdentityRepository
    {
        public RegisterIdentityRequest LastIdentityRegistered { get; private set; }

        public bool GetUserCalled { get; private set; }

        public async Task Register(RegisterIdentityRequest request)
        {
            await Task.CompletedTask;
            this.LastIdentityRegistered = request;
        }
    }
}
