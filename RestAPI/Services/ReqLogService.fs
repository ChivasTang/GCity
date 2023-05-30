namespace RestAPI.Services

open Database
open RestAPI.Repositories

type ReqLogService(_context: ApiDbContext, _reqLogRepository: IReqLogRepository) =
    interface IReqLogService with
        member this.SaveOne(reqLog) =
            _reqLogRepository.Insert(reqLog) |> ignore
            _context.SaveChanges() |> ignore
