using System.ComponentModel.DataAnnotations;

namespace DTO
{
    //החלטת המנהל אם לאשר את המקלט ששותף
    public class UpdateShelterStatusDTO
    {
        [Required(ErrorMessage = "יש לציין סטטוס")]
        [RegularExpression("^(Pending|Approved|Rejected)$", ErrorMessage = "סטטוס לא תקין")]
        public string Status { get; set; } = "";

        [StringLength(500, ErrorMessage = "סיבת הדחייה ארוכה מדי (עד 500 תווים)")]
        public string? RejectionReason { get; set; }
    }
}