namespace section3._3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Sale",
                c => new
                    {
                        SaleId = c.Int(nullable: false, identity: true),
                        Total = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaleId)
                .ForeignKey("dbo.Employee", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sale", "EmployeeId", "dbo.Employee");
            DropIndex("dbo.Sale", new[] { "EmployeeId" });
            DropTable("dbo.Sale");
            DropTable("dbo.Employee");
        }
    }
}
