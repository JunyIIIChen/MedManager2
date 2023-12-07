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
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appointments
        //public ActionResult Index()
        //{
        //    var appointments = db.Appointments.Include(a => a.Clinic);
        //    return View(appointments.ToList());
        //}
        [Authorize]
        public ActionResult Index()
        {
            if (!User.IsInRole("admin"))
            {
                var currentUserId = User.Identity.GetUserId();
                var appointments = db.Appointments
                    .Where(a => a.UserId == currentUserId)
                    .Include(a => a.Clinic)
                    .ToList();
                return View(appointments.ToList());
            }
            else
            {
            //GET: Appointments
                {
                    var appointments = db.Appointments.Include(a => a.Clinic);
                    return View(appointments.ToList());
                }
            }
        }

        // GET: Appointments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (!User.IsInRole("patient"))
            {
                return HttpNotFound();
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Appointment appointment = db.Appointments.Include(a => a.Clinic).Where(a => a.Id == id).FirstOrDefault();
                if (appointment == null)
                {
                    return HttpNotFound();
                }
                return View(appointment);
            }
        }

        // GET: Appointments/Create
        [Authorize]
        public ActionResult Create()
        {
            if (!User.IsInRole("patient"))
            {
                return HttpNotFound();
            }
            else
            {

                ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName");
                return View();
            }
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,ClinicId,StartTime,EndTime")] Appointment appointment)
        {
            if (!User.IsInRole("patient"))
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Appointments.Add(appointment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName", appointment.ClinicId);
                return View(appointment);
            }
        }

        // GET: Appointments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("patient"))
            {
                return HttpNotFound();
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Appointment appointment = db.Appointments.Include(a => a.Clinic).Where(a => a.Id == id).FirstOrDefault();
                if (appointment == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName", appointment.ClinicId);
                return View(appointment);
            }
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,ClinicId,StartTime,EndTime")] Appointment appointment)
        {
            if (!User.IsInRole("patient"))
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(appointment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName", appointment.ClinicId);
                return View(appointment);
            }
        }

        // GET: Appointments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("patient"))
            {
                return HttpNotFound();
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Appointment appointment = db.Appointments.Include(a => a.Clinic).Where(a => a.Id == id).FirstOrDefault();
                if (appointment == null)
                {
                    return HttpNotFound();
                }
                return View(appointment);
            }
        }

        // POST: Appointments/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("patient"))
            {
                return HttpNotFound();
            }
            else
            {
                Appointment appointment = db.Appointments.Find(id);
                db.Appointments.Remove(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public JsonResult GetMonthlyAppointmentsCount()
        {
            var monthlyCounts = db.Appointments
                                   .GroupBy(a => a.StartTime.Month)
                                   .Select(g => new { Month = g.Key, Count = g.Count() })
                                   .ToList();
            return Json(monthlyCounts, JsonRequestBehavior.AllowGet);
        }

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
