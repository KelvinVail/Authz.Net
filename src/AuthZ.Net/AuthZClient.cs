namespace AuthZ.Net
{
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;
    using AuthZ.Net.Interfaces;
    using AuthZ.Net.Roles;

    public class AuthZClient
    {
        private readonly IIdentityRepository identityRepo;
        private readonly IRole role;

        // TODO make internal so that role can not be manipulated
        public AuthZClient(IIdentityRepository identityRepo, IRole role)
        {
            this.identityRepo = identityRepo;
            this.role = role;
        }

        public async Task Register(RegisterIdentityRequest request)
        {
            await this.identityRepo.Register(request);
        }

        public async Task<IIdentity> Identity(string userId)
        {
            return await this.role.GetIdentity(userId);
        }
    }
}
