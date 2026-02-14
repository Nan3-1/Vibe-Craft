using System.ComponentModel.DataAnnotations;

namespace VibeCraft.Web.Helpers
{
    
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date <= DateTime.UtcNow)
                {
                    return new ValidationResult("Датата трябва да бъде в бъдещето!");
                }
            }
            return ValidationResult.Success;
        }
    }

    
    public class WithinTwoYearsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                var maxDate = DateTime.UtcNow.AddYears(2);
                if (date > maxDate)
                {
                    return new ValidationResult($"Датата не може да бъде по-късно от {maxDate:dd/MM/yyyy}");
                }
            }
            return ValidationResult.Success;
        }
    }
}