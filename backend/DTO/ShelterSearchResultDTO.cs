using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShelterSearchResultDTO
    {
        //צד שרת שולח את נתוני המקלט הנבחר ונתוני החיפוש שלו כדי להציגם
        public int ShelterId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string ShelterType { get; set; } = "";
        public string City { get; set; } = "";


        //המרחק מהמקלט למשתמש במטרים
        public double DistanceMeters { get; set; }
        //זמן הנסיעה מהמקלט למשתמש בשניות
        public int TravelTimeSeconds { get; set; }
        //רק למידע
        public int MaxCapacity { get; set; }

        //public double MatchScore { get; set; }
        //המאפינים שיש במקלט
        public List<ShelterFeatureDTO> Features { get; set; }
        //public List<string> MatchReasons { get; set; } = new();

        public List<string> Files { get; set; } = new(); // תמונות או קבצים של המקלט
    }

}
