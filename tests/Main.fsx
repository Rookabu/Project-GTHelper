#r "nuget: Expecto, 10.2.1"

type interaction = obj
let state: Map<int, interaction list>  = Map.empty //key= number of publication 
let setState: Map<int, interaction list> -> unit = fun _ -> ()
let addInteraction (newInteraction: interaction, pubIndex: int, state:Map<int, interaction list>) =
    let change (valueOption:option<interaction list>) =
        match valueOption with
        | Some list -> Some (newInteraction::list)   
        | None -> Some [newInteraction]  
    state.Change (pubIndex, change) 
    // |> fun t ->
    //     t |> setState
    //if not empty then change the given list and add the new interaction to the list
   
open Expecto //patronum

let tests = testList "main" [
    testCase "emptyState" (fun _ -> 
        let state: Map<int, interaction list> = Map.empty
        printfn "KABOOM %A" state
        let interaction: interaction = "13"
        let pubIndex = 2
        let actual = addInteraction (interaction, pubIndex, state)
        let expected = Map<int, interaction list> ([pubIndex,["13"]]) 
        Expect.equal actual expected "fail"
        )
    testCase "notEmptyState" (fun _ -> 
        let pubIndex = 2
        let state = Map<int, interaction list>  ([pubIndex,["13"]])
        let interaction: interaction = "14"
        let actual = addInteraction (interaction, pubIndex, state)
        let expected = Map<int, interaction list> ([pubIndex,["14";"13"]])
        Expect.equal actual expected "fail"
        )
]

runTestsWithCLIArgs [] [||] tests
