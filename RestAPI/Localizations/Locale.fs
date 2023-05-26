namespace RestAPI.Localizations

open Microsoft.Extensions.Localization
open System.Globalization

type Locale(locale: IStringLocalizer<Locale>) =
    static member Of(name: string) : CultureInfo = CultureInfo(name)
