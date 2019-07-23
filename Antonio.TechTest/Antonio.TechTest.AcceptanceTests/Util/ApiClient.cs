using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Antonio.TechTest.AcceptanceTests.Util
{
    public class ApiClient : IDisposable
    {
        private HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public ApiClient(string url)
        {
            _httpClient = new HttpClient();
            _apiEndpoint = url;
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            return await _httpClient.GetAsync($"{_apiEndpoint}/{url}");
        }

        public async Task<HttpResponseMessage> PostAsync(string url, StringContent jsonContent)
        {
            return await _httpClient.PostAsync($"{_apiEndpoint}/{url}", jsonContent);
        }
        //public async Task<HttpResponseMessage> GetStartShipListByPage(int pageNumber)
        //{
        //    var pagedUrl = string.Concat(_url, "?page=", pageNumber);
        //    return await _httpClient.GetAsync(pagedUrl);
        //}

        public void Dispose()
        {
            this._httpClient.Dispose();
        }
    }
}
