namespace AuthZ.Net.Tests.TestDoubles
{
    using System.Security.Claims;

    public class AuthenticatedClaimsIdentityStub : ClaimsIdentity
    {
        public override string AuthenticationType { get; } = "None";

        public override bool IsAuthenticated { get; } = true;

        public override string Name { get; } = "Stub";
    }
}
