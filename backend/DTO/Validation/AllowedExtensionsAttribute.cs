using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DTO.Validation
{
    // בודק סיומת של קובץ אחד (IFormFile) או של רשימת קבצים (IEnumerable<IFormFile>)
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IEnumerable<IFormFile> files = value switch
            {
                IFormFile single => new[] { single },
                IEnumerable<IFormFile> many => many,
                _ => Enumerable.Empty<IFormFile>()
            };

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(ext))
                    return new ValidationResult($"מותר להעלות רק קבצים מהסוגים: {string.Join(", ", _extensions)}");
            }

            // אין קובץ? מבחינתנו תקין. אם הוא חובה, זה התפקיד של [Required]
            return ValidationResult.Success;
        }
    }
}