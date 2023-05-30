namespace RestAPI.Repositories

open Microsoft.EntityFrameworkCore.ChangeTracking
open Models.Users

type IReqLogRepository =
    abstract member Insert: reqLog: ReqLog -> EntityEntry<ReqLog>
