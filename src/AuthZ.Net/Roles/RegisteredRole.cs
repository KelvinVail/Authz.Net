namespace AuthZ.Net.Roles
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;

    public class RegisteredRole : IRole
    {
        private readonly IIdentity identity;

        public RegisteredRole(IIdentity identity)
        {
            this.identity = identity;
        }

        public async Task<IIdentity> GetIdentity(string identityId)
        {
            if (identityId != this.identity.Id)
                throw new UnauthorizedAccessException("You are not authorized to access this information.");

            return await Task.FromResult(this.identity);
        }
    }
}
