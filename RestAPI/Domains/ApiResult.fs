namespace RestAPI.Domains

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Identity
open Microsoft.FSharp.Core

type ApiResult(code: int, errors: List<ApiError>, timestamp: int64, data: 'T) =

    let data: 'T = data
    let code = code
    let errors = errors
    let timestamp = timestamp
    member this.Data = data
    member this.Code = code
    member this.Errors = errors
    member this.Timestamp = timestamp

    new(code: int, errors: List<ApiError>, data: 'T) =
        ApiResult(code, errors, DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(), data)

    new(data: 'T) = ApiResult(200, null, DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(), data)

    new(code: int, errors: List<ApiError>) =
        ApiResult(code, errors, DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(), null)

    new(code: int, identityErrors: IEnumerable<IdentityError>) =
        ApiResult(
            code,
            ApiError.FromIdentityErrors identityErrors,
            DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
            null
        )

    static member SUCCESS(data: 'T) = ApiResult(data)

    static member FAIL(code: int, errors: List<ApiError>) = ApiResult(code, errors)

    static member FAIL(code: int, identityErrors: IEnumerable<IdentityError>) = ApiResult(code, identityErrors)
