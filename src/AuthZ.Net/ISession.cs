namespace AuthZ.Net
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public interface ISession
    {
        Task Register(IIdentity identity);

        Task<IIdentity> Identity(string identityId);

        Task<IEnumerable<IIdentity>> Identities();

        IIdentity LoggedInIdentity();
    }
}
