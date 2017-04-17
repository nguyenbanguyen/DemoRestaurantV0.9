namespace DemoRestaurant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3302017 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cart",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        ProductId = c.Int(nullable: false),
                        ProductQuantity = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false),
                        Description = c.String(),
                        CategoryID = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                        MaximunQuantity = c.Int(nullable: false),
                        TotalSold = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        ParentCategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        OrderDetailId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductQuantity = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Discount = c.Int(nullable: false),
                        TotalPrice = c.Int(nullable: false),
                        ShippingAddress = c.String(nullable: false),
                        ShippingDate = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        PaymentInfo = c.String(),
                        Customer_CustomerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Customer", t => t.Customer_CustomerId)
                .Index(t => t.Customer_CustomerId);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerId = c.String(nullable: false, maxLength: 128),
                        CustomerName = c.String(nullable: false),
                        CustomerPhone = c.String(maxLength: 11),
                        ShippingAddress = c.String(nullable: false),
                        CustomerType = c.String(),
                        Account = c.Double(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cart", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Order", "Customer_CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Product", "CategoryID", "dbo.Category");
            DropIndex("dbo.Order", new[] { "Customer_CustomerId" });
            DropIndex("dbo.OrderDetail", new[] { "ProductId" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.Product", new[] { "CategoryID" });
            DropIndex("dbo.Cart", new[] { "ProductId" });
            DropTable("dbo.Customer");
            DropTable("dbo.Order");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Category");
            DropTable("dbo.Product");
            DropTable("dbo.Cart");
        }
    }
}
