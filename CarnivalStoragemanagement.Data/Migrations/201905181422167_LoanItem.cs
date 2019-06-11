namespace CarnivalStoragemanagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "Item_Id", c => c.Int());
            CreateIndex("dbo.Loans", "Item_Id");
            AddForeignKey("dbo.Loans", "Item_Id", "dbo.Items", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Loans", "Item_Id", "dbo.Items");
            DropIndex("dbo.Loans", new[] { "Item_Id" });
            DropColumn("dbo.Loans", "Item_Id");
        }
    }
}
