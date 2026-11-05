using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DTO
{
    public class ShelterRequestDTO
    {
        //משמש להוספה ועריכה של מקלטים
        //בעריכה הקוד של המקלט ישלח בבקשה לשרת 
        public string Name { get; set; } = "";
        public string ShelterType { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; }= "";
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180,180)]
        public double Longitude { get; set; }
        [Range(0, 100000)]
        public int MaxCapacity { get; set; }

        // פה נשתמש בDTO שמשנים בו על כל העדפה רק אם קימת
        //ואי אפשר לשנות פה את ההעדפה בעצמה
        public List<ShelterFeatureUpdateDTO> Features { get; set; } = new();
        
        // תמונות חדשות שהמשתמש מעלה
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>(); // רשימת הקבצים המועלים

        // בעריכה: שמות של תמונות קיימות שרוצים לשמור (מה שלא נשלח יימחק)
        public List<string> ExistingFiles { get; set; } = new();
    }
}
