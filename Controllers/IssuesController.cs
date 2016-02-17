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
    public class IssuesController : Controller
    {
        private HouseCaseEntities3 db = new HouseCaseEntities3();

        // GET: Issues
        [Authorize(Roles = "admin,moderator")]
        public ActionResult Index()
        {
            var issues = db.Issues.Include(i => i.Announcement).Include(i => i.Location).Include(i => i.Person).Include(i => i.Status);
            return View(issues.ToList());
        }

        // GET: Issues/Details/5
        public PartialViewResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception();
            }
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                throw new Exception();
            }
            return PartialView("Details", issue);
        }

        // GET: Issues/Create
        public ActionResult Create(int? announcerID, int? announcementID)
        {

          /* if (announcerID != null)
            {
                Issue issue = new Issue();
                issue.informerID = (int)announcerID;
                ViewBag.announcementID = new SelectList(db.Announcements, "announcementId", "title");
                ViewBag.locationID = new SelectList(db.Locations, "locationId", "locationName");
               
                ViewBag.statusID = new SelectList(db.Status, "statusId", "status1");

                return View(issue);
            }
            ViewBag.announcementID = new SelectList(db.Announcements, "announcementId", "title");
            ViewBag.locationID = new SelectList(db.Locations, "locationId", "locationName");
            ViewBag.informerID = new SelectList(db.People, "personId", "firstName");
            ViewBag.statusID = new SelectList(db.Status, "statusId", "status1");
              
            return View();
            */

            
                       if (announcerID != null && announcementID != null)
                       {
                           Person person = db.People.Find(announcerID);
                           Announcement announcement = db.Announcements.Find(announcementID);
                           //Issue issue = db.Issues.Where(a=> a.informerID);
                           ViewBag.announcementID = new SelectList(db.Announcements, "announcementId", "title", announcement.announcementId);
                           ViewBag.locationID = new SelectList(db.Locations, "locationId", "locationName", person.locationId);
                           ViewBag.informerID = new SelectList(db.People, "personId", "firstName", person.personId);
                           ViewBag.statusID = new SelectList(db.Status, "statusId", "status1");
                           return View();
                       }
                       else
                       {
                           ViewBag.announcementID = new SelectList(db.Announcements, "announcementId", "title");
                           ViewBag.locationID = new SelectList(db.Locations, "locationId", "locationName");
                           ViewBag.informerID = new SelectList(db.People, "personId", "firstName");
                           ViewBag.statusID = new SelectList(db.Status, "statusId", "status1");

                           return View();
                       }  

        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin,moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "issueID,issueName,description,reportingTime,workloadEstimate,costEstimate,informerID,locationID,statusID,announcementID")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                db.Issues.Add(issue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.announcementID = new SelectList(db.Announcements, "announcementId", "title", issue.announcementID);
            ViewBag.locationID = new SelectList(db.Locations, "locationId", "locationName", issue.locationID);
            ViewBag.informerID = new SelectList(db.People, "personId", "firstName", issue.informerID);
            ViewBag.statusID = new SelectList(db.Status, "statusId", "status1", issue.statusID);
            return View(issue);
        }

        // GET: Issues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            ViewBag.announcementID = new SelectList(db.Announcements, "announcementId", "title", issue.announcementID);
            ViewBag.locationID = new SelectList(db.Locations, "locationId", "locationName", issue.locationID);
            ViewBag.informerID = new SelectList(db.People, "personId", "firstName", issue.informerID);
            ViewBag.statusID = new SelectList(db.Status, "statusId", "status1", issue.statusID);
            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize (Roles="admin,moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "issueID,issueName,description,reportingTime,workloadEstimate,costEstimate,informerID,locationID,statusID,announcementID")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.announcementID = new SelectList(db.Announcements, "announcementId", "title", issue.announcementID);
            ViewBag.locationID = new SelectList(db.Locations, "locationId", "locationName", issue.locationID);
            ViewBag.informerID = new SelectList(db.People, "personId", "firstName", issue.informerID);
            ViewBag.statusID = new SelectList(db.Status, "statusId", "status1", issue.statusID);
            return View(issue);
        }

        // GET: Issues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin,moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Issue issue = db.Issues.Find(id);
            db.Issues.Remove(issue);
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
