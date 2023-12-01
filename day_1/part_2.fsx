open System
open System.IO

let digitMap = Map[ "one", 1; "two", 2; "three", 3; "four", 4; "five", 5; "six", 6; "seven", 7; "eight", 8; "nine", 9; "0", 0; "1", 1; "2", 2; "3", 3; "4", 4; "5", 5; "6", 6; "7", 7; "8", 8; "9", 9 ]

let rec findFirst (str: string) (list: (string*int) list) (minIndex, current) = 
    match list with 
    | [] -> current
    | (key,value)::tail -> 
        let index = str.IndexOf(key)
        if index = -1 || index > minIndex then findFirst str tail (minIndex,current)
        else findFirst str tail (index, value)

let rec findLast (str: string) (list: (string*int) list) (maxIndex, current) = 
    match list with 
    | [] -> current
    | (key,value)::tail -> 
        let index = str.LastIndexOf(key)
        if index = -1 || index < maxIndex then findLast str tail (maxIndex, current)
        else findLast str tail (index, value)

let getLineDigits (str: string) =
    // find first 
    let first = findFirst str (digitMap |> Map.toList) (Int32.MaxValue, Int32.MaxValue)

    // find last 
    let last = findLast str (digitMap |> Map.toList) (-1, -1)
    first * 10 + last 

// compose all together
let result =
    File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
    |> Array.filter (String.IsNullOrWhiteSpace >> not)
    |> Array.map getLineDigits
    |> Array.sum

