using BladeVault.Domain.Common;
using BladeVault.Domain.Enums;

namespace BladeVault.Domain.Entities
{
    public class CallLog : BaseEntity
    {
        public Guid CustomerId { get; private set; }
        public Guid? OrderId { get; private set; }
        public CallStatus Status { get; private set; }
        public string? Comment { get; private set; }
        public DateTime? NextCallAt { get; private set; }
        public Guid PerformedByUserId { get; private set; }

        protected CallLog() { }

        public static CallLog Create(
            Guid customerId,
            Guid performedByUserId,
            CallStatus status,
            Guid? orderId = null,
            string? comment = null,
            DateTime? nextCallAt = null)
        {
            return new CallLog
            {
                CustomerId = customerId,
                PerformedByUserId = performedByUserId,
                Status = status,
                OrderId = orderId,
                Comment = comment,
                NextCallAt = nextCallAt
            };
        }
    }
}
