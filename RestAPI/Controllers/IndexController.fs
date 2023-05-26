namespace RestAPI.Controllers

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Localization
open Microsoft.Extensions.Logging
open RestAPI.Domains
open RestAPI.Localizations

[<ApiController; Route "[controller]"; Route "">]
type IndexController(logger: ILogger<IndexController>, locale: IStringLocalizer<Locale>) =
    inherit ControllerBase()

    [<HttpGet; AllowAnonymous>]
    member _.Index() =
        logger.LogDebug "IndexController----->GET.Index"
        let greetingMessage = locale["GreetingMessage"]
        ApiResult.SUCCESS(greetingMessage.Value)
