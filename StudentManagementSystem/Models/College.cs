using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class College
    {
        public College()
        {
            Students = new List<Student>();
            Courses = new List<Course>();
            CoursesList = new List<CheckBoxModel>();
        }
        public int CollegeId { get; set; }

        [Display(Name = "College Name")]
        public string CollegeName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        [NotMapped]
        public List<CheckBoxModel> CoursesList { get; set; }
    }
}