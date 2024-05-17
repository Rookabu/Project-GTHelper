let content = 
    "Distinct Clades of Protein Phosphatase 2A Regulatory

    Protein phosphatase 2A (PP2A) is a strongly conserved and major protein phosphatase in all eukaryotes
    
    
    Histone Deacetylase Complex

    Early responses of plants to environmental stress 

    
    WRKY transcription factors 

    WRKY transcription factors in plants are known to be able "

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


let parsePaperText (txt: string) =
    let publications = txt.Split([|'\n'|], System.StringSplitOptions.RemoveEmptyEntries)//split paper from each other
    printfn "%A" publications
    let pubmergPairs = publications |> mergePairs
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