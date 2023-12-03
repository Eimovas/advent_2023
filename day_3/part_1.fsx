open System 
open System.IO

let isSymbol (char: char) = char <> '.' && not(Char.IsDigit(char))
    
let isPart (y, x) (matrix: char array2d) : bool =
    let ySize = Array2D.length1 matrix
    let xSize = Array2D.length2 matrix
    
    // check x + 1
    if x + 1 < xSize && isSymbol matrix[y, x + 1] then true
    // check x - 1
    elif x - 1 >= 0 && isSymbol matrix[y, x - 1] then true  
    // check y + 1
    elif y + 1 < ySize && isSymbol matrix[y + 1, x] then true
    // check y - 1
    elif y - 1 >= 0 && isSymbol matrix[y - 1, x] then true
    // check y + 1, x + 1
    elif x + 1 < xSize && y + 1 < ySize && isSymbol matrix[y + 1, x + 1] then true
    // check y + 1, x - 1
    elif y + 1 < ySize && x - 1 >= 0 && isSymbol matrix[y + 1, x - 1] then true
    // check y - 1, x + 1
    elif x + 1 < xSize && y - 1 >= 0 && isSymbol matrix[y - 1, x + 1] then true
    // check y - 1, x - 1
    elif y - 1 >= 0 && x - 1 >= 0 && isSymbol matrix[y - 1, x - 1] then true
    else false

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
    let rec loop (y,x) sequenceIsPart numberSequence result =
        if y = -1 then result // finish iterate
        else 
            match Char.IsDigit matrix[y,x] with
            | true when isPart (y,x) matrix ->
                // its number and is part - we'll add it to sequence and will continue
                loop (getNextPoint (y,x) matrix) true (matrix[y,x]::numberSequence) result
            | true ->
                // its a digit char, but not part - so we'll just add it to potential sequence 
                loop (getNextPoint (y,x) matrix) sequenceIsPart (matrix[y,x]::numberSequence) result
            | false when sequenceIsPart && numberSequence.Length > 0 ->
                // number sequence ended - add it to the final number, reset everything
                let number = getNumberFromChars (List.rev numberSequence)
                loop (getNextPoint (y,x) matrix) false List.empty (number + result)
            | false ->
                // just continue 
                loop (getNextPoint (y,x) matrix) false List.empty result 

    loop (0,0) false List.empty 0

File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> Array.map (fun line -> line.ToCharArray())
|> array2D
|> getResult
|> printfn "%A"