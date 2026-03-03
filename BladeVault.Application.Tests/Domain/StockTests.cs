using BladeVault.Domain.Entities;
using FluentAssertions;

namespace BladeVault.Application.Tests.Domain
{
    public class StockTests
    {
        [Fact]
        public void Reserve_WhenAvailableQuantityEnough_ShouldSucceed()
        {
            var stock = Stock.Create(Guid.NewGuid(), quantity: 10);

            var result = stock.Reserve(4);

            result.IsSuccess.Should().BeTrue();
            stock.ReservedQuantity.Should().Be(4);
            stock.AvailableQuantity.Should().Be(6);
        }

        [Fact]
        public void WriteOff_WhenReservedQuantityIsLowerThanAmount_ShouldFail()
        {
            var stock = Stock.Create(Guid.NewGuid(), quantity: 10);
            stock.Reserve(2);

            var result = stock.WriteOff(3);

            result.IsSuccess.Should().BeFalse();
            stock.Quantity.Should().Be(10);
            stock.ReservedQuantity.Should().Be(2);
        }

        [Fact]
        public void RemoveStock_WhenAvailableQuantityIsLowerThanAmount_ShouldFail()
        {
            var stock = Stock.Create(Guid.NewGuid(), quantity: 5);
            stock.Reserve(4);

            var result = stock.RemoveStock(2);

            result.IsSuccess.Should().BeFalse();
            stock.Quantity.Should().Be(5);
            stock.AvailableQuantity.Should().Be(1);
        }
    }
}
