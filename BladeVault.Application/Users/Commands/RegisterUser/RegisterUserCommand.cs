using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand : IRequest<Guid>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string ConfirmPassword { get; init; } = string.Empty;
    }
}
