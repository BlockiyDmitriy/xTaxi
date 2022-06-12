using LiteDB;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using xTaxi.Client.Models;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Utils;

namespace xTaxi.Client.Services.LoaclDB
{
    public class DBService : IDBService
    {
        private readonly LiteDatabase _liteDatabase;
        private readonly ILiteCollection<CardDataModel> _cardDataModel;
        private readonly ILiteCollection<PaymentMethod> _cardPaymentMethod;
        private readonly ILiteCollection<ActivityDate> _activityDateCollection;

        private ILogService _logService { get; set; }

        public DBService()
        {
            _liteDatabase = new LiteDatabase(DBHelper.DBPath);
            _cardDataModel = _liteDatabase.GetCollection<CardDataModel>(DBHelper.CreditCardCollection);
            _cardPaymentMethod = _liteDatabase.GetCollection<PaymentMethod>(DBHelper.CreditCardPaymentMethodCollection);
            _activityDateCollection = _liteDatabase.GetCollection<ActivityDate>(DBHelper.ActivityDateId);

            _logService = DependencyResolver.Get<ILogService>();
        }

        public Task<CardDataModel> SetCreditCardDataAsync(CardDataModel stepometerModel)
        {
            try
            {
                _cardDataModel.Insert(stepometerModel);
                return Task.FromResult(stepometerModel);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Task.FromResult(stepometerModel ?? new CardDataModel());
            }
        }

        public Task<CardDataModel> GetCreditCardDataAsync()
        {
            try
            {
                var data = _cardDataModel.FindAll().LastOrDefault();
                return Task.FromResult(data);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Task.FromResult(new CardDataModel());
            }
        }

        public Task<CardDataModel> UpdateCreditCardDataAsync(CardDataModel stepometerModel)
        {
            try
            {
                var result = _cardDataModel.Update(stepometerModel);
                if (!result)
                {
                    _logService.Log("Local db. Document not found");
                    _cardDataModel.Insert(stepometerModel);
                }
                return Task.FromResult(stepometerModel);
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return Task.FromResult(stepometerModel ?? new CardDataModel());
            }
        }

        public Task UpdateLastActivityDate(DateTimeOffset date)
        {
            try
            {
                var activityDate = _activityDateCollection.FindOne(a => a.Id == DBHelper.ActivityDateId);

                if (activityDate == null)
                {
                    activityDate = new ActivityDate
                    {
                        Id = DBHelper.ActivityDateId,
                        Date = date
                    };
                    _activityDateCollection.Insert(activityDate);

                    return Task.CompletedTask;
                }
                else
                {
                    activityDate.Date = date;
                    _activityDateCollection.Update(activityDate);

                    return Task.CompletedTask;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Task.FromResult(new DateTimeOffset());
            }
        }

        public Task<DateTimeOffset> GetLastActivityDate()
        {
            var activityDate = _activityDateCollection.FindOne(a => a.Id == DBHelper.ActivityDateId);
            return Task.FromResult(activityDate == null ? default(DateTimeOffset) : activityDate.Date);
        }

        public Task<PaymentMethod> SetPaymentMethod(PaymentMethod paymentMethod)
        {
            try
            {
                _cardPaymentMethod.Insert(paymentMethod);
                return Task.FromResult(paymentMethod);
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return Task.FromResult(new PaymentMethod());
            }
        }

        public Task<PaymentMethod> GetPaymentMethod()
        {
            try
            {
                var paymentMethod = _cardPaymentMethod.FindAll().ToList().LastOrDefault();

                return Task.FromResult(paymentMethod);
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return Task.FromResult(new PaymentMethod());
            }
        }
    }
    public class ActivityDate
    {
        public string Id { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
