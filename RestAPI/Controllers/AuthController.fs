namespace RestAPI.Controllers

open System.Collections.Generic
open System.Net.Http.Headers
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Identity
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.Net.Http.Headers
open RestAPI.Services
open RestAPI.Domains


[<ApiController;Route "[controller]">]
type AuthController
    (
        logger: ILogger<AuthController>,
        _jwtTokenService: IJwtTokenService,
        _userManager: UserManager<IdentityUser>,
        _signinManager: SignInManager<IdentityUser>,
        _httpContextAccessor: IHttpContextAccessor
    ) =
    inherit ControllerBase()

    (*Login*)
    [<HttpPost "Login"; AllowAnonymous>]
    member _.Login([<FromBody>] _loginReq: LoginReq) =
        logger.LogDebug "AuthController----->GET.Login"

        let signinResult =
            _signinManager.PasswordSignInAsync(_loginReq.Username, _loginReq.Password, false, false)

        if signinResult.Result.Succeeded then
            let identityUser = _userManager.FindByNameAsync(_loginReq.Username).Result
            let token = _jwtTokenService.Generate identityUser

            let loginRes =
                { Username = _loginReq.Username
                  Token = $"{HeaderNames.Authorization} header added." }

            _httpContextAccessor.HttpContext.Response.Headers.Add(
                HeaderNames.Authorization,
                $"{JwtBearerDefaults.AuthenticationScheme} {token}"
            )

            ApiResult.SUCCESS(loginRes)
        else
            let errors = List<ApiError>()

            errors.Add(
                { Name = "BadCredential"
                  Description = "Username or Password is not correct." }
            )

            ApiResult.FAIL(8000, errors)

    (*Register*)
    [<HttpPost "Register"; AllowAnonymous>]
    member _.Register([<FromBody>] _registerReq: RegisterReq) =
        logger.LogDebug "AuthController----->GET.Register"
        let identityUser = IdentityUser(UserName = _registerReq.Username)
        let rawPassword = _registerReq.Password
        let Confirm = _registerReq.Confirm

        if rawPassword <> Confirm then
            let errors = List<ApiError>()

            errors.Add(
                { Name = "PasswordAndConfirmIsNotSame"
                  Description =
                    "Password should be same to confirm password, please check your password and confirm one." }
            )

            ApiResult.FAIL(8000, errors)
        else
            let identityUserResult =
                _userManager.CreateAsync(identityUser, _registerReq.Password)

            if identityUserResult.Result.Succeeded then
                let registerRes = { Username = identityUser.UserName }
                ApiResult.SUCCESS(registerRes)
            else
                let errors: IEnumerable<IdentityError> = identityUserResult.Result.Errors
                ApiResult.FAIL(9000, errors)
