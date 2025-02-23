using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioManagerApi.Attributes
{
    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        // Konstruktor przyjmujący nazwę właściwości do porównania
        public DateAfterAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        // Walidacja - sprawdza, czy data jest późniejsza niż porównywana
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime?)value;

            // Pobiera wartość porównywanej właściwości
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("Property not found");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            // Sprawdza, czy data jest późniejsza
            if (currentValue.HasValue && currentValue.Value <= comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? $"Data musi być późniejsza niż {_comparisonProperty}");
            }

            return ValidationResult.Success;
        }
    }
}
