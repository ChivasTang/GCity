namespace RestAPI.Services

open System.Collections.Generic
open Microsoft.AspNetCore.Identity
open Microsoft.Extensions.Configuration
open Microsoft.IdentityModel.Tokens
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open System
open System.Text

type JwtTokenService(_configuration: IConfiguration) =
    let issuer = _configuration["Authentication:Issuer"]
    let audience = _configuration["Authentication:Audience"]
    let secretKey = _configuration["Authentication:SecretKey"]
    let securityAlgorithms = SecurityAlgorithms.HmacSha256

    interface IJwtTokenService with
        member this.Generate(identityUser: IdentityUser) : string =
            let claims = List<Claim>()
            claims.Add(Claim(JwtRegisteredClaimNames.Sub, identityUser.Id))
            //let roleNames = _userManager.GetRolesAsync(identityUser).Result
            //for roleName in roleNames do
            //    claims.Add(Claim(ClaimTypes.Role, roleName))
            let secretBytes = Encoding.UTF8.GetBytes secretKey
            let signingKey = SymmetricSecurityKey secretBytes
            let signingCredentials = SigningCredentials(signingKey, securityAlgorithms)
            let nowDate = DateTime.UtcNow

            let token =
                JwtSecurityToken(
                    issuer = issuer,
                    audience = audience,
                    claims = claims,
                    notBefore = Nullable<DateTime>(nowDate),
                    expires = Nullable<DateTime>(nowDate.AddDays 1),
                    signingCredentials = signingCredentials
                )

            JwtSecurityTokenHandler().WriteToken token
