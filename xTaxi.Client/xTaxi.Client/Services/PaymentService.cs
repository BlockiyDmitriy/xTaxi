using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xTaxi.Client.Models;
using xTaxi.Client.Services.LoaclDB;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Utils;

namespace xTaxi.Client.Services
{
    public class PaymentService : IPaymentService
    {
        private ILogService _logService;
        private IDBService _dbService;

        public PaymentService()
        {
            _logService = DependencyResolver.Get<ILogService>();
            _dbService = DependencyResolver.Get<IDBService>();
        }
        public async Task<CardDataModel> GetPaymentCard()
        {
            try
            {
                var result = await _dbService.GetCreditCardDataAsync();

                if (result == null)
                {
                    _logService.Log("Card is not exist in local db");
                    return new CardDataModel();
                }

                return result;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return new CardDataModel();
            }
        }

        public async Task<CardDataModel> SetNewPaymentCard(CardDataModel cardDataModel)
        {
            try
            {
                if (!IsCreditCardInfoValid(cardDataModel.CardNumber, cardDataModel.ExpireCardDate, cardDataModel.Cvv))
                {
                    _logService.Log("Card is not valid");
                    return null;
                }
                var result = await _dbService.SetCreditCardDataAsync(cardDataModel);

                if (result == null)
                {
                    _logService.Log("Card is not add in local db");
                    return new CardDataModel();
                }
                return result;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return new CardDataModel();
            }
        }

        public static bool IsCreditCardInfoValid(string cardNo, string expiryDate, string cvv)
        {
            // ^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$
            //var cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$");
            var cardCheck = new Regex(@"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cardCheck.IsMatch(cardNo)) // <1>check card number is valid
                return false;
            if (!cvvCheck.IsMatch(cvv)) // <2>check cvv is valid as "999"
                return false;

            var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy            
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
                return false; // ^ check date format is valid as "MM/yyyy"

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            //check expiry greater than today & within next 6 years <7, 8>>
            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }

        public async Task<PaymentMethod> SetPaymentMethod(PaymentMethod paymentMethod)
        {
            try
            {
                return await _dbService.SetPaymentMethod(paymentMethod);
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return new PaymentMethod();
            }
        }

        public async Task<PaymentMethod> GetPaymentMethod()
        {
            try
            {
                var method = await _dbService.GetPaymentMethod();
                if (method == null)
                {
                    method = new PaymentMethod() { PayMethod = EnumPaymentMethod.Cash };
                }
                return method;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return new PaymentMethod();
            }
        }
    }
}
