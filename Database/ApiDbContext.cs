using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class ApiDbContext : IdentityDbContext<IdentityUser>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        //migrations add --project Database/Database.csproj --startup-project RestAPI/RestAPI.fsproj --context Database.ApiDbContext --configuration Debug InitialDB --output-dir Migrations
        //database update --project Database/Database.csproj --startup-project RestAPI/RestAPI.fsproj --context Database.ApiDbContext --configuration Debug 20230521004128_InitialDB
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var entityTypes = builder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
            builder.Entity(entityType.ClrType).ToTable(entityType.GetTableName()!.Replace("AspNet", ""));
    }
}