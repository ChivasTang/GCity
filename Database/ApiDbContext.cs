using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Users;

namespace Database;

public class ApiDbContext : IdentityDbContext<IdentityUser>
{

    public DbSet<UserProfile> UserProfiles;
    public DbSet<ReqLog> ReqLogs;
    public ApiDbContext(DbContextOptions<ApiDbContext> options, DbSet<UserProfile> userProfiles, DbSet<ReqLog> reqLogs) : base(options)
    {
        UserProfiles = userProfiles;
        ReqLogs = reqLogs;
        // ¥/usr/local/share/dotnet/dotnet ef migrations add --project Database/Database.csproj --startup-project RestAPI/RestAPI.fsproj --context Database.ApiDbContext --configuration Debug initialDBWithUserProfile --output-dir Migrations
        // ¥/usr/local/share/dotnet/dotnet ef database update --project Database/Database.csproj --startup-project RestAPI/RestAPI.fsproj --context Database.ApiDbContext --configuration Debug 20230529140916_InitialDBWithUserProfile
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var entityTypes = builder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
            builder.Entity(entityType.ClrType).ToTable(entityType.GetTableName()!.Replace("AspNet", ""));
    }
}