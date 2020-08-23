using CheckoutProj.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutProj.Models
{
    public class FacilityRepository : DefaultRepository<Facility>
    {
        public FacilityRepository(DatabaseContext ctx) : base(ctx)
        {
            List = ctx.Facilities;
        }

        public DbSet<Facility> GetFacilities()
        {
            return List;
        }

        public Facility CreateFacility(string name, string type)
        {
            Facility facility = new Facility()
            {
                Name = name,
                Type = type,
            };
            List.Add(facility);
            DbContext.SaveChanges();
            return facility;
        }

        public Place AddPlaceToFacility(
            User user,
            Facility facility,
            string name,
            string address)
        {
            Place place = new Place()
            {
                Facility = facility,
                Name = name,
                Address = address
            };
            facility.Places.Add(place);
            DbContext.SaveChanges();
            return place;
        }

        public bool IsAdmin(User user, Facility facility)
        {
            return user.WorkingIn.Where(w => w.Facility == facility).FirstOrDefault().IsAdmin;
        }
    }
}
