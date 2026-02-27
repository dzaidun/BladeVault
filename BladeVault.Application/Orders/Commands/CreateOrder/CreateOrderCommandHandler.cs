using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Enums;
using BladeVault.Domain.Interfaces;
using MediatR;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IUnitOfWork _uow;

        public CreateOrderCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Handle(
            CreateOrderCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Перевіряємо користувача
            var user = await _uow.Users.GetByIdAsync(command.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User), command.UserId);

            // 2. Перевіряємо товари та їх наявність
            var orderItems = new List<(Guid ProductId, string Name, string SKU, int Quantity, decimal Price)>();

            foreach (var itemDto in command.Items)
            {
                var stock = await _uow.Stock.GetByProductIdAsync(itemDto.ProductId, cancellationToken)
                    ?? throw new NotFoundException("Stock", itemDto.ProductId);

                if (stock.AvailableQuantity < itemDto.Quantity)
                    throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { "stock", [$"Недостатньо товару на складі. Доступно: {stock.AvailableQuantity}"] }
                });

                // Отримуємо продукт щоб взяти Name, SKU та ціну
                var product = await _uow.Products.GetByIdAsync(itemDto.ProductId, cancellationToken)
                    ?? throw new NotFoundException("Product", itemDto.ProductId);

                orderItems.Add((
                    product.Id,
                    product.Name,
                    product.SKU,
                    itemDto.Quantity,
                    product.GetActualPrice()));
            }

            // 3. Формуємо рядок адреси доставки
            var deliveryAddress = command.DeliveryMethod switch
            {
                DeliveryMethod.NovaPost => command.NovaPostWarehouse ?? string.Empty,
                DeliveryMethod.SelfPickup => "Самовивіз",
                DeliveryMethod.Courier => await ResolveAddressAsync(
                                                  command.UserId,
                                                  command.AddressId,
                                                  cancellationToken),
                _ => string.Empty
            };

            // 4. Створюємо замовлення
            var order = Order.Create(
                userId: command.UserId,
                deliveryMethod: command.DeliveryMethod,
                deliveryAddress: deliveryAddress,
                comment: command.Comment);

            // 5. Додаємо позиції
            foreach (var item in orderItems)
            {
                var orderItem = OrderItem.Create(
                    orderId: order.Id,
                    productId: item.ProductId,
                    productName: item.Name,
                    productSKU: item.SKU,
                    quantity: item.Quantity,
                    unitPrice: item.Price);

                order.AddItem(orderItem);
            }

            // 6. Резервуємо склад + зберігаємо в транзакції
            await _uow.BeginTransactionAsync(cancellationToken);
            try
            {
                foreach (var itemDto in command.Items)
                {
                    var stock = await _uow.Stock.GetByProductIdAsync(itemDto.ProductId, cancellationToken);
                    var reserveResult = stock!.Reserve(itemDto.Quantity);

                    if (!reserveResult.IsSuccess)
                        throw new ApplicationValidationException(new Dictionary<string, string[]>
                    {
                        { "stock", [reserveResult.Error!] }
                    });

                    _uow.Stock.Update(stock);
                }

                await _uow.Orders.AddAsync(order, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                await _uow.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            return order.Id;
        }

        private async Task<string> ResolveAddressAsync(
            Guid userId,
            Guid? addressId,
            CancellationToken cancellationToken)
        {
            if (!addressId.HasValue)
                return string.Empty;

            var user = await _uow.Users.GetWithAddressesAsync(userId, cancellationToken);
            var address = user?.Addresses.FirstOrDefault(a => a.Id == addressId.Value)
                ?? throw new NotFoundException(nameof(Address), addressId.Value);

            return $"{address.City}, {address.Street}, {address.HouseNumber}" +
                   (address.Apartment != null ? $", кв. {address.Apartment}" : string.Empty);
        }
    }
}
