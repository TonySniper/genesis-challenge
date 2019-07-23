using Antonio.TechTest.Core.DTO;
using Antonio.TechTest.Core.Entities;
using Antonio.TechTest.UnitTests.Builders;
using Antonio.TechTest.UnitTests.Personas;
using Antonio.TechTest.UnitTests.Personas.OrderQuery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Antonio.TechTest.AcceptanceTests.Api
{
    [TestClass]
    public class WhenQueryingOrders : AcceptanceTestBase
    {
        [TestMethod]
        public async Task GivenASearchByIdItShouldReturnAnOrder()
        {
            var newOrder = new StandardOrder();

            var dto = new OrderBuilder()
                .With(newOrder)
                .WithCustomerId(5000)
                .BuildDTO();

            var assumptionHttpResponse = await AssumeOrderCreatedOnApi(dto);
            var orderOnApi = JsonConvert.DeserializeObject<Order>(await assumptionHttpResponse.Content.ReadAsStringAsync());

            var requestUrl = $"/{orderOnApi.Id}";
            var response = await _apiClient.Get(requestUrl);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);

            var orderFromRequest = JsonConvert.DeserializeObject<OrderQueryResponseDTO>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(orderOnApi.Id, orderFromRequest.OrderId);
            Assert.AreEqual(orderOnApi.OrderStatus, orderFromRequest.OrderStatus);
            Assert.AreEqual(orderOnApi.ProductId, orderFromRequest.ProductId);
            Assert.AreEqual(orderOnApi.Quantity, orderFromRequest.Quantity);
            Assert.AreEqual(orderOnApi.UnitPrice, orderFromRequest.UnitPrice);
            Assert.AreEqual(orderOnApi.UnitPrice * orderOnApi.Quantity, orderFromRequest.OrderTotal);
        }

        [TestMethod]
        public async Task GivenASearchCriteriaItShouldReturnAnOrder()
        {
            var newOrder = new StandardOrder();
            int productId = 25;
            int customerId = new Random(DateTime.Now.Millisecond).Next(15000, 35000);

            var dto = new OrderBuilder()
                .With(newOrder)
                .WithProductId(productId)
                .WithCustomerId(customerId)
                .BuildDTO();

            for (int i = 0; i < 2; i++)
            {
                var assumptionHttpResponse = await AssumeOrderCreatedOnApi(dto);
            }

            var queryRequest = new StandardQueryRequest();

            var queryDto = new OrderQueryRequestBuilder()
                .With(queryRequest)
                .WithCustomerId(customerId)
                .WithProductId(productId)
                .Build();

            var jsonContent = base.CreateJsonContentFor<OrderQueryRequestDTO>(queryDto);

            string queryUrl = "query";
            var response = await _apiClient.PostAsync(queryUrl, jsonContent);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);

            var deserializedResponse = JsonConvert.DeserializeObject<IEnumerable<OrderQueryResponseDTO>>(await response.Content.ReadAsStringAsync());

            Assert.IsTrue(deserializedResponse.Count() >= 2);

            foreach (var item in deserializedResponse)
            {
                Assert.AreEqual(dto.DeliveryAddress, item.DeliveryAddress);
                Assert.AreEqual(dto.CustomerId, item.CustomerId);
                Assert.AreEqual(dto.ProductId, item.ProductId);
            }
        }

        private async Task<HttpResponseMessage> AssumeOrderCreatedOnApi(CreateOrderRequestDTO dto)
        {
            var jsonContent = base.CreateJsonContentFor<CreateOrderRequestDTO>(dto);

            return await _apiClient.PostAsync(string.Empty, jsonContent);
        }
    }
}

