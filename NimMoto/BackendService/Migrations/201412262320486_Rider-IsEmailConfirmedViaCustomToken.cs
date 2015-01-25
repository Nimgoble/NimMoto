namespace BackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RiderIsEmailConfirmedViaCustomToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Riders", "IsEmailConfirmedViaCustomToken", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Riders", "IsEmailConfirmedViaCustomToken");
        }
    }
}
