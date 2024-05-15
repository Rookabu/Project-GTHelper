open Expecto //patronum
open Components


type FinishedTable = {
    Title: string 
    Partner1: string
    Partner2: string
    InteractionType: string
}

//zuerst die erste Interaction

let table = [
    {
    Title = ["test";"hihi"]
    Content = ["Content"]
    }
    {
    Title = ["test2";"hihi2"]
    Content = ["Content2"]
    }
    ]

let interactionStateOne = Map [
    (1, [{Partner1 = "MARCO"; Partner2 = "POLO"; InteractionType = ProteinProtein}]);
    (2, [{Partner1 = "KARL"; Partner2 = "COLUMBUS"; InteractionType = ProteineGene}])
    ]

let interactionStateNo = Map.empty

let interactionStateMany = Map [
    (1, [{Partner1 = "MARCO"; Partner2 = "POLO"; InteractionType = ProteinProtein}; {Partner1 = "JULIUS"; Partner2 = "CAESAR"; InteractionType = ProteineGene}]);
    (2, [{Partner1 = "KARL"; Partner2 = "COLUMBUS"; InteractionType = ProteineGene}])
    ]

//folgende Funktion soll
//1. über den index in das GTelement den titel rausholen
//2. über den selben index den key in der interaction list alle interactions rausholen
//3. aus den informationen eine liste aus strings erstellen, jeder string steht für eine interaction mit entsprechenden titel und alle interactionn, getrennt durch \t

let gtElementsToCSV (ele: GTelement) (interaction: Map<int, Interaction list>) (pubIndex: int) =
    let title =
        ele.Title |> String.concat " "

    let interList = interaction.Item pubIndex 

    if interaction.IsEmpty = false then
        interList
        |> List.map (fun e ->
            [title; e.Partner1; e.Partner2; e.InteractionType.ToStringRdb()] |> String.concat "\t"        
        ) //this is a string list, where each one contains a different interaction but the same title of the element 
    else [] //if true 

let applyOnEachElementOne (list: GTelement list) =
    List.mapi (fun i a ->
        gtElementsToCSV a interactionStateOne (i+1) 
        |> String.concat "\n"
    ) list

let applyOnEachElementNo (list: GTelement list) =
    List.mapi (fun i a ->
        if interactionStateNo.ContainsKey i = true then
            gtElementsToCSV a interactionStateNo (i+1) 
            |> String.concat "\n"
        else ""
    ) list

// let applyOnEachElementMany (list: GTelement list) =
//     List.mapi (fun i a ->
//         gtElementsToCSV a interactionStateOne (i+1) 
//     ) list

let csvRowsWithHeaderONE = 
    String.concat "\n" (applyOnEachElementOne table)  

let csvRowsWithHeaderNO = 
    String.concat "\n" (applyOnEachElementNo table)  

let tests = testList "main" [

    // testCase "oneInteraction" (fun _ -> 
    //     let actual =  "Title\tPartner1\tPartner2\tInteractionType\n" + csvRowsWithHeaderONE
    //     let expected = 
    //         "Title\tPartner1\tPartner2\tInteractionType\ntest hihi\tMARCO\tPOLO\tProtein-Protein\ntest2 hihi2\tKARL\tCOLUMBUS\tProtein-Gene"
    //     Expect.equal actual expected ""
    //     )

    testCase "noInteraction" (fun _ -> 
        let actual =  "Title\tPartner1\tPartner2\tInteractionType\n" + csvRowsWithHeaderNO
        let expected = 
            "Title\tPartner1\tPartner2\tInteractionType\n"
        Expect.equal actual expected ""
        )

    // testCase "manyInteraction" (fun _ -> 
    // let actual =  "Title\tPartner1\tPartner2\tInteractionType\n" + csvRowsWithHeader
    // let expected = 
    //     "Title\tPartner1\tPartner2\tInteractionType\ntest hihi\tMARCO\tPOLO\tProtein-Protein\ntest hihi\tJULIUS\tCAESAR\tProtein-Gene\ntest2 hihi2\tKARL\tCOLUMBUS\tProtein-Gene"
    // Expect.equal actual expected ""
    // )

    
]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] [||] tests


