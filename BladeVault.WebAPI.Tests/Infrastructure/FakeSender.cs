using BladeVault.Application.Analytics.Queries.GetDashboardAnalytics;
using BladeVault.Application.CallCenter.Queries.GetCallLogsByCustomer;
using BladeVault.Application.Common.Models;
using BladeVault.Application.Orders.Queries.GetOrdersForWarehouse;
using BladeVault.Application.Stocks.Queries.GetStockMovementsByProduct;
using MediatR;

namespace BladeVault.WebAPI.Tests.Infrastructure
{
    public class FakeSender : ISender
    {
        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
            => Task.FromResult<object?>(null);

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            object response = request switch
            {
                GetDashboardAnalyticsQuery => new DashboardAnalyticsDto
                {
                    From = DateTime.UtcNow.AddDays(-7),
                    To = DateTime.UtcNow,
                    TotalOrders = 0,
                    DeliveredOrders = 0,
                    CancelledOrders = 0,
                    Revenue = 0
                },
                GetStockMovementsByProductQuery q => new PagedResult<StockMovementDto>([], 0, q.Page, q.PageSize),
                GetCallLogsByCustomerQuery q => new PagedResult<CallLogDto>([], 0, q.Page, q.PageSize),
                GetOrdersForWarehouseQuery q => new PagedResult<WarehouseOrderDto>([], 0, q.Page, q.PageSize),
                _ => Activator.CreateInstance(typeof(TResponse))
                     ?? throw new InvalidOperationException($"No fake response for {typeof(TResponse).Name}")
            };

            return Task.FromResult((TResponse)response);
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
            => Task.CompletedTask;

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();
    }
}
