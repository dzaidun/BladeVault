using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Enums;
using BladeVault.Domain.Interfaces;
using MediatR;
using System.Security.Cryptography;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Users.Commands.CreateStaffUser
{
    public class CreateStaffUserCommandHandler : IRequestHandler<CreateStaffUserCommand, CreateStaffUserResult>
    {
        private const string PasswordChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@$#";

        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        public CreateStaffUserCommandHandler(IUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task<CreateStaffUserResult> Handle(
            CreateStaffUserCommand command,
            CancellationToken cancellationToken)
        {
            var creator = await _uow.Users.GetByIdAsync(command.CreatedByUserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User), command.CreatedByUserId);

            if (creator.Role is not UserRole.Owner and not UserRole.Admin)
                throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { "role", ["Лише Owner/Admin може створювати працівників"] }
                });

            if (!await _uow.Users.IsEmailUniqueAsync(command.Email, cancellationToken))
                throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { nameof(command.Email), ["Користувач з таким Email вже існує"] }
                });

            if (!await _uow.Users.IsPhoneUniqueAsync(command.PhoneNumber, cancellationToken))
                throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { nameof(command.PhoneNumber), ["Користувач з таким номером телефону вже існує"] }
                });

            var temporaryPassword = GenerateTemporaryPassword();

            var staffUser = User.CreateStaff(
                firstName: command.FirstName,
                lastName: command.LastName,
                email: command.Email,
                phoneNumber: command.PhoneNumber,
                temporaryPasswordHash: _passwordHasher.Hash(temporaryPassword),
                role: command.Role,
                createdByUserId: command.CreatedByUserId);

            await _uow.Users.AddAsync(staffUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return new CreateStaffUserResult(
                UserId: staffUser.Id,
                Login: staffUser.Email,
                TemporaryPassword: temporaryPassword,
                Role: staffUser.Role.ToString());
        }

        private static string GenerateTemporaryPassword(int length = 12)
        {
            var buffer = new char[length];

            for (var i = 0; i < length; i++)
            {
                var index = RandomNumberGenerator.GetInt32(PasswordChars.Length);
                buffer[i] = PasswordChars[index];
            }

            return new string(buffer);
        }
    }
}
