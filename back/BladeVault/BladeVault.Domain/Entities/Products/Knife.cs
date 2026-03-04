using BladeVault.Domain.Common;
using BladeVault.Domain.Enums.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities.Products
{
    public class Knife : Product
    {
        // --- Параметри клинка ---
        public string SteelType { get; private set; } = string.Empty;   // D2, S30V, VG-10, 440C...
        public double BladeLengthMm { get; private set; }
        public double BladeThicknessMm { get; private set; }
        public BladeShape BladeShape { get; private set; }
        public EdgeType EdgeType { get; private set; }
        public bool IsCoated { get; private set; }                       // Покриття клинка (DLC, Cerakote)
        public string? CoatingType { get; private set; }                 // Тип покриття якщо є

        // --- Параметри руків'я ---
        public string HandleMaterial { get; private set; } = string.Empty;  // G10, Micarta, Titanium, FRN...
        public double? HandleLengthMm { get; private set; }

        // --- Тип та механізм ---
        public KnifeType KnifeType { get; private set; }
        public LockType? LockType { get; private set; }                  // Null для Fixed blade
        public OpeningMechanism? OpeningMechanism { get; private set; }  // Null для Fixed blade

        // --- Додаткові характеристики ---
        public bool HasClip { get; private set; }                        // Кліпса для кишені
        public bool HasGuard { get; private set; }                       // Гарда (упор для руки)
        public bool HasPommel { get; private set; }                      // Навершя (для fixed)
        public bool IncludesSheath { get; private set; }                 // Піхви в комплекті (для fixed)
        public string? SheathMaterial { get; private set; }             // Матеріал піхов якщо є

        protected Knife() { }

        public static Result<Knife> Create(
            // Базові поля
            string name,
            string slug,
            string sku,
            string brand,
            string model,
            string countryOfOrigin,
            Guid categoryId,
            decimal price,

            // Клинок
            string steelType,
            double bladeLengthMm,
            double bladeThicknessMm,
            BladeShape bladeShape,
            EdgeType edgeType,

            // Руків'я
            string handleMaterial,

            // Тип
            KnifeType knifeType,

            // Опціональні
            string? description = null,
            double? weightGrams = null,
            double? overallLengthMm = null,
            double? closedLengthMm = null,
            double? handleLengthMm = null,
            LockType? lockType = null,
            OpeningMechanism? openingMechanism = null,
            bool hasClip = false,
            bool hasGuard = false,
            bool hasPommel = false,
            bool includesSheath = false,
            string? sheathMaterial = null,
            bool isCoated = false,
            string? coatingType = null,
            decimal? discountPrice = null)
        {
            // Валідація
            if (string.IsNullOrWhiteSpace(name))
                return Result<Knife>.Failure("Назва не може бут�� порожньою");

            if (string.IsNullOrWhiteSpace(steelType))
                return Result<Knife>.Failure("Тип сталі не може бути порожнім");

            if (bladeLengthMm <= 0)
                return Result<Knife>.Failure("Довжина клинка має бути більше 0");

            if (bladeThicknessMm <= 0)
                return Result<Knife>.Failure("Товщина клинка має бути більше 0");

            if (price <= 0)
                return Result<Knife>.Failure("Ціна має бути більше 0");

            if (knifeType == KnifeType.Folding && lockType == null)
                return Result<Knife>.Failure("Складний ніж повинен мати тип замка");

            if (knifeType == KnifeType.Folding && openingMechanism == null)
                return Result<Knife>.Failure("Складний ніж повинен мати механізм відкриття");

            if (isCoated && string.IsNullOrWhiteSpace(coatingType))
                return Result<Knife>.Failure("Вкажіть тип покриття клинка");

            if (discountPrice.HasValue && discountPrice >= price)
                return Result<Knife>.Failure("Ціна зі знижкою має бути менша за звичайну ціну");

            var knife = new Knife
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
                ClosedLengthMm = knifeType == KnifeType.Folding ? closedLengthMm : null,

                // Клинок
                SteelType = steelType,
                BladeLengthMm = bladeLengthMm,
                BladeThicknessMm = bladeThicknessMm,
                BladeShape = bladeShape,
                EdgeType = edgeType,
                IsCoated = isCoated,
                CoatingType = coatingType,

                // Руків'я
                HandleMaterial = handleMaterial,
                HandleLengthMm = handleLengthMm,

                // Тип
                KnifeType = knifeType,
                LockType = lockType,
                OpeningMechanism = openingMechanism,

                // Додатково
                HasClip = hasClip,
                HasGuard = hasGuard,
                HasPommel = hasPommel,
                IncludesSheath = includesSheath,
                SheathMaterial = sheathMaterial
            };

            return Result<Knife>.Success(knife);
        }

        public Result UpdateKnifeDetails(
            string steelType,
            double bladeLengthMm,
            double bladeThicknessMm,
            BladeShape bladeShape,
            EdgeType edgeType,
            string handleMaterial,
            LockType? lockType,
            OpeningMechanism? openingMechanism,
            bool hasClip,
            bool hasGuard,
            bool hasPommel,
            bool includesSheath,
            string? sheathMaterial,
            bool isCoated,
            string? coatingType)
        {
            if (bladeLengthMm <= 0)
                return Result.Failure("Довжина клинка має бути більше 0");

            if (KnifeType == KnifeType.Folding && lockType == null)
                return Result.Failure("Складний ніж повинен мати тип замка");

            SteelType = steelType;
            BladeLengthMm = bladeLengthMm;
            BladeThicknessMm = bladeThicknessMm;
            BladeShape = bladeShape;
            EdgeType = edgeType;
            HandleMaterial = handleMaterial;
            LockType = lockType;
            OpeningMechanism = openingMechanism;
            HasClip = hasClip;
            HasGuard = hasGuard;
            HasPommel = hasPommel;
            IncludesSheath = includesSheath;
            SheathMaterial = sheathMaterial;
            IsCoated = isCoated;
            CoatingType = coatingType;

            SetUpdatedAt();
            return Result.Success();
        }
    }
}
