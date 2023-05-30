﻿namespace RestAPI.Filters

open System
open System.IdentityModel.Tokens.Jwt
open Database
open Microsoft.AspNetCore.Mvc.Filters
open RestAPI.Services
open RestAPI.Utils

type JwtTokenFilter(_reqLogService: IReqLogService) =
    interface IActionFilter with
        //经过身份认证之后 ，在进入请求路由前对context进行过滤
        member this.OnActionExecuting(context: ActionExecutingContext) : unit =
            printfn "JwtTokenFilter.OnActionExecuting"

        //在执行完成路由请求之后 ，在为用户返回结果前对context进行过滤
        member this.OnActionExecuted(context: ActionExecutedContext) : unit =
            printfn "JwtTokenFilter.OnActionExecuted"
            //从context中获得Url
            let pathUrl = context.HttpContext.Request.Path.Value
            let id = Guid.NewGuid()
            //检查头中是否有Bearer
            let containsBearHeader = ReqUtil.ContainsBearHeader(context.HttpContext)

            if containsBearHeader then
                let tokenString = ReqUtil.GetJwtToken(context.HttpContext).Trim()
                let securityToken = JwtSecurityTokenHandler().ReadJwtToken tokenString

                _reqLogService.SaveOne(
                    { Id = id
                      Url = pathUrl
                      UserId = securityToken.Subject
                      CreateTime = DateTime.Now }
                )
            else if ReqUtil.IsNonAuthorizedUrl(context.HttpContext) then
                _reqLogService.SaveOne
                    { Id = id
                      Url = pathUrl
                      UserId = null
                      CreateTime = DateTime.Now }
