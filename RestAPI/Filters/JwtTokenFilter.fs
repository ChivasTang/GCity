namespace RestAPI.Filters

open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Mvc.Filters
open Microsoft.Net.Http.Headers
open RestAPI.Services
open RestAPI.Utils

type JwtTokenFilter() =
    interface IActionFilter with
        //经过身份认证之后 ，在进入请求路由前对context进行过滤
        member this.OnActionExecuting(context: ActionExecutingContext) : unit =
            printfn "JwtTokenFilter.OnActionExecuting"
        //在执行完成路由请求之后 ，在为用户返回结果前对context进行过滤
        member this.OnActionExecuted(context: ActionExecutedContext) : unit =
            printfn "JwtTokenFilter.OnActionExecuted"

            if ReqUtil.ContainsBearHeader context.HttpContext then
                //TODO 如果Token将要过期，可以更新Token
                printfn "JwtTokenFilter.OnActionExecuted ContainsBearHeader"
