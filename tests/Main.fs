open Expecto //patronum
open Components
open FSharpAux.IO


//index = key

type finishedTable = {
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

let finsihedList (title: GTelement list) (interaction:Map<int, Interaction list>)=
    //gets the first interaction/element of each list
    // let interactionspartner1 =
    //     match interaction.TryFind index with
    //     |Some list -> list.Head.Partner1
    //     |None -> "" 
    // let interactionspartner2 =
    //     match interaction.TryFind index with
    //     |Some list -> list.Head.Partner2
    //     |None -> "" 
    // let interactionType =
    //     match interaction.TryFind index with
    //     |Some list -> list.Head.InteractionType.ToStringRdb()
    //     |None -> "" 

    let interactionspartner1 = //Mappt über interaction list und geht in jeder interaction list per key in die erste interaction (header)
        interaction
        |> Map.map (fun (a:int) (i: Interaction list) ->
            match i with
            |list -> list.Head.Partner1 
        ) // -> gibt eine map aus strings raus
        |> Map.toList
        |> List.map snd
        

    let interactionspartner2 =
        interaction
        |> Map.map (fun (a:int) (i: Interaction list) ->
            match i with
            |list -> list.Head.Partner2 
        )
        |> Map.toList
        |> List.map snd

    let interactionType =
        interaction
        |> Map.map (fun (a:int) (i: Interaction list) ->
            match i with
            |list -> list.Head.InteractionType.ToStringRdb()
        ) 
        |> Map.toList
        |> List.map snd
    List.zip 
        (List.zip interactionspartner1 interactionspartner2) 
        (List.zip interactionType title)
    |> List.map (fun ((ip1,ip2),(it,t)) ->
        {
            Title= t.Title |> String.concat " " 
            Partner1 = ip1
            Partner2 = ip2
            InteractionType = it
        }    
    )

    

let GTparseToCSV (table: finishedTable list) =
    SeqIO.Seq.CSV "\t" true false table |> String.concat "\n"      

let tests = testList "main" [
    testCase "ensureCSVElement" (fun _ -> 
        let actual =  GTparseToCSV (finsihedList table interactionState)    
        let expected = 
            "Title\tPartner1\tPartner2\tInteractionType\ntest hihi\tMARCO\tPOLO\tProtein-Protein\ntest2 hihi2\tKARL\tCOLUMBUS\tProtein-Gene"
        Expect.equal actual expected actual
        )
    
]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] [||] tests


