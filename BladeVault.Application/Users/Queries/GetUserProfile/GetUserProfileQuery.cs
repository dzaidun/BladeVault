using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Users.Queries.GetUserProfile
{
    public record GetUserProfileQuery(Guid UserId) : IRequest<UserProfileDto>;
}
