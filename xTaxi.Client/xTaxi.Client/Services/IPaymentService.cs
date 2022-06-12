using System.Threading.Tasks;
using xTaxi.Client.Models;

namespace xTaxi.Client.Services
{
    public interface IPaymentService
    {
        Task<CardDataModel> SetNewPaymentCard(CardDataModel cardDataModel);
        Task<CardDataModel> GetPaymentCard();

        Task<PaymentMethod> SetPaymentMethod(PaymentMethod paymentMethod);
        Task<PaymentMethod> GetPaymentMethod();
    }
}
