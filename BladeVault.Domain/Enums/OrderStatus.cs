using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Enums
{
    public enum OrderStatus
    {
        New = 1,
        Confirmed = 2,
        InAssembly = 3,
        ReadyToShip = 4,
        Shipped = 5,
        Delivered = 6,
        Cancelled = 7,
        Returned = 8
    }
}
