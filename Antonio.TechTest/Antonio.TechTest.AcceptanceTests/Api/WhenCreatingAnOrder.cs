using Antonio.TechTest.Core.DTO;
using Antonio.TechTest.Core.Entities;
using Antonio.TechTest.UnitTests.Builders;
using Antonio.TechTest.UnitTests.Personas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Antonio.TechTest.AcceptanceTests.Api
{
    [TestClass]
    public class WhenCreatingAnOrder : AcceptanceTestBase
    {
        [TestMethod]
        public void ItShouldBeCreatedOnTheApi()
        {
            var newOrder = new StandardOrder();

            var dto = new OrderBuilder()
                .With(newOrder)
                .BuildDTO();

            var jsonContent = base.CreateJsonContentFor<CreateOrderRequestDTO>(dto);

            var response = _apiClient.PostAsync(string.Empty, jsonContent).Result;

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);

            var deserializedResponse = JsonConvert.DeserializeObject<Order>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(dto.CustomerId, deserializedResponse.CustomerId);
            Assert.AreEqual(dto.ProductId, deserializedResponse.ProductId);
            Assert.AreEqual(dto.Quantity, deserializedResponse.Quantity);
            Assert.AreEqual(dto.UnitPrice, deserializedResponse.UnitPrice);
            Assert.AreEqual(dto.DeliveryAddress, deserializedResponse.DeliveryAddress);
        }

        [TestMethod]
        public void GivenAnOrderHasMoreThan10OfAnyProductItShouldBeRejected()
        {
            var newOrder = new StandardOrder();

            var dto = new OrderBuilder()
                .With(newOrder)
                .WithQuantity(11)
                .BuildDTO();

            string expectedErrorMessage = $"Order was rejected because it contains more than 10 units of product {dto.ProductId}";

            var jsonContent = base.CreateJsonContentFor<CreateOrderRequestDTO>(dto);

            var response = _apiClient.PostAsync(string.Empty, jsonContent).Result;

            Assert.IsTrue(response.StatusCode == HttpStatusCode.InternalServerError);
            Assert.AreEqual(response.Content.ReadAsStringAsync().Result.Replace("\"", ""), expectedErrorMessage);
        }

        [TestMethod]
        public void GivenAClientWithOutstandingOrdersExceeding100EuroItShouldBeRejected()
        {
            var newOrder = new StandardOrder();

            var dto = new OrderBuilder()
                .With(newOrder)
                .BuildDTO();

            string expectedErrorMessage = $"Order was rejected because customer with id {dto.CustomerId} has outstanding orders with a total value in excess of one hundred Euro";

            this.AssumeOrdersCreatedWithUnitPriceAndQuantityForClient(1, dto.CustomerId, 51, 2);

            var jsonContent = base.CreateJsonContentFor<CreateOrderRequestDTO>(dto);

            var response = _apiClient.PostAsync(string.Empty, jsonContent).Result;

            Assert.IsTrue(response.StatusCode == HttpStatusCode.InternalServerError);
            Assert.AreEqual(response.Content.ReadAsStringAsync().Result.Replace("\"", ""), expectedErrorMessage);
        }

        [TestMethod]
        public void GivenTheApiRequestHasMissingRequiredFieldsItShouldReturnError500()
        {
            var newOrder = new StandardOrder();

            var dto = new OrderBuilder()
                .WithEmptyObject()
                .BuildDTO();

            var jsonContent = base.CreateJsonContentFor<CreateOrderRequestDTO>(dto);

            var response = _apiClient.PostAsync(string.Empty, jsonContent).Result;

            Assert.IsTrue(response.StatusCode == HttpStatusCode.InternalServerError);
        }

        private void AssumeOrdersCreatedWithUnitPriceAndQuantityForClient(int numberOfOrders, int clientId, decimal unitPrice, int quantity)
        {
            for (int i = 0; i < numberOfOrders; i++)
            {
                var newOrder = new StandardOrder();

                var dto = new OrderBuilder()
                    .With(newOrder)
                    .WithCustomerId(clientId)
                    .WithUnitPrice(unitPrice)
                    .WithQuantity(quantity)
                    .BuildDTO();

                var jsonContent = base.CreateJsonContentFor<CreateOrderRequestDTO>(dto);

                _apiClient.PostAsync(string.Empty, jsonContent).Wait();
            }
        }
    }
}
