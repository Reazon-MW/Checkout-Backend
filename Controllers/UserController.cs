using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CheckoutProj.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Web.Helpers;
using Microsoft.Extensions.Logging;
using BC = BCrypt.Net.BCrypt;
using System.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;

namespace CheckoutProj.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user/")]
    public class UserController : ControllerBase
    {
        private DatabaseContext DbContext;
        private UserRepository UserRep;
        private PlaceRepository PlaceRep;
        private WorkRepository WorkRep;
        private FacilityRepository FacilityRep;
        public UserController(DatabaseContext context)
        {
            DbContext = context;
            UserRep = new UserRepository(context);
            PlaceRep = new PlaceRepository(context);
            WorkRep = new WorkRepository(context);
            FacilityRep = new FacilityRepository(context);
        }

        [HttpGet]
        public ActionResult GetUser()
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            return new ObjectResult(user);
        }

        [HttpPut]
        public ActionResult<User> ChangeUser(string email, string password, string name, string surname)
        {
            User current = UserRep.FindByEmail(User.Identity.Name);
            if (current == null)
            {
                return NotFound();
            }
            if (email != null)
                current.EMail = email;
            if (password != null)
                current.PasswordHash = BC.HashPassword(password);
            if (name != null)
                current.Name = name;
            if (surname != null)
                current.Surname = surname;

            UserRep.ChangeUser(current);
            return Ok(current);
        }

        [HttpPost("works")]
        public ActionResult AddWork(int facilityID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Work work = new Work
            {
                User = user,
                FacilityID = facilityID,
                Position = "Waiting For Approval",
                IsAdmin = false

            };
            if (UserRep.AddWork(user, work))
            {
                return new ObjectResult(work);
            }
            else
            {
                return StatusCode(500, "Internal Server Error. Somthing went Wrong!");
            }
        }

        [HttpGet("works")]
        public ActionResult<ICollection<Work>> GetWorks()
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            return new ObjectResult(user.WorkingIn);
        }

        [HttpDelete("works/{workID}")]
        public ActionResult DeleteWork(int workID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            if (user != WorkRep.Find(workID).User)
            {
                return Forbid();
            }
            else if (UserRep.DeleteWork(user, workID))
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, "Internal Server Error. Somthing went Wrong!");
            }
        }

        [HttpGet("works/current-location")]
        public ActionResult GetCurrentLocation()
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Place place = UserRep.CurrentPlace(user);
            return Ok(place);
        }

        [HttpPut("works/{workID}/location")]
        public ActionResult<Work> ChangeLocation(int workID, int placeID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            UserRep.ClearCurrentPlaces(user);
            Place place = PlaceRep.Find(placeID);
            Work work = WorkRep.Find(workID);
            if (place == null || work == null || work.User != user)
            {
                return new ForbidResult();
            }
            Facility facility = PlaceRep.FindFacilityByPlace(placeID);
            if (!facility.Workers.Contains(work))
            {
                return new ForbidResult();
            }
            else
            {
                WorkRep.ChangeCurrentPlace(work, placeID);
                return new ObjectResult(work);
            }
        }

        [HttpDelete("works/{workID}/location")]
        public ActionResult DeleteLocation(int workID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Work work = WorkRep.Find(workID);

            if (work == null)
            {
                return new NotFoundResult();
            }
            else if (!user.WorkingIn.Contains(work))
            {
                return new ForbidResult();
            }
            else
            {
                WorkRep.LeaveCurrentPlace(work);
                return Ok();
            }
        }

        [HttpPost("checkouts")]
        public ActionResult<Models.Checkout> CreateCheckout(int diseaseID, bool result)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Place place = UserRep.CurrentPlace(user);
            Models.Checkout checkout = new Models.Checkout
            {
                User = user,
                PlaceID = place.PlaceID,
                DiseaseID = diseaseID,
                Result = result,
                IsRevieved = false
            };
            UserRep.AddCheckout(user, checkout);
            return new ObjectResult(checkout);
        }

        [HttpGet("checkouts")]
        public ActionResult<ICollection<Models.Checkout>> GetCheckouts()
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            return new ObjectResult(user.Checkouts);
        }

        [HttpGet("works/current/schedules")]
        public ActionResult<ICollection<Schedule>> GetCurrentSchedules()
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Place place = UserRep.CurrentPlace(user);
            if (place == null)
            {
                return null;
            }
            else
            {
                return Ok(place.Schedules);
            }
        }

        [HttpGet("works/{workID}/places/{placeID}/schedules")]
        public ActionResult<ICollection<Schedule>> GetPlaceSchedules(int workID, int placeID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Work work = WorkRep.Find(workID);
            Place place = PlaceRep.Find(placeID);
            if (place == null || work == null)
            {
                return new ForbidResult();
            }
            else if (work.Facility != place.Facility || work.User != user)
            {
                return new ForbidResult();
            }
            else
            {
                return Ok(place.Schedules);
            }
        }
    }
}