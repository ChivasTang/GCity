namespace RestAPI.Repositories

open Database

type ReqLogRepository(_context: ApiDbContext) =
    interface IReqLogRepository with
        member this.Insert(reqLog) = _context.ReqLogs.Add(reqLog)
