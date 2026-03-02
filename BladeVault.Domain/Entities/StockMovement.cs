using BladeVault.Domain.Common;
using BladeVault.Domain.Enums;

namespace BladeVault.Domain.Entities
{
    public class StockMovement : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public StockMovementType MovementType { get; private set; }
        public int Quantity { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        public string? DocumentReference { get; private set; }
        public Guid? PerformedByUserId { get; private set; }

        protected StockMovement() { }

        public static StockMovement Create(
            Guid productId,
            StockMovementType movementType,
            int quantity,
            string reason,
            Guid? performedByUserId = null,
            string? documentReference = null)
        {
            return new StockMovement
            {
                ProductId = productId,
                MovementType = movementType,
                Quantity = quantity,
                Reason = reason,
                PerformedByUserId = performedByUserId,
                DocumentReference = documentReference
            };
        }
    }
}
