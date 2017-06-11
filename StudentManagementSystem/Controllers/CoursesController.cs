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
    public class CoursesController : Controller
    {
        private StudentInfoDbContext db = new StudentInfoDbContext();

        public CoursesController()
        {
            db.Configuration.ValidateOnSaveEnabled = false;
        }

        // GET: Courses
        public ActionResult Index()
        {
            try
            {
                return View(db.Courses.ToList());
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                return View(course);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            try
            {
                Course course = new Course();
                int i = 1;
                List<Semester> subjectCollection = db.Semesters.ToList();
                if (subjectCollection != null)
                {
                    subjectCollection.ForEach(
                        item => course.SemestersList.Add(new CheckBoxModel() { Name = item.SemesterName, ID = i++, Value = item.TotalCredits })
                      );
                }
                return View(course);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseId,CourseName,NoOfYears,NoOfSemesters,SemestersList")] Course course)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var selectedItem = course.SemestersList.Where(x => x.IsSelected);
                    foreach (var item in selectedItem)
                    {
                        var subject = db.Semesters.FirstOrDefault(x => x.SemesterName == item.Name);
                        course.Semesters.Add(subject);
                        subject.Course.Add(course);
                    }
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(course);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = db.Courses.Find(id);

                if (course == null)
                {
                    return HttpNotFound();
                }
                int i = 0;

                List<Semester> subjectCollection = new List<Semester>(course.Semesters);
                foreach (var each in subjectCollection)
                {
                    course.SemestersList.Add(new CheckBoxModel() { Name = each.SemesterName, ID = i++, IsSelected = true, Value = each.TotalCredits });
                }
                db.Semesters.ToList().Except(course.Semesters).ToList().ForEach(item => course.SemestersList.Add(new CheckBoxModel() { Name = item.SemesterName, ID = i++, Value = item.TotalCredits }));

                return View(course);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName,NoOfYears,NoOfSemesters,SemestersList")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exisitingSemesters = db.Courses.Where(s => s.CourseId == course.CourseId).Include(y => y.Semesters);
                    var sem = exisitingSemesters.FirstOrDefault();
                    sem.NoOfSemesters = course.NoOfSemesters;
                    sem.NoOfYears = course.NoOfYears;

                    sem.Semesters.Clear();
                    var selectedItem = course.SemestersList.Where(x => x.IsSelected);
                    foreach (var item in selectedItem)
                    {
                        var semester = db.Semesters.FirstOrDefault(x => x.SemesterName == item.Name);
                        sem.Semesters.Add(semester);
                    }
                    db.Entry(sem).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(course);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                return View(course);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Course course = db.Courses.Find(id);
                if (course.Students != null && course.Students.Count > 0)
                {
                    ViewBag.StudentNames = course.Students.Select(x => x.StudentName).ToList();
                    ViewBag.Delete = "Course";
                    return View("Error");
                }
                db.Courses.Remove(course);
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
