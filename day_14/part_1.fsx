open System.IO

(*
    Start from top, iterate chars:
    1. if '0' is found, see if I can move it up,
        1.1 if I can't, do nothing and continue,
        1.2 if I can, move it up, and continue until I can't move it up anymore,
    2. if '#' or '.' is found, do nothing and continue,
*)

let parse (lines: string[]) = Array2D.init lines.Length lines[0].Length (fun y x -> lines[y][x])

// iterates the grid, moves boulders, and memorizes their coordinates
let tiltAndRoll (grid: char array2d) =
    let rows = Array2D.length1 grid 
    let cols = Array2D.length2 grid 
    
    let rec moveUpIfAppropriate y x (grid: char array2d) rockCoordinates =
        match grid[y,x] = 'O' with
        | true when y > 0 && grid[y-1,x] = '.' ->
            grid[y, x] <- '.'
            grid[y-1, x] <- 'O'
            moveUpIfAppropriate (y - 1) x grid rockCoordinates 
        | true -> grid, (y,x)::rockCoordinates // reached the top, can't move up anymore, will add to rockCoordinates
        | _ -> grid, rockCoordinates
    
    let rec iterate grid y x rockCoordinates =
        let updatedGrid, updatedCoordinates = moveUpIfAppropriate y x grid rockCoordinates       
        if x = cols - 1 && y = rows - 1 then updatedGrid, updatedCoordinates      // end of grid
        elif x = cols - 1 then iterate updatedGrid (y + 1) 0 updatedCoordinates   // end of row, but not grid
        else iterate updatedGrid y (x + 1) updatedCoordinates                     // not end of row, not end of grid
    
    iterate grid 0 0 List.empty

// Compose and count the result 
File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
|> parse
|> tiltAndRoll
|> fun (grid,rockCoordinates) ->
    let rows = Array2D.length1 grid
    rockCoordinates
    |> List.fold (fun acc (y,_) -> acc + (rows - y)) 0 
|> printfn "Result: %A"