using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private StudentInfoDbContext db = new StudentInfoDbContext();

        public ActionResult Index()
        {
            List<Student> studentCollection = db.Students.ToList();
            ReportViewModel viewModel = new ReportViewModel();

            if (studentCollection != null && studentCollection.Count > 0)
            {
                List<Student> sortedStudents = studentCollection.OrderBy(x => x.CreatedDate).ToList();
                viewModel.LastAddedStudent = sortedStudents.LastOrDefault();

                var startOfTthisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var firstDay = startOfTthisMonth.AddMonths(-1);
                var lastDay = startOfTthisMonth.AddDays(-1);

                List<Student> moduleItems = studentCollection
                                                    .Where(x => x.CreatedDate >= firstDay &&
                                                                x.CreatedDate <= lastDay).ToList();
                viewModel.LastMonthAddedStudent = moduleItems.OrderBy(c => c.CreatedDate).LastOrDefault();

                List<Student> lastYear = studentCollection.Where(y => y.CreatedDate.HasValue && y.CreatedDate.Value.Year == DateTime.Now.Year - 1).ToList();

                foreach (var student in lastYear)
                {
                    switch (student.CreatedDate.Value.Month)
                    {
                        case 1:
                            viewModel.count1++;
                            break;

                        case 2:
                            viewModel.count2++;
                            break;

                        case 3:
                            viewModel.count3++;
                            break;

                        case 4:
                            viewModel.count4++;
                            break;

                        case 5:
                            viewModel.count5++;
                            break;

                        case 6:
                            viewModel.count6++;
                            break;

                        case 7:
                            viewModel.count7++;
                            break;

                        case 8:
                            viewModel.count8++;
                            break;

                        case 9:
                            viewModel.count9++;
                            break;

                        case 10:
                            viewModel.count10++;
                            break;

                        case 11:
                            viewModel.count11++;
                            break;

                        case 12:
                            viewModel.count12++;
                            break;
                    }
                }

            }

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}