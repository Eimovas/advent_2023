open System
open System.IO

module List = 
    let tryMax list =
        list
        |> List.sortDescending
        |> List.tryHead

// parse input into a grid 
let parse (input: string) =
    let patterns =  input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
    [| for pattern in patterns do
        let lines = pattern.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        Array2D.init lines.Length lines[0].Length (fun y x  -> lines[y][x]) |]

// assuming there is potentially more than one pair
let findMatchingRows (arr: char array2d) =
    Array.init (Array2D.length1 arr) (fun i -> i, String.Join(String.Empty, arr[i,*]))
    |> Array.pairwise
    |> Array.choose (fun ((i1, line1), (i2, line2)) -> if line1 = line2 then Some(i1,i2) else None)

// assuming there is potentially more than one pair
let findMatchingCols (arr: char array2d) =
    Array.init (Array2D.length2 arr) (fun i -> i, String.Join(String.Empty, arr[*,i]))
    |> Array.pairwise
    |> Array.choose (fun ((i1, line1), (i2, line2)) -> if line1 = line2 then Some(i1,i2) else None)

// get indexes to check in array bound safe way
let rec getIndexesToCheck length x1 x2 acc =
    if x1 - 1 >= 0 && x2 + 1 < length then getIndexesToCheck length (x1 - 1)  (x2 + 1) ((x1 - 1, x2 + 1) :: acc)
    else acc

// find perfect match - expecting rows/columns going away from the center to be matching
let chooseBestMatch (cols: (int*int) array) (rows: (int*int) array) (arr: char array2d) =
    let columnSize = Array2D.length2 arr 
    let rowSize = Array2D.length1 arr
    
    let maxCol =
        [ for x1,x2 in cols do
            let indexes = getIndexesToCheck columnSize x1 x2 List.empty
            if indexes |> List.forall (fun (x1,x2) -> String.Join(String.Empty, arr[*,x1]) = String.Join(String.Empty, arr[*,x2])) then x1 + 1 ]
        |> List.tryMax
        
    let maxRow = 
        [ for y1,y2 in rows do
            let indexes = getIndexesToCheck rowSize y1 y2 List.empty
            if indexes |> List.forall (fun (y1,y2) -> String.Join(String.Empty, arr[y1,*]) = String.Join(String.Empty, arr[y2,*])) then y1 + 1 ]
        |> List.tryMax
        
    match maxCol, maxRow with
    | Some maxCol, Some maxRow when maxCol = maxRow -> failwith "Not expecting col and row result the same, not sure what to do" 
    | Some maxCol, Some maxRow when maxCol > maxRow -> maxCol
    | Some maxCol, Some maxRow when maxCol < maxRow -> maxRow * 100
    | Some maxCol, None -> maxCol
    | None, Some maxRow -> maxRow * 100
    | _ -> failwith "Not expecting no matches"

// compose all together 
File.ReadAllText(__SOURCE_DIRECTORY__ + "/input")
|> parse
|> Array.map (fun pattern ->
    let rows = findMatchingRows pattern
    let columns = findMatchingCols pattern
    chooseBestMatch columns rows pattern)
|> Array.sum
|> printfn "%A"
