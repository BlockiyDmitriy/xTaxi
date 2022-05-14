using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace xTaxi.Client.Services.Logger
{
    public interface ILogService
    {
        void Log(string message);
        void TrackException(Exception e);
        void TrackException(Exception e, string methodName);
        void TrackEvent(string message);
        void TrackResponse(HttpResponseMessage response);
        Task TrackResponseAsync(HttpResponseMessage response);
    }
}
