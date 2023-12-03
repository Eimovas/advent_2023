open System 
open System.IO

(*
    I'll adjust the the whole thing and store coordinates of gear + count of numbers its adjacent to.
    After I have this map, I'll look for count = 2 and multiply + add them up.
*)

let isGear (char: char) = char = '*'
let isAdjacentToGear (y, x) (matrix: char array2d) : (int*int) option =
    let ySize = Array2D.length1 matrix
    let xSize = Array2D.length2 matrix
    
    // check x + 1
    if x + 1 < xSize && isGear matrix[y, x + 1] then Some(y, x + 1)
    // check x - 1
    elif x - 1 >= 0 && isGear matrix[y, x - 1] then Some(y, x - 1)
    // check y + 1
    elif y + 1 < ySize && isGear matrix[y + 1, x] then Some(y + 1, x)
    // check y - 1
    elif y - 1 >= 0 && isGear matrix[y - 1, x] then Some(y - 1, x)
    // check y + 1, x + 1
    elif x + 1 < xSize && y + 1 < ySize && isGear matrix[y + 1, x + 1] then Some(y + 1, x + 1)
    // check y + 1, x - 1
    elif y + 1 < ySize && x - 1 >= 0 && isGear matrix[y + 1, x - 1] then Some(y + 1, x - 1)
    // check y - 1, x + 1
    elif x + 1 < xSize && y - 1 >= 0 && isGear matrix[y - 1, x + 1] then Some(y - 1, x + 1)
    // check y - 1, x - 1
    elif y - 1 >= 0 && x - 1 >= 0 && isGear matrix[y - 1, x - 1] then Some(y - 1, x - 1)
    else None

let getNextPoint (y,x) (matrix: char array2d) : int * int =
    let ySize = Array2D.length1 matrix
    let xSize = Array2D.length2 matrix
    
    if x + 1 >= xSize && y + 1 < ySize then y + 1, 0
    elif x + 1 >= xSize && y + 1 >= ySize then -1, 0 // end
    else y, x + 1 // just continue 
    
let getNumberFromChars (chars: char list) = Int32.Parse(String.Join("", chars))

(*
    Limitations/bugs:
    - final sequence will not be added,
    - when a number sequence breaks to a new line, we'll continue adding them up
*)
let getResult (matrix: char array2d) =
    let rec loop (y,x) (gearCoordinates: (int*int) option) numberSequence result =
        if y = -1 then result // finish iterate
        else 
            match Char.IsDigit matrix[y,x] with
            | true when gearCoordinates.IsSome ->
                // if its already caught, i'll just take the digit and move on
                loop (getNextPoint (y,x) matrix) gearCoordinates (matrix[y,x]::numberSequence) result
            | true ->
                match isAdjacentToGear (y,x) matrix with
                | Some coordinates ->
                    // this is when we find a new adjacent gear - will store its coordinates
                    loop (getNextPoint (y,x) matrix) (Some coordinates) (matrix[y,x]::numberSequence) result
                | None -> 
                    // its a digit char, but not adjacent to any gears - we just store a number and move on 
                    loop (getNextPoint (y,x) matrix) gearCoordinates (matrix[y,x]::numberSequence) result
                
            | false when gearCoordinates.IsSome && numberSequence.Length > 0 ->
                // number sequence ended - add it to the final number, reset everything
                let number = getNumberFromChars (List.rev numberSequence)
                match gearCoordinates with
                | Some coordinates ->
                    match result |> Map.tryFind coordinates with
                    | Some list ->
                        loop (getNextPoint (y,x) matrix) None List.empty (result |> Map.add coordinates (number::list))
                    | None -> 
                        loop (getNextPoint (y,x) matrix) None List.empty (result |> Map.add coordinates (List.singleton number))
                | None ->
                    failwith "Not expecting to ever be here"
                    
            | false ->
                // just continue and reset everything
                loop (getNextPoint (y,x) matrix) None List.empty result 

    loop (0,0) None List.empty Map.empty // gets gear counts
    |> Map.filter (fun key value -> value.Length = 2)
    |> Seq.toList
    |> List.map (fun kv -> kv.Value[0] * kv.Value[1])
    |> List.sum  

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.map (fun line -> line.ToCharArray())
|> array2D
|> getResult
|> printfn "%A"