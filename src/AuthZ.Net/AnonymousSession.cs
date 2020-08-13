namespace AuthZ.Net
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public class AnonymousSession : ISession
    {
        private readonly IIdentityRepository identityRepo;

        private readonly IAudit audit;

        public AnonymousSession(IIdentityRepository identityRepo, IAudit audit)
        {
            this.identityRepo = identityRepo;
            this.audit = audit;
        }

        public async Task Register(RegisterIdentityRequest request)
        {
            await this.identityRepo.Register(request);
            await this.audit.RecordEvent("Register Identity", "Anon", request.Id);
        }

        public async Task<IIdentity> Identity(string identityId)
        {
            await Task.CompletedTask;
            throw new UnauthorizedAccessException(
                "You are not authorized to access this information.");
        }

        public async Task<IEnumerable<IIdentity>> Identities()
        {
            await Task.CompletedTask;
            throw new UnauthorizedAccessException(
                "You are not authorized to access this information.");
        }

        public IIdentity LoggedInIdentity()
        {
            return new AnonymousIdentity();
        }
    }
}
