open System
open System.IO

let parse (line: string) =
    line.Split([| ' ' |], StringSplitOptions.RemoveEmptyEntries) |> Array.map Int32.Parse
    
let rec extrapolateBackwards newNumber (fullHistory: int[] list) =
    match fullHistory with
    | [] -> newNumber
    | head::tail -> extrapolateBackwards (head[0] - newNumber) tail 

let rec processSequence (history: int[] list) arr =
    let newArr =
        arr
        |> Array.pairwise
        |> Array.map (fun (prev,next) -> next - prev)
        
    if newArr |> Array.forall ((=) 0) then extrapolateBackwards 0 history 
    else processSequence (newArr::history) newArr
    
File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.map parse
|> Array.map (fun line -> processSequence (List.singleton line) line)
|> Array.fold (+) 0
|> printfn "%A"
