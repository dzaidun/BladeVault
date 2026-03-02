using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Enums;
using BladeVault.Domain.Interfaces;
using MediatR;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Stocks.Commands.ChangeStockBalance
{
    public class ChangeStockBalanceCommandHandler : IRequestHandler<ChangeStockBalanceCommand>
    {
        private readonly IUnitOfWork _uow;

        public ChangeStockBalanceCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(ChangeStockBalanceCommand command, CancellationToken cancellationToken)
        {
            var product = await _uow.Products.GetByIdAsync(command.ProductId, cancellationToken)
                ?? throw new NotFoundException("Product", command.ProductId);

            var performer = await _uow.Users.GetByIdAsync(command.PerformedByUserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User), command.PerformedByUserId);

            var stock = await _uow.Stock.GetByProductIdAsync(command.ProductId, cancellationToken)
                ?? throw new NotFoundException(nameof(Stock), command.ProductId);

            var movementType = command.Delta > 0
                ? StockMovementType.Inbound
                : StockMovementType.Outbound;

            var operationResult = command.Delta > 0
                ? stock.AddStock(command.Delta)
                : stock.RemoveStock(Math.Abs(command.Delta));

            if (!operationResult.IsSuccess)
                throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { "stock", [operationResult.Error!] }
                });

            var movement = StockMovement.Create(
                productId: product.Id,
                movementType: movementType,
                quantity: Math.Abs(command.Delta),
                reason: command.Reason,
                performedByUserId: performer.Id,
                documentReference: command.DocumentReference);

            _uow.Stock.Update(stock);
            await _uow.StockMovements.AddAsync(movement, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
