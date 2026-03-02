namespace BladeVault.WebAPI.Authorization
{
    public static class AuthorizationPolicies
    {
        public const string OwnerOrAdmin = nameof(OwnerOrAdmin);
        public const string ProductManagement = nameof(ProductManagement);
        public const string OrderStatusManagement = nameof(OrderStatusManagement);
        public const string StockManagement = nameof(StockManagement);
        public const string StockRead = nameof(StockRead);
        public const string CallCenterOperations = nameof(CallCenterOperations);
    }
}
