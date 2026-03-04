using MediatR;

namespace BladeVault.Application.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid UserId) : IRequest;
}
