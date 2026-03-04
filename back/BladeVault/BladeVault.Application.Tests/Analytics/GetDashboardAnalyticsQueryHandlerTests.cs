using BladeVault.Application.Analytics.Queries.GetDashboardAnalytics;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Enums;
using BladeVault.Domain.Enums.ProductSpecs;
using BladeVault.Domain.Interfaces;
using BladeVault.Domain.Interfaces.Repositories;
using FluentAssertions;
using NSubstitute;

namespace BladeVault.Application.Tests.Analytics
{
    public class GetDashboardAnalyticsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCalculateSummaryMetrics()
        {
            var deliveredOrder = CreateOrderWithStatus(OrderStatus.Delivered, "knife-1", "SKU-1", 2, 100m);
            var cancelledOrder = CreateOrderWithStatus(OrderStatus.Cancelled, "knife-2", "SKU-2", 1, 80m);

            var stocks = new List<Stock>
            {
                Stock.Create(deliveredOrder.Items.First().ProductId, 5),
                Stock.Create(Guid.NewGuid(), 20)
            };

            var uow = Substitute.For<IUnitOfWork>();
            var ordersRepo = Substitute.For<IOrderRepository>();
            var stockRepo = Substitute.For<IStockRepository>();
            var productRepo = Substitute.For<IProductRepository>();

            uow.Orders.Returns(ordersRepo);
            uow.Stock.Returns(stockRepo);
            uow.Products.Returns(productRepo);

            ordersRepo.GetForAnalyticsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
                .Returns([deliveredOrder, cancelledOrder]);
            stockRepo.GetAllAsync(Arg.Any<CancellationToken>()).Returns(stocks);
            productRepo.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<Product>());

            var handler = new GetDashboardAnalyticsQueryHandler(uow);
            var query = new GetDashboardAnalyticsQuery(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

            var result = await handler.Handle(query, CancellationToken.None);

            result.TotalOrders.Should().Be(2);
            result.DeliveredOrders.Should().Be(1);
            result.CancelledOrders.Should().Be(1);
            result.Revenue.Should().Be(200m);
            result.TopProducts.Should().HaveCount(1);
            result.TopProducts[0].QuantitySold.Should().Be(2);
            result.LowStock.Should().HaveCount(1);
            result.LowStock[0].AvailableQuantity.Should().Be(5);
        }

        [Fact]
        public async Task Handle_ShouldPreferCatalogProductDataForTopProducts()
        {
            var catalogProduct = CreateKnife("Catalog Knife", "CAT-SKU");
            var deliveredOrder = CreateOrderWithStatus(
                OrderStatus.Delivered,
                "fallback-name",
                "fallback-sku",
                1,
                50m,
                catalogProduct.Id);

            var uow = Substitute.For<IUnitOfWork>();
            var ordersRepo = Substitute.For<IOrderRepository>();
            var stockRepo = Substitute.For<IStockRepository>();
            var productRepo = Substitute.For<IProductRepository>();

            uow.Orders.Returns(ordersRepo);
            uow.Stock.Returns(stockRepo);
            uow.Products.Returns(productRepo);

            ordersRepo.GetForAnalyticsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
                .Returns([deliveredOrder]);
            stockRepo.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<Stock>());
            productRepo.GetAllAsync(Arg.Any<CancellationToken>()).Returns([catalogProduct]);

            var handler = new GetDashboardAnalyticsQueryHandler(uow);
            var result = await handler.Handle(new GetDashboardAnalyticsQuery(null, null), CancellationToken.None);

            result.TopProducts.Should().ContainSingle();
            result.TopProducts[0].ProductName.Should().Be("Catalog Knife");
            result.TopProducts[0].ProductSku.Should().Be("CAT-SKU");
        }

        private static Order CreateOrderWithStatus(
            OrderStatus targetStatus,
            string productName,
            string productSku,
            int qty,
            decimal unitPrice,
            Guid? productId = null)
        {
            var order = Order.Create(Guid.NewGuid(), DeliveryMethod.SelfPickup, "Самовивіз");
            var item = OrderItem.Create(order.Id, productId ?? Guid.NewGuid(), productName, productSku, qty, unitPrice);
            order.AddItem(item);

            PromoteOrderToStatus(order, targetStatus);
            return order;
        }

        private static void PromoteOrderToStatus(Order order, OrderStatus status)
        {
            if (status == OrderStatus.New)
                return;

            order.ChangeStatus(OrderStatus.Confirmed);

            if (status == OrderStatus.Confirmed)
                return;

            if (status == OrderStatus.Cancelled)
            {
                order.ChangeStatus(OrderStatus.Cancelled);
                return;
            }

            order.ChangeStatus(OrderStatus.InAssembly);
            if (status == OrderStatus.InAssembly)
                return;

            order.ChangeStatus(OrderStatus.ReadyToShip);
            if (status == OrderStatus.ReadyToShip)
                return;

            order.ChangeStatus(OrderStatus.Shipped);
            if (status == OrderStatus.Shipped)
                return;

            order.ChangeStatus(OrderStatus.Delivered);
        }

        private static Product CreateKnife(string name, string sku)
        {
            var result = Knife.Create(
                name: name,
                slug: $"slug-{Guid.NewGuid():N}",
                sku: sku,
                brand: "Brand",
                model: "Model",
                countryOfOrigin: "UA",
                categoryId: Guid.NewGuid(),
                price: 100,
                steelType: "D2",
                bladeLengthMm: 100,
                bladeThicknessMm: 3,
                bladeShape: BladeShape.DropPoint,
                edgeType: EdgeType.PlainEdge,
                handleMaterial: "G10",
                knifeType: KnifeType.Fixed);

            return result.Value!;
        }
    }
}
