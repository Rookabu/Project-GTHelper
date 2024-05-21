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

let content =
    "Distinct Clades of Protein Phosphatase 2A Regulatory

Protein phosphatase 2A (PP2A) is a strongly conserved and major protein phosphatase in all eukaryotes


Histone Deacetylase Complex

Early responses of plants to environmental stress


WRKY transcription factors

WRKY transcription factors in plants are known to be able"


let splitTextIntoWords (text: string) =
    text.Split([|' '; '\n'; '\t'; '\r'|], System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun s -> s.Replace(",", "").Replace(".", ""))
    |> Array.toList 
    

let mergePairs (words: string array) =
    words
    |> Array.chunkBySize 2
    |> Array.map (fun pair -> 
            match pair with
            | [|a; b|] -> a + "\n" + b
            | _ -> failwith "uneven chunk size")

let splitIntoArray (txt: string)= 
    txt.Split([|'\n'|], System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun s -> s.Trim())
    |> Array.filter (fun s -> s <> "")

let parsePaperText (txt: string) =
    let publications = splitIntoArray txt//split paper from each other
    let pubmergPairs = publications |> mergePairs //divide pairs by "/n"
    pubmergPairs
    |> Array.map (fun (pub: string) ->
        let split = pub.Split ("\n") //split = array of content and title
        //split from each string in the array between one paragraph
        let title, content = split[0], split[1] //0 is title, 1 is content
        let titleWords = splitTextIntoWords title
        let contentWords = splitTextIntoWords content 
        {
            Title = titleWords
            Content = contentWords
        }
        )
    |> Array.toList

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

let test_parsing = testList "parsing" [
    testCase "parsing Abstracts" (fun _ -> 

        let actual = parsePaperText content
        let expected =  [
            {
            Title = ["Distinct"; "Clades"; "of"; "Protein"; "Phosphatase"; "2A"; "Regulatory"]
            Content = ["Protein";"phosphatase";"2A"; "(PP2A)"; "is"; "a"; "strongly"; "conserved"; "and"; "major"; "protein"; "phosphatase"; "in"; "all"; "eukaryotes"]
            }
            {
            Title = ["Histone"; "Deacetylase"; "Complex"]
            Content = ["Early"; "responses"; "of" ;"plants"; "to"; "environmental"; "stress"]
            }
            {
            Title = ["WRKY" ;"transcription" ;"factors"]
            Content = ["WRKY"; "transcription"; "factors"; "in" ;"plants"; "are"; "known" ;"to"; "be"; "able"]
            }
        ]

        Expect.equal actual expected ""
        )
    testCase "showing string array" (fun _ -> 
        let actual = splitIntoArray content
        let expected = [|"Distinct Clades of Protein Phosphatase 2A Regulatory";
    "Protein phosphatase 2A (PP2A) is a strongly conserved and major protein phosphatase in all eukaryotes";
    "Histone Deacetylase Complex";
    "Early responses of plants to environmental stress";
    "WRKY transcription factors";
    "WRKY transcription factors in plants are known to be able"|]

        Expect.sequenceEqual actual expected ""
        )
]

let tests = 
    testList "main" [
        // tests_gtElementsToCSV
        test_parsing
    ]
[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] [||] tests


