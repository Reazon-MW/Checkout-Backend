using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CheckoutProj.Models
{
    public class Work : DefEntity
    {
        public int WorkID { get; set; }

        public int FacilityID { get; set; }

        [JsonIgnore]
        public virtual Facility Facility { get; set; }
        public int UserID { get; set; }

        //[JsonIgnore]
        public virtual User User { get; set; }
        public string Position { get; set; }
        public int? CurrentPlaceID { get; set; }
        //[JsonIgnore]
        public virtual Place CurrentPlace { get; set; }
        public bool IsAdmin { get; set; } = false;

    }
}
