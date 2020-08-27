namespace AuthZ.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AuthZ.Net.Identities;
    using AuthZ.Net.Interfaces;

    public class InMemoryIdentityRepository : IIdentityRepository
    {
        private readonly Dictionary<string, IIdentity> repo = new Dictionary<string, IIdentity>();

        public async Task Register(IIdentity identity)
        {
            Validate(identity);
            this.RemoveIfExists(identity);
            this.repo.Add(identity.Id, this.MakeGlobalAdminIfFirst(identity));

            await Task.CompletedTask;
        }

        public async Task<IIdentity> GetIdentity(string id)
        {
            await Task.CompletedTask;

            return this.repo.ContainsKey(id) ? this.repo[id] : null;
        }

        private static void Validate(IIdentity identity)
        {
            if (identity is null) throw new ArgumentNullException(nameof(identity));
            if (string.IsNullOrEmpty(identity.Id)) throw new InvalidOperationException();
            if (identity.GetType().Name == nameof(AnonymousIdentity)) throw new InvalidOperationException();
        }

        private void RemoveIfExists(IIdentity identity)
        {
            if (this.repo.ContainsKey(identity.Id))
                this.repo.Remove(identity.Id);
        }

        private IIdentity MakeGlobalAdminIfFirst(IIdentity identity)
        {
            return !this.repo.Any() ? new GlobalAdminIdentity(identity) : identity;
        }
    }
}
