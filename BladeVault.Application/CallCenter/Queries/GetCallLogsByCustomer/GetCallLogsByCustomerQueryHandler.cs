using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.CallCenter.Queries.GetCallLogsByCustomer
{
    public class GetCallLogsByCustomerQueryHandler
        : IRequestHandler<GetCallLogsByCustomerQuery, IReadOnlyList<CallLogDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetCallLogsByCustomerQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<CallLogDto>> Handle(
            GetCallLogsByCustomerQuery query,
            CancellationToken cancellationToken)
        {
            var logs = await _uow.CallLogs.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

            return logs
                .Select(x => new CallLogDto(
                    Id: x.Id,
                    CustomerId: x.CustomerId,
                    OrderId: x.OrderId,
                    Status: x.Status.ToString(),
                    Comment: x.Comment,
                    NextCallAt: x.NextCallAt,
                    PerformedByUserId: x.PerformedByUserId,
                    CreatedAt: x.CreatedAt))
                .ToList();
        }
    }
}
