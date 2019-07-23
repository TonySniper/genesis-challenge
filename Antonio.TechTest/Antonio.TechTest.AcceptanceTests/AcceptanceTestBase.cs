using Antonio.TechTest.AcceptanceTests.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Antonio.TechTest.AcceptanceTests
{
    public abstract class AcceptanceTestBase
    {
        protected readonly ApiClient _apiClient;
        private readonly string _url;
        private const string _contentType = "application/json";

        protected AcceptanceTestBase()
        {
            _url = "http://localhost:5656/api/orders/";
            _apiClient = new ApiClient(_url);
        }


        protected StringContent CreateJsonContentFor<T>(T obj) where T : class
        {
            var jsonString = JsonConvert.SerializeObject(obj);

            return new StringContent(jsonString, Encoding.UTF8, _contentType);
        }
    }
}
