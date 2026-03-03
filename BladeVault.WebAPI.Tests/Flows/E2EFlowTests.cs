using BladeVault.Application.CallCenter.Queries.GetCallLogsByCustomer;
using BladeVault.Application.Common.Models;
using BladeVault.Application.Stocks.Queries.GetStockMovementsByProduct;
using BladeVault.Domain.Enums;
using BladeVault.WebAPI.Tests.Infrastructure;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace BladeVault.WebAPI.Tests.Flows
{
    public class E2EFlowTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly ApiWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public E2EFlowTests(ApiWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Warehouse_Workflow_ShouldMoveOrderToShippedAndSetTracking()
        {
            var ct = TestContext.Current.CancellationToken;
            var fakeSender = _factory.Services.GetRequiredService<ISender>().Should().BeOfType<FakeSender>().Subject;
            var orderId = fakeSender.SeedWarehouseOrderId;

            using var startAssembly = new HttpRequestMessage(HttpMethod.Post, $"/api/warehouse/orders/{orderId}/start-assembly");
            startAssembly.Headers.Add(TestAuthHandler.RolesHeader, "Warehouse");
            (await _client.SendAsync(startAssembly, ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var readyToShip = new HttpRequestMessage(HttpMethod.Post, $"/api/warehouse/orders/{orderId}/ready-to-ship");
            readyToShip.Headers.Add(TestAuthHandler.RolesHeader, "Warehouse");
            (await _client.SendAsync(readyToShip, ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var ship = new HttpRequestMessage(HttpMethod.Post, $"/api/warehouse/orders/{orderId}/ship")
            {
                Content = JsonContent.Create(new { trackingNumber = "NP-REAL-123456" })
            };
            ship.Headers.Add(TestAuthHandler.RolesHeader, "Warehouse");
            (await _client.SendAsync(ship, ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);

            fakeSender.GetOrderStatus(orderId).Should().Be(OrderStatus.Shipped);
            fakeSender.GetOrderTracking(orderId).Should().Be("NP-REAL-123456");
        }

        [Fact]
        public async Task Stock_ChangeBalance_ThenGetMovements_ShouldReturnAuditHistory()
        {
            var ct = TestContext.Current.CancellationToken;
            var productId = Guid.NewGuid();

            using var addStock = new HttpRequestMessage(HttpMethod.Post, "/api/stock/change-balance")
            {
                Content = JsonContent.Create(new
                {
                    productId,
                    delta = 7,
                    reason = "Restock",
                    documentReference = "DOC-001"
                })
            };
            addStock.Headers.Add(TestAuthHandler.RolesHeader, "CatalogManager");
            (await _client.SendAsync(addStock, ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var writeOff = new HttpRequestMessage(HttpMethod.Post, "/api/stock/change-balance")
            {
                Content = JsonContent.Create(new
                {
                    productId,
                    delta = -2,
                    reason = "Adjustment",
                    documentReference = "DOC-002"
                })
            };
            writeOff.Headers.Add(TestAuthHandler.RolesHeader, "CatalogManager");
            (await _client.SendAsync(writeOff, ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var getMovements = new HttpRequestMessage(
                HttpMethod.Get,
                $"/api/stock/{productId}/movements?movementType=Inbound&page=1&pageSize=20");
            getMovements.Headers.Add(TestAuthHandler.RolesHeader, "CallCenter");

            var response = await _client.SendAsync(getMovements, ct);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = await response.Content.ReadFromJsonAsync<PagedResult<StockMovementDto>>(ct);
            data.Should().NotBeNull();
            data!.TotalCount.Should().Be(1);
            data.Items.Should().ContainSingle();
            data.Items[0].MovementType.Should().Be(StockMovementType.Inbound.ToString());
        }

        [Fact]
        public async Task CallCenter_CreateLog_ThenGetLogs_ShouldReturnCreatedEntry()
        {
            var ct = TestContext.Current.CancellationToken;
            var customerId = Guid.NewGuid();

            using var createLog = new HttpRequestMessage(HttpMethod.Post, "/api/callcenter/logs")
            {
                Content = JsonContent.Create(new
                {
                    customerId,
                    status = (int)CallStatus.NeedCallback,
                    comment = "Client asked to call tomorrow",
                    nextCallAt = DateTime.UtcNow.AddDays(1)
                })
            };
            createLog.Headers.Add(TestAuthHandler.RolesHeader, "CallCenter");

            var createResponse = await _client.SendAsync(createLog, ct);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            using var getLogs = new HttpRequestMessage(
                HttpMethod.Get,
                $"/api/callcenter/customers/{customerId}/logs?status=NeedCallback&page=1&pageSize=20");
            getLogs.Headers.Add(TestAuthHandler.RolesHeader, "CallCenter");

            var logsResponse = await _client.SendAsync(getLogs, ct);
            logsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = await logsResponse.Content.ReadFromJsonAsync<PagedResult<CallLogDto>>(ct);
            data.Should().NotBeNull();
            data!.TotalCount.Should().Be(1);
            data.Items.Should().ContainSingle();
            data.Items[0].Status.Should().Be(CallStatus.NeedCallback.ToString());
        }
    }
}
