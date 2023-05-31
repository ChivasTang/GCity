namespace RestAPI.Services

open Models.Users

type IReqLogService =
    abstract member SaveOneAsync: reqLog: ReqLog -> unit
