namespace AuthZ.Net.Tests.TestDoubles
{
    using System.Security.Claims;

    public class AuthenticatedClaimsIdentityStub : ClaimsIdentity
    {
        public string AuthenticationType { get; set; }

        public override bool IsAuthenticated { get; } = true;

        public string Name { get; set; }
    }
}
