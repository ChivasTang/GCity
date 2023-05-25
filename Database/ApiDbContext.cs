using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class ApiDbContext : IdentityDbContext<IdentityUser>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var entityTypes = builder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
            builder.Entity(entityType.ClrType).ToTable(entityType.GetTableName()!.Replace("AspNet", ""));
    }
}