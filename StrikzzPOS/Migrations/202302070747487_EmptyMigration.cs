namespace StrikzzPOS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmptyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        FK_PaymentTypeId = c.Int(nullable: false),
                        FK_CustomerId = c.Int(nullable: false),
                        OrderNumber = c.String(),
                        OrderDate = c.DateTime(),
                        OrderCancelDate = c.DateTime(),
                        FinalTotal = c.Double(nullable: false),
                        OrderStatus = c.String(),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailId = c.Int(nullable: false, identity: true),
                        FK_OrderId = c.Int(nullable: false),
                        FK_ProductId = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDetailId);
            
            CreateTable(
                "dbo.PaymentTypes",
                c => new
                    {
                        pk_PaymentTypeId = c.Int(nullable: false, identity: true),
                        PaymentType = c.String(),
                    })
                .PrimaryKey(t => t.pk_PaymentTypeId);
            
            CreateTable(
                "dbo.UserProduct",
                c => new
                    {
                        UserProductId = c.Int(nullable: false, identity: true),
                        fk_productId = c.Int(nullable: false),
                        fk_UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserProductId);
            
            AddColumn("dbo.CustomerMsts", "Email", c => c.String());
            AddColumn("dbo.CustomerMsts", "FirstVisit", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerMsts", "LastVisit", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerMsts", "TotalVisits", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerMsts", "TotalSpent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CustomerMsts", "PointBalance", c => c.Long(nullable: false));
            AlterColumn("dbo.CustomerMsts", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.CustomerMsts", "MobNo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerMsts", "MobNo", c => c.String());
            AlterColumn("dbo.CustomerMsts", "Name", c => c.String());
            DropColumn("dbo.CustomerMsts", "PointBalance");
            DropColumn("dbo.CustomerMsts", "TotalSpent");
            DropColumn("dbo.CustomerMsts", "TotalVisits");
            DropColumn("dbo.CustomerMsts", "LastVisit");
            DropColumn("dbo.CustomerMsts", "FirstVisit");
            DropColumn("dbo.CustomerMsts", "Email");
            DropTable("dbo.UserProduct");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
        }
    }
}
