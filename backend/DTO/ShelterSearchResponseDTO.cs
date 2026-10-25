using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShelterSearchResponseDTO
    {
        //כל תוצאות החיפוש
        public List<ShelterSearchResultDTO> Results { get; set; } = new();
        public bool HasResults { get; set; }
        public string Message { get; set; } = "";
    }
}
