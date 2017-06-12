namespace StudentManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentContext3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.Students", "UpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "UpdatedDate");
            DropColumn("dbo.Students", "CreatedDate");
        }
    }
}
