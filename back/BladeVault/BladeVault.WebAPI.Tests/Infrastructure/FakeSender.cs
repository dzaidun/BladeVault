using BladeVault.Application.Analytics.Queries.GetDashboardAnalytics;
using BladeVault.Application.CallCenter.Commands.CreateCallLog;
using BladeVault.Application.CallCenter.Queries.GetCallLogsByCustomer;
using BladeVault.Application.Common.Models;
using BladeVault.Application.Orders.Commands.ChangeOrderStatus;
using BladeVault.Application.Orders.Queries.GetOrdersForWarehouse;
using BladeVault.Application.Stocks.Commands.ChangeStockBalance;
using BladeVault.Application.Stocks.Queries.GetStockMovementsByProduct;
using BladeVault.Domain.Enums;
using MediatR;

namespace BladeVault.WebAPI.Tests.Infrastructure
{
    public class FakeSender : ISender
    {
        private readonly Dictionary<Guid, TestOrder> _orders;
        private readonly List<TestStockMovement> _stockMovements = [];
        private readonly List<TestCallLog> _callLogs = [];

        public Guid SeedWarehouseOrderId { get; } = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public FakeSender()
        {
            _orders = new Dictionary<Guid, TestOrder>
            {
                [SeedWarehouseOrderId] = new TestOrder(SeedWarehouseOrderId, "BV-TEST-ORDER-001", OrderStatus.Confirmed)
            };
        }

        public OrderStatus? GetOrderStatus(Guid orderId)
            => _orders.TryGetValue(orderId, out var order) ? order.Status : null;

        public string? GetOrderTracking(Guid orderId)
            => _orders.TryGetValue(orderId, out var order) ? order.TrackingNumber : null;

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
                GetStockMovementsByProductQuery q => BuildStockMovements(q),
                GetCallLogsByCustomerQuery q => BuildCallLogs(q),
                GetOrdersForWarehouseQuery q => BuildWarehouseOrders(q),
                CreateCallLogCommand cmd => CreateCallLog(cmd),
                _ => Activator.CreateInstance(typeof(TResponse))
                     ?? throw new InvalidOperationException($"No fake response for {typeof(TResponse).Name}")
            };

            return Task.FromResult((TResponse)response);
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            switch (request)
            {
                case ChangeOrderStatusCommand cmd:
                    ApplyOrderStatus(cmd);
                    break;
                case ChangeStockBalanceCommand cmd:
                    AddStockMovement(cmd);
                    break;
            }

            return Task.CompletedTask;
        }

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        private PagedResult<WarehouseOrderDto> BuildWarehouseOrders(GetOrdersForWarehouseQuery query)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize is < 1 or > 200 ? 20 : query.PageSize;

            IEnumerable<TestOrder> orders = _orders.Values
                .Where(x => x.Status is OrderStatus.Confirmed or OrderStatus.InAssembly);

            if (query.Status.HasValue)
                orders = orders.Where(x => x.Status == query.Status.Value);

            var totalCount = orders.Count();
            var items = orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new WarehouseOrderDto
                {
                    Id = x.Id,
                    OrderNumber = x.OrderNumber,
                    Status = x.Status.ToString(),
                    DeliveryAddress = "Test",
                    TotalAmount = 0,
                    CreatedAt = DateTime.UtcNow,
                    Items = []
                })
                .ToList();

            return new PagedResult<WarehouseOrderDto>(items, totalCount, page, pageSize);
        }

        private PagedResult<StockMovementDto> BuildStockMovements(GetStockMovementsByProductQuery query)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize is < 1 or > 200 ? 20 : query.PageSize;

            var filtered = _stockMovements
                .Where(x => x.ProductId == query.ProductId)
                .AsEnumerable();

            if (query.MovementType.HasValue)
                filtered = filtered.Where(x => x.MovementType == query.MovementType.Value);

            if (query.From.HasValue)
                filtered = filtered.Where(x => x.CreatedAt >= query.From.Value);

            if (query.To.HasValue)
                filtered = filtered.Where(x => x.CreatedAt <= query.To.Value);

            var totalCount = filtered.Count();
            var items = filtered
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new StockMovementDto(
                    x.Id,
                    x.ProductId,
                    x.MovementType.ToString(),
                    x.Quantity,
                    x.Reason,
                    x.DocumentReference,
                    x.PerformedByUserId,
                    x.CreatedAt))
                .ToList();

            return new PagedResult<StockMovementDto>(items, totalCount, page, pageSize);
        }

        private PagedResult<CallLogDto> BuildCallLogs(GetCallLogsByCustomerQuery query)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize is < 1 or > 200 ? 20 : query.PageSize;

            var filtered = _callLogs
                .Where(x => x.CustomerId == query.CustomerId)
                .AsEnumerable();

            if (query.Status.HasValue)
                filtered = filtered.Where(x => x.Status == query.Status.Value);

            if (query.From.HasValue)
                filtered = filtered.Where(x => x.CreatedAt >= query.From.Value);

            if (query.To.HasValue)
                filtered = filtered.Where(x => x.CreatedAt <= query.To.Value);

            var totalCount = filtered.Count();
            var items = filtered
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new CallLogDto(
                    x.Id,
                    x.CustomerId,
                    x.OrderId,
                    x.Status.ToString(),
                    x.Comment,
                    x.NextCallAt,
                    x.PerformedByUserId,
                    x.CreatedAt))
                .ToList();

            return new PagedResult<CallLogDto>(items, totalCount, page, pageSize);
        }

        private Guid CreateCallLog(CreateCallLogCommand command)
        {
            var id = Guid.NewGuid();
            _callLogs.Add(new TestCallLog(
                id,
                command.CustomerId,
                command.OrderId,
                command.Status,
                command.Comment,
                command.NextCallAt,
                command.PerformedByUserId,
                DateTime.UtcNow));

            return id;
        }

        private void ApplyOrderStatus(ChangeOrderStatusCommand command)
        {
            if (!_orders.TryGetValue(command.OrderId, out var order))
            {
                order = new TestOrder(command.OrderId, $"BV-TEST-{command.OrderId.ToString()[..8]}", OrderStatus.New);
                _orders[command.OrderId] = order;
            }

            order.Status = command.NewStatus;
            if (command.NewStatus == OrderStatus.Shipped)
            {
                order.TrackingNumber = string.IsNullOrWhiteSpace(command.TrackingNumber)
                    ? $"NP-MOCK-{order.OrderNumber}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}"
                    : command.TrackingNumber;
            }
        }

        private void AddStockMovement(ChangeStockBalanceCommand command)
        {
            var type = command.Delta >= 0 ? StockMovementType.Inbound : StockMovementType.Outbound;
            _stockMovements.Add(new TestStockMovement(
                Guid.NewGuid(),
                command.ProductId,
                type,
                Math.Abs(command.Delta),
                command.Reason,
                command.DocumentReference,
                command.PerformedByUserId,
                DateTime.UtcNow));
        }

        private sealed class TestOrder(Guid id, string orderNumber, OrderStatus status)
        {
            public Guid Id { get; } = id;
            public string OrderNumber { get; } = orderNumber;
            public OrderStatus Status { get; set; } = status;
            public string? TrackingNumber { get; set; }
        }

        private sealed record TestStockMovement(
            Guid Id,
            Guid ProductId,
            StockMovementType MovementType,
            int Quantity,
            string Reason,
            string? DocumentReference,
            Guid PerformedByUserId,
            DateTime CreatedAt);

        private sealed record TestCallLog(
            Guid Id,
            Guid CustomerId,
            Guid? OrderId,
            CallStatus Status,
            string? Comment,
            DateTime? NextCallAt,
            Guid PerformedByUserId,
            DateTime CreatedAt);
    }
}
