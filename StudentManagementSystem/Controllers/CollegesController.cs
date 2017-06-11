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
    public class CollegesController : Controller
    {
        private StudentInfoDbContext db = new StudentInfoDbContext();

        // GET: Colleges
        public ActionResult Index()
        {
            return View(db.Colleges.ToList());
        }

        // GET: Colleges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            College college = db.Colleges.Find(id);
            if (college == null)
            {
                return HttpNotFound();
            }
            return View(college);
        }

        // GET: Colleges/Create
        public ActionResult Create()
        {
            College college = new College();
            int i = 1;
            List<Course> coursesCollection = db.Courses.ToList();
            if (coursesCollection != null)
            {
                coursesCollection.ForEach(
                    item => college.CoursesList.Add(new CheckBoxModel() { Name = item.CourseName, ID = i++, Value = item.NoOfYears })
                  );
            }
            return View(college);
        }

        // POST: Colleges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CollegeId,CollegeName,City,State,CoursesList")] College college)
        {
            if (ModelState.IsValid)
            {
                var selectedItem = college.CoursesList.Where(x => x.IsSelected);
                foreach (var item in selectedItem)
                {
                    var subject = db.Courses.FirstOrDefault(x => x.CourseName == item.Name);
                    college.Courses.Add(subject);
                    subject.Colleges.Add(college);
                }
                db.Colleges.Add(college);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(college);
        }

        // GET: Colleges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            College college = db.Colleges.Find(id);
            if (college == null)
            {
                return HttpNotFound();
            }
            int i = 0;

            List<Course> subjectCollection = new List<Course>(college.Courses);
            foreach (var each in subjectCollection)
            {
                college.CoursesList.Add(new CheckBoxModel() { Name = each.CourseName, ID = i++, IsSelected = true, Value = each.NoOfYears });
            }
            db.Courses.ToList().Except(college.Courses).ToList().ForEach(item => college.CoursesList.Add(new CheckBoxModel() { Name = item.CourseName, ID = i++, Value = item.NoOfYears }));
            return View(college);
        }

        // POST: Colleges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CollegeId,CollegeName,City,State,CoursesList")] College college)
        {
            if (ModelState.IsValid)
            {
                var exisitingSemesters = db.Colleges.Where(s => s.CollegeId == college.CollegeId).Include(y => y.Courses);
                var cour = exisitingSemesters.FirstOrDefault();
                cour.CollegeName = college.CollegeName;
                cour.City = college.City;
                cour.State = college.State;

                cour.Courses.Clear();
                var selectedItem = college.CoursesList.Where(x => x.IsSelected);
                foreach (var item in selectedItem)
                {
                    var course = db.Courses.FirstOrDefault(x => x.CourseName == item.Name);
                    cour.Courses.Add(course);
                }
                db.Entry(cour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(college);
        }

        // GET: Colleges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            College college = db.Colleges.Find(id);
            if (college == null)
            {
                return HttpNotFound();
            }
            return View(college);
        }

        // POST: Colleges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            College college = db.Colleges.Find(id);
            db.Colleges.Remove(college);
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
