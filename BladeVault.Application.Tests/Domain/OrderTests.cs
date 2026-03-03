using BladeVault.Domain.Entities;
using BladeVault.Domain.Enums;
using FluentAssertions;

namespace BladeVault.Application.Tests.Domain
{
    public class OrderTests
    {
        [Fact]
        public void ChangeStatus_ValidTransition_ShouldSucceedAndUpdateStatus()
        {
            var order = Order.Create(
                userId: Guid.NewGuid(),
                deliveryMethod: DeliveryMethod.SelfPickup,
                deliveryAddress: "Самовивіз");

            var result = order.ChangeStatus(OrderStatus.Confirmed);

            result.IsSuccess.Should().BeTrue();
            order.Status.Should().Be(OrderStatus.Confirmed);
        }

        [Fact]
        public void ChangeStatus_InvalidTransition_ShouldFail()
        {
            var order = Order.Create(
                userId: Guid.NewGuid(),
                deliveryMethod: DeliveryMethod.SelfPickup,
                deliveryAddress: "Самовивіз");

            var result = order.ChangeStatus(OrderStatus.Shipped);

            result.IsSuccess.Should().BeFalse();
            order.Status.Should().Be(OrderStatus.New);
        }
    }
}
