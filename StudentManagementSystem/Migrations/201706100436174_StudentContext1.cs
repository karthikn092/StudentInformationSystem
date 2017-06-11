namespace StudentManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentContext1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "Courses_CourseId", c => c.Int());
            CreateIndex("dbo.Students", "Courses_CourseId");
            AddForeignKey("dbo.Students", "Courses_CourseId", "dbo.Courses", "CourseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Courses_CourseId", "dbo.Courses");
            DropIndex("dbo.Students", new[] { "Courses_CourseId" });
            DropColumn("dbo.Students", "Courses_CourseId");
            DropColumn("dbo.Students", "Age");
        }
    }
}
