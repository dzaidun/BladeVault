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

            var filtered = logs.AsEnumerable();

            if (query.Status.HasValue)
                filtered = filtered.Where(x => x.Status == query.Status.Value);

            if (query.From.HasValue)
                filtered = filtered.Where(x => x.CreatedAt >= query.From.Value);

            if (query.To.HasValue)
                filtered = filtered.Where(x => x.CreatedAt <= query.To.Value);

            var totalCount = filtered.Count();
            var items = filtered
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
