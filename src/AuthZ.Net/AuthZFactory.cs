namespace AuthZ.Net
{
    using System.Security.Principal;
    using AuthZ.Net.Interfaces;

    public class AuthZFactory
    {
        private readonly IIdentityRepository repo;
        private readonly IAudit audit;

        public AuthZFactory(IIdentityRepository repo, IAudit audit)
        {
            this.repo = repo;
            this.audit = audit;
        }

        public ISession GetSession(IPrincipal principal)
        {
            return new AnonymousSession(this.repo, this.audit);
        }
    }
}
