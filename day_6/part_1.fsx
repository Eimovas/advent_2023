open System
open System.IO

let parse (lines: string[]) =
    let times =
        lines[0].Split([| ' '; ':' |], StringSplitOptions.RemoveEmptyEntries)[1..]
        |> Array.map Int64.Parse
        
    let distances =
        lines[1].Split([| ' '; ':' |], StringSplitOptions.RemoveEmptyEntries)[1..]
        |> Array.map Int64.Parse
        
    Array.zip times distances

/// process a game and produce a count of wins 
let processGame time maxDistance =
    let rec loop current wins =
        if current = time then wins
        else 
            let potentialTime = current * (time - current)
            if potentialTime > maxDistance then loop (current + 1L) (current::wins)
            else loop (current + 1L) wins 

    loop 1L List.empty |> List.length

// compose all together     
File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> parse
|> Array.fold (fun acc (time, distance) -> acc * (processGame time distance)) 1 
|> printfn "%A"
