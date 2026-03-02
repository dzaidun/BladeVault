using BladeVault.Domain.Common;
using BladeVault.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool MustChangePassword { get; private set; }
        public DateTime? TemporaryPasswordIssuedAt { get; private set; }
        public Guid? CreatedByUserId { get; private set; }

        // Navigation
        public ICollection<Order> Orders { get; private set; } = new List<Order>();
        public ICollection<Address> Addresses { get; private set; } = new List<Address>();

        protected User() { }

        public static User Create(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string passwordHash,
            UserRole role = UserRole.Customer)
        {
            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = passwordHash,
                Role = role
            };
        }

        public static User CreateStaff(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string temporaryPasswordHash,
            UserRole role,
            Guid createdByUserId)
        {
            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = temporaryPasswordHash,
                Role = role,
                MustChangePassword = true,
                TemporaryPasswordIssuedAt = DateTime.UtcNow,
                CreatedByUserId = createdByUserId
            };
        }

        public void Update(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            SetUpdatedAt();
        }

        public void ChangePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
            MustChangePassword = false;
            TemporaryPasswordIssuedAt = null;
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }
    }
}
