namespace RestAPI.Services

open System
open Microsoft.AspNetCore.Identity

type IJwtTokenService =
    abstract member Generate: identityUser: IdentityUser -> string
