using BladeVault.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities.Products
{
    public class ProductImage : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public string Url { get; private set; } = string.Empty;
        public bool IsMain { get; private set; }
        public int SortOrder { get; private set; }

        // Navigation
        public Product Product { get; private set; } = null!;

        protected ProductImage() { }

        public static ProductImage Create(Guid productId, string url, bool isMain = false, int sortOrder = 0)
        {
            return new ProductImage
            {
                ProductId = productId,
                Url = url,
                IsMain = isMain,
                SortOrder = sortOrder
            };
        }

        public void SetAsMain() => IsMain = true;
    }
}
