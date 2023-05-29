namespace Models.Users

open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

[<CLIMutable;Table("ReqLog")>]
type ReqLog =
    {
        [<Column("Id");Key>]
        Id:Guid
        [<Column("Url");Required;MaxLength(256)>]
        Url:string
        [<Column("UserId")>]
        UserId:Guid
        [<Column("CreateTime")>]
        CreateTime:DateTime
    }

