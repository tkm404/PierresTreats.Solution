using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PierresTreats.Models
{
  public class PierresTreatsContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<PrimaryModel> PrimaryModel { get; set; }
    public DbSet<SecondaryModel> SecondaryModel { get; set; }

    public PierresTreatsContext(DbContextOptions options) : base(options) { }
  }
}
