using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CheckoutProj.Models;
using Microsoft.AspNetCore.Authorization;
using System.Web.Helpers;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;

namespace CheckoutProj.Controllers
{
    [Authorize]
    [Route("api/facilities/my/")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private DatabaseContext DbContext;
        private UserRepository UserRep;
        private PlaceRepository PlaceRep;
        private WorkRepository WorkRep;
        private FacilityRepository FacilityRep;
        private DiseaseRepository DiseaseRep;
        private ScheduleRepository ScheduleRep;
        private CheckoutRepository CheckoutRep;
        public FacilityController(DatabaseContext context)
        {
            DbContext = context;
            UserRep = new UserRepository(context);
            PlaceRep = new PlaceRepository(context);
            WorkRep = new WorkRepository(context);
            FacilityRep = new FacilityRepository(context);
            DiseaseRep = new DiseaseRepository(context);
            ScheduleRep = new ScheduleRepository(context);
            CheckoutRep = new CheckoutRepository(context);
        }

        [HttpGet]
        [Route("~/api/facilities/all")]
        public ActionResult<ICollection<Facility>> GetFacilities()
        {
            DbSet<Facility> Facilities = FacilityRep.GetFacilities();
            return Ok(Facilities);
        }

        [HttpGet]
        [Route("~/api/facilities/{facilityID}")]
        public ActionResult<Facility> GetFacility(int facilityID)
        {
            Facility facility = FacilityRep.Find(facilityID);
            return Ok(facility);
        }

        [HttpGet]
        public ActionResult<ICollection<Facility>> GetAdminFacilities()
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            return Ok(UserRep.GetAdminFacilities(user));
        }

        [HttpPost]
        public ActionResult<Facility> CreateMyFacility(string name, string type)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.CreateFacility(name, type);
            WorkRep.Add(user, facility, "CEO", true);
            return Ok(facility);
        }

        [HttpPut("{facilityID}")]

        public ActionResult<Facility> EditMyFacility(int facilityID, string name, string type)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            if (FacilityRep.IsAdmin(user, facility))
            {
                if (name != null)
                    facility.Name = name;
                if (type != null)
                    facility.Type = type;
                DbContext.SaveChanges();
                return Ok(facility);
            }
            else
            {
                return new ForbidResult();
            }
        }


        [HttpDelete("{facilityID}")]
        public ActionResult DeleteMyFacility(int facilityID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            if (FacilityRep.IsAdmin(user, facility))
            {
                FacilityRep.Delete(facility);
                return Ok();
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpGet("{facilityID}/workers")]
        public ActionResult<ICollection<Work>> GetMyFacilityWorkers(int facilityID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            if (FacilityRep.IsAdmin(user, facility))
            {
                return Ok(facility.Workers);
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpPut("{facilityID}/workers/{workID}")]
        public ActionResult<Work> EditMyFacilityWorker(int facilityID, int workID, string position, bool? isAdmin)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Work work = WorkRep.Find(workID);


            if (work.FacilityID != facilityID)
            {
                return new ForbidResult();
            }
            else if (FacilityRep.IsAdmin(user, work.Facility))
            {
                if (position != null)
                    work.Position = position;
                if (isAdmin != null)
                    work.IsAdmin = (bool)isAdmin;
                DbContext.SaveChanges();
                return Ok(work);
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpDelete("{facilityID}/workers/{workID}")]
        public ActionResult<Work> DeleteMyFacilityWorker(int facilityID, int workID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Work work = WorkRep.Find(workID);

            if (work != null)
            {
                return NotFound();
            }
            else if (work.FacilityID != facilityID)
            {
                return new ForbidResult();
            }
            else if (FacilityRep.IsAdmin(user, work.Facility))
            {
                WorkRep.Delete(workID);
                return Ok();
            }
            else
            {
                return new ForbidResult();
            }
        }
        [HttpGet("{facilityID}/places")]
        public ActionResult<ICollection<Place>> GetMyPlaces(int facilityID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);

            if (FacilityRep.IsAdmin(user, facility))
            {
                return Ok(facility.Places);
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpPost("{facilityID}/places")]
        public ActionResult<Place> AddPlace(int facilityID, string name, string address)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);

            if (FacilityRep.IsAdmin(user, facility))
            {
                return Ok(FacilityRep.AddPlaceToFacility(user, facility, name, address));
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpDelete("{facilityID}/places/{placeID}")]
        public ActionResult DeletePlace(int facilityID, int placeID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            Place place = PlaceRep.Find(placeID);
            if (place.FacilityID != facilityID)
            {
                return new ForbidResult();
            }
            else if (FacilityRep.IsAdmin(user, facility))
            {
                PlaceRep.Delete(placeID);
                return Ok();
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpPut("{facilityID}/places/{placeID}")]
        public ActionResult<Place> EditPlace(int facilityID, int placeID, string name, string address)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            Place place = PlaceRep.Find(placeID);
            if (place.FacilityID != facilityID)
            {
                return new ForbidResult();
            }
            else if (FacilityRep.IsAdmin(user, facility))
            {
                if (name != null)
                    place.Name = name;
                if (address != null)
                    place.Address = address;
                DbContext.SaveChanges();
                return Ok(place);
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpGet("{facilityID}/places/{placeID}/schedules")]
        public ActionResult<ICollection<Schedule>> GetPlaceSchedules(int facilityID, int placeID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            Place place = PlaceRep.Find(placeID);
            if (place.FacilityID != facilityID)
            {
                return new ForbidResult();
            }
            else if (FacilityRep.IsAdmin(user, facility))
            {
                return Ok(place.Schedules);
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpPost("{facilityID}/places/{placeID}/schedules")]
        public ActionResult<Schedule> AddPlaceSchedule(int facilityID, int placeID, int diseaseID, int intervalMin = 300)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            Place place = PlaceRep.Find(placeID);
            Disease disease = DiseaseRep.Find(diseaseID);
            if (place.FacilityID != facilityID)
            {
                return new ForbidResult();
            }
            else if (FacilityRep.IsAdmin(user, facility))
            {
                return Ok(PlaceRep.AddSchedule(place, disease, intervalMin));
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpPut("{facilityID}/places/{placeID}/schedules/{scheduleID}")]
        public ActionResult<Schedule> EditPlaceSchedule(int facilityID, int placeID, int scheduleID, int diseaseID, int intervalMin)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            Place place = PlaceRep.Find(placeID);
            Schedule schedule = ScheduleRep.Find(scheduleID);
            Disease disease = DiseaseRep.Find(diseaseID);
            if (place.FacilityID != facilityID || schedule.PlaceID != placeID)
            {
                return new ForbidResult();
            }
            else if (FacilityRep.IsAdmin(user, facility))
            {
                return Ok(ScheduleRep.EditSchedule(schedule, place, disease, intervalMin));
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpDelete("{facilityID}/places/{placeID}/schedules/{scheduleID}")]
        public ActionResult DeletePlaceSchedule(int facilityID, int placeID, int scheduleID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            if(!FacilityRep.IsAdmin(user, facility))
            {
                return new ForbidResult();
            }
            Place place = PlaceRep.Find(placeID);
            Schedule schedule = ScheduleRep.Find(scheduleID);
            if (place.FacilityID != facilityID || schedule.PlaceID != placeID)
            {
                return new ForbidResult();
            }
            else if (FacilityRep.IsAdmin(user, facility))
            {
                ScheduleRep.Delete(scheduleID);
                return Ok();
            }
            else
            {
                return new ForbidResult();
            }
        }
        [HttpGet("{facilityID}/report")]
        public ActionResult<List<Checkout>> GetPositiveCheckouts(int facilityID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Facility facility = FacilityRep.Find(facilityID);
            if (!FacilityRep.IsAdmin(user, facility))
            {
                return new ForbidResult();
            }
            var places = facility.Places;
            var checkouts = CheckoutRep.List;
            var positiveCheckouts = checkouts.Where(c => places.Contains(c.Place));
            var unreviewedCheckouts = positiveCheckouts.Where(c => !c.IsRevieved);
            return Ok(unreviewedCheckouts);
        }

        [HttpPut("review_checkout/{checkoutID}")]
        public ActionResult ReviewCheckout(int checkoutID)
        {
            User user = UserRep.FindByEmail(User.Identity.Name);
            Checkout checkout = CheckoutRep.Find(checkoutID);
            Place place = PlaceRep.Find(checkout.PlaceID);
            Facility facility = FacilityRep.Find(place.FacilityID);

            if (FacilityRep.IsAdmin(user, facility))
            {
                CheckoutRep.ReviewCheckout(checkoutID);
                return Ok(checkout);
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpGet("~/api/diseases")]
        public ActionResult<ICollection<Disease>> GetDiseases() {
            return Ok(DiseaseRep.List);
        }

    }
}
