namespace MedManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentUserAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Appointments", "UserId");
            AddForeignKey("dbo.Appointments", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "UserId" });
            DropColumn("dbo.Appointments", "UserId");
        }
    }
}
