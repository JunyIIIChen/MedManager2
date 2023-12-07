using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedManager.Models;
using Microsoft.AspNet.Identity;

namespace MedManager.Controllers
{
    public class ClinicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clinics
        public ActionResult Index()
        {
            return View(db.Clinics.ToList());
        }

        // GET: Clinics/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClinicId = id;
            clinic.Ratings = db.Ratings.Where(r => r.ClinicId == id).ToList();
            if(clinic.Ratings.Count != 0)
            {
                var total = 0;
                foreach(var rating in clinic.Ratings)
                {
                    total += rating.Rate;
                }
                clinic.AverageRating = (decimal)total / clinic.Ratings.Count;
            }
            return View(clinic);
        }

        // GET: Clinics/Create
        [Authorize]
        public ActionResult Create()
        {
            if (!User.IsInRole("admin"))
            {
                return HttpNotFound();
            }
            return View();
        }

        // POST: Clinics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClinicName,ClinicAddress,ClinicPhone,ClinicDescription")] Clinic clinic)
        {
            if (!User.IsInRole("admin"))
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Clinics.Add(clinic);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(clinic);
            }
        }

        [Authorize]
        // GET: Clinics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("admin"))
            {
                return HttpNotFound();
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Clinic clinic = db.Clinics.Find(id);
                if (clinic == null)
                {
                    return HttpNotFound();
                }
                return View(clinic);
            }
        }

        // POST: Clinics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClinicName,ClinicAddress,ClinicPhone,ClinicDescription")] Clinic clinic)
        {
            if (!User.IsInRole("admin"))
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(clinic).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(clinic);
            }
        }

        // GET: Clinics/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("admin"))
            { return HttpNotFound(); }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Clinic clinic = db.Clinics.Find(id);
                if (clinic == null)
                {
                    return HttpNotFound();
                }
                return View(clinic);
            }
        }

        // POST: Clinics/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("admin"))
            { return HttpNotFound(); }
            else
            {
                Clinic clinic = db.Clinics.Find(id);
                db.Clinics.Remove(clinic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult RateClinic(int Id, int Rate,string Comment)
        {
            var userId = User.Identity.GetUserId();
            var rate = new Models.Rating
            {
                ClinicId = Id,
                UserId = userId,
                Rate = Rate,
                Comment = Comment
            };
            db.Ratings.Add(rate);
            db.SaveChanges();
            return RedirectToAction($"Details/{Id}");
        }


        public JsonResult GetClinics()
        {
            var clinics = db.Clinics.ToList();
            return new JsonResult { Data = clinics, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetClinicAppointments(int Id)
        {
            var appointments = db.Appointments.Where(a=> a.ClinicId == Id).ToList();
            return new JsonResult { Data = appointments, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public ActionResult AddAppointment(DateTime StartTime, DateTime EndTime, int ClinicId)
        {
            try
            {
                Appointment appointment = new Appointment
                {
                    ClinicId = ClinicId,
                    UserId = User.Identity.GetUserId(),
                    StartTime = StartTime,
                    EndTime = EndTime
                };

                if (ModelState.IsValid)
                {
                    //add to db
                    db.Appointments.Add(appointment);
                    db.SaveChanges();

                    // return true
                    return Json(new
                    {
                        success = true,
                        start = StartTime,
                        end = EndTime,
                        clinicId = ClinicId
                    });
                }
                else
                {
                    // if model is not valid return false
                    return Json(new { success = false, message = "Invalid appointment details" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        //public JsonResult GetClinicRating()
        //{
        //    var clinicRating = db.Ratings.ToList();
        //    return new JsonResult { Data = clinicRating, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



}
}
