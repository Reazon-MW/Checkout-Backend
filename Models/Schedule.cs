using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CheckoutProj.Models
{
    public class Schedule : DefEntity
    {
        public int ScheduleID { get; set; }
        public int PlaceID { get; set; }

        [JsonIgnore]
        public virtual Place Place { get; set; }

        public int IntervalMin { get; set; }
        public int DiseaseID { get; set; }

        //[JsonIgnore]
        public virtual Disease Disease { get; set; }
    }
}
