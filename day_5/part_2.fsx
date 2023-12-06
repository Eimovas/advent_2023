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
    
    pairs 
    // let sortedPairs =
    //     pairs
    //     |> List.sortBy fst
    //     |> List.sortByDescending snd 
    //
    // let ranges =
    //     let rec loop list (previousStart, previousEnd) acc =
    //         match list with
    //         | [] -> (previousStart, previousEnd)::acc
    //         | (start', end')::tail ->
    //             // if current start is in range of previous range, check if current end is > than previous end
    //             if start' >= previousStart && start' <= previousEnd then
    //                 if end' <= previousEnd then
    //                     loop tail (previousStart,previousEnd) acc // completely ignore this iteration
    //                 else
    //                     loop tail (previousStart, end') acc // change the end to current one
    //             else
    //                 loop tail (start', end') ((previousStart,previousEnd)::acc) 
    //         
    //     loop sortedPairs[1..] sortedPairs[0] List.empty
    // ranges 

/// traverse the seeds and the linked list to find the destination     
let traverse seedRanges (linkedList: LinkedList<_>) =
    let rec loop seed (node: LinkedListNode<_>) =
        if isNull node then seed 
        else
            let target = convertToTarget seed (node.Value |> Array.toList)
            loop target node.Next
        
    seedRanges
    |> List.fold (fun (acc: int64) (start',end') ->
        printfn $"Processing %A{(start',end')}"
        
        let mutable min = acc
        for i in start'..end' do 
            let result = (loop i linkedList.First)
            if result < min then
                min <- result
        min) Int64.MaxValue

let getResultPart2() =
    let input = loadInput("input")
    let seedRanges = parseSeeds input[0]
    let linkedList =
        input[1..]
        |> Array.map (fun str -> str.Split(Environment.NewLine))
        |> Array.map parseMapping
        |> Array.fold (fun (acc: LinkedList<_>) (_,_,ranges) -> acc.AddLast(ranges) |> ignore ; acc) (LinkedList<_>())

    traverse seedRanges linkedList

// let input = loadInput("input")
// let seeds = parseSeeds input[0]
// printfn "%A" seeds

// TODO: not working :(
getResultPart2() |> printfn "Result: %A"