using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using PeoplesDb.Shared;

namespace PeoplesDb.Client.Services
{
    public class PeopleClient : IPeopleClient
    {
        private const string EndpointPath = "person";

        private readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly HttpClient httpClient;

        public PeopleClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> AddPerson(Person person)
        {
            HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync($"/{EndpointPath}/", person);
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePerson(int id)
        {
            HttpResponseMessage responseMessage = await httpClient.DeleteAsync($"/{EndpointPath}/{id}");
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Person>> GetPeople()
        {
            return await httpClient.GetFromJsonAsync<Person[]>($"/{EndpointPath}");
        }

        public async Task<Person> GetPerson(int id)
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync($"/{EndpointPath}/{id}");
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            return await responseMessage.Content.ReadFromJsonAsync<Person>();
        }

        public async Task<bool> UpdatePerson(Person person)
        {
            HttpResponseMessage responseMessage = await httpClient.PutAsJsonAsync($"/{EndpointPath}/{person.Id}", person);
            return responseMessage.IsSuccessStatusCode;
        }
    }
}
