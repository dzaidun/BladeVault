using BladeVault.Domain.Enums;
using MediatR;

namespace BladeVault.Application.Users.Commands.CreateStaffUser
{
    public record CreateStaffUserCommand : IRequest<CreateStaffUserResult>
    {
        public Guid CreatedByUserId { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public UserRole Role { get; init; }
    }

    public record CreateStaffUserResult(Guid UserId, string Login, string TemporaryPassword, string Role);
}
