#n "FSharp.Data.dll"

open Expecto //patronum
open Components
open FSharp.Data
open FSharpAux.IO


type finishedTable = {
    Title: string 
    Partner1: string
    Partner2: string
    InteractionType: string
}

let finsihedList (title: GTelement) (interaction:Map<int, Interaction list>): finishedTable list= ()


let GTparseToCSV (table: GTelement list) (interactions: Map<int, Interaction list> ): string =
    let myCsvTable = SeqIO.Seq.CSV "\t" true true table |> String.concat "\n"
    myCsvTable 
                             

let tests = testList "main" [
    testCase "ensureCSVElement" (fun _ -> 
        GTparseToCSV   
        Expect.equal gtElement.Title ["hallo"] ""
        )
    
]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] [||] tests


