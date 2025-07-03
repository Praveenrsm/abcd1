using Microsoft.EntityFrameworkCore;
using razorpractice.Models;
using System;

namespace razorpractice.Data
{
    public class AppDbContaxt : DbContext
    {
        public AppDbContaxt(DbContextOptions<AppDbContaxt> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer("Server=IN-6YYZFY3;Database=RazorPages;User Id=sa;Password=Praveen0077$$$$;TrustServerCertificate=True;");
        }
    }
}
