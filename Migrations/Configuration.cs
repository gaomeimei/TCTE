namespace TCTE.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TCTE.Models;
    internal sealed class Configuration : DbMigrationsConfiguration<TCTE.Models.TCTEContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TCTE.Models.TCTEContext";
        }

        protected override void Seed(TCTE.Models.TCTEContext context)
        {
           
        }
    }
}
