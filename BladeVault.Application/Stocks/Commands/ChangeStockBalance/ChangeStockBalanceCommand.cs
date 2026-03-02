using MediatR;

namespace BladeVault.Application.Stocks.Commands.ChangeStockBalance
{
    public record ChangeStockBalanceCommand : IRequest
    {
        public Guid ProductId { get; init; }
        public int Delta { get; init; }
        public string Reason { get; init; } = string.Empty;
        public string? DocumentReference { get; init; }
        public Guid PerformedByUserId { get; init; }
    }
}
