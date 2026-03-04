using MediatR;

namespace BladeVault.Application.Users.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand : IRequest
    {
        public Guid UserId { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
    }
}
