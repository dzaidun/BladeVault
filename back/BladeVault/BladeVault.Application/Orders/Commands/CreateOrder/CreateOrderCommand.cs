using BladeVault.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand : IRequest<Guid>
    {
        public Guid UserId { get; init; }
        public DeliveryMethod DeliveryMethod { get; init; }
        public Guid? AddressId { get; init; }          // Null якщо SelfPickup
        public string? NovaPostWarehouse { get; init; } // Відділення НП якщо NovaPost
        public string? Comment { get; init; }
        public IReadOnlyList<OrderItemDto> Items { get; init; } = [];
    }

    public record OrderItemDto(Guid ProductId, int Quantity);
}
