using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.CallCenter.Commands.CreateCallLog
{
    public class CreateCallLogCommandHandler : IRequestHandler<CreateCallLogCommand, Guid>
    {
        private readonly IUnitOfWork _uow;

        public CreateCallLogCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Handle(CreateCallLogCommand command, CancellationToken cancellationToken)
        {
            var customer = await _uow.Users.GetByIdAsync(command.CustomerId, cancellationToken)
                ?? throw new NotFoundException(nameof(User), command.CustomerId);

            var performer = await _uow.Users.GetByIdAsync(command.PerformedByUserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User), command.PerformedByUserId);

            if (command.OrderId.HasValue)
            {
                var order = await _uow.Orders.GetByIdAsync(command.OrderId.Value, cancellationToken)
                    ?? throw new NotFoundException(nameof(Order), command.OrderId.Value);
            }

            var callLog = CallLog.Create(
                customerId: customer.Id,
                performedByUserId: performer.Id,
                status: command.Status,
                orderId: command.OrderId,
                comment: command.Comment,
                nextCallAt: command.NextCallAt);

            await _uow.CallLogs.AddAsync(callLog, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return callLog.Id;
        }
    }
}
