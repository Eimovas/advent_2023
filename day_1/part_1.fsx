open System
open System.IO

let getLineDigits (str: string) = [| for char in str do if Char.IsDigit(char) then yield int (char - '0') |]
let combineRelevantDigits (arr : int array) =
    if arr.Length = 0 then failwith "Not expecting empty array"
    ((Array.head arr) * 10) + Array.last arr

// compose all together
let result =
    File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
    |> Array.filter (String.IsNullOrWhiteSpace >> not)
    |> Array.map (getLineDigits >> combineRelevantDigits)
    |> Array.sum