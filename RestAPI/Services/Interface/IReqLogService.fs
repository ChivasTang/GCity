namespace RestAPI.Services

open Models.Users

type IReqLogService =
    abstract member SaveOne: reqLog: ReqLog -> unit
