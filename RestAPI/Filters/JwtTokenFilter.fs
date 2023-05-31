namespace RestAPI.Filters

open System
open System.IdentityModel.Tokens.Jwt
open Microsoft.AspNetCore.Mvc.Filters
open RestAPI.Services
open RestAPI.Utils

type JwtTokenFilter(_reqLogService: IReqLogService) =
    member this.ActionExecuting(context: ActionExecutingContext) : unit =
        //记录到访时间
        context.HttpContext.Request.Headers.Add(
            ReqUtil.InTimeHeader,
            DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds().ToString()
        )

    member this.ActionExecuted(context: ActionExecutedContext) : unit =
        //从context中获得Url
        let inTimeStr = context.HttpContext.Request.Headers[ReqUtil.InTimeHeader].ToString()
        let inTime = Int64.Parse(inTimeStr)
        let pathUrl = context.HttpContext.Request.Path.Value
        let id = Guid.NewGuid()
        //检查头中是否有Bearer
        let containsBearHeader = ReqUtil.ContainsBearHeader(context.HttpContext)

        if containsBearHeader then
            let tokenString = ReqUtil.GetJwtToken(context.HttpContext).Trim()
            let securityToken = JwtSecurityTokenHandler().ReadJwtToken tokenString
            let outTime = DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()
            let consuming = outTime - inTime

            _reqLogService.SaveOneAsync
                { Id = id
                  Url = pathUrl
                  UserId = securityToken.Subject
                  InTime = inTime
                  OutTime = outTime
                  Consuming = consuming }

        else
            let outTime = DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()
            let consuming = outTime - inTime

            _reqLogService.SaveOneAsync
                { Id = id
                  Url = pathUrl
                  UserId = null
                  InTime = inTime
                  OutTime = outTime
                  Consuming = consuming }

    interface IActionFilter with
        //经过身份认证之后 ，在进入请求路由前对context进行过滤
        member this.OnActionExecuting(context: ActionExecutingContext) : unit =
            printfn "JwtTokenFilter.OnActionExecuting"
            this.ActionExecuting(context)

        //在执行完成路由请求之后 ，在为用户返回结果前对context进行过滤
        member this.OnActionExecuted(context: ActionExecutedContext) : unit =
            printfn "JwtTokenFilter.OnActionExecuted"
            this.ActionExecuted(context)
