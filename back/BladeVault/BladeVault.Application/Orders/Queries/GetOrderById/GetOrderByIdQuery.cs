using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;
}
