using MediatR;

namespace BladeVault.Application.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand : IRequest
    {
        public Guid UserId { get; init; }
        public string CurrentPassword { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;
    }
}
