open System
open System.Collections.Generic
open System.IO

let parseInstructions (instruction: string) = instruction.ToCharArray()
let parseNode (line: string) =
    let split = line.Split([| '='; '('; ','; ')'; ' ' |], StringSplitOptions.RemoveEmptyEntries)
    split[0], split[1], split[2]
    
let buildStringNodeMap (list: string list) =
    list |> List.map parseNode |> List.map (fun (x,left,right) -> x,(left,right)) |> Map
    
let traverse map root whatToFind (instructions: Queue<char>) =
    let rec loop current hops =
        match map |> Map.tryFind current with
        | Some (left,right) ->
            let instruction = instructions.Dequeue()// take the next one out
            instructions.Enqueue(instruction)       // put it back in the queue
            
            if instruction = 'L' && left = whatToFind then hops 
            elif instruction = 'L' then loop left (hops + 1)
            elif instruction = 'R' && right = whatToFind then hops 
            else loop right (hops + 1)
            
        | None -> failwith "Not sure this is expected"
    
    loop root 1

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.filter (fun line -> not (String.IsNullOrWhiteSpace(line)))
|> fun arr ->
    let instructions = parseInstructions arr[0]
    let map = buildStringNodeMap (Array.toList arr[1..])
    let root =
        let name, _, _ = parseNode arr[1]
        name
        
    traverse map root "ZZZ" (Queue(instructions))
|> printfn "%A"

