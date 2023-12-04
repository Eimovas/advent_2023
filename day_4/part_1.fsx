open System
open System.IO

let parseGame (line: string) =
    line.Split([| ':'; '|' |], StringSplitOptions.RemoveEmptyEntries)
    |> fun split -> split[1], split[2]
    |> fun (winning,ours) -> Set.ofArray (winning.Split(' ', StringSplitOptions.RemoveEmptyEntries)), Set.ofArray (ours.Split(' ', StringSplitOptions.RemoveEmptyEntries))

let getCardValue (winningSet, ourSet) =
    Set.intersect winningSet ourSet
    |> Set.fold (fun acc _ -> if acc = 0 then 1 else acc * 2) 0

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.map parseGame
|> Array.map getCardValue
|> Array.sum
|> printfn "%A"