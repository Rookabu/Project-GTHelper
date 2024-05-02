type interaction = obj
let state: Map<int, interaction list>  = Map.empty
let setState: Map<int, interaction list> -> unit


let addInteraction (newInteraction: interaction, pubIndex: int) =
    let sampleInterlist: interaction list = []
    let newInteractionList = newInteraction :: sampleInterlist   
    let change pubIndex =
        match pubIndex with
        | Some s -> Some (newInteractionList)
        | None -> None
    if state.IsEmpty = true then state.Add (pubIndex, newInteractionList) 
    else state.Change (pubIndex, change) 
    |> fun t ->
        t |> setState




//1. überprüfe ob dieser index bereits interactions hat 
//2a. Falls Ja: bestehende Interaction bekommen, neue Interaction an bestehende interactions dranhängen
//2b. Falls Nein: Neue interaction in die leere Liste hinzufügen 
//3. Setze die neue interaction list in den state über die key, welche den selben wert wie der index besitzt 
//4. setze state

