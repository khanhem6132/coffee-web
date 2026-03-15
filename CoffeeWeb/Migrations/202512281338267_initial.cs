namespace CoffeeWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "COFFEEWEB.ABOUTUS",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Title = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(maxLength: 250),
                        Description = c.String(),
                        Detail = c.String(),
                        Image = c.String(maxLength: 250),
                        CategoryId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        IsActive = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CreateDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("COFFEEWEB.CATEGORY", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "COFFEEWEB.CATEGORY",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Title = c.String(nullable: false, maxLength: 150),
                        Alias = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "COFFEEWEB.NEWS",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Title = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(),
                        Description = c.String(),
                        Detail = c.String(),
                        Image = c.String(),
                        CategoryId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        IsActive = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CreateDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("COFFEEWEB.CATEGORY", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "COFFEEWEB.ORDERDETAIL",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        OrderId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ProductId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Price = c.Decimal(nullable: false, precision: 19, scale: 0),
                        Quantity = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("COFFEEWEB.ORDERS", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("COFFEEWEB.PRODUCT", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "COFFEEWEB.ORDERS",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Code = c.String(nullable: false),
                        CustomerName = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Email = c.String(),
                        TotalAmount = c.Decimal(nullable: false, precision: 19, scale: 0),
                        TypePayment = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Status = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CreateDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "COFFEEWEB.PRODUCT",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Title = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(),
                        ProductCode = c.String(maxLength: 50),
                        Description = c.String(),
                        Detail = c.String(),
                        Image = c.String(maxLength: 500),
                        OriginalPrice = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Price = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PriceSale = c.Decimal(precision: 10, scale: 0),
                        Quantity = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ViewCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                        IsHome = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IsSale = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IsHot = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IsFeature = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IsActive = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ProductCategoryId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CreateDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("COFFEEWEB.PRODUCTCATEGORY", t => t.ProductCategoryId, cascadeDelete: true)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "COFFEEWEB.PRODUCTCATEGORY",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Title = c.String(nullable: false, maxLength: 150),
                        Alias = c.String(nullable: false, maxLength: 150),
                        Description = c.String(),
                        Icon = c.String(maxLength: 250),
                        CreateDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "COFFEEWEB.PRODUCTIMAGE",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ProductId = c.Decimal(precision: 10, scale: 0),
                        Image = c.String(nullable: false),
                        IsDefault = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("COFFEEWEB.PRODUCT", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "COFFEEWEB.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "COFFEEWEB.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("COFFEEWEB.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("COFFEEWEB.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "COFFEEWEB.STATISTICAL",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0),
                        D_TIME = c.DateTime(nullable: false),
                        PERSONVISIT = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "COFFEEWEB.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TwoFactorEnabled = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Decimal(nullable: false, precision: 1, scale: 0),
                        AccessFailedCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "COFFEEWEB.AspNetUserClaims",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("COFFEEWEB.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "COFFEEWEB.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("COFFEEWEB.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("COFFEEWEB.AspNetUserRoles", "UserId", "COFFEEWEB.AspNetUsers");
            DropForeignKey("COFFEEWEB.AspNetUserLogins", "UserId", "COFFEEWEB.AspNetUsers");
            DropForeignKey("COFFEEWEB.AspNetUserClaims", "UserId", "COFFEEWEB.AspNetUsers");
            DropForeignKey("COFFEEWEB.AspNetUserRoles", "RoleId", "COFFEEWEB.AspNetRoles");
            DropForeignKey("COFFEEWEB.PRODUCTIMAGE", "ProductId", "COFFEEWEB.PRODUCT");
            DropForeignKey("COFFEEWEB.PRODUCT", "ProductCategoryId", "COFFEEWEB.PRODUCTCATEGORY");
            DropForeignKey("COFFEEWEB.ORDERDETAIL", "ProductId", "COFFEEWEB.PRODUCT");
            DropForeignKey("COFFEEWEB.ORDERDETAIL", "OrderId", "COFFEEWEB.ORDERS");
            DropForeignKey("COFFEEWEB.NEWS", "CategoryId", "COFFEEWEB.CATEGORY");
            DropForeignKey("COFFEEWEB.ABOUTUS", "CategoryId", "COFFEEWEB.CATEGORY");
            DropIndex("COFFEEWEB.AspNetUserLogins", new[] { "UserId" });
            DropIndex("COFFEEWEB.AspNetUserClaims", new[] { "UserId" });
            DropIndex("COFFEEWEB.AspNetUsers", "UserNameIndex");
            DropIndex("COFFEEWEB.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("COFFEEWEB.AspNetUserRoles", new[] { "UserId" });
            DropIndex("COFFEEWEB.AspNetRoles", "RoleNameIndex");
            DropIndex("COFFEEWEB.PRODUCTIMAGE", new[] { "ProductId" });
            DropIndex("COFFEEWEB.PRODUCT", new[] { "ProductCategoryId" });
            DropIndex("COFFEEWEB.ORDERDETAIL", new[] { "ProductId" });
            DropIndex("COFFEEWEB.ORDERDETAIL", new[] { "OrderId" });
            DropIndex("COFFEEWEB.NEWS", new[] { "CategoryId" });
            DropIndex("COFFEEWEB.ABOUTUS", new[] { "CategoryId" });
            DropTable("COFFEEWEB.AspNetUserLogins");
            DropTable("COFFEEWEB.AspNetUserClaims");
            DropTable("COFFEEWEB.AspNetUsers");
            DropTable("COFFEEWEB.STATISTICAL");
            DropTable("COFFEEWEB.AspNetUserRoles");
            DropTable("COFFEEWEB.AspNetRoles");
            DropTable("COFFEEWEB.PRODUCTIMAGE");
            DropTable("COFFEEWEB.PRODUCTCATEGORY");
            DropTable("COFFEEWEB.PRODUCT");
            DropTable("COFFEEWEB.ORDERS");
            DropTable("COFFEEWEB.ORDERDETAIL");
            DropTable("COFFEEWEB.NEWS");
            DropTable("COFFEEWEB.CATEGORY");
            DropTable("COFFEEWEB.ABOUTUS");
        }
    }
}
