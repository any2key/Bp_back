using Bp_back.Models.Responses;
using Bp_back.Utils;

namespace Bp_back.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient;
        public HttpService()
        {
            httpClient = new HttpClient();
        }
        public async Task<T> GetAsync<T>(string url) where T : Response
        {
            var res = await httpClient.GetAsync(url);
            var data = await res.Content.ReadAsStringAsync();
            return data.FromJson<T>();
        }

        public async Task<Tres> PostAsync<Tres, Treq>(string url, Treq data) where Tres : Response
        {
            StringContent content = new(data.ToJson());
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            using var response = await httpClient.SendAsync(request);
            string responseData = await response.Content.ReadAsStringAsync();

            return responseData.FromJson<Tres>();
        }
    }
}
