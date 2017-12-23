namespace ScheduleReport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conversions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Type = c.Int(nullable: false),
                        FilePath = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        Comment = c.String(unicode: false),
                        UserId = c.String(unicode: false),
                        TimeEventDetailId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberUserId = c.String(unicode: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        CreatedUserId = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PeriodEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        Content = c.String(unicode: false),
                        DateStart = c.DateTime(nullable: false, precision: 0),
                        DateEnd = c.DateTime(nullable: false, precision: 0),
                        TimeStart = c.DateTime(nullable: false, precision: 0),
                        TimeEnd = c.DateTime(nullable: false, precision: 0),
                        CreatedUserId = c.String(unicode: false),
                        DaySelect = c.String(unicode: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        RoleId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.TimeEventDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TE_Id = c.Int(nullable: false),
                        UserId = c.String(unicode: false),
                        ReportStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        Content = c.String(unicode: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        TimeStart = c.DateTime(nullable: false, precision: 0),
                        TimeEnd = c.DateTime(nullable: false, precision: 0),
                        PE_Id = c.Int(nullable: false),
                        CreatedUserId = c.String(unicode: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(unicode: false),
                        Type = c.Int(nullable: false),
                        CreatedUserId = c.String(unicode: false),
                        Email = c.String(maxLength: 256, storeType: "nvarchar"),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 0),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ProviderKey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TimeEvents");
            DropTable("dbo.TimeEventDetails");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PeriodEvents");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupDetails");
            DropTable("dbo.Conversions");
        }
    }
}
