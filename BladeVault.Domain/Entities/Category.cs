using BladeVault.Domain.Common;
using BladeVault.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Slug { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public Guid? ParentCategoryId { get; private set; }
        public bool IsActive { get; private set; } = true;

        // Navigation
        public Category? ParentCategory { get; private set; }
        public ICollection<Category> SubCategories { get; private set; } = new List<Category>();
        public ICollection<Product> Products { get; private set; } = new List<Product>();

        protected Category() { }

        public static Category Create(
            string name,
            string slug,
            string? description = null,
            Guid? parentCategoryId = null)
        {
            return new Category
            {
                Name = name,
                Slug = slug,
                Description = description,
                ParentCategoryId = parentCategoryId
            };
        }

        public void Update(string name, string slug, string? description)
        {
            Name = name;
            Slug = slug;
            Description = description;
            SetUpdatedAt();
        }
    }
}
