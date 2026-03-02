using BladeVault.Domain.Enums;
using MediatR;

namespace BladeVault.Application.CallCenter.Commands.CreateCallLog
{
    public record CreateCallLogCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; init; }
        public Guid? OrderId { get; init; }
        public CallStatus Status { get; init; }
        public string? Comment { get; init; }
        public DateTime? NextCallAt { get; init; }
        public Guid PerformedByUserId { get; init; }
    }
}
