namespace RestAPI.Utils

open Microsoft.AspNetCore.Http
open Microsoft.Net.Http.Headers

type ReqUtil =

    static member NonAuthorizedUrls: Set<string> =
        Set.empty
            .Add("/Auth/Login")
            .Add("/Auth/Register")
            .Add("/Index")
            .Add("/")
            .Add("/Token")

    static member ContainsBearHeader(context: HttpContext) : bool =
        context.Request.Headers.ContainsKey(HeaderNames.Authorization)


    static member IsLoginRequest(context: HttpContext) : bool =
        context.Request.Path.Value.ToLower().Contains "auth/login"

    static member IsNonAuthorizedUrl(context: HttpContext) : bool =
        let reqUrl = context.Request.Path.Value
        ReqUtil.NonAuthorizedUrls.Contains reqUrl

    static member GetJwtToken(context: HttpContext) : string =
        let authString = context.Request.Headers[HeaderNames.Authorization].ToString()

        if authString.Length > 6 then
            authString.Substring 6
        else
            null
