using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Course
    {
        public Course()
        {
            Semesters = new List<Semester>();
            SemestersList = new List<CheckBoxModel>();
            Colleges = new List<College>();
            Students = new List<Student>();
        }

        public int CourseId { get; set; }

        [Required]
        [RegularExpression("([A-Za-z])+( [A-Za-z]+)*", ErrorMessage = "Invalid Course Name")]
        [Display(Name ="Course Name")]
        public string CourseName {get;set;}

        [Display(Name ="No of Years")]
        [Required]
        [RegularExpression("^[1-9]{1}$", ErrorMessage = "Invalid No of Years")]
        public int NoOfYears{get;set;}

        [Display(Name = "Registered No of Semesters")]
        [RegularExpression("^[1-9][0-9]{1}$", ErrorMessage = "Invalid No of Years")]
        public int NoOfSemesters { get; set; }

        public virtual ICollection<Semester> Semesters { get; set; }
        public virtual ICollection<College> Colleges { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        [NotMapped]
        public List<CheckBoxModel> SemestersList { get; set; }
    }
}