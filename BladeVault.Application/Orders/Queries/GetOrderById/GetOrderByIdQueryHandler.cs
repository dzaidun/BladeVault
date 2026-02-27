using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IUnitOfWork _uow;

        public GetOrderByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<OrderDto> Handle(
            GetOrderByIdQuery query,
            CancellationToken cancellationToken)
        {
            var order = await _uow.Orders.GetFullOrderAsync(query.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Order), query.Id);

            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Status = order.Status,
                DeliveryMethod = order.DeliveryMethod,
                DeliveryAddress = order.DeliveryAddress,  // ← один рядок замість NovaPostWarehouse
                Comment = order.Comment,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,

                UserId = order.UserId,
                UserFullName = $"{order.User.FirstName} {order.User.LastName}",
                UserEmail = order.User.Email,
                UserPhone = order.User.PhoneNumber,

                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    ProductSKU = i.ProductSKU,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList(),

                Payment = order.Payment == null ? null : new OrderPaymentDto
                {
                    Id = order.Payment.Id,
                    Amount = order.Payment.Amount,
                    Status = order.Payment.Status.ToString(),
                    PaymentMethod = order.Payment.PaymentMethod,
                    PaidAt = order.Payment.PaidAt
                }
            };
        }
    }
}
