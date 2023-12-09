open System
open System.Collections.Generic
open System.IO

let parseInstructions (instruction: string) = instruction.ToCharArray()
let parseNode (line: string) =
    let split = line.Split([| '='; '('; ','; ')'; ' ' |], StringSplitOptions.RemoveEmptyEntries)
    split[0], split[1], split[2]
    
let buildStringNodeMap (list: string list) =
    list |> List.map parseNode |> List.map (fun (x,left,right) -> x,(left,right)) |> dict
    
let traverse (map: IDictionary<string,string*string>) startNodes (whatToFind: char) (instructions: Queue<char>) =
    let rec loop (current: (string*string*string) list) (hops: int64) =
        let instruction = instructions.Dequeue()// take the next one out
        instructions.Enqueue(instruction)       // put it back in the queue
        
        if hops % 10_000_000L = 0 then printfn "%A" hops
        
        // printfn "Instruction %A" instruction
        // printfn "Nodes %A" current
        
        // check if every node has the correct ending letter
        let allFound =
            if instruction = 'L' && current |> List.forall (fun (_,left,_) -> left[2] = whatToFind) then true  
            elif instruction = 'R' && current |> List.forall (fun (_,_,right) -> right[2] = whatToFind) then true
            else false
            
        if allFound then hops
        else
            let newNodes =
                current
                |> List.map (fun (_,left,right) ->
                    let newNode, (newLeft, newRight) =
                        if instruction = 'L' then left, map[left]
                        else right, map[right]
                    newNode, newLeft, newRight)
                
            loop newNodes (hops + 1L)
    
    loop startNodes 1L

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.filter (fun line -> not (String.IsNullOrWhiteSpace(line)))
|> fun arr ->
    let instructions = parseInstructions arr[0]
    let map = buildStringNodeMap (Array.toList arr[1..])
    let startNodes =
        map
        |> Seq.filter (fun kv -> kv.Key.EndsWith("A"))
        |> Seq.toList
        |> List.map (fun kv ->
            let x, (left,right) = kv.Key, kv.Value
            x,left,right)
                
    traverse map startNodes 'Z' (Queue(instructions))
|> printfn "%A"

(*
    The above has been running a little longer than i'd like.
    I think if I could find the first index Z of each starting number, I could find LCM between them and I'd get the answer.
*)
let denominators = [
    16343
    11911
    20221
    21883
    13019
    19667
]
// used: https://www.calculatorsoup.com/calculators/math/lcm.php
// got answer: 13524038372771

