using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository;

public class RepositoryContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public RepositoryContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new TimeZoneConfiguration());
    }

    public DbSet<EmailTemplate>? EmailTemplate { get; set; }
    public DbSet<ApplicationMenu>? ApplicationMenu { get; set; }
    public DbSet<Clients>? Clients { get; set; }


}