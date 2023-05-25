namespace RestAPI

open Database
open Microsoft.EntityFrameworkCore
open Pomelo.EntityFrameworkCore.MySql.Infrastructure
open RestAPI.Filters
open Microsoft.AspNetCore.Identity
open System.Text
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.IdentityModel.Tokens
open RestAPI.Services

#nowarn "20"

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        // Settings
        let configurationBuilder = ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).AddEnvironmentVariables().Build()

        let builder = WebApplication.CreateBuilder(args)

        // Add Localization
        builder.Services.AddLocalization(fun options -> options.ResourcesPath <- "Resources")

        // Add DataSource
        //let msConnectionString = configurationBuilder.GetConnectionString "MSSQLSERVER"
        let myConnectionString = configurationBuilder.GetConnectionString "MYSQL"
        let version = Version "8.0.33"
        let serverVersion = ServerVersion.Create(version, ServerType.MySql)

        builder.Services.AddDbContext<ApiDbContext>(fun (options: DbContextOptionsBuilder) ->
            options.UseMySql(myConnectionString, serverVersion) |> ignore)
        |> ignore

        // Add Identity Framework Dependency
        builder.Services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApiDbContext>()

        // Add Authentication
        let issuer = configurationBuilder.GetValue("Authentication:Issuer").ToString()
        let audience = configurationBuilder.GetValue("Authentication:Audience").ToString()
        let secretKey = configurationBuilder.GetValue("Authentication:SecretKey").ToString()
        let secretKeyBytes: byte[] = Encoding.UTF8.GetBytes(secretKey)

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(fun options ->
                options.TokenValidationParameters <-
                    TokenValidationParameters(
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = SymmetricSecurityKey(secretKeyBytes)
                    ))
        // Add Service
        builder.Services.AddTransient<IJwtTokenService, JwtTokenService>()

        // Add Controller
        builder.Services.AddControllers(fun options -> options.Filters.Add(JwtTokenFilter()))
        builder.Services.AddHttpContextAccessor()

        let app = builder.Build()
        
        app.UseHttpsRedirection()
        app.UseRouting()
        app.UseAuthentication()
        app.UseAuthorization()

        app.UseAuthorization()
        app.MapControllers()

        let supportedLocales = [|"en";"ja";"ja-jp";"zh";"zh-hans";|]
        let localizationOptions = RequestLocalizationOptions()
        localizationOptions.AddSupportedCultures supportedLocales
        localizationOptions.AddSupportedUICultures supportedLocales
        localizationOptions.ApplyCurrentCultureToResponseHeaders <- true

        app.UseRequestLocalization(localizationOptions)
        

        app.Run()

        exitCode
