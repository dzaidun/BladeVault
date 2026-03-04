using BladeVault.Domain.Common;
using BladeVault.Domain.Enums.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities.Products
{
    public class MultiTool : Product
    {
        // --- Основні характеристики ---
        public string HandleMaterial { get; private set; } = string.Empty;  // Нержавійка, алюміній, титан...
        public bool HasPliers { get; private set; }
        public bool ReplaceableWireCutters { get; private set; }            // Змінні кусачки
        public bool HasLocking { get; private set; }                        // Чи фіксуються інструменти
        public bool IsOneHandOpenable { get; private set; }                 // Відкриття однією рукою
        public bool IncludesPouch { get; private set; }                     // Чохол в комплекті
        public string? PouchMaterial { get; private set; }                  // Матеріал чохла
        public bool HasBitSet { get; private set; }                         // Набір біт в комплекті
        public int? BitCount { get; private set; }                          // Кількість біт

        // --- Інструменти ---
        public ICollection<ToolComponent> IncludedTools { get; private set; } = new List<ToolComponent>();

        public int TotalToolCount => IncludedTools.Count;

        protected MultiTool() { }

        public static Result<MultiTool> Create(
            // Базові поля
            string name,
            string slug,
            string sku,
            string brand,
            string model,
            string countryOfOrigin,
            Guid categoryId,
            decimal price,

            // Специфічні
            string handleMaterial,
            bool hasPliers,

            // Опціональні
            string? description = null,
            double? weightGrams = null,
            double? overallLengthMm = null,
            double? closedLengthMm = null,
            bool replaceableWireCutters = false,
            bool hasLocking = false,
            bool isOneHandOpenable = false,
            bool includesPouch = false,
            string? pouchMaterial = null,
            bool hasBitSet = false,
            int? bitCount = null,
            decimal? discountPrice = null)
        {
            // Валідація
            if (string.IsNullOrWhiteSpace(name))
                return Result<MultiTool>.Failure("Назва не може бути порожньою");

            if (string.IsNullOrWhiteSpace(handleMaterial))
                return Result<MultiTool>.Failure("Матеріал корпусу не може бути порожнім");

            if (price <= 0)
                return Result<MultiTool>.Failure("Ціна має бути більше 0");

            if (hasBitSet && (bitCount == null || bitCount <= 0))
                return Result<MultiTool>.Failure("Вкажіть кількість біт у наборі");

            if (includesPouch && string.IsNullOrWhiteSpace(pouchMaterial))
                return Result<MultiTool>.Failure("Вкажіть матеріал чохла");

            if (discountPrice.HasValue && discountPrice >= price)
                return Result<MultiTool>.Failure("Ціна зі знижкою має бути менша за звичайну ціну");

            var multiTool = new MultiTool
            {
                // Базові
                Name = name,
                Slug = slug,
                SKU = sku,
                Brand = brand,
                Model = model,
                CountryOfOrigin = countryOfOrigin,
                CategoryId = categoryId,
                Price = price,
                DiscountPrice = discountPrice,
                Description = description,
                WeightGrams = weightGrams,
                OverallLengthMm = overallLengthMm,
                ClosedLengthMm = closedLengthMm,

                // Специфічні
                HandleMaterial = handleMaterial,
                HasPliers = hasPliers,
                ReplaceableWireCutters = replaceableWireCutters,
                HasLocking = hasLocking,
                IsOneHandOpenable = isOneHandOpenable,
                IncludesPouch = includesPouch,
                PouchMaterial = pouchMaterial,
                HasBitSet = hasBitSet,
                BitCount = bitCount
            };

            return Result<MultiTool>.Success(multiTool);
        }

        public Result AddTool(ToolType type, string? description = null)
        {
            var exists = IncludedTools.Any(t => t.Type == type && t.Description == description);
            if (exists)
                return Result.Failure($"Інструмент {type} вже є в списку");

            var tool = ToolComponent.Create(Id, type, description, IncludedTools.Count + 1);
            IncludedTools.Add(tool);
            SetUpdatedAt();
            return Result.Success();
        }

        public Result RemoveTool(Guid toolComponentId)
        {
            var tool = IncludedTools.FirstOrDefault(t => t.Id == toolComponentId);
            if (tool == null)
                return Result.Failure("Інструмент не знайдено");

            IncludedTools.Remove(tool);
            SetUpdatedAt();
            return Result.Success();
        }

        public Result UpdateMultiToolDetails(
            string handleMaterial,
            bool hasPliers,
            bool replaceableWireCutters,
            bool hasLocking,
            bool isOneHandOpenable,
            bool includesPouch,
            string? pouchMaterial,
            bool hasBitSet,
            int? bitCount)
        {
            if (string.IsNullOrWhiteSpace(handleMaterial))
                return Result.Failure("Матеріал корпусу не може бути порожнім");

            if (hasBitSet && (bitCount == null || bitCount <= 0))
                return Result.Failure("Вкажіть кількість біт у наборі");

            HandleMaterial = handleMaterial;
            HasPliers = hasPliers;
            ReplaceableWireCutters = replaceableWireCutters;
            HasLocking = hasLocking;
            IsOneHandOpenable = isOneHandOpenable;
            IncludesPouch = includesPouch;
            PouchMaterial = pouchMaterial;
            HasBitSet = hasBitSet;
            BitCount = bitCount;

            SetUpdatedAt();
            return Result.Success();
        }
    }
}
