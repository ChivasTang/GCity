namespace Models.Users

open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

[<CLIMutable; Table("UserProfile")>]
type UserProfile =
    { [<Column("Id"); Key>]
      Id: Guid
      [<Column("Locale")>]
      Locale: string }
