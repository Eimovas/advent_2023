#load "part_1.fsx"

open System
open System.IO
open Part_1

let getLineScorePart2 (line: Token list) =
    let rec loop list red green blue =
        match list with
        | [] -> red,green,blue
        | head::tail ->
            match head with
            | Red x when x > red -> loop tail x green blue  
            | Blue x when x > blue -> loop tail red green x  
            | Green x when x > green -> loop tail red x blue
            | _ -> loop tail red green blue
        
    let red,green,blue = loop line 0 0 0
    red * green * blue

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.filter (String.IsNullOrWhiteSpace >> not)
|> Array.map (parseLine >> getLineScorePart2)
|> Array.sum
|> printfn "%A"