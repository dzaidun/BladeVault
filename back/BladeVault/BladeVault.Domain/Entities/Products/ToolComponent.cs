using BladeVault.Domain.Common;
using BladeVault.Domain.Enums.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities.Products
{
    public class ToolComponent : BaseEntity
    {
        public Guid MultiToolId { get; private set; }
        public ToolType Type { get; private set; }
        public string? Description { get; private set; }  // "Phillips #2", "Flat 3mm" тощо
        public int SortOrder { get; private set; }

        // Navigation
        public MultiTool MultiTool { get; private set; } = null!;

        protected ToolComponent() { }

        public static ToolComponent Create(
            Guid multiToolId,
            ToolType type,
            string? description = null,
            int sortOrder = 0)
        {
            return new ToolComponent
            {
                MultiToolId = multiToolId,
                Type = type,
                Description = description,
                SortOrder = sortOrder
            };
        }

        public void Update(ToolType type, string? description, int sortOrder)
        {
            Type = type;
            Description = description;
            SortOrder = sortOrder;
            SetUpdatedAt();
        }
    }
}
