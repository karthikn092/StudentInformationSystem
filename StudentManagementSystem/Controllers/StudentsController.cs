using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManagementSystem.Models;
using System.IO;

namespace StudentManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private StudentInfoDbContext db = new StudentInfoDbContext();

        // GET: Students
        public ActionResult Index()
        {
            List<Student> student = db.Students.Include(x => x.College).Include(y => y.Courses).Include(y => y.Semesters).ToList();
            student.ForEach(item =>
            {
                item.CollegeString = item.College.CollegeName;
                item.CourseString = item.Courses.CourseName;
                item.SemesterString = item.Semesters.SemesterName;
            }
                );
            return View(student);
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Include(y => y.College).Include(y => y.Courses).Include(y => y.Semesters).FirstOrDefault(x => x.StudentId == id);
            if (student == null)
            {
                return HttpNotFound();
            }

            student.CollegeString = student.College.CollegeName;
            student.CourseString = student.Courses.CourseName;
            student.SemesterString = student.Semesters.SemesterName;
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            Student student = new Student();
            List<College> collegeCollection = db.Colleges.ToList();
            student.CollegeCollection = new List<object>();
            student.SemestersList = new List<object>();
            student.CoursesList = new List<object>();

            if (collegeCollection != null)
            {
                collegeCollection.ForEach(
                    item => student.CollegeCollection.Add(new SelectListItem() { Text = item.CollegeName, Value = item.CollegeName })
                  );
            }
            return View(student);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "StudentId,StudentName,Dob,Photo,Grade,Age," +
            "CollegeString,CourseString,SemesterString,ImageData")] Student student)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["ImageData"];
                student.Photo = ConvertToBytes(file);

                var college = db.Colleges.FirstOrDefault(x => x.CollegeName == student.CollegeString);
                student.College = college;
                college.Students.Add(student);

                var course = db.Courses.FirstOrDefault(x => x.CourseName == student.CourseString);
                student.Courses = course;
                course.Students.Add(student);

                var semester = db.Semesters.FirstOrDefault(x => x.SemesterName == student.SemesterString);
                student.Semesters = semester;
                semester.Students.Add(student);

                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<College> collegeCollection = db.Colleges.ToList();
            student.CollegeCollection = new List<object>();
            student.SemestersList = new List<object>();
            student.CoursesList = new List<object>();
            if (collegeCollection != null)
            {
                collegeCollection.ForEach(
                    item => student.CollegeCollection.Add(new SelectListItem() { Text = item.CollegeName, Value = item.CollegeName })
                  );
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            student.CollegeCollection = new List<object>();
            student.SemestersList = new List<object>();
            student.CoursesList = new List<object>();

            List<College> collegeCollection = db.Colleges.ToList();
            if (collegeCollection != null)
            {
                collegeCollection.ForEach(
                    item => student.CollegeCollection.Add(new SelectListItem() { Text = item.CollegeName, Value = item.CollegeName })
                  );
            }
            List<Course> coursseCollection = db.Courses.ToList();
            if (coursseCollection != null)
            {
                coursseCollection.ForEach(
                    item => student.CoursesList.Add(new SelectListItem() { Text = item.CourseName, Value = item.CourseName })
                  );
            }
            List<Semester> semesterCollection = db.Semesters.ToList();
            if (semesterCollection != null)
            {
                semesterCollection.ForEach(
                    item => student.SemestersList.Add(new SelectListItem() { Text = item.SemesterName, Value = item.SemesterName })
                  );
            }
            student.CollegeString = student.College.CollegeName;
            student.CourseString = student.Courses.CourseName;
            student.SemesterString = student.Semesters.SemesterName;

            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,StudentName,Dob,Grade,Age," +
            "CollegeString,CourseString,SemesterString")] Student student)
        {
            if (ModelState.IsValid)
            {
                var exisitingStudent = db.Students.Where(s => s.StudentId == student.StudentId).Include(y => y.College)
                                                                                               .Include(y => y.Courses)
                                                                                               .Include(y => y.Semesters);
                var stud = exisitingStudent.FirstOrDefault();

                stud.StudentName = student.StudentName;
                stud.Grade = student.Grade;
                stud.Age = student.Age;
                stud.Dob = student.Dob;

                var college = db.Colleges.FirstOrDefault(x => x.CollegeName == student.CollegeString);
                stud.College = college;
                college.Students.Add(stud);

                var course = db.Courses.FirstOrDefault(x => x.CourseName == student.CourseString);
                stud.Courses = course;
                course.Students.Add(stud);

                var semester = db.Semesters.FirstOrDefault(x => x.SemesterName == student.SemesterString);
                stud.Semesters = semester;
                semester.Students.Add(stud);


                HttpPostedFileBase file = Request.Files["ImageData"];
                if (file != null && file.ContentLength > 0)
                {
                    stud.Photo = ConvertToBytes(file);
                }

                db.Entry(stud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Include(y => y.College).Include(y => y.Courses).Include(y => y.Semesters).FirstOrDefault(x => x.StudentId == id);
            if (student == null)
            {
                return HttpNotFound();
            }

            student.CollegeString = student.College.CollegeName;
            student.CourseString = student.Courses.CourseName;
            student.SemesterString = student.Semesters.SemesterName;
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetDropDown(string dropId, string dropValue)
        {
            List<SelectListItem> dropCollection = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(dropId) && !string.IsNullOrEmpty(dropValue))
            {
                switch (dropId)
                {
                    case "collegedrop":
                        var college = db.Colleges.FirstOrDefault(x => x.CollegeName == dropValue);
                        List<Course> courseCollection = college.Courses.ToList();
                        if (courseCollection != null)
                        {
                            courseCollection.ForEach(
                                item => dropCollection.Add(new SelectListItem() { Text = item.CourseName, Value = item.CourseName })
                              );
                        }
                        break;

                    case "coursedrop":
                        var course = db.Courses.FirstOrDefault(x => x.CourseName == dropValue);
                        List<Semester> semesterCollection = course.Semesters.ToList();
                        if (semesterCollection != null)
                        {
                            semesterCollection.ForEach(
                                item => dropCollection.Add(new SelectListItem() { Text = item.SemesterName, Value = item.SemesterName })
                              );
                        }
                        break;
                }
            }
            return Json(new SelectList(dropCollection, "Value", "Text"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}
