﻿namespace RestAPI.Controllers

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Authorization
open Models.Users
open RestAPI.Domains

[<ApiController; Route "[controller]">]
type WeatherForecastController(logger: ILogger<WeatherForecastController>) =
    inherit ControllerBase()

    let summaries =
        [| "Freezing"
           "Bracing"
           "Chilly"
           "Cool"
           "Mild"
           "Warm"
           "Balmy"
           "Hot"
           "Sweltering"
           "Scorching" |]

    [<HttpGet; Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)>]
    member _.WeatherForecast() =
        logger.LogDebug "WeatherForecastController----->GET.WeatherForecast"
        let rng = Random()
        let weathers = List<WeatherForecast>()

        for index in 0..4 do
            let weather: WeatherForecast =
                { Date = DateTime.Now.AddDays(float index)
                  TemperatureC = rng.Next(-20, 55)
                  Summary = summaries[rng.Next(summaries.Length)] }

            weathers.Add(weather)

        ApiResult.SUCCESS(weathers)
