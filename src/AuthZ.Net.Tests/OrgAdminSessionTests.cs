namespace AuthZ.Net.Tests
{
    using Xunit;

    public class OrgAdminSessionTests
    {
        private OrgAdminSession session1 = new OrgAdminSession();

        [Fact]
        public void ImplementsISession()
        {
            Assert.True(typeof(ISession).IsAssignableFrom(typeof(OrgAdminSession)));
        }
    }
}
