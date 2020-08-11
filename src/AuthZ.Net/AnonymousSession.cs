namespace AuthZ.Net
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public class AnonymousSession : ISession
    {
        private readonly IIdentityRepository identityRepo;

        // TODO make internal so that session cannot be manipulated
        public AnonymousSession(IIdentityRepository identityRepo)
        {
            this.identityRepo = identityRepo;
        }

        public async Task Register(RegisterIdentityRequest request)
        {
            await this.identityRepo.Register(request);
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
    }
}
