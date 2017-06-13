using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Invalid Name")]
        public string SubjectName { get; set; }

        [Required]
        [RegularExpression("^[1-9]{1}$",ErrorMessage ="Invalid Credit Score")]
        public int Credit { get; set; }

        public virtual ICollection<Semester> Semesters { get; set; }
    }
}