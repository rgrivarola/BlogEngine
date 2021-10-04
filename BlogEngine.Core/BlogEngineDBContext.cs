using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogEngine.Core
{
    public class BlogEngineDBContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Action> Actions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            optionsBuilder.UseSqlServer(Helpers.ReadAppSettings());
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostComment>().HasKey(x=> new { x.PostId, x.ItemId });

            modelBuilder
            .Entity<Role>()
            .Property(e => e.Name)
            .HasConversion(
                v => v.ToString(),
                v => (eRoles) Enum.Parse(typeof(eRoles), v));

            modelBuilder
            .Entity<Action>()
            .Property(e => e.Name)
            .HasConversion(
                v => v.ToString(),
                v => (eActions)Enum.Parse(typeof(eActions), v));

        }

    }
}
