using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Repository
{
   public class DbClass:DbContext
    {
        public DbClass(DbContextOptions<DbClass> db) : base(db) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\pragrammalar\dasturlar\c#\Asp.net core\Yahyo\Repository\CarsShop.mdf;Integrated Security=True;Connect Timeout=30");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>()
                .HasOne(p => p.car)
                .WithMany(p => p.Photos)
                .HasForeignKey(i => i.CarId);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Car> Car { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }

}
