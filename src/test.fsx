#r "nuget: Expecto, 10.2.1"

type interaction = obj
let state: Map<int, interaction list>  = Map.empty //key= number of publication 
let setState: Map<int, interaction list> -> unit = fun _ -> ()

let addInteraction (newInteraction: interaction, pubIndex: int, state:Map<int, interaction list>) =
    let extractedList = Map.find pubIndex state //aktuelle liste am index
    let change (valueOption:option<interaction list>) =
        match valueOption with
        | Some list -> Some (newInteraction::list)   //if option is some then replace nextlist
        | None -> None 
    if state.IsEmpty = true then state.Add (pubIndex, [newInteraction]) //if empty then add list
    else state.Change (pubIndex, change) //if not empty then change
   
open Expecto //patronum

let tests = testList "main" [
    testCase "emptyState" (fun _ -> 
        let state: Map<int, interaction list> = Map.empty
        printfn "%A" state
        let interaction: interaction = "13"
        let pubIndex = 2
        let actual = addInteraction (interaction, pubIndex, state)
        let expected = Map<int, interaction list> ([pubIndex,["13"]])
        Expect.equal actual expected "fail"
        )
    // testCase "notEmptyState" (fun _ -> 
    //     let pubIndex = 2
    //     let state = Map<int, interaction list>  ([pubIndex,["13"]])
    //     let interaction: interaction = "14"
    //     let actual = addInteraction (interaction, pubIndex, state)
    //     let expected = Map<int, interaction list> ([pubIndex,["14";"13"]])
    //     Expect.equal actual expected "fail"
    //     )
]

runTestsWithCLIArgs [] [||] tests

//1. überprüfe ob dieser index bereits interactions hat 
//2a. Falls Ja: bestehende Interaction bekommen, neue Interaction an bestehende interactions dranhängen
//2b. Falls Nein: Neue interaction in die leere Liste hinzufügen 
//3. Setze die neue interaction list in den state über die key, welche den selben wert wie der index besitzt 
//4. setze state

