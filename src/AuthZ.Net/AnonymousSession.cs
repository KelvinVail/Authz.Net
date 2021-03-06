﻿namespace AuthZ.Net
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;
    using AuthZ.Net.Interfaces;

    public class AnonymousSession : ISession
    {
        private readonly IAudit audit;

        internal AnonymousSession(IAudit audit)
        {
            this.audit = audit;
        }

        public async Task Register(IIdentity identity)
        {
            await this.audit.RecordEvent("Register Identity Attempt", "Anon", identity.Id);
            throw new UnauthorizedAccessException(
                "You are not authorized to perform this action.");
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
