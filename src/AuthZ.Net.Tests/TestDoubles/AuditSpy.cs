namespace AuthZ.Net.Tests.TestDoubles
{
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public class AuditSpy : IAudit
    {
        public string LastEvent { get; private set; }

        public string LastExecutor { get; private set; }

        public string LastTarget { get; private set; }

        public async Task RecordEvent(string eventName, string executingIdentityId, string targetIdentityId)
        {
            await Task.CompletedTask;
            this.LastEvent = eventName;
            this.LastExecutor = executingIdentityId;
            this.LastTarget = targetIdentityId;
        }
    }
}
