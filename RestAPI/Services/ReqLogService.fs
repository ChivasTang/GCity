namespace RestAPI.Services

open Database

type ReqLogService(_context: ApiDbContext) =
    interface IReqLogService with
        member this.SaveOneAsync(reqLog) =
            async {
                _context.ReqLogs.Add(reqLog)
                |> ignore
                _context.SaveChanges()
                |> ignore
            }
            |> Async.RunSynchronously
