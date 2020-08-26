namespace AuthZ.Net.Tests.Helpers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using AuthZ.Net.Tests.TestDoubles;

    public class ClaimsPrincipleBuilder
    {
        private readonly List<Claim> claims = new List<Claim>();

        private bool isAuthenticated;

        public ClaimsPrincipleBuilder WithId(string id)
        {
            var c = new Claim(ClaimTypes.NameIdentifier, id);
            this.claims.Add(c);
            return this;
        }

        public ClaimsPrincipleBuilder IsAuthenticated()
        {
            this.isAuthenticated = true;
            return this;
        }

        public ClaimsPrincipal Build()
        {
            var cp = new ClaimsPrincipal();
            var i = this.isAuthenticated ? new AuthenticatedClaimsIdentityStub() : new ClaimsIdentity();
            i.AddClaims(this.claims);
            cp.AddIdentity(i);

            return cp;
        }
    }
}
