using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using xTaxi.Client.Models;
using xTaxi.Client.Services.Logger;
using xTaxi.Client.Utils;

namespace xTaxi.Client.Services.HttpApi.Repository
{
    public class RestApiClient<TData> : IRestApiClient<TData> where TData : class
    {
        private StringBuilder _baseUrlStringBuilder = new StringBuilder(Constants.Constants.BaseUrl);

        private ILogService _logService { get; }
        internal HttpClient HttpClient { get; private set; }

        public RestApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
            _logService = DependencyResolver.Get<ILogService>();
        }

        public async Task<bool> Register(string controllerUrl, RegisterModel registerModel)
        {
            try
            {
                _baseUrlStringBuilder = new StringBuilder(Constants.Constants.BaseUrl);
                var response = await HttpClient.PostAsync(_baseUrlStringBuilder.Append(controllerUrl).ToString(),
                    new StringContent(SerializeDeserialize<RegisterModel>.ConvertToJson(registerModel), Encoding.UTF8, "application/json"));

                await _logService.TrackResponseAsync(response);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                return true;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return false;
            }
        }

        public async Task<bool> Login(string controllerUrl, LoginModel loginModel)
        {
            try
            {
                _baseUrlStringBuilder = new StringBuilder(Constants.Constants.BaseUrl);

                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", loginModel.Email),
                    new KeyValuePair<string, string>("password", loginModel.Password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                };

                var request = new HttpRequestMessage(HttpMethod.Post, _baseUrlStringBuilder.Append(controllerUrl).ToString());

                request.Content = new FormUrlEncodedContent(keyValues);

                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                HttpClient client = new HttpClient(handler);
                var response = await client.SendAsync(request);

                await _logService.TrackResponseAsync(response);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                return true;
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return false;
            }
        }

        //TODO: Get token doesn't work
        public async Task<string> GetToken(string controllerUrl, LoginModel loginModel)
        {
            try
            {
                _baseUrlStringBuilder = new StringBuilder(Constants.Constants.BaseUrl);

                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", loginModel.Email),
                    new KeyValuePair<string, string>("password", loginModel.Password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                };

                var request = new HttpRequestMessage(HttpMethod.Post, _baseUrlStringBuilder.Append(controllerUrl).ToString());

                request.Content = new FormUrlEncodedContent(keyValues);

                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                HttpClient client = new HttpClient(handler);
                var response = await client.SendAsync(request);

                await _logService.TrackResponseAsync(response);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                return SerializeDeserialize<string>.ConvertFromJson(await response.Content.ReadAsStringAsync()).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logService.TrackException(e, MethodBase.GetCurrentMethod()?.Name);
                return string.Empty;
            }
        }
    }
}
