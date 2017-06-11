using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Semester
    {
        public Semester()
        {
            Subjects = new List<Subject>();
            Course = new List<Course>();
            Students = new List<Student>();
            SubjectsList = new List<CheckBoxModel>();
        }
        public int SemesterId { get; set; }

        [Display(Name ="Semester Name")]
        public string SemesterName { get; set; }

        [Display(Name = "Total Credits")]
        public int TotalCredits { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        public virtual ICollection<Course> Course { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        [NotMapped]
        public List<CheckBoxModel> SubjectsList { get; set; }
    }
}