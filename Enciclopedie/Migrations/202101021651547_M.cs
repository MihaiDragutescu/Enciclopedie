namespace Enciclopedie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Phone");
        }
    }
}