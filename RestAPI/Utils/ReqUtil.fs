namespace RestAPI.Utils

open System.Net.Http.Headers
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc.Filters
open Microsoft.Net.Http.Headers

type ReqUtil =
    static member ContainsBearHeader(context: HttpContext) : bool =
        context.Request.Headers.ContainsKey HeaderNames.Authorization

    static member IsLoginRequest(context: HttpContext) : bool =
        context.Request.Path.Value.ToLower().Contains "auth/login"
