#load "part_1.fsx"

open System 
open System.IO
open Part_1

let parse (lines: string[]) =
    let time =
        lines[0].Split([| ' '; ':' |], StringSplitOptions.RemoveEmptyEntries)[1..]
        |> fun list -> String.Join("", list)
        |> Int64.Parse
        
    let distance =
        lines[1].Split([| ' '; ':' |], StringSplitOptions.RemoveEmptyEntries)[1..]
        |> fun list -> String.Join("", list)
        |> Int64.Parse
        
    time,distance

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> parse
|> fun (time,distance) -> processGame time distance
|> printfn "%A"