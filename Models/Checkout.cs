using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CheckoutProj.Models
{
    public class Checkout : DefEntity
    {
        [Key]
        public int ChechoutID { get; set; }
        //[JsonIgnore]
        public int UserID { get; set; }

        public virtual User User { get; set; }

        public int PlaceID { get; set; }

        //[JsonIgnore]
        public virtual Place Place { get; set; }
        public int DiseaseID { get; set; }

        //[JsonIgnore]
        public virtual Disease Disease { get; set; }
        public DateTime Time { get; set; }
        public bool Result { get; set; }
        public bool IsRevieved { get; set; }
    }
}
