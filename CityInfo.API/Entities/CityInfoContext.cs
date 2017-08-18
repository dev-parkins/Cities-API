using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        public CityInfoContext(DbContextOptions<CityInfoContext> opts) : base(opts)
        {
            Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optsBuilder)
        //{
        //    optsBuilder.UseSqlServer("connectionString");

        //    base.OnConfiguring(optsBuilder);
        //}
    }
}
