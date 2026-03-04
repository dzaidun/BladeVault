using MediatR;

namespace BladeVault.Application.Users.Commands.UpdateStaffUser
{
    public record UpdateStaffUserCommand : IRequest
    {
        public Guid StaffUserId { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }
}
