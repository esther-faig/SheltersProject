using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DTO.Validation
{
    // בודק גודל של קובץ אחד (IFormFile) או של כל קובץ ברשימה (IEnumerable<IFormFile>)
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxMb;

        public MaxFileSizeAttribute(int maxMb)
        {
            _maxMb = maxMb;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IEnumerable<IFormFile> files = value switch
            {
                IFormFile single => new[] { single },
                IEnumerable<IFormFile> many => many,
                _ => Enumerable.Empty<IFormFile>()
            };

            if (files.Any(f => f.Length > _maxMb * 1024L * 1024L))
                return new ValidationResult($"גודל כל קובץ לא יכול לעבור {_maxMb}MB");

            return ValidationResult.Success;
        }
    }
}