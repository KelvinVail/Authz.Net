﻿namespace AuthZ.Net.Interfaces
{
    using System.Threading.Tasks;

    public interface IIdentityRepository
    {
        Task Register(IIdentity identity);

        Task<IIdentity> GetIdentity(string id);
    }
}