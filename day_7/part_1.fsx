open System
open System.IO

// encoding to make it easy to sort 
let rankingEncoding = Map [
    'A', 'a'
    'K', 'b'
    'Q', 'c'
    'J', 'd'
    'T', 'e'
    '9', 'f'
    '8', 'g'
    '7', 'h'
    '6', 'i'
    '5', 'j'
    '4', 'k'
    '3', 'l'
    '2', 'm'
]

let parseAndEncode (line: string) =
    let split = line.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
    split[0].ToCharArray() |> Array.map (fun char -> rankingEncoding[char]) |> fun arr -> String.Join("", arr), Int32.Parse(split[1])

let isFiveOfAKind (hand: string) =
    hand.ToCharArray() |> Set.ofArray |> Set.count = 1

let isFourOfAKind (hand: string) =
    let map =
        hand.ToCharArray()
        |> Array.fold (fun acc char ->
            match acc |> Map.tryFind char with
            | Some x -> acc |> Map.add char (x + 1)
            | None -> acc |> Map.add char 1) Map.empty
    
    // total number of unique cards MUST be 2
    if map.Count <> 2 then false
    else 
        let firstCount, secondCount =
            Map.values map
            |> Seq.toArray
            |> fun arr -> arr[0], arr[1]
            
        // has 1 cards and one has 4 cards
        (firstCount = 4 && secondCount = 1) || (firstCount = 1 && secondCount = 4)

let isThreeOfAKind (hand: string) =
    let map =
        hand.ToCharArray()
        |> Array.fold (fun acc char ->
            match acc |> Map.tryFind char with
            | Some x -> acc |> Map.add char (x + 1)
            | None -> acc |> Map.add char 1) Map.empty
    
    // total number of unique cards MUST be 3
    if map.Count <> 3 then false
    else
        let values = Map.values map |> Seq.sortDescending |> Seq.toArray
        values[0] = 3 && values[1] = 1 && values[2] = 1

let isTwoOfAKind (hand: string) =
    hand.ToCharArray() |> Set.ofArray |> Set.count = 4

let isFullHouse (hand: string) =
    let map =
        hand.ToCharArray()
        |> Array.fold (fun acc char ->
            match acc |> Map.tryFind char with
            | Some x -> acc |> Map.add char (x + 1)
            | None -> acc |> Map.add char 1) Map.empty
    
    // total number of unique cards MUST be 2
    if map.Count <> 2 then false
    else 
        let firstCount, secondCount =
            Map.values map
            |> Seq.toArray
            |> fun arr -> arr[0], arr[1]
            
        // has 2 cards and one has 3 cards
        (firstCount = 2 && secondCount = 3) || (firstCount = 3 && secondCount = 2) 

let isTwoPairs (hand: string) = 
    let map =
        hand.ToCharArray()
        |> Array.fold (fun acc char ->
            match acc |> Map.tryFind char with
            | Some x -> acc |> Map.add char (x + 1)
            | None -> acc |> Map.add char 1) Map.empty
    
    // total number of unique cards MUST be 3
    if map.Count <> 3 then false
    else
        let values = Map.values map |> Seq.sortDescending |> Seq.toArray
        values[0] = 2 && values[1] = 2 && values[2] = 1

let rules = [
    isFiveOfAKind
    isFourOfAKind
    isFullHouse
    isThreeOfAKind
    isTwoPairs
    isTwoOfAKind
    fun _ -> true // everything else
]

let calculateScore rules (startingHands: (string * int) list) =
    // find all highest hands, rank them, then lower, rank, and etc
    let rec evalRule rule hands result =
        match hands with
        | [] -> result
        | head::tail ->
            if rule (fst head) then evalRule rule tail (head::result)
            else evalRule rule tail result
    
    let rec rank rules hands ranks result =
        match rules with
        | [] -> result |> List.sortByDescending (fun (x,_,_) -> x) 
        | rule::tail ->
            let scores = evalRule rule hands List.empty
            let remainingHands = List.except scores hands
            let ranked =
                scores
                |> List.sortBy fst
                |> List.mapi (fun i (game,winnings) -> ranks - i, game, winnings)
            
            rank tail remainingHands (ranks - scores.Length) (ranked @ result)
    
    let ranked = rank rules startingHands startingHands.Length List.empty
    ranked
    |> List.fold (fun result (rank,_,score) -> result + (int64 rank * int64 score)) 0L

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.map parseAndEncode
|> Array.toList
|> calculateScore rules
|> printfn "%A"
