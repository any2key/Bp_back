using Bp_back.Models.Responses;

namespace Bp_back.Services
{
    public interface IHttpService
    {
        public Task<T> GetAsync<T>(string url) where T : Response;
        public Task<Tres> PostAsync<Tres, Treq>(string url, Treq data) where Tres : Response;
    }
}
