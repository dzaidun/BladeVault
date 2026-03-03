using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Enums;
using BladeVault.Domain.Interfaces;
using MediatR;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Orders.Commands.ChangeOrderStatus
{
    public class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommand>
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentTrackingProvider _shipmentTrackingProvider;

        public ChangeOrderStatusCommandHandler(
            IUnitOfWork uow,
            IShipmentTrackingProvider shipmentTrackingProvider)
        {
            _uow = uow;
            _shipmentTrackingProvider = shipmentTrackingProvider;
        }

        public async Task Handle(
            ChangeOrderStatusCommand command,
            CancellationToken cancellationToken)
        {
            var order = await _uow.Orders.GetWithItemsAsync(command.OrderId, cancellationToken)
                ?? throw new NotFoundException(nameof(Order), command.OrderId);

            var result = order.ChangeStatus(command.NewStatus);

            if (!result.IsSuccess)
                throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { "status", [result.Error!] }
                });

            if (command.NewStatus == OrderStatus.Shipped)
            {
                var trackingNumber = string.IsNullOrWhiteSpace(command.TrackingNumber)
                    ? await _shipmentTrackingProvider.GenerateTrackingNumberAsync(order.OrderNumber, cancellationToken)
                    : command.TrackingNumber.Trim();

                order.SetTrackingNumber(trackingNumber);

                await _uow.BeginTransactionAsync(cancellationToken);
                try
                {
                    foreach (var item in order.Items)
                    {
                        var stock = await _uow.Stock.GetByProductIdAsync(item.ProductId, cancellationToken);
                        if (stock != null)
                        {
                            stock.WriteOff(item.Quantity);

                            var movement = StockMovement.Create(
                                productId: item.ProductId,
                                movementType: StockMovementType.WriteOff,
                                quantity: item.Quantity,
                                reason: $"Списання при відправці замовлення {order.OrderNumber}",
                                documentReference: order.OrderNumber);

                            _uow.Stock.Update(stock);
                            await _uow.StockMovements.AddAsync(movement, cancellationToken);
                        }
                    }

                    _uow.Orders.Update(order);
                    await _uow.SaveChangesAsync(cancellationToken);
                    await _uow.CommitTransactionAsync(cancellationToken);
                }
                catch
                {
                    await _uow.RollbackTransactionAsync(cancellationToken);
                    throw;
                }

                return;
            }

            if (command.NewStatus == OrderStatus.Cancelled)
            {
                await _uow.BeginTransactionAsync(cancellationToken);
                try
                {
                    foreach (var item in order.Items)
                    {
                        var stock = await _uow.Stock.GetByProductIdAsync(item.ProductId, cancellationToken);
                        if (stock != null)
                        {
                            stock.Release(item.Quantity);

                            var movement = StockMovement.Create(
                                productId: item.ProductId,
                                movementType: StockMovementType.Release,
                                quantity: item.Quantity,
                                reason: $"Звільнення резерву при скасуванні замовлення {order.OrderNumber}",
                                documentReference: order.OrderNumber);

                            _uow.Stock.Update(stock);
                            await _uow.StockMovements.AddAsync(movement, cancellationToken);
                        }
                    }

                    _uow.Orders.Update(order);
                    await _uow.SaveChangesAsync(cancellationToken);
                    await _uow.CommitTransactionAsync(cancellationToken);
                }
                catch
                {
                    await _uow.RollbackTransactionAsync(cancellationToken);
                    throw;
                }

                return;
            }

            _uow.Orders.Update(order);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
