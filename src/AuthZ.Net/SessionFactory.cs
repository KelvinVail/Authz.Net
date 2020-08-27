namespace AuthZ.Net
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;
    using AuthZ.Net.Interfaces;

    public class SessionFactory
    {
        private readonly IIdentityRepository repo;
        private readonly IAudit audit;

        public SessionFactory(IIdentityRepository repo, IAudit audit)
        {
            this.repo = repo;
            this.audit = audit;
        }

        public async Task<ISession> GetSession(ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.Identity.IsAuthenticated) return new AnonymousSession(this.audit);
            var identity = await this.GetIdentity(claimsPrincipal);
            return new AnonymousSession(this.audit);
        }

        private async Task<IIdentity> GetIdentity(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) return new AnonymousIdentity();
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(claim?.Value)) return new AnonymousIdentity();
            return await this.repo.GetIdentity(claim.Value);
        }
    }
}
