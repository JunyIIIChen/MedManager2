namespace MedManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClinicId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinics", t => t.ClinicId, cascadeDelete: true)
                .Index(t => t.ClinicId);
            
            CreateTable(
                "dbo.Clinics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClinicName = c.String(nullable: false),
                        ClinicAddress = c.String(nullable: false),
                        ClinicPhone = c.Int(nullable: false),
                        ClinicDescription = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "ClinicId", "dbo.Clinics");
            DropIndex("dbo.Appointments", new[] { "ClinicId" });
            DropTable("dbo.Clinics");
            DropTable("dbo.Appointments");
        }
    }
}
