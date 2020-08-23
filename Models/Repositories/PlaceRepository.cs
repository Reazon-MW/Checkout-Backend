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
    public class PlaceRepository : DefaultRepository<Place>
    {
        public PlaceRepository(DatabaseContext ctx) : base(ctx)
        {
            List = ctx.Places;
        }

        public Facility FindFacilityByPlace(int placeID)
        {
            Place Place = List
                .Where(p => p.PlaceID == placeID)
                .FirstOrDefault();
            return Place.Facility;
        }

        public Schedule AddSchedule (Place place, Disease disease, int intervalMin)
        {
            Schedule schedule = new Schedule()
            {
                Place = place,
                Disease = disease,
                IntervalMin = intervalMin
            };
            place.Schedules.Add(schedule);
            DbContext.SaveChanges();
            return schedule;
        }
    }
}
