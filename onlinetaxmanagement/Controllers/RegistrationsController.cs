﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using onlinetaxmanagement.Models;

namespace onlinetaxmanagement.Controllers
{
    public class RegistrationsController : Controller
    {
        private TaxSystemEntities1 db = new TaxSystemEntities1();

        // GET: Registrations
        public ActionResult Index()
        {
            return View(db.Registrations.ToList());
        }

        // GET: Registrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: Registrations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Uid,FirstName,LastName,PanNumber,MobileNumber,Email, Password,BankName,AccountNumber,IFSC")] Registration registration)
        {
            string pannumber =  registration.PanNumber;
            if(registration.Password != Request["cpassword"])
            {
                ViewBag.Message2 = "The password and confirmation password do not match.";
                return View(registration);
            }
            if (pannumber != null)
            {
                var data = db.PanCardDetails.Where(m => m.PanNumber.Equals(pannumber)).FirstOrDefault();
                if (data == null)
                {
                    ViewBag.Message = "PanCard is Not Exist Try Another One.";
                    return View(registration);
                }
                if (data.MobileNumber != registration.MobileNumber)
                {
                    ViewBag.Message1 = "Mobile Number Does Not Match With Pan Card!";
                    return View(registration);
                }
            }
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Index","Login");
            }

            return View(registration);
        }

        // GET: Registrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Uid,FirstName,LastName,PanNumber,MobileNumber,Email,Password,BankName,AccountNumber,IFSC")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Registration registration = db.Registrations.Find(id);
            db.Registrations.Remove(registration);
            db.SaveChanges();
            return RedirectToAction("Index");
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
