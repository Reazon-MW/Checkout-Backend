using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CheckoutProj.Models
{
    public class Disease : DefEntity
    {
        public Disease()
        {
            Schedules = new List<Schedule>();
            Checkouts = new List<Checkout>();
        }

        public int DiseaseID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual List<Schedule> Schedules { get; set; }

        [JsonIgnore]
        public virtual List<Checkout> Checkouts { get; set; }
    }
}
