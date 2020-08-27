namespace AuthZ.Net
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public class InMemoryIdentityRepository : IIdentityRepository
    {
        private readonly Dictionary<string, IIdentity> repo = new Dictionary<string, IIdentity>();

        public async Task Register(IIdentity identity)
        {
            if (identity is null) throw new ArgumentNullException(nameof(identity));
            if (string.IsNullOrEmpty(identity.Id)) throw new InvalidOperationException();

            if (this.repo.ContainsKey(identity.Id))
                this.repo.Remove(identity.Id);

            this.repo.Add(identity.Id, identity);

            await Task.CompletedTask;
        }

        public async Task<IIdentity> GetIdentity(string id)
        {
            await Task.CompletedTask;

            return this.repo.ContainsKey(id) ? this.repo[id] : null;
        }
    }
}
