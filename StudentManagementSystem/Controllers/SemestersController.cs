using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class SemestersController : Controller
    {
        private StudentInfoDbContext db = new StudentInfoDbContext();

        // GET: Semesters
        public ActionResult Index()
        {
            return View(db.Semesters.ToList());
        }

        // GET: Semesters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // GET: Semesters/Create
        public ActionResult Create()
        {
            Semester sem = new Semester();
            int i = 1;
            List<Subject> subjectCollection = db.Subjects.ToList();
            if (subjectCollection != null)
            {
                subjectCollection.ForEach(
                    item => sem.SubjectsList.Add(new CheckBoxModel() { Name = item.SubjectName, ID = i++, Value=item.Credit })
                  );
            }
            return View(sem);
        }

        // POST: Semesters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SemesterId,SemesterName,SubjectsList,TotalCredits")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                var selectedItem = semester.SubjectsList.Where(x => x.IsSelected);
                foreach (var item in selectedItem)
                {
                    var subject = db.Subjects.FirstOrDefault(x => x.SubjectName == item.Name);
                    semester.Subjects.Add(subject);
                    if (subject.Semesters == null)
                        subject.Semesters = new List<Semester>();
                    subject.Semesters.Add(semester);
                }
                db.Semesters.Add(semester);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(semester);
        }

        // GET: Semesters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);

            if (semester == null)
            {
                return HttpNotFound();
            }
            int i = 0;

            List<Subject> subjectCollection = new List<Subject>(semester.Subjects);
            foreach (var each in subjectCollection)
            {
                semester.SubjectsList.Add(new CheckBoxModel() { Name = each.SubjectName, ID = i++, IsSelected = true, Value = each.Credit });
            }
            db.Subjects.ToList().Except(semester.Subjects).ToList().ForEach(item => semester.SubjectsList.Add(new CheckBoxModel() { Name = item.SubjectName, ID = i++, Value = item.Credit }));
            
            return View(semester);
        }

        // POST: Semesters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SemesterId,SemesterName,SubjectsList")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                var exisitingSubjects = db.Semesters.Where(s => s.SemesterId == semester.SemesterId).Include(y => y.Subjects);
                var sem = exisitingSubjects.FirstOrDefault();
                sem.SemesterName = semester.SemesterName;
                sem.TotalCredits = semester.TotalCredits;

                sem.Subjects.Clear();
                var selectedItem = semester.SubjectsList.Where(x => x.IsSelected);
                foreach (var item in selectedItem)
                {
                    var subject = db.Subjects.FirstOrDefault(x => x.SubjectName == item.Name);
                    sem.Subjects.Add(subject);
                }
                db.Entry(sem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(semester);
        }

        // GET: Semesters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Semester semester = db.Semesters.Find(id);
            db.Semesters.Remove(semester);
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
