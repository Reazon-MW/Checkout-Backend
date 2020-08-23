using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CheckoutProj.Models
{
    public class User : DefEntity
    {
        public User()
        {
            WorkingIn = new List<Work>();
            Checkouts = new List<Checkout>();
        }

        [Key]
        public int UserID { get; set; }
        public string EMail{ get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; } = "user";
        [JsonIgnore]
        public string PasswordHash { get; set; }

        [JsonIgnore]
        public virtual List<Work> WorkingIn { get; set; }

        [JsonIgnore]
        public virtual List<Checkout> Checkouts { get; set; }
    }
}
