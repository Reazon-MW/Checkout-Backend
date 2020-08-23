using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CheckoutProj.Models
{
    public class Facility : DefEntity
    {
        public Facility()
        {
            Places = new List<Place>();
            Workers = new List<Work>();
        }

        [Key]
        public int FacilityID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        
        //[JsonIgnore]
        public virtual List<Place> Places { get; set; }

        //[JsonIgnore]
        public virtual List<Work> Workers { get; set; }
    }
}
