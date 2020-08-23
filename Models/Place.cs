using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CheckoutProj.Models
{
    public class Place : DefEntity
    {
        public Place()
        {
            WorkersIn = new List<Work>();
            Schedules = new List<Schedule>();
            Checkouts = new List<Checkout>();
        }

        public int PlaceID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int FacilityID { get; set; }

        [JsonIgnore]
        public virtual Facility Facility { get; set; }

        [JsonIgnore]
        public virtual List<Work> WorkersIn { get; set; }

        //[JsonIgnore]
        public virtual List<Schedule> Schedules { get; set; }

        [JsonIgnore]
        public virtual List<Checkout> Checkouts { get; set; }


    }

}
