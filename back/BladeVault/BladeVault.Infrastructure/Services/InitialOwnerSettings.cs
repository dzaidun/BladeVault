namespace BladeVault.Infrastructure.Services
{
    public class InitialOwnerSettings
    {
        public const string SectionName = "InitialOwner";

        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
