namespace AuthZ.Net
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public interface ISession
    {
        Task Register(RegisterIdentityRequest request);

        Task<IIdentity> Identity(string identityId);

        Task<IEnumerable<IIdentity>> Identities();

        IIdentity LoggedInIdentity();
    }
}
