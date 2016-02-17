using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseCase.Models;

namespace HouseCase.Controllers
{
    [Authorize]
    
    public class AnnouncementsController : Controller
    {
        private HouseCaseEntities3 db = new HouseCaseEntities3();

        // GET: Announcements
        public ActionResult Index()
        {
            var announcements = db.Announcements.Include(a => a.Person);
            Person person = new Person();
            

            return View(announcements.ToList());
        }

        // GET: Announcements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // GET: Announcements/Create
        public ActionResult Create(string announcerEmail)
        {

            if (announcerEmail != null )
            {
                
                Person person = db.People.FirstOrDefault(a=> a.emailAddress==announcerEmail);
                Announcement announcemnt = new Announcement();
                announcemnt.publishTime=Convert.ToDateTime(DateTime.Now.Date);
                
                
                ViewBag.announcerID = new SelectList(db.People, "personId", "firstName", person.personId);
                ViewBag.publishTime = new SelectList(db.Announcements, "publishTime", "publishTime", announcemnt.publishTime);
                
                return View();
            }
            else
            {
                ViewBag.announcerID = new SelectList(db.People, "personId", "firstName");
                ViewBag.publishTime = new SelectList(db.Announcements, "publishTime", "publishTime", DateTime.Now.Date);
                return View();

               
            }  
            
            
            
            
            
            
            
            
           
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "announcementId,title,description,publishTime,solutionTargetSchedule,flatNotification,announcerID")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                db.Announcements.Add(announcement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.announcerID = new SelectList(db.People, "personId", "firstName",announcement.announcerID);
            return View(announcement);
        }

        // GET: Announcements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.announcerID = new SelectList(db.People, "personId", "firstName", announcement.announcerID);
            return View(announcement);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "announcementId,title,description,publishTime,solutionTargetSchedule,flatNotification,announcerID")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.announcerID = new SelectList(db.People, "personId", "firstName", announcement.announcerID);
            return View(announcement);
        }

        // GET: Announcements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = db.Announcements.Find(id);
            db.Announcements.Remove(announcement);
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
