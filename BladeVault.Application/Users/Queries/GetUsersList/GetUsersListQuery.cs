using BladeVault.Application.Common.Models;
using MediatR;

namespace BladeVault.Application.Users.Queries.GetUsersList
{
    public record GetUsersListQuery(
        string? Search,
        string? Role,
        bool? IsActive,
        string? SortBy = "createdAt",
        string? SortOrder = "desc",
        int Page = 1,
        int PageSize = 20) : IRequest<PagedResult<UserListItemDto>>;

    public record UserListItemDto
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public Guid? CreatedByUserId { get; init; }
        public string? CreatedByUserName { get; init; }
        public bool MustChangePassword { get; init; }
        public DateTime? TemporaryPasswordIssuedAt { get; init; }
    }
}
