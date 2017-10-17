namespace Lunch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FoodPreferences : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FoodPreferences",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        CuisineId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.CuisineId })
                .ForeignKey("dbo.Cuisines", t => t.CuisineId, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.CuisineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FoodPreferences", "PersonId", "dbo.People");
            DropForeignKey("dbo.FoodPreferences", "CuisineId", "dbo.Cuisines");
            DropIndex("dbo.FoodPreferences", new[] { "CuisineId" });
            DropIndex("dbo.FoodPreferences", new[] { "PersonId" });
            DropTable("dbo.FoodPreferences");
        }
    }
}
