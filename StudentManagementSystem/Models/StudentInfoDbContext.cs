using System.Data.Entity;

namespace StudentManagementSystem.Models
{
    public class StudentInfoDbContext : DbContext
    {
        public StudentInfoDbContext() : base("StudentContext")
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<College> Colleges { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Semester> Semesters { get; set; }
    }
}