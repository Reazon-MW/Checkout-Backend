using CheckoutProj.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutProj.Models
{
    public class WorkRepository : DefaultRepository<Work>
    {
        public WorkRepository(DatabaseContext ctx) : base(ctx)
        {
            List = ctx.Works;
        }
        public Work Add(
            User user,
            Facility facility,
            string position = "Waiting for approval",
            bool isAdmin = false)
        {
            Work work = new Work()
            {
                User = user,
                Facility = facility,
                Position = position,
                IsAdmin = isAdmin
            };
            Add(work);
            DbContext.SaveChanges();
            return work;
        }
        public bool ChangeCurrentPlace(Work work, int placeID)
        {
            work.CurrentPlaceID = placeID;
            DbContext.SaveChanges();
            return true;
        }

        public bool LeaveCurrentPlace(Work work)
        {
            work.CurrentPlaceID = null;
            DbContext.SaveChanges();
            return true;
        }
    }
}
