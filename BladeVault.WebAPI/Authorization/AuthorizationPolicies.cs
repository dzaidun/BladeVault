namespace BladeVault.WebAPI.Authorization
{
    public static class AuthorizationPolicies
    {
        public const string OwnerOrAdmin = nameof(OwnerOrAdmin);
        public const string ProductManagement = nameof(ProductManagement);
        public const string OrderStatusManagement = nameof(OrderStatusManagement);
    }
}
