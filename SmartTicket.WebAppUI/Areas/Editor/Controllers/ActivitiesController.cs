using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartTicket.Business;
using SmartTicket.DataAccess.EntityFramework;
using SmartTicket.Entities;

namespace SmartTicket.WebAppUI.Areas.Editor.Controllers
{
    public class ActivitiesController : Controller
    {
        ActivityManager ac = new ActivityManager();
        CompanyManager cm = new CompanyManager();
        CategoryManager ct = new CategoryManager();
        PlaceManager pm = new PlaceManager();

        // GET: Editor/Activities
        public ActionResult Index()
        {
            var activities = ac.List();
            return View(activities);
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = ac.Find(x => x.Id == id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }


        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(ct.List(), "Id", "Name");
            ViewBag.CompanyId = new SelectList(cm.List(), "Id", "Name");
            ViewBag.PlaceId = new SelectList(pm.List(), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,CategoryId,PlaceId,CompanyId,StartDate,FinishDate,TicketCount,TicketPrice,CreateDate,CreatedBy,UpdateDate,UpdatedBy")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                ac.Insert(activity);
              
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(ct.List(), "Id", "Name", activity.CategoryId);
            ViewBag.CompanyId = new SelectList(cm.List(), "Id", "Name", activity.CompanyId);
            ViewBag.PlaceId = new SelectList(pm.List(), "Id", "Name", activity.PlaceId);
            return View(activity);
        }

        // GET: Editor/Activities/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Activity activity = db.Activities.Find(id);
        //    if (activity == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", activity.CategoryId);
        //    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", activity.CompanyId);
        //    ViewBag.PlaceId = new SelectList(db.Places, "Id", "Name", activity.PlaceId);
        //    return View(activity);
        //}

        // POST: Editor/Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,Description,CategoryId,PlaceId,CompanyId,StartDate,FinishDate,TicketCount,TicketPrice,CreateDate,CreatedBy,UpdateDate,UpdatedBy")] Activity activity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(activity).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", activity.CategoryId);
        //    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", activity.CompanyId);
        //    ViewBag.PlaceId = new SelectList(db.Places, "Id", "Name", activity.PlaceId);
        //    return View(activity);
        //}

        // GET: Editor/Activities/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Activity activity = db.Activities.Find(id);
        //    if (activity == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(activity);
        //}

        // POST: Editor/Activities/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Activity activity = db.Activities.Find(id);
        //    db.Activities.Remove(activity);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
