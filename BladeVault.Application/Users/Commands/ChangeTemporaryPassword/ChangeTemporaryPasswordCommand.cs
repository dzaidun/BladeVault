using MediatR;

namespace BladeVault.Application.Users.Commands.ChangeTemporaryPassword
{
    public record ChangeTemporaryPasswordCommand : IRequest
    {
        public Guid UserId { get; init; }
        public string TemporaryPassword { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;
        public string ConfirmNewPassword { get; init; } = string.Empty;
    }
}
