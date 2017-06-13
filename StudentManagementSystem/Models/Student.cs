using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        public Student()
        {
            Semesters = new Semester();
            College = new College();
            Courses = new Course();
        }
        public int StudentId { get; set; }

        [Display(Name = "Name")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage ="Invalid Name")]
        public string StudentName { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? Dob { get; set; }

        [Display(Name = "Photo")]
        public byte[] Photo { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        public string Grade { get; set; }
        [NotMapped]
        public List<object> GradeCollection { get; set; }

        public virtual Course Courses { get; set; }
        public virtual Semester Semesters { get; set; }
        public virtual College College { get; set; }


        [Display(Name = "College")]
        [NotMapped]
        public string CollegeString { get; set; }

        [Display(Name = "Course")]
        [NotMapped]
        public string CourseString { get; set; }

        [Display(Name = "Semester")]
        [NotMapped]
        public string SemesterString { get; set; }

        [NotMapped]
        public List<object> CollegeCollection { get; set; }

        [NotMapped]
        public List<object> CoursesList { get; set; }

        [NotMapped]
        public List<object> SemestersList { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }


    }
}