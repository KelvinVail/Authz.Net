namespace AuthZ.Net
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public class OrgAdminSession : ISession
    {
        public async Task Register(RegisterIdentityRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IIdentity> Identity(string identityId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<IIdentity>> Identities()
        {
            throw new System.NotImplementedException();
        }

        public IIdentity LoggedInIdentity()
        {
            throw new System.NotImplementedException();
        }
    }
}
