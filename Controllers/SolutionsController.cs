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
    public class SolutionsController : Controller
    {
        private HouseCaseEntities3 db = new HouseCaseEntities3();

        // GET: Solutions
        [Authorize(Roles = "admin,moderator")]
        public ActionResult Index()
        {
            var solutions = db.Solutions.Include(s => s.Issue).Include(s => s.Status);
            return View(solutions.ToList());
        }

        // GET: Solutions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
               
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = db.Solutions.FirstOrDefault(s=> s.issueID==id);
            if (solution == null)
            {
                var solutions = db.Solutions.Include(s => s.Issue).Include(s => s.Status);
                return View("Index", solutions.ToList());
                // return HttpNotFound();
            }
            return View(solution);
        }

        // GET: Solutions/Create
        public ActionResult Create(int? id)
        {

            if (id != null)
            {
                Issue issue = db.Issues.Find(id);
                
                //Issue issue = db.Issues.Where(a=> a.informerID);
                ViewBag.issueID = new SelectList(db.Issues, "issueID", "issueName",issue.issueID);
                ViewBag.statusID = new SelectList(db.Status, "statusId", "status1");

                return View();
            }
            else
            {
                ViewBag.issueID = new SelectList(db.Issues, "issueID", "issueName");
                ViewBag.statusID = new SelectList(db.Status, "statusId", "status1");

                return View();
            }  

            /*ViewBag.issueID = new SelectList(db.Issues, "issueID", "issueName");
            ViewBag.statusID = new SelectList(db.Status, "statusId", "status1");
            return View();*/
        }

        // POST: Solutions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "solutionName,explanation,decisionTime,targetSchedule,issueID,statusID")] Solution solution)
        {
            if (ModelState.IsValid)
            {
                db.Solutions.Add(solution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.issueID = new SelectList(db.Issues, "issueID", "issueName", solution.issueID);
            ViewBag.statusID = new SelectList(db.Status, "statusId", "status1", solution.statusID);
            return View(solution);
        }

        // GET: Solutions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = db.Solutions.Find(id);
            if (solution == null)
            {
                return HttpNotFound();
            }
            ViewBag.issueID = new SelectList(db.Issues, "issueID", "issueName", solution.issueID);
            ViewBag.statusID = new SelectList(db.Status, "statusId", "status1", solution.statusID);
            return View(solution);
        }

        // POST: Solutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "solutionName,explanation,decisionTime,targetSchedule,issueID,statusID")] Solution solution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.issueID = new SelectList(db.Issues, "issueID", "issueName", solution.issueID);
            ViewBag.statusID = new SelectList(db.Status, "statusId", "status1", solution.statusID);
            return View(solution);
        }

        // GET: Solutions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = db.Solutions.Find(id);
            if (solution == null)
            {
                return HttpNotFound();
            }
            return View(solution);
        }

        // POST: Solutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Solution solution = db.Solutions.Find(id);
            db.Solutions.Remove(solution);
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
