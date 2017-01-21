using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTGWatcher.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using Newtonsoft.Json;

namespace MTGWatcher.DAL
{
    public class MTGWatcherContext : DbContext
    {
        public MTGWatcherContext():base("MTGWatcherContext")
        {

        }

        public DbSet <Card> Cards { get; set; }
        public DbSet<SetModel> Sets { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }

    public class  CardInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MTGWatcherContext>
    {
        protected override void Seed(MTGWatcherContext context)
        {
            //TODO iniatilize table Data
        }
    }


}