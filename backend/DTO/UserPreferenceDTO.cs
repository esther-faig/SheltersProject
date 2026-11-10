using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class UserPreferenceDTO
    {
        //משמש להצגת ההעדפות של המשתמש וגם לשליחת העדפותיו לשמירה
        [Range(1, int.MaxValue, ErrorMessage = "מזהה העדפה לא תקין")]
        public int PreferenceId { get; set; }

        public string? Name { get; set; } = "";

        [Range(0, int.MaxValue, ErrorMessage = "העדיפות חייבת להיות מספר לא שלילי")]
        public int Priority { get; set; }
        public bool IsRequired { get; set; }
    }
}