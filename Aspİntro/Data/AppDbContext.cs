using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderDetail> SliderDetails { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PostImage> PostImages{ get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ConsultingService> ConsultingServices { get; set; }
        public DbSet<About> Abouts { get; set; }

        public DbSet<AboutImage> AboutImages { get; set; }

        public DbSet<Feature> Features { get; set; }
        public DbSet<Advisor> Advisors { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder) 
        //{
        //    //modelBuilder.Entity<Post>().HasQueryFilter(m => !m.IsDeleted);
        //}

    }
}
