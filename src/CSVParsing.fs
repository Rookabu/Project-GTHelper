module CSVParsing
open Types

let removeSymbols (word: string) =
    word.Trim[|'.'; ','; '(' ; ')';';'|]


let gtElementToCSV (ele: GTelement) (interaction: Map<int, Interaction list>) (pubIndex: int) =

    let title =
        ele.Title |> String.concat " "

    match interaction.TryFind pubIndex with
    |Some l -> 
        l
        |> List.map (fun e ->
            [title; removeSymbols e.Partner1; removeSymbols e.Partner2; e.InteractionType.ToOutput()] |> String.concat "\t"        
        ) //this is a string list, where each one contains a different interaction but the same title of the element 
    |None -> []


let gtElementsToCSV (list: GTelement list)(interState: Map<int, Interaction list> ) =
    [
        "Title\tPartner1\tPartner2\tInteractionType"
        for i in 0 .. (list.Length - 1) do
            let e = list.Item i
            yield! gtElementToCSV e interState i
    ]
    |> String.concat "\n"