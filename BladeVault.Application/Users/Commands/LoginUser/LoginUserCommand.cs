using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Users.Commands.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : IRequest<LoginResult>;

    public record LoginResult(string AccessToken, Guid UserId, string FullName, string Role);
}
