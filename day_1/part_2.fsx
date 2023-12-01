open System
open System.IO
open System.Text

let digitMap = Map[ "one", 1 ]

let naiveReplaceDigits (str: string) =
    
    let rec loop index (sb: StringBuilder) =
        if index = str.Length - 1 then sb
        else 
            for kv in digitMap do
                if str.Substring(index, kv.Key.Length) = kv.Key then
                    sb.Append(kv.Value) |> ignore 
                    loop (index + kv.Key.Length) sb
                // else
                    // sb.Append(str[index]) |> ignore
                    // loop (index + 1) sb
    
    // TODO: FINISHED HERE!
    let sb = StringBuilder()
    loop 0 sb     
    
    for index in 0..str.Length do
        
        
        for kv in digitMap do
            if str.Substring(index, kv.Key.Length) = kv.Key then
                sb.Append(kv.Value) |> ignore 
                // jump over kv.key.length through index somehow
            else
                sb.Append(str[index]) |> ignore 
    
    sb.ToString()

let getLineDigits (str: string) =
    let updated = str.Replace("one", "1").Replace("two", "2").Replace("three", "3").Replace("four", "4").Replace("five", "5").Replace("six", "6").Replace("seven", "7").Replace("eight", "8").Replace("nine", "9")
    [| for char in updated do if Char.IsDigit(char) then yield int (char - '0') |]

let combineRelevantDigits (arr : int array) =
    if arr.Length = 0 then failwith "Not expecting empty array"
    ((Array.head arr) * 10) + Array.last arr

let test = [|
    "two1nine"
    "eightwothree"
    "abcone2threexyz"
    "xtwone3four"
    "4nineeightseven2"
    "zoneight234"
    "7pqrstsixteen"
|]

// compose all together
let result =
    // File.ReadAllLines(__SOURCE_DIRECTORY__ + "/input")
    test
    |> Array.filter (String.IsNullOrWhiteSpace >> not)
    |> Array.map getLineDigits
    |> Array.map combineRelevantDigits
    |> Array.take 5
    // |> Array.sum
    
