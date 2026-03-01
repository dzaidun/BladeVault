using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Users.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IUnitOfWork _uow;

        public GetUserProfileQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<UserProfileDto> Handle(
            GetUserProfileQuery query,
            CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByIdAsync(query.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User), query.UserId);

            return new UserProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
