using BladeVault.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities
{
    public class Address : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string City { get; private set; } = string.Empty;
        public string Street { get; private set; } = string.Empty;
        public string HouseNumber { get; private set; } = string.Empty;
        public string? Apartment { get; private set; }
        public string PostalCode { get; private set; } = string.Empty;
        public bool IsDefault { get; private set; }

        // Navigation
        public User User { get; private set; } = null!;

        protected Address() { }

        public static Address Create(
            Guid userId,
            string city,
            string street,
            string houseNumber,
            string postalCode,
            string? apartment = null,
            bool isDefault = false)
        {
            return new Address
            {
                UserId = userId,
                City = city,
                Street = street,
                HouseNumber = houseNumber,
                PostalCode = postalCode,
                Apartment = apartment,
                IsDefault = isDefault
            };
        }

        public void SetAsDefault() => IsDefault = true;
        public void UnsetDefault() => IsDefault = false;
    }
}
