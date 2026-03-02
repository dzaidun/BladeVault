using BladeVault.Domain.Enums;
using MediatR;
using System;

namespace BladeVault.Application.Orders.Commands.ChangeOrderStatus
{
    public record ChangeOrderStatusCommand(
        Guid OrderId,
        OrderStatus NewStatus,
        string? TrackingNumber = null) : IRequest;
}
