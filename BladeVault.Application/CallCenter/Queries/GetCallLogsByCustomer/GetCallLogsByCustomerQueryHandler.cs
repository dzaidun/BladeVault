using BladeVault.Application.Common.Models;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.CallCenter.Queries.GetCallLogsByCustomer
{
    public class GetCallLogsByCustomerQueryHandler
        : IRequestHandler<GetCallLogsByCustomerQuery, PagedResult<CallLogDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetCallLogsByCustomerQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<CallLogDto>> Handle(
            GetCallLogsByCustomerQuery query,
            CancellationToken cancellationToken)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize is < 1 or > 200 ? 20 : query.PageSize;

            var logs = await _uow.CallLogs.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

            var totalCount = logs.Count;
            var items = logs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            return new PagedResult<CallLogDto>(items, totalCount, page, pageSize);
        }
    }
}
