namespace Models.Users

open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

[<CLIMutable; Table("ReqLog")>]
type ReqLog =
    { [<Column("Id"); Key>]
      Id: Guid
      [<Column("Url"); Required; MaxLength(256)>]
      Url: string
      [<Column("UserId")>]
      UserId: string
      [<Column("InTime")>]
      InTime: int64
      [<Column("OutTime")>]
      OutTime: int64 }
