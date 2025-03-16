using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistance.Configurations;

namespace Persistance;

public partial class UserManagementContext : DbContext
{
    public UserManagementContext()
    {
    }

    public UserManagementContext(DbContextOptions<UserManagementContext> options)
        :base(options)
    {
    }
    
    public virtual DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("[DeletedAt] IS NULL");
        
        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.DeletedAt == null);
        
        modelBuilder.Entity<User>()
            .Property(u => u.RegistrationTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        modelBuilder.Entity<User>()
            .Property(u => u.Status)
            .HasDefaultValue("active");
        
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}