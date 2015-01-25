namespace BackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetUsers", newName: "Riders");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Riders", "UserNameIndex");
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            AddColumn("dbo.AspNetUserRoles", "IdentityUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Riders", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUserClaims", "IdentityUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUserLogins", "IdentityUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Riders", "Email", c => c.String());
            AlterColumn("dbo.Riders", "UserName", c => c.String());
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String());
            CreateIndex("dbo.AspNetUserRoles", "IdentityUser_Id");
            CreateIndex("dbo.AspNetUserClaims", "IdentityUser_Id");
            CreateIndex("dbo.AspNetUserLogins", "IdentityUser_Id");
            AddForeignKey("dbo.AspNetUserClaims", "IdentityUser_Id", "dbo.Riders", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "IdentityUser_Id", "dbo.Riders", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "IdentityUser_Id", "dbo.Riders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "IdentityUser_Id", "dbo.Riders");
            DropForeignKey("dbo.AspNetUserLogins", "IdentityUser_Id", "dbo.Riders");
            DropForeignKey("dbo.AspNetUserClaims", "IdentityUser_Id", "dbo.Riders");
            DropIndex("dbo.AspNetUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "IdentityUser_Id" });
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Riders", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Riders", "Email", c => c.String(maxLength: 256));
            DropColumn("dbo.AspNetUserLogins", "IdentityUser_Id");
            DropColumn("dbo.AspNetUserClaims", "IdentityUser_Id");
            DropColumn("dbo.Riders", "Discriminator");
            DropColumn("dbo.AspNetUserRoles", "IdentityUser_Id");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.Riders", "UserName", unique: true, name: "UserNameIndex");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Riders", newName: "AspNetUsers");
        }
    }
}
