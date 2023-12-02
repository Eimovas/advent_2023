open System
open System.IO

let [<Literal>] redTotal = 12
let [<Literal>] greenTotal = 13
let [<Literal>] blueTotal = 14

type Token = Game of int | Red of int | Green of int | Blue of int
    
let parseLine (line: string) =
    let split = line.Split([| ' '; ':'; ';'; ',' |], StringSplitOptions.RemoveEmptyEntries) |> Array.toList
    if split.Length % 2 <> 0 then failwith "Token count should be % 2 = 0"
    
    let rec loop list acc =
        match list with
        | [] -> acc |> List.rev
        | first::second::tail ->
            let token =
                match first, second with
                | "Game", _ -> Game (Int32.Parse second)
                | _, "red" -> Red (Int32.Parse first) 
                | _, "green" -> Green (Int32.Parse first) 
                | _, "blue" -> Blue (Int32.Parse first)
                | x -> failwith $"Unexpected token: {x}"
            loop tail (token::acc)
        | _ -> failwith "Unexpected count of tokens"

    loop split List.empty
    
let getLineScore (line: Token list) : int =
    let gameId = List.head line |> function | Game x -> x | _ -> failwith "Expecting game at slot 1"
    let rec isPossible list =
        match list with
        | [] -> true
        | head::tail ->
            match head with
            | Red x -> if redTotal >= x then isPossible tail else false  
            | Green x -> if greenTotal >= x then isPossible tail else false 
            | Blue x -> if blueTotal >= x then isPossible tail else false
            | _ -> false
            
    if isPossible line[1..] then gameId else 0

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.filter (String.IsNullOrWhiteSpace >> not)
|> Array.map (parseLine >> getLineScore)
|> Array.sum
|> printfn "%A"
