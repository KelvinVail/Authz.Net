namespace AuthZ.Net
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuthZ.Net.Interfaces;

    public class InMemoryIdentityRepository : IIdentityRepository
    {
        private readonly Dictionary<string, IIdentity> repo = new Dictionary<string, IIdentity>();

        public async Task Register(IIdentity request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.Id)) throw new InvalidOperationException();

            this.repo.Add(request.Id, request);

            await Task.CompletedTask;
        }

        public async Task<IIdentity> GetIdentity(string id)
        {
            await Task.CompletedTask;

            return this.repo.ContainsKey(id) ? new GenericIdentity(id) : null;
        }
    }
}
