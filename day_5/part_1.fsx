open System 

Int64.Parse("3171885613")

// seeds: 79 14 55 13
(*
    dest: 50, 51
    source: 98, 99
    (nothing matches)
    
    dest: 52, 53, 54, 55, 56...
    source: 50, 51, 52, 53, 54...
    
    55 -> 57, 79 -> 81, 
*)

let parseSeeds (line: string) =
    line.Split([|':'; ' '|], StringSplitOptions.RemoveEmptyEntries)[1..]
    |> Array.map Int64.Parse
    |> Set
    
let parseMapping (lines: string[]) =
    let sourceTitle, destinationTitle =
        lines[0].Split([| "-to-"; " " |], StringSplitOptions.RemoveEmptyEntries)
        |> fun split -> split[0], split[1]
        
    let ranges =
        lines[1..]
        |> Array.map (fun line -> line.Split(' ', StringSplitOptions.RemoveEmptyEntries) |> fun split -> split[0], split[1], split[2])
        |> Array.map (fun (destination,source,length) -> Int64.Parse(destination), Int64.Parse(source), Int64.Parse(length))
    
    sourceTitle, destinationTitle, ranges

// TODO: FINISHED HERE! Fixed the parsing
