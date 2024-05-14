open Expecto //patronum
open Components



//index = key

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

let interactionState = Map [
    (1, [{Partner1 = "MARCO"; Partner2 = "POLO"; InteractionType = ProteinProtein}]);
    (2, [{Partner1 = "KARL"; Partner2 = "COLUMBUS"; InteractionType = ProteineGene}])
    ]

let finsihedList (table: GTelement list) (interaction:Map<int, Interaction list>)=

     //Mappt über interaction list und geht in jeder interaction list per key in die erste interaction (header)
        // interaction
        // |> Map.map (fun (a:int) (i: Interaction list) ->
        //     match i with
        //     |list -> list.Head.Partner1 
        // ) // -> gibt eine map aus strings raus
        // |> Map.toList
        // |> List.map snd
    let bigList =
        interaction |> Map.toList |> List.map snd 

    // let interactionspartner2 =
    //     interaction
    //     |> Map.map (fun (a:int) (i: Interaction list) ->
    //         match i with
    //         |list -> list.Head.Partner2 
    //     )
    //     |> Map.toList
    //     |> List.map snd

    // let interactionType =
    //     interaction
    //     |> Map.map (fun (a:int) (i: Interaction list) ->
    //         match i with
    //         |list -> list.Head.InteractionType.ToStringRdb()
    //     ) 
    //     |> Map.toList
    //     |> List.map snd

    [for i in [0 .. (table.Length - 1)] do
        {
            Title= table.[i].Title |> String.concat " " 
            Partner1 = bigList[i].Head.Partner1
            Partner2 = bigList[i].Head.Partner2
            InteractionType = bigList[i].Head.InteractionType.ToStringRdb()
        
        } 
    ]   

let recordToCsv (record: FinishedTable) = //function to build a csv row
    sprintf "%s\t%s\t%s\t%s" record.Title record.Partner1 record.Partner2 record.InteractionType 
let csvStrings =
    List.map recordToCsv (finsihedList table interactionState) //apply on each record
let csvRowsWithHeader = 
    String.concat "\n" csvStrings   //concat the row with "\n" between them
    
let tests = testList "main" [

    testCase "ensureCSVElement" (fun _ -> 
        let actual =  "Title\tPartner1\tPartner2\tInteractionType\n" + csvRowsWithHeader
        let expected = 
            "Title\tPartner1\tPartner2\tInteractionType\ntest hihi\tMARCO\tPOLO\tProtein-Protein\ntest2 hihi2\tKARL\tCOLUMBUS\tProtein-Gene"
        Expect.equal actual expected actual
        )
    
]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] [||] tests


