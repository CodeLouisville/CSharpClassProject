namespace Lunch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonNameRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.People", "FirstName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "FirstName", c => c.String());
            AlterColumn("dbo.People", "LastName", c => c.String());
        }
    }
}
