using BladeVault.Application.Common.Models;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Users.Queries.GetUsersList
{
    public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, PagedResult<UserListItemDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetUsersListQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<UserListItemDto>> Handle(GetUsersListQuery query, CancellationToken cancellationToken)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize is < 1 or > 200 ? 20 : query.PageSize;

            var users = await _uow.Users.GetAllAsync(cancellationToken);

            var filtered = users.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.ToLower();
                filtered = filtered.Where(x =>
                    x.FirstName.ToLower().Contains(search) ||
                    x.LastName.ToLower().Contains(search) ||
                    x.Email.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
                filtered = filtered.Where(x => x.Role.ToString() == query.Role);

            if (query.IsActive.HasValue)
                filtered = filtered.Where(x => x.IsActive == query.IsActive.Value);

            // Sorting
            filtered = (query.SortBy?.ToLower() switch
            {
                "firstname" => query.SortOrder?.ToLower() == "asc"
                    ? filtered.OrderBy(x => x.FirstName)
                    : filtered.OrderByDescending(x => x.FirstName),
                "lastname" => query.SortOrder?.ToLower() == "asc"
                    ? filtered.OrderBy(x => x.LastName)
                    : filtered.OrderByDescending(x => x.LastName),
                "email" => query.SortOrder?.ToLower() == "asc"
                    ? filtered.OrderBy(x => x.Email)
                    : filtered.OrderByDescending(x => x.Email),
                "role" => query.SortOrder?.ToLower() == "asc"
                    ? filtered.OrderBy(x => x.Role)
                    : filtered.OrderByDescending(x => x.Role),
                _ => query.SortOrder?.ToLower() == "asc"
                    ? filtered.OrderBy(x => x.CreatedAt)
                    : filtered.OrderByDescending(x => x.CreatedAt)
            });

            var totalCount = filtered.Count();
            var items = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new UserListItemDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Role = x.Role.ToString(),
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    CreatedByUserId = x.CreatedByUserId,
                    CreatedByUserName = null, // TODO: join with creator user
                    MustChangePassword = x.MustChangePassword,
                    TemporaryPasswordIssuedAt = x.TemporaryPasswordIssuedAt
                })
                .ToList();

            return new PagedResult<UserListItemDto>(items, totalCount, page, pageSize);
        }
    }
}
