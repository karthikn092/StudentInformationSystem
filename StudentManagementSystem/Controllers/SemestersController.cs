﻿using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using StudentManagementSystem.Models;
using System;

namespace StudentManagementSystem.Controllers
{
    public class SemestersController : Controller
    {
        private StudentInfoDbContext db = new StudentInfoDbContext();

        public SemestersController()
        {
            db.Configuration.ValidateOnSaveEnabled = false;
        }

        // GET: Semesters
        public ActionResult Index()
        {
            try
            {
                return View(db.Semesters.ToList());
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Semesters/Details/5
        public ActionResult Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Semesters/Create
        public ActionResult Create()
        {
            try
            {
                Semester sem = new Semester();
                int i = 1;
                List<Subject> subjectCollection = db.Subjects.ToList();
                if (subjectCollection != null)
                {
                    subjectCollection.ForEach(
                        item => sem.SubjectsList.Add(new CheckBoxModel() { Name = item.SubjectName, ID = i++, Value = item.Credit })
                      );
                }
                return View(sem);
            }
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // POST: Semesters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SemesterId,SemesterName,SubjectsList,TotalCredits")] Semester semester)
        {
            try
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
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Semesters/Edit/5
        public ActionResult Edit(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // POST: Semesters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SemesterId,SemesterName,SubjectsList")] Semester semester)
        {
            try
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
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // GET: Semesters/Delete/5
        public ActionResult Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return View("ErrorPage");
            }
        }

        // POST: Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Semester semester = db.Semesters.Find(id);
                if (semester.Students != null && semester.Students.Count > 0)
                {
                    ViewBag.StudentNames = semester.Students.Select(x => x.StudentName).ToList();
                    ViewBag.Delete = "Semester";
                    return View("Error");
                }
                db.Semesters.Remove(semester);
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
