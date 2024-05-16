open Expecto //patronum
open Types
open CSVParsing

module Expect = 
    /// Trims whitespace and normalizes lineendings to "\n"
    let trimEqual (actual: string) (expected: string) message =
        let a = actual.Trim().Replace("\r\n", "\n")
        let e = expected.Trim().Replace("\r\n", "\n")
        Expect.equal a e message

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

//folgende Funktion soll
//1. über den index in das GTelement den titel rausholen
//2. über den selben index den key in der interaction list alle interactions rausholen
//3. aus den informationen eine liste aus strings erstellen, jeder string steht für eine interaction mit entsprechenden titel und alle interactionn, getrennt durch \t



let tests_gtElementsToCSV = testList "gtElementsToCSV" [

    testCase "oneInteraction" (fun _ -> 

        let interactionStateOne = Map [
            (0, [{Partner1 = "MARCO"; Partner2 = "POLO"; InteractionType = ProteinProtein}]);
            (1, [{Partner1 = "KARL"; Partner2 = "COLUMBUS"; InteractionType = ProteineGene}])
            ]
        let actual =  gtElementsToCSV table interactionStateOne
        let expected = 
            "Title\tPartner1\tPartner2\tInteractionType
test hihi\tMARCO\tPOLO\tProtein-Protein
test2 hihi2\tKARL\tCOLUMBUS\tProtein-Gene"
        Expect.trimEqual actual expected ""
        )

    testCase "noInteraction" (fun _ -> 
        let interactionStateNo = Map.empty
        let actual = gtElementsToCSV table interactionStateNo
        let expected = 
            "Title\tPartner1\tPartner2\tInteractionType"
        Expect.trimEqual actual expected ""
        )

    testCase "manyInteraction" (fun _ -> 
        let interactionStateMany = Map [
                (0, [{Partner1 = "MARCO"; Partner2 = "POLO"; InteractionType = ProteinProtein}; {Partner1 = "JULIUS"; Partner2 = "CAESAR"; InteractionType = ProteineGene}]);
                (1, [{Partner1 = "KARL"; Partner2 = "COLUMBUS"; InteractionType = ProteineGene}])
                ]
        let actual =  gtElementsToCSV table interactionStateMany
        let expected = 
            "Title\tPartner1\tPartner2\tInteractionType
test hihi\tMARCO\tPOLO\tProtein-Protein
test hihi\tJULIUS\tCAESAR\tProtein-Gene
test2 hihi2\tKARL\tCOLUMBUS\tProtein-Gene"
        Expect.trimEqual actual expected ""
    )

    testCase "EmptyInteraction" (fun _ -> 
        let interactionStateEmpty = Map [0,[]; 1,[]; 2,[]]
        let actual = gtElementsToCSV table interactionStateEmpty
        let expected = 
            "Title\tPartner1\tPartner2\tInteractionType"
        Expect.trimEqual actual expected ""
        
    )

    testCase "OtherInteraction" (fun _ -> 

        let interactionStateOne = Map [
            (0, [{Partner1 = "MARCO"; Partner2 = "POLO"; InteractionType = Other "wewewds"}]);
            (1, [{Partner1 = "KARL"; Partner2 = "COLUMBUS"; InteractionType = ProteineGene}])
            ]
        let actual =  gtElementsToCSV table interactionStateOne
        let expected = 
            "Title\tPartner1\tPartner2\tInteractionType
test hihi\tMARCO\tPOLO\twewewds
test2 hihi2\tKARL\tCOLUMBUS\tProtein-Gene"
        Expect.trimEqual actual expected ""
        )
]

let tests = 
    testList "main" [
        tests_gtElementsToCSV
    ]
[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] [||] tests


