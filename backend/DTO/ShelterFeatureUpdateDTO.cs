using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class ShelterFeatureUpdateDTO
    {
        //לשנות העדפות של מקלט
        [Range(1, int.MaxValue, ErrorMessage = "מזהה העדפה לא תקין")]
        public int PreferenceId { get; set; }
        public bool IsAvailable { get; set; }
    }
}