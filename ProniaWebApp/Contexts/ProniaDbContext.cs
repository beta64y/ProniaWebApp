using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Models;
using System;

namespace ProniaWebApp.Contexts
{
    public class ProniaDbContext : DbContext
    {
        public ProniaDbContext(DbContextOptions<ProniaDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; } = null!;
    }
}
