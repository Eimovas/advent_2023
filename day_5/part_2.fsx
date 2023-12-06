#load "part_1.fsx"

open System 
open System.Collections.Generic
open Part_1

(*
    What if I pair them up and get start-end range?
    Then I can sort all ranges by start asc and end desc,
    and I should get overlaps.
    
    seeds: 79 14 55 13 45 20
    79 + 14 => 93, meaning 79..93
    55 + 13 => 68, meaning 55..68
    45 + 20 => 65, meaning 45..65
    
    after sorting:
    45..65, 55..68, 79..93
    meaning i can get rid of middle ranges and get
    45..68, 79..93
*)
let parseSeeds (line: string) =
    let original =
        line.Split([|':'; ' '|], StringSplitOptions.RemoveEmptyEntries)[1..]
        |> Array.map Int64.Parse
        |> Array.toList

    let pairs =
        let rec loop (list: int64 list) acc =
            match list with
            | [] -> acc 
            | item::length::tail -> loop tail ((item, item + length) :: acc)
            | _ -> failwith "Invalid format of seeds - expecting pairs all over" 
        loop original List.empty
    
    let sortedPairs =
        pairs
        |> List.sortBy fst
        |> List.sortByDescending snd 
    
    let concatenated =
        let rec loop list (previousStart, previousEnd) acc =
            match list with
            | [] -> (previousStart, previousEnd)::acc
            | (start', end')::tail ->
                // if current start is in range of previous range, check if current end is > than previous end
                if start' >= previousStart && start' <= previousEnd then
                    if end' <= previousEnd then
                        loop tail (previousStart,previousEnd) acc // completely ignore this iteration
                    else
                        loop tail (previousStart, end') acc // change the end to current one
                else
                    loop tail (start', end') ((previousStart,previousEnd)::acc) 
            
        loop sortedPairs[1..] sortedPairs[0] List.empty
    
    let result = ResizeArray<_>()
    for start', end' in concatenated do
        printfn $"Processing {start'} to {end'} - range of {end' - start'}"
        result.Add([| for i in start' .. end' -> i |])
        printfn "Added range"

    //
    // let result = ResizeArray()
    // for i in fst concatenated[0] .. snd concatenated[0] do
    //     result.Add(i)
    //     
    // result 
    
    // concatenated
    // |> List.toArray
    // |> Array.Parallel.collect (fun (start',end') -> [| for i in start'..end' -> i |])

// let getResultPart2() =
//     let input = loadInput("input")
//     let seeds = parseSeeds input[0]
//     let linkedList =
//         input[1..]
//         |> Array.map (fun str -> str.Split(Environment.NewLine))
//         |> Array.map parseMapping
//         |> Array.fold (fun (acc: LinkedList<_>) (_,_,ranges) -> acc.AddLast(ranges) |> ignore ; acc) (LinkedList<_>())
//
//     traverse seeds linkedList
//     |> List.min

// TODO: FINISHED HERE! Seem to be running out of memory :(
let input = loadInput("input")
let seeds = parseSeeds input[0]
printfn "%A" seeds

// getResultPart2()