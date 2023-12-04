open System
open System.IO

let parseCard (line: string) =
    line.Split([| ':'; '|' |], StringSplitOptions.RemoveEmptyEntries)
    |> fun split -> split[0], split[1], split[2]
    |> fun (game, winning,ours) ->
        Int32.Parse(game.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]),
        Set.ofArray (winning.Split(' ', StringSplitOptions.RemoveEmptyEntries)),
        Set.ofArray (ours.Split(' ', StringSplitOptions.RemoveEmptyEntries))

let getHitCount (winningSet, ourSet) = Set.intersect winningSet ourSet |> Set.count
let processCards (cards: (int*Set<_>*Set<_>) list) =
    let rec loop list acc =
        match list with
        | [] -> acc 
        | (game,winning,ours)::tail ->
            let currentGameHitCount = getHitCount (winning,ours)
            match acc |> Map.tryFind game with
            | Some previousHits ->
                // i already have hits, meaning everything i add to acc should be + hits
                let updatedAcc =
                    List.init currentGameHitCount (fun i -> game + i + 1) // moving to game + 1
                    |> List.fold (fun acc gameIndex ->
                        match acc |> Map.tryFind gameIndex with
                        | Some x -> acc |> Map.add gameIndex (x + previousHits)
                        | None -> acc |> Map.add gameIndex previousHits) acc
                    
                loop tail updatedAcc
                
            | None ->
                // add hit counts to consecutive games and update acc
                failwith "Not expecting to ever get here"
        
    // including each starting card here
    let startingAcc =
        cards
        |> List.fold (fun acc (game, _,_) -> acc |> Map.add game 1) Map.empty
        
    loop cards startingAcc

// testInput
File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.map parseCard
|> Array.toList
|> processCards
|> Seq.toList
|> List.map (fun kv -> kv.Value)
|> List.sum
|> printfn "%A"
