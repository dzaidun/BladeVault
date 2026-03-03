using BladeVault.WebAPI.Tests.Infrastructure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace BladeVault.WebAPI.Tests.Authorization
{
    public class PolicyAuthorizationTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PolicyAuthorizationTests(ApiWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Analytics_AnalystRole_ShouldBeAllowed()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/analytics/dashboard");
            request.Headers.Add(TestAuthHandler.RolesHeader, "Analyst");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
            response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Analytics_CallCenterRole_ShouldBeForbidden()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/analytics/dashboard");
            request.Headers.Add(TestAuthHandler.RolesHeader, "CallCenter");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task StockChange_CatalogManagerRole_ShouldBeAllowed()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/stock/change-balance")
            {
                Content = JsonContent.Create(new
                {
                    productId = Guid.NewGuid(),
                    delta = 5,
                    reason = "test"
                })
            };
            request.Headers.Add(TestAuthHandler.RolesHeader, "CatalogManager");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
            response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task StockRead_CallCenterRole_ShouldBeAllowed()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/stock/{Guid.NewGuid()}/movements");
            request.Headers.Add(TestAuthHandler.RolesHeader, "CallCenter");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
            response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CallCenterLogs_WarehouseRole_ShouldBeForbidden()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/callcenter/customers/{Guid.NewGuid()}/logs");
            request.Headers.Add(TestAuthHandler.RolesHeader, "Warehouse");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task WarehouseOrders_WarehouseRole_ShouldBeAllowed()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/warehouse/orders");
            request.Headers.Add(TestAuthHandler.RolesHeader, "Warehouse");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
            response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task WarehouseOrders_AnalystRole_ShouldBeForbidden()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/warehouse/orders");
            request.Headers.Add(TestAuthHandler.RolesHeader, "Analyst");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
