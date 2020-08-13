namespace AuthZ.Net.Interfaces
{
    using System.Threading.Tasks;

    public interface IAudit
    {
        public Task RecordEvent(string eventName, string executingIdentityId, string targetIdentityId);
    }
}