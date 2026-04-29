using Microsoft.EntityFrameworkCore;
using CafeApi.Models;
using System.Collections.Generic;

namespace CafeApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reserva> Reservas { get; set; }
    }
}