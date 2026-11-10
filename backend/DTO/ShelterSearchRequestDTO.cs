using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class ShelterSearchRequestDTO
    {
        //צד לקוח שולח לשרת את הנתונים הדרושים לו לחיפוש
        [Range(-90, 90, ErrorMessage = "קו רוחב חייב להיות בין -90 ל-90")]
        public double Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "קו אורך חייב להיות בין -180 ל-180")]
        public double Longitude { get; set; }

        [RegularExpression("^(WALKING|DRIVING)$", ErrorMessage = "אופן ההגעה חייב להיות WALKING או DRIVING")]
        public string TravelMode { get; set; } = "WALKING";//ברירת מחדל

        public int? UserId { get; set; } // null עבור משתמש אורח
    }
}