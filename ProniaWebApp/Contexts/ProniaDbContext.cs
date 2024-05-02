using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Models;
using System;

namespace ProniaWebApp.Contexts
{
    public class ProniaDbContext : IdentityDbContext<AppUser>
    {
        public ProniaDbContext(DbContextOptions<ProniaDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<Shipping > Shippers { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Setting> Settings { get; set; } = null!;
    }
}
