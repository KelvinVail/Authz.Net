namespace AuthZ.Net.Roles
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;

    public class SuspendedRole : IRole
    {
        public SuspendedRole(IIdentity identity)
        {
        }

        public async Task<IIdentity> GetIdentity(string identityId)
        {
            await Task.CompletedTask;
            throw new UnauthorizedAccessException("You are not authorized to access this information.");
        }
    }
}
