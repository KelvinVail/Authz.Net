﻿namespace AuthZ.Net.Interfaces
{
    using System.Threading.Tasks;

    public interface IIdentityRepository
    {
        Task Register(RegisterIdentityRequest request);
    }
}