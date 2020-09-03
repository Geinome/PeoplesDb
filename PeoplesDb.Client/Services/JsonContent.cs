using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace PeoplesDb.Client.Services
{
    public class JsonContent : StringContent
    {
        protected JsonContent(string json) 
            : base(json, Encoding.UTF8, "application/json")
        {
        }

        public static JsonContent From<T>(T instance)
        {
            string json = JsonSerializer.Serialize<T>(instance);
            return new JsonContent(json);
        }
    }
}
