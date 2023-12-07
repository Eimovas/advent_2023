open System
open System.IO

let parse (line: string) =
    let split = line.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
    split[0], Int32.Parse(split[1])

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
    hand.ToCharArray() |> Set.ofArray |> Set.count = 2

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

let calculateScore rules (hands: (string * int) list) =
    // find all highest hands, rank them, then lower, rank, and etc
    let rec evalRule eval hands result =
        match hands with
        | [] -> result
        | head::tail ->
            if eval (fst head) then evalRule eval tail (head::result)
            else evalRule eval tail result
    
    // TODO: FINISHED HERE! I need to make sure I remove hands I already ranked.
    // TODO: check if I use the correct rank - best card is rank max?
    let rec rankAndScore rules ranks result =
        match rules with
        | [] -> result
        | rule::tail ->
            let currentScore, remainingRanks =
                evalRule rule hands List.empty
                |> List.sortBy fst
                |> List.fold (fun (score,rank) (game,winnings) ->
                    score + (rank * winnings), rank - 1) (result,ranks)
            
            rankAndScore tail remainingRanks currentScore
    
    rankAndScore rules hands.Length 0

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/test_input")
|> Array.map parse
|> Array.toList
|> calculateScore rules
|> printfn "%A"
