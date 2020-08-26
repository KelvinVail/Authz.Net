namespace AuthZ.Net
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using AuthZ.Net.Interfaces;

    public class RegisterIdentityRequest : IIdentity
    {
        public RegisterIdentityRequest(ClaimsPrincipal claimsPrincipal)
        {
            this.Id = ValidateId(claimsPrincipal);
        }

        public string Id { get; }

        private static string ValidateId(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) throw new ArgumentNullException(nameof(claimsPrincipal));
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(claim?.Value))
                throw new InvalidOperationException();
            return claim.Value;
        }
    }
}
