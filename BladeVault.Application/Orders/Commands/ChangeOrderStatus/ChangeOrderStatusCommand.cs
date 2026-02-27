using BladeVault.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Orders.Commands.ChangeOrderStatus
{
    public record ChangeOrderStatusCommand(Guid OrderId, OrderStatus NewStatus) : IRequest;
}
