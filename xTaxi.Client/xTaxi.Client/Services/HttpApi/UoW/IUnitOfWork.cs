using System;
using xTaxi.Client.Models;
using xTaxi.Client.Services.HttpApi.Repository;

namespace xTaxi.Client.Services.HttpApi.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IRestApiClient<RegisterModel> LoginRestApiClient { get; }
    }
}
