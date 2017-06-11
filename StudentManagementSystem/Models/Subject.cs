using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Subject
    {
        public Subject()
        {
            Semesters = new List<Semester>();
        }

        public int SubjectId { get; set; }

        [Required]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Required]
        public int Credit { get; set; }

        public virtual ICollection<Semester> Semesters { get; set; }
    }
}