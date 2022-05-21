using Newtonsoft.Json;
using System.Collections.Generic;

namespace xTaxi.Client.Utils
{
    public class SerializeDeserialize<TModel> where TModel : class
    {
        public static string ConvertToJson(TModel self) =>
            JsonConvert.SerializeObject(self);

        public static List<TModel> ConvertFromJson(string json) =>
            JsonConvert.DeserializeObject<List<TModel>>(json);
    }
}
