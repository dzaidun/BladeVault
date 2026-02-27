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

        public ChangeOrderStatusCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(
            ChangeOrderStatusCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Знаходимо замовлення з позиціями
            var order = await _uow.Orders.GetWithItemsAsync(command.OrderId, cancellationToken)
                ?? throw new NotFoundException(nameof(Order), command.OrderId);

            // 2. Змінюємо статус через доменний метод
            // Вся логіка переходів — всередині Order.ChangeStatus()
            var result = order.ChangeStatus(command.NewStatus);

            if (!result.IsSuccess)
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { "status", [result.Error!] }
            });

            // 3. Якщо замовлення відправлено — спис��ємо товар зі складу
            if (command.NewStatus == OrderStatus.Shipped)
            {
                await _uow.BeginTransactionAsync(cancellationToken);
                try
                {
                    foreach (var item in order.Items)
                    {
                        var stock = await _uow.Stock.GetByProductIdAsync(item.ProductId, cancellationToken);
                        if (stock != null)
                        {
                            stock.WriteOff(item.Quantity);
                            _uow.Stock.Update(stock);
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

            // 4. Якщо замовлення скасоване — звільняємо резерв
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
                            _uow.Stock.Update(stock);
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

            // 5. Всі інші переходи — просто зберігаємо новий статус
            _uow.Orders.Update(order);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
