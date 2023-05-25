namespace RestAPI.Controllers

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Localization
open Microsoft.Extensions.Logging
open RestAPI.Domains

[<ApiController;Route "[controller]"; Route "">]
type IndexController(logger: ILogger<IndexController>, locale: IStringLocalizer<IndexController>) =
    inherit ControllerBase()
  
    [<HttpGet; AllowAnonymous>]
    member _.Index() =
        logger.LogDebug "IndexController----->GET.Index"
        let greetingMessage = locale["GreetingMessage"].Value
        ApiResult.SUCCESS($"this is index page...{greetingMessage}")
