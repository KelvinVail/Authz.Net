namespace AuthZ.Net.Roles
{
    using System;
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;

    public class AnonymousRole : IRole
    {
        public async Task<IIdentity> GetIdentity(string identityId)
        {
            await Task.CompletedTask;
            throw new UnauthorizedAccessException(
                $"You are not allowed to access information about user {identityId}");
        }
    }
}
