using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

using BackendService.Models;

namespace BackendService.DAL
{
    public class NimMotoContext : IdentityDbContext<Rider>
    {
        public NimMotoContext() : base("name=NimMotoContext", throwIfV1Schema: false)
        {
        }

        public static NimMotoContext Create()
        {
            return new NimMotoContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Change the name of the table to be Riders instead of AspNetUsers
            modelBuilder.Entity<IdentityUser>().ToTable("Riders");
            modelBuilder.Entity<Rider>().ToTable("Riders");
        }

        //public DbSet<Rider> Riders { get; set; }
    }
}
