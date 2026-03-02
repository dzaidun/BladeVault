using MediatR;

namespace BladeVault.Application.CallCenter.Queries.GetCallLogsByCustomer
{
    public record GetCallLogsByCustomerQuery(Guid CustomerId)
        : IRequest<IReadOnlyList<CallLogDto>>;

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
