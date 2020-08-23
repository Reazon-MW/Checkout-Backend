using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutProj.Models
{
    public class ScheduleRepository : DefaultRepository<Schedule>
    {
        public ScheduleRepository(DatabaseContext ctx) : base(ctx)
        {
            List = ctx.Schedules;
        }

        public Schedule CreateSchedule(Place place, Disease disease, int intervalMin)
        {
            Schedule schedule = new Schedule
            {
                Place = place,
                Disease = disease,
                IntervalMin = intervalMin
            };
            Add(schedule);
            DbContext.SaveChanges();
            return schedule;
        }
        
        public Schedule EditSchedule(Schedule schedule, Place place, Disease disease, int intervalMin)
        {
            if (place != null)
                schedule.Place = place;
            if (disease != null)
                schedule.Disease = disease;
            if (intervalMin != null)
                schedule.IntervalMin = intervalMin;
            DbContext.SaveChanges();
            return schedule;
        }
    }
}
