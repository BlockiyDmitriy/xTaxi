using System;
using System.Threading.Tasks;
using xTaxi.Client.Models;

namespace xTaxi.Client.Services.LoaclDB
{
    public interface IDBService
    {
        #region Card

        Task<CardDataModel> SetCreditCardDataAsync(CardDataModel stepometerModel);
        Task<CardDataModel> GetCreditCardDataAsync();
        Task<CardDataModel> UpdateCreditCardDataAsync(CardDataModel stepometerModel);

        Task<PaymentMethod> SetPaymentMethod(PaymentMethod paymentMethod);
        Task<PaymentMethod> GetPaymentMethod();

        #endregion

        #region Last Activity Date
        Task<DateTimeOffset> GetLastActivityDate();
        Task UpdateLastActivityDate(DateTimeOffset date);
        #endregion
    }
}
