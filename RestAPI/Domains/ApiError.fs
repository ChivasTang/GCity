namespace RestAPI.Domains

open System.Collections.Generic
open Microsoft.AspNetCore.Identity

type ApiError =
    { Name: string
      Description: string }

    static member FromIdentityErrors(identityErrors: IEnumerable<IdentityError>) : List<ApiError> =
        let apiErrors = List<ApiError>()

        for identityError in identityErrors do
            let apiError =
                { Name = identityError.Code
                  Description = identityError.Description }

            apiErrors.Add apiError

        apiErrors
