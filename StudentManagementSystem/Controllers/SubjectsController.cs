using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class SubjectsController : Controller
    {
        private StudentInfoDbContext db = new StudentInfoDbContext();

        public SubjectsController()
        {
            db.Configuration.ValidateOnSaveEnabled = false;
        }

        // GET: Subjects
        public ActionResult Index()
        {
            try
            {
                return View(db.Subjects.ToList());
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Subject subject = db.Subjects.Find(id);
                if (subject == null)
                {
                    return HttpNotFound();
                }
                return View(subject);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubjectId,SubjectName,Credit")] Subject subject)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.Subjects.Add(subject);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(subject);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Subject subject = db.Subjects.Find(id);
                if (subject == null)
                {
                    return HttpNotFound();
                }
                return View(subject);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubjectId,SubjectName,Credit")] Subject subject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(subject).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(subject);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Subject subject = db.Subjects.Find(id);
                if (subject == null)
                {
                    return HttpNotFound();
                }
                return View(subject);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Subject subject = db.Subjects.Find(id);
                db.Subjects.Remove(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
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
