using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Domain.Entities;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace UnitTest.OrderApi.Services
{
    public class OrderServiceTest
    {
        private readonly IOrderService _orderService;
        private readonly IOrder _Order;

        public OrderServiceTest()
        {
            _Order = A.Fake<IOrder>();
            _orderService = A.Fake<IOrderService>();
        }

        public class FakeHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response = response;
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => Task.FromResult(_response);
        }

        private static HttpClient CreateFakeHttpClient(object o)
        {
            var httpResponseMesssage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = JsonContent.Create(o)
            };
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(httpResponseMesssage);
            var _httpClient = new HttpClient(fakeHttpMessageHandler)
            {
                BaseAddress = new Uri("http://localhost")
            };

            return _httpClient;
        }

        [Fact]
        public async Task GetProduct_ValidProductId_ReturnProduct()
        {
            //Arrange
            int productId = 1;
            var productDto = new ProductDto(1, "Product 1", 13, 56.78m);
            var _httpClient = CreateFakeHttpClient(productDto);
            var _orderService = new OrderService(null!, _httpClient, null!);

            //Act
            var result = await _orderService.GetProduct(productId);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task GetProduct_InvalidProductId_ReturnNull()
        {
            var productId = 1;
            var _httpClient = CreateFakeHttpClient(null!);
            var orderService = new OrderService(null!, _httpClient, null!);

            //Act
            var result = await _orderService.GetProduct(productId);

            //Assert
            result.Id.Should().Be(0);
        }

        [Fact]
        public async Task GetOrdersByClientIs_OrderExist_ReturnOrderDetails()
        {
            //Arrange
            int clientId = 1;
            var orders = new List<Order>()
            {
                new(){Id=1,ProductId=1,ClientId=clientId,PurchaseQuantity=2,OrderDate=new DateTime()},
                new(){Id=2,ProductId=2,ClientId=clientId,PurchaseQuantity=1,OrderDate=new DateTime()}
            };
            A.CallTo(() => _Order.GetORdersAsync(A<Expression<Func<Order,bool>>>.Ignored)).Returns(orders);
            var _orderService = new OrderService(_Order, null!, null!);
            //Act
            var result=await _orderService.GetOrdersByClientId(clientId);

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(orders.Count);
            result.Should().HaveCountGreaterThanOrEqualTo(2);
        }
    }
}
