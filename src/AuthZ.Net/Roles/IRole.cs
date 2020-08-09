namespace AuthZ.Net.Roles
{
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;

    public interface IRole
    {
        Task<IIdentity> GetIdentity(string identityId);
    }
}
