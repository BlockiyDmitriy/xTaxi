using System;
using System.Threading.Tasks;
using xTaxi.Client.Models;

namespace xTaxi.Client.Services.LoaclDB
{
    public interface IDBService
    {
        #region Stepometer

        Task<CardDataModel> SetCreditCardDataAsync(CardDataModel stepometerModel);
        Task<CardDataModel> GetCreditCardDataAsync();
        Task<CardDataModel> UpdateCreditCardDataAsync(CardDataModel stepometerModel);

        #endregion

        #region Last Activity Date
        Task<DateTimeOffset> GetLastActivityDate();
        Task UpdateLastActivityDate(DateTimeOffset date);
        #endregion
    }
}
