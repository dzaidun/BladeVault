using BladeVault.Application.Common.Models;
using MediatR;

namespace BladeVault.Application.CallCenter.Queries.GetCallLogsByCustomer
{
    public record GetCallLogsByCustomerQuery(
        Guid CustomerId,
        int Page = 1,
        int PageSize = 20)
        : IRequest<PagedResult<CallLogDto>>;

    public record CallLogDto(
        Guid Id,
        Guid CustomerId,
        Guid? OrderId,
        string Status,
        string? Comment,
        DateTime? NextCallAt,
        Guid PerformedByUserId,
        DateTime CreatedAt);
}
