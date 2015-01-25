namespace BackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RiderMembers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Riders", "CustomEmailConfirmationToken", c => c.String(maxLength: 6));
            AddColumn("dbo.Riders", "FirstName", c => c.String());
            AddColumn("dbo.Riders", "LastName", c => c.String());
            AddColumn("dbo.Riders", "Age", c => c.Int());
            AddColumn("dbo.Riders", "ZipCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Riders", "ZipCode");
            DropColumn("dbo.Riders", "Age");
            DropColumn("dbo.Riders", "LastName");
            DropColumn("dbo.Riders", "FirstName");
            DropColumn("dbo.Riders", "CustomEmailConfirmationToken");
        }
    }
}
