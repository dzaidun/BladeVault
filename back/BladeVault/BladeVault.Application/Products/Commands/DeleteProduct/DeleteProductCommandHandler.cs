using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IUnitOfWork _uow;

        public DeleteProductCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(
            DeleteProductCommand command,
            CancellationToken cancellationToken)
        {
            var product = await _uow.Products.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Product), command.Id);

            product.Deactivate(); // IsActive = false

            _uow.Products.Update(product);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
