open System
open System.IO

type GameResult =
    | FiveOfAKind of string * int
    | FourOfAKind of string * int
    | FullHouse of string * int
    | ThreeOfAKind of string * int
    | TwoPair of string * int
    | TwoOfAKind of string * int
    | HighCard of string * int

// encoding to make it easy to sort 
let rankingEncoding = Map [
    'A', 'a'
    'K', 'b'
    'Q', 'c'
    'T', 'e'
    '9', 'f'
    '8', 'g'
    '7', 'h'
    '6', 'i'
    '5', 'j'
    '4', 'k'
    '3', 'l'
    '2', 'm'
    'J', 'n' // weakest from now on for the purposes of ranking
]

let getCharCountMap (hand: string) = 
    hand.ToCharArray()
    |> Array.fold (fun acc char ->
        match acc |> Map.tryFind char with
        | Some x -> acc |> Map.add char (x + 1)
        | None -> acc |> Map.add char 1) Map.empty
    
let parseAndEncode (line: string) =
    let split = line.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
    split[0].ToCharArray() |> Array.map (fun char -> rankingEncoding[char]) |> fun arr -> String.Join("", arr), Int32.Parse(split[1])

let scoreGame (hand, score)=
    let map = getCharCountMap hand
    
    // find jokers count
    match map.TryFind 'n' with
    | Some jokerCount when jokerCount = 1 && map.Count = 5 -> TwoOfAKind (hand,score)
    | Some jokerCount when jokerCount = 1 && map.Count = 4 -> ThreeOfAKind (hand,score)
    | Some jokerCount when jokerCount = 1 && map.Count = 3 ->
        let sorted = Map.values map |> Seq.sortDescending |> Seq.toArray
        if sorted[0] = 3 && sorted[1] = 1 && sorted[2] = 1 then FourOfAKind (hand,score)
        else FullHouse (hand,score)
    | Some jokerCount when jokerCount = 1 && map.Count = 2 -> FiveOfAKind (hand,score)
    | Some jokerCount when jokerCount = 2 && map.Count = 4 -> ThreeOfAKind (hand,score)
    | Some jokerCount when jokerCount = 2 && map.Count = 3 -> FourOfAKind (hand,score)
    | Some jokerCount when jokerCount = 2 && map.Count = 2 -> FiveOfAKind (hand,score)
    | Some jokerCount when jokerCount = 3 && map.Count = 3 -> FourOfAKind (hand,score)
    | Some jokerCount when jokerCount = 3 && map.Count = 2 -> FiveOfAKind (hand,score)
    | Some jokerCount when jokerCount = 4 && map.Count = 2 -> FiveOfAKind (hand,score)
    | Some jokerCount when jokerCount = 5 && map.Count = 1 -> FiveOfAKind (hand,score)
    
    // all the same
    | None when map.Count = 1 -> FiveOfAKind (hand,score)
    
    // either four of a kind, or full house
    | None when map.Count = 2 ->
        let sorted = Map.values map |> Seq.sortDescending |> Seq.toArray
        if sorted[0] = 4 && sorted[1] = 1 then FourOfAKind (hand,score)
        else FullHouse (hand,score)
    
    // either three of a kind, or two pairs
    | None when map.Count = 3 ->
        let sorted = Map.values map |> Seq.sortDescending |> Seq.toArray
        if sorted[0] = 2 && sorted[1] = 2 && sorted[2] = 1 then TwoPair (hand,score)
        else ThreeOfAKind (hand,score)
    
    | None when map.Count = 4 -> TwoOfAKind (hand,score)
    | None when map.Count = 5 -> HighCard (hand,score)
    | _ -> failwith "not supported" 

let rank (games : GameResult list) =
    let fiveOfKinds =
        games
        |> List.choose (function | FiveOfAKind (hand,score) -> Some(hand,score) | _ -> None)
        |> List.sortBy fst
        |> List.mapi (fun i (hand,score) -> games.Length - i, hand, score)
        
    let fourOfKinds =
        games
        |> List.choose (function | FourOfAKind (hand,score) -> Some(hand,score) | _ -> None)
        |> List.sortBy fst
        |> List.mapi (fun i (hand,score) -> games.Length - fiveOfKinds.Length - i, hand,score)
        
    let fullHouses =
        games
        |> List.choose (function | FullHouse (hand,score) -> Some(hand,score) | _ -> None)
        |> List.sortBy fst
        |> List.mapi (fun i (hand,score) -> games.Length - fiveOfKinds.Length - fourOfKinds.Length - i, hand,score)
        
    let threeOfKinds =
        games
        |> List.choose (function | ThreeOfAKind (hand,score) -> Some(hand,score) | _ -> None)
        |> List.sortBy fst
        |> List.mapi (fun i (hand,score) -> games.Length - fiveOfKinds.Length - fourOfKinds.Length - fullHouses.Length - i, hand,score)
        
    let twoPairs =
        games
        |> List.choose (function | TwoPair (hand,score) -> Some(hand,score) | _ -> None)
        |> List.sortBy fst
        |> List.mapi (fun i (hand,score) -> games.Length - fiveOfKinds.Length - fourOfKinds.Length - fullHouses.Length - threeOfKinds.Length - i, hand,score)
        
    let onePairs =
        games
        |> List.choose (function | TwoOfAKind (hand,score) -> Some(hand,score) | _ -> None)
        |> List.sortBy fst
        |> List.mapi (fun i (hand,score) -> games.Length - fiveOfKinds.Length - fourOfKinds.Length - fullHouses.Length - threeOfKinds.Length - twoPairs.Length - i, hand,score)
        
    let highCards =
        games
        |> List.choose (function | HighCard (hand,score) -> Some(hand,score) | _ -> None)
        |> List.sortBy fst
        |> List.mapi (fun i (hand,score) -> games.Length - fiveOfKinds.Length - fourOfKinds.Length - fullHouses.Length - threeOfKinds.Length - twoPairs.Length - onePairs.Length - i, hand,score)
        
    fiveOfKinds @ fourOfKinds @ fullHouses @ threeOfKinds @ twoPairs @ onePairs @ highCards
    
let calculateScore (ranked : (int*string*int) list) =
    ranked
    |> List.fold (fun result (rank,_,score) -> result + (int64 rank * int64 score)) 0L
    
File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.map parseAndEncode
|> Array.map scoreGame
|> Array.toList
|> rank 
|> calculateScore
|> printfn "%A"
