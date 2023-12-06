open System
open System.Collections.Generic
open System.IO

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

/// convert source into target by considering all target ranges
let convertToTarget (from: int64) (ranges: (int64 * int64 * int64) list) =
    let rec loop list =
        match list with
        | [] -> from 
        | (destination,source,length)::tail ->
            let adjustedSource = source + length
            if from >= source && from <= adjustedSource then
                // number is within range, we find the appropriate coordinates for it
                destination - source + from
            else
                loop tail 
    
    loop ranges

/// traverse the seeds and the linked list to find the destination     
let traverse seeds (linkedList: LinkedList<_>) =
    let rec loop seed (node: LinkedListNode<_>) =
        if isNull node then seed 
        else
            let target = convertToTarget seed (node.Value |> Array.toList)
            loop target node.Next
        
    seeds
    |> Set.fold (fun acc seed -> (loop seed linkedList.First)::acc) List.empty
    
let loadInput (fileName: string) =
    File.ReadAllText(__SOURCE_DIRECTORY__ + $"/{fileName}")
    |> fun str -> str.Split(Environment.NewLine + Environment.NewLine)

let getResultPart1() =
    let input = loadInput("input")
    let seeds = parseSeeds input[0]
    let linkedList =
        input[1..]
        |> Array.map (fun str -> str.Split(Environment.NewLine))
        |> Array.map parseMapping
        |> Array.fold (fun (acc: LinkedList<_>) (_,_,ranges) -> acc.AddLast(ranges) |> ignore ; acc) (LinkedList<_>())

    traverse seeds linkedList
    |> List.min
