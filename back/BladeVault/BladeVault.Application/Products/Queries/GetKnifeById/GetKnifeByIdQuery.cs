using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Queries.GetKnifeById
{
    public record GetKnifeByIdQuery(Guid Id) : IRequest<KnifeDto>;
}
