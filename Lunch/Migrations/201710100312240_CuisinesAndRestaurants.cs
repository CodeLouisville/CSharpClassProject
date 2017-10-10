namespace Lunch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CuisinesAndRestaurants : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cuisines",
                c => new
                    {
                        CuisineId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CuisineId);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        RestaurantId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CuisineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RestaurantId)
                .ForeignKey("dbo.Cuisines", t => t.CuisineId, cascadeDelete: true)
                .Index(t => t.CuisineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Restaurants", "CuisineId", "dbo.Cuisines");
            DropIndex("dbo.Restaurants", new[] { "CuisineId" });
            DropTable("dbo.Restaurants");
            DropTable("dbo.Cuisines");
        }
    }
}
