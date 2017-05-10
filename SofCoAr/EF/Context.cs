////using SofCoAr.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SofCoAr.EF
//{
//    public class Context: DbContext
//    {
//        public Context() : base("ExampleDB")
//        {

//        }

//        static Context()
//        {
//            Database.SetInitializer(new ContextInitializer());
//        }

//        //public DbSet<Province> Provinces { get; set; }
//        //public DbSet<Customer> Customers { get; set; }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            //modelBuilder.Properties()
//            //            .Where(p => p.Name == "Key")
//            //            .Configure(p => p.IsKey());

//            //modelBuilder.Entity<Customer>()
//            //    .HasRequired(f => f.Province)
//            //    .WithRequiredDependent()
//            //    .WillCascadeOnDelete(false);

//        }
//    }
//}
