using Authentication.Domain.Models;
using Marques.EFCore.SnakeCase;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);
        modelBuilder.ToSnakeCase();
        
        modelBuilder.HasSequence<long>("UserSequence").StartsAt(1).IncrementsBy(1);

        base.OnModelCreating(modelBuilder);
    }
}