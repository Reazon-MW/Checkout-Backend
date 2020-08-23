using CheckoutProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutProj.Models
{
    public class UserRepository : DefaultRepository<User>
    {
        public UserRepository(DatabaseContext ctx) : base(ctx)
        {
            List = ctx.Users;
        }

        public User FindByEmail(string username)
        {
            User[] users = List.ToArray();
            for(int i = 0; i < List.Count(); i++)
            {
                if (users[i].EMail == username)
                    return users[i];
            }
            return null;
        }

        public bool ChangeUser(User user)
        {
            List.Update(user);
            DbContext.SaveChanges();
            return true;
        }
        public bool AddWork(User user, Work work)
        {
            if (user == null)
            {
                return false;
            }
            user.WorkingIn.Add(work);
            DbContext.SaveChanges();
            return true;
        }

        public bool DeleteWork(User user, int workID)
        {
            if (user == null)
            {
                return false;
            }
            else
            {
                Work work = user.WorkingIn.Where(w => w.WorkID == workID).FirstOrDefault();
                user.WorkingIn.Remove(work);
                DbContext.SaveChanges();
                return true;
            }    
        }
        public bool AddCheckout(User user, Checkout checkout)
        {
            if (user == null)
            {
                return false;
            }
            user.Checkouts.Add(checkout);
            DbContext.SaveChanges();
            return true;
        }

        public Work IsWorkingIn(User user, Facility facility)
        {
            Work foundWork = user
                .WorkingIn.Where(w => w.Facility == facility)
                .FirstOrDefault();
            if(foundWork == null)
            {
                return null;
            }
            return foundWork;
        }

        public Place CurrentPlace(User user)
        {
            var userWorks = user.WorkingIn;
            Work work = userWorks
                .Where(w => w.CurrentPlace != null)
                .FirstOrDefault();
            if (work == null)
            {
                return null;
            }
            else
            {
                return work.CurrentPlace;
            }
        }

        public void ClearCurrentPlaces(User user)
        {
            Place place = CurrentPlace(user);
            if (place != null)
            {
                DbSet<Work> currentPlaces = (DbSet<Work>)user.WorkingIn.Where(w => w.CurrentPlaceID != null);
                foreach (Work work in currentPlaces) {
                    work.CurrentPlaceID = null;
                }
                DbContext.SaveChanges();
            }
        }
        public List<Facility> GetAdminFacilities(User user)
        {
            List<Work> userWorks = user.WorkingIn;
            List<Facility> userFacilities = new List<Facility>();
            if(userWorks == null)
            {
                return userFacilities;
            }
            List <Work> userAdminWorks = userWorks.Where(p => p.IsAdmin).ToList();
            foreach(Work work in userAdminWorks)
            {
                userFacilities.Add(work.Facility);
            }
            return userFacilities;
        }
    }
}
