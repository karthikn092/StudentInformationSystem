namespace StudentManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentContext : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CourseSemesters", newName: "SemesterCourses");
            DropPrimaryKey("dbo.SemesterCourses");
            CreateTable(
                "dbo.CourseColleges",
                c => new
                    {
                        Course_CourseId = c.Int(nullable: false),
                        College_CollegeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Course_CourseId, t.College_CollegeId })
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Colleges", t => t.College_CollegeId, cascadeDelete: true)
                .Index(t => t.Course_CourseId)
                .Index(t => t.College_CollegeId);
            
            AddPrimaryKey("dbo.SemesterCourses", new[] { "Semester_SemesterId", "Course_CourseId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseColleges", "College_CollegeId", "dbo.Colleges");
            DropForeignKey("dbo.CourseColleges", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.CourseColleges", new[] { "College_CollegeId" });
            DropIndex("dbo.CourseColleges", new[] { "Course_CourseId" });
            DropPrimaryKey("dbo.SemesterCourses");
            DropTable("dbo.CourseColleges");
            AddPrimaryKey("dbo.SemesterCourses", new[] { "Course_CourseId", "Semester_SemesterId" });
            RenameTable(name: "dbo.SemesterCourses", newName: "CourseSemesters");
        }
    }
}
