namespace StudentManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentContext2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Colleges", "CollegeName", c => c.String(nullable: false));
            AlterColumn("dbo.Colleges", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Colleges", "State", c => c.String(nullable: false));
            AlterColumn("dbo.Courses", "CourseName", c => c.String(nullable: false));
            AlterColumn("dbo.Semesters", "SemesterName", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "StudentName", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Dob", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "Dob", c => c.DateTime());
            AlterColumn("dbo.Students", "StudentName", c => c.String());
            AlterColumn("dbo.Semesters", "SemesterName", c => c.String());
            AlterColumn("dbo.Courses", "CourseName", c => c.String());
            AlterColumn("dbo.Colleges", "State", c => c.String());
            AlterColumn("dbo.Colleges", "City", c => c.String());
            AlterColumn("dbo.Colleges", "CollegeName", c => c.String());
        }
    }
}
