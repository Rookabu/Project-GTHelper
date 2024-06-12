
namespace Components

open Types
open Feliz
open Feliz.DaisyUI
open Fable.SimpleJson
open Fable.Core.JsInterop
open Browser

type ActiveField = 
    |Partner1
    |Partner2

module List =
  let rec removeAt index list =
      match index, list with
      | _, [] -> failwith "Index out of bounds"
      | 0, _ :: tail -> tail
      | _, head :: tail -> head :: removeAt (index - 1) tail

module private Helper =
    let splitTextIntoWords (text: string) =
        text.Split([|' '; '\n'; '\t'; '\r'|], System.StringSplitOptions.RemoveEmptyEntries)
        // |> Array.map (fun s -> s.Replace(",", "").Replace(".", ""))
        |> Array.toList 
    
    let log x = Browser.Dom.console.log x 

    let headerRow = 
        Html.thead [                   
            Html.tr [
                prop.className "title"
                prop.style [
                    style.fontSize 16
                ]
                prop.children [
                    Html.th "No."
                    Html.th "Title"
                    Html.th [
                        prop.className "lg:tooltip my-lg-tooltip tooltip-open"
                        // tooltip.open'
                        tooltip.text "Press left Ctrl-key to switch between partners and Enter to add!"
                        prop.text "No. of Interactions"
                    ]
                ]
            ]
        ]

    let minitable(interactionState: Map<int, Interaction list>, interactionList: Interaction list, removeInteraction: int * Map<int,list<Interaction>> * int -> unit, pubIndex: int) =
        match interactionList with
        | [] -> Html.none
        | _ ->
            Daisy.table [
                Html.thead [Html.tr [Html.th "Partner 1"; Html.th "Partner 2"; Html.th "Interaction Type"]]
                for i in 0 .. interactionList.Length - 1 do  
                    let interaction = List.item i interactionList
                    Html.tbody [
                        Html.tr [
                            Html.td [
                                prop.text (interaction.Partner1)
                            ] 
                            Html.td [
                                prop.text (interaction.Partner2)
                            ]
                            Html.td [
                                prop.text (
                                    match interaction.InteractionType with
                                    |ProteinProtein -> "Protein-Protein"
                                    |ProteineGene -> "Protein-Gene"
                                    |Other s -> s 
                                )
                            ]
                            Html.td [
                                Daisy.button.button [
                                    prop.text "X"
                                    button.xs
                                    prop.className "button"
                                    prop.onClick (fun _ ->
                                        removeInteraction (pubIndex, interactionState, i)
                                    )  
                                                                      
                                ]
                            ]
                        ] 
                    ]
            ]    

    let clickableWords (text, setNewClickedWord, interactionWordList: string list, activeWordList: string[]) =
        prop.children [
            for word: string in text do
                let trimmedWord = CSVParsing.removeSymbols word
                Html.span [
                    prop.onClick (fun _ -> 
                        setNewClickedWord trimmedWord 
                    )
                    prop.text word
                    prop.className "hover:bg-orange-700"
                    if interactionWordList |> List.contains trimmedWord then prop.className "clickedText"
                    if activeWordList |> Array.contains trimmedWord then prop.className "clickedTextActive" //if word is in the List, then it changes its color
                    prop.style [style.cursor.pointer; style.userSelect.none] 
                ]
        ]

    let checkHandle (checkState: bool, setCheckState: bool -> unit) =
        Html.input [
            prop.type' "checkbox" 
            prop.onCheckedChange (fun (isChecked: bool) ->
                setCheckState isChecked

                
            )                                              
            prop.isChecked (checkState)
        ]

    let tableCellPaperContent (abst: string list, title: string list, setNewClickedWord: string -> unit, checkState: bool, interactionWordList, activeWordList, setCheckState) =
        Html.td [
            Daisy.collapse [
                prop.className [if checkState then "collapse-open"]
                prop.children [
                    if checkState = false then checkHandle (checkState, setCheckState) 
                    Daisy.collapseTitle [ 
                        Daisy.cardTitle [
                            prop.style [
                                // style.minWidth 800
                                style.fontSize 16
                                style.pointerEvents.unset
                                style.flexWrap.wrap
                            ]
                            // prop.className "min-w-96"
                            if checkState then clickableWords (title, setNewClickedWord, interactionWordList, activeWordList)
                            else //checkSate = false
                                prop.children [
                                    for word: string in title do
                                        Html.span [
                                            prop.text word  
                                            prop.style [style.cursor.pointer; style.userSelect.none] 
                                        ]
                                ] 
                        ]
                    ]
                    Daisy.collapseContent [
                        Daisy.cardBody [
                            prop.style [
                                style.flexDirection.row
                                style.flexWrap.wrap
                                style.fontSize 16
                            ] 
                            clickableWords (abst, setNewClickedWord, interactionWordList, activeWordList)
                        ]
                    ]
                ]
            ]
        ]

    let labelAndInputField (title: string, partnerStrValue: string, activeFieldOption: ActiveField option, activeField: ActiveField, setField, setInput) =
        Daisy.formControl [
            
            Daisy.label [
                prop.className "title" 
                prop.text title
                prop.style [style.fontSize 16]
            ]
            
            Daisy.input [
                prop.tabIndex 0
                // prop.onKeyDown(fun e -> 
                //     if e.code = "KeyR" then removeSymbols partnerStrValue
                //     else ""        
                // )
                input.bordered 
                input.sm 
                prop.style [style.color.white; style.maxWidth 150]
                
                prop.onMouseDown (fun _ ->
                    setField (Some activeField)
                ) 
                prop.onChange (fun (x:string) -> 
                    if activeFieldOption = Some activeField then setInput x
                )
                prop.valueOrDefault (partnerStrValue)
                //prop.valueOrDefault partnerStrValue //use addingWords
                prop.className "tableElement"

                if activeFieldOption = Some activeField then prop.className "tableElementChecked" 

                else prop.className "tableElement"
            ]
        ]
        

    let DropDownElement (inputType: InteractionType, setInputType) =
        let blurActiveElement() =
            let e = Browser.Dom.document.activeElement
            if e |> isNull |> not then e?blur()
   
        Html.li [
            Html.a [
                prop.text (inputType.ToStringRdb())
                prop.onClick (fun _ -> 
                    setInputType inputType
                    blurActiveElement()
                )
                prop.className "button"
            ]
        ]

    let form(setField: option<ActiveField> -> unit, input1: string, input2: string, inputType: InteractionType, setInputType, setInput1: string -> unit, setInput2: string -> unit, activeField: option<ActiveField>) =

        Html.div [
            prop.className "flex gap-1 flex-col lg:flex-row"
            prop.children [
                labelAndInputField ("Partner 1", input1, activeField, Partner1, setField, setInput1)
                labelAndInputField ("Partner 2", input2, activeField, Partner2, setField, setInput2)
                Daisy.formControl [
                    Daisy.label [
                        prop.className "title" 
                        prop.text "Interaction Type"
                        prop.style [style.fontSize 16]]
                    match inputType with
                    |Other _ ->
                       Daisy.input [
                           input.bordered 
                           input.sm 
                           prop.style [style.color.white; style.maxWidth 135]
                           prop.className "tableElement"
                           prop.placeholder "What is it?"
                           prop.onChange (fun (s:string) ->
                                setInputType (Other s)
                           )
                           prop.autoFocus (true)
                        ] 
                    |_ -> Html.none
                    Daisy.dropdown [
                        Daisy.button.button [
                            button.sm
                            prop.style [
                                style.width 135
                                style.fontSize 16  
                            ]                                            
                            prop.text (inputType.ToStringRdb())
                            prop.className "tableElement flex"
                        ]
                        Daisy.dropdownContent [
                            prop.className "p-1 shadow menu sm-base-200"
                            prop.style [
                                style.width 145
                                style.fontSize 16
                                style.zIndex 10
                                
                            ]
                            prop.tabIndex 0
                            prop.children [
                                DropDownElement (ProteineGene, setInputType)
                                DropDownElement (ProteinProtein, setInputType)
                                DropDownElement (Other "", setInputType)
                            ]
                        ]
                    ]
                ]
            ]
        ]

    let tableCellInteractions (
        interactionState: Map<int, Interaction list>, input1, input2, inputType: InteractionType, setInputType, setField, 
        addInteraction: Interaction * int * Map<int,list<Interaction>> -> unit, pubIndex, removeInteraction, checkState, setCheckState, setInput1, setInput2, activeField
        ) =
        Html.td [
            prop.className "flex"
            prop.children [
                Daisy.collapse [
                    prop.className "overflow-visible"
                    collapse.arrow
                    prop.children [
                        checkHandle (checkState, setCheckState)
                        Daisy.collapseTitle [ 
                            Daisy.badge [
                                badge.lg
                                prop.style [
                                style.fontSize 16
                                ] 
                                let list =
                                        match interactionState.TryFind pubIndex with 
                                        | Some list -> list.Length
                                        | None -> 0
                                if list = 0 then prop.className "textCardError"
                                else prop.className "textCard"
                                prop.text (
                                    if list = 0 then "unedited"
                                    else list.ToString()
                                )
                            ] 
                        ]
                        Daisy.collapseContent [
                            form(setField, input1, input2, inputType, setInputType, setInput1, setInput2, activeField)
                            Daisy.button.button [
                                button.sm
                                prop.text "add Interaction"
                                if input1 = "" || input2 = "" || inputType = Other "" then prop.disabled true; prop.className "button flex lg:inline-flex"
                                else prop.className "button flex lg:inline-flex"
                                prop.style [
                                    style.marginTop 10 //style of add button
                                    style.alignItems.center
                                ]

                                prop.onClick (fun _ ->
                                    let newInteraction = {Partner1 = input1; Partner2 = input2; InteractionType = inputType}
                                    addInteraction (newInteraction, pubIndex, interactionState)
                                )

                            ]
                            let interactionList = 
                                match Map.tryFind pubIndex interactionState with
                                |Some list -> list
                                |None -> [] 
                            minitable(interactionState, interactionList, removeInteraction, pubIndex)
                        ]
                    ]
                ]
            ]
        ]

type GTtable =

    [<ReactComponent>]
    static member PaperElement (pubIndex: int, element: GTelement, interactionState: Map<int, Interaction list>, updateElement: int -> list<Interaction> -> unit) =
        let (input1: string , setInput1) = React.useState ("")
        let (input2: string, setInput2) = React.useState ("")
        let (inputType: InteractionType, setInputType) = React.useState (ProteinProtein)
        let (checkState: bool, setCheckState) = React.useState (if pubIndex = 0 && interactionState.IsEmpty then true else false)
        let (activeField: ActiveField option, setActiveField) = React.useState (Some Partner1)
        
        let interactionWordList: string list = [
            match (Map.tryFind pubIndex interactionState) with
            |Some interactions ->
                for interaction in interactions do
                    yield! interaction.Partner1.Split([|' '|])
                    yield! interaction.Partner2.Split([|' '|])
            |None -> ()
        ] 

        let activeWordList: string array = 
            input1.Split([|' '|])|> Array.append (input2.Split([|' '|])) //merged input 1 und input 2, welches gesplitet wurden nach whitespace
            //input 1/2 sind nur die aktuellen angeklcikten wörter, nach add interaction sind diese wieder ein leerer String
                 
        let reset () = 
            setInput1 ""
            setInput2 ""
            setActiveField (Some Partner1)
            setInputType (ProteinProtein)//input liste muss auch resettet werden
            
        let setNewClickedWord (word: string) =
            match activeField with
            |Some Partner1 -> 
                let newInput = if input1 = "" then word else input1 + " " + word
                setInput1 newInput
                log "setted input 1"
                // setActiveField (if input2 = "" then Some Partner2 else None)
            |Some Partner2 -> 
                let newInput = if input2 = "" then word else input2 + " " + word
                setInput2 newInput 
                log "setted input 2"
                // setActiveField (if input1 = "" then Some Partner1 else None)
            |None -> 
                if input1 = "" then setInput1 word
                elif input2 = "" then setInput2 word
                else () // do nothing

        let addInteraction (newInteraction: Interaction, pubIndex: int, state: Map<int, Interaction list>) =
            let nextList =
                match state.TryFind pubIndex with //try to find a list at the index
                | Some list -> (newInteraction::list)//if the option is a list then add the interaction to this list
                | None -> [newInteraction]  //if empty/None, then just the new interaction
            nextList 
            |> updateElement pubIndex 
            reset()   
            
        let removeInteraction (pubIndex: int, state:Map<int, Interaction list>, listIndx: int) =
            match state.TryFind pubIndex with
            | Some list -> 
                Some (list |> List.removeAt listIndx) //if the option is this list then remove this Interaction
            | None -> None //if not, do nothing
            |> function 
                | Some updatedList -> updateElement pubIndex updatedList 
                | None -> ()
            

        let onClickHandler _ =
            let newInteraction = {Partner1 = input1; Partner2 = input2; InteractionType = inputType}
            addInteraction (newInteraction, pubIndex, interactionState)

        Html.tr [
            prop.tabIndex 0
            prop.onKeyDown(fun e -> 
                if e.code = "Enter" then onClickHandler e            
                if e.code = "ControlLeft" && activeField = (Some Partner2) then setActiveField (Some Partner1) 
                elif e.code = "ControlLeft" && activeField = (Some Partner1) then setActiveField (Some Partner2)            
            )
            prop.children [
                Html.td (pubIndex + 1)
                Helper.tableCellPaperContent (element.Content, element.Title, setNewClickedWord, checkState, interactionWordList, activeWordList, setCheckState) 
                Helper.tableCellInteractions (interactionState, input1, input2, inputType, setInputType, setActiveField, addInteraction, pubIndex, removeInteraction, checkState, setCheckState, setInput1, setInput2, activeField)
            ]
            prop.key pubIndex
        ]
        
    [<ReactComponent>]
    static member Main() =
        let inputRef:IRefValue<Browser.Types.HTMLElement option> = React.useRef(None)

        let isLocalStorageClear (key:string) () =
            match (Browser.WebStorage.localStorage.getItem key) with
            | null -> true // Local storage is clear if the item doesn't exist
            | _ -> false //if false then something exists and the else case gets started

        let initialInteraction (key: string) =
            if isLocalStorageClear key () = true then Map.empty
            else Json.parseAs<Map<int, Interaction list>> (Browser.WebStorage.localStorage.getItem key)  

        let (interactionState: Map<int, Interaction list>, setInteractionState) = React.useState (initialInteraction "Interaction")

        let initialTable (key: string) =
            if isLocalStorageClear key () = true then [] 
            else Json.parseAs<GTelement list> (Browser.WebStorage.localStorage.getItem key)  

        let (table, setTable) = React.useState (initialTable "GTlist")
        let (isOnLoad, setonLoad) = React.useState (false)

        let setLocalStorage (key: string)(nextTable: 'a list) =
            let JSONString = Json.stringify nextTable //tabelle wird zu einem string convertiert
            Browser.WebStorage.localStorage.setItem(key,JSONString) //local storage wird gesettet

        let setLocalStorageInteraction (key: string)(nextInter: Map<int, Interaction list>) =
            let JSONString = Json.stringify nextInter //tabelle wird zu einem string convertiert
            Browser.WebStorage.localStorage.setItem(key, JSONString)
        
        let focusFileGetter() =
            match inputRef.current with
            | None -> ()
            | Some element ->
                element?click()

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
                let titleWords = Helper.splitTextIntoWords title
                let contentWords = Helper.splitTextIntoWords content 
                {
                    Title = titleWords
                    Content = contentWords
                }
                )
            |> Array.toList

        let upLoadButton =
            Daisy.formControl [
                Daisy.button.button [
                    button.md
                    prop.className "button"
                    prop.onClick (fun _ ->
                        focusFileGetter()
                    )
                    prop.children [
                        Html.i [prop.className "fa-solid fa-upload"]
                        Html.span [ prop.text "Upload abstracts"]
                    ]
                ]
                Daisy.input [
                    prop.type' "file"
                    prop.ref inputRef
                    file.ghost
                    prop.hidden true
                    prop.accept ".txt, .csv, .tsv"
                    prop.onChange (fun (file: Types.File) ->
                        let reader = FileReader.Create() //creates a file reader
                        reader.onload <- fun e -> 
                            let allContent:string = e.target?result //reads the file after a load and prints it as a string
                            log allContent
                            let newAbstract = parsePaperText allContent
                            setTable newAbstract
                            setLocalStorage "GTlist" newAbstract 
                            setonLoad false
                        file.slice()
                        |> reader.readAsText //reads the file as a text  
                        setonLoad true
                        
                    )
                    // prop.onLoad (fun _ ->
                    //     setonLoad true
                    // )
                ] 
            ]
        let threeButtonElement =
            Html.div [
                prop.className "contents lg:flex size-full justify-between"
                prop.style [
                    style.paddingLeft 100
                    style.paddingRight 100
                    style.marginBottom 50
                    style.marginTop 50
                ]
                prop.children [
                    Daisy.button.button [
                        button.md
                        prop.className "button mt-6 lg:mt-0"
                        prop.onClick (fun _ ->
                            focusFileGetter()
                        )
                        prop.text "Upload abstracts"
                        // prop.style [
                        //     style.marginTop (length.rem 1)
                        // ]
                    ]
                    if isOnLoad = true then
                        Daisy.loading [
                            loading.spinner 
                            loading.lg
                            // prop.className "flex"
                        ]
                    Daisy.input [
                        prop.type' "file"
                        prop.ref inputRef
                        file.ghost
                        prop.hidden true
                        prop.accept ".txt, .csv, .tsv"
                        prop.onChange (fun (file: Types.File) ->
                            let reader = FileReader.Create() //creates a file reader
                            reader.onload <- fun e -> 
                                let allContent:string = e.target?result //reads the file after a load and prints it as a string
                                log allContent
                                let newAbstract = parsePaperText allContent
                                setTable newAbstract
                                setLocalStorage "GTlist" newAbstract 
                                setonLoad false
                            file.slice()
                            |> reader.readAsText //reads the file as a text
                            setonLoad true
                        )
                    ]
                    Daisy.button.button [
                        button.md
                        prop.className "button"
                        if table = [] then
                            prop.style [
                                style.visibility.hidden
                            ]
                        else prop.className "button" 
                        prop.onClick (fun _ ->
                            []
                            |> fun t ->
                                t |> setTable 
                                t |> setLocalStorage "GTlist"
                            Map.empty
                            |> fun t ->
                                t |> setInteractionState
                                t |> setLocalStorageInteraction "Interaction"
                            false
                            |> fun t ->
                                t |> setonLoad    
                        )
                        prop.text "Reset to Start"
                    ]
                    
                    Daisy.button.button [
                        prop.className "button mb-6 lg:mb-0"
                        button.md
                        // prop.className "button"
                        prop.onClick (fun _ ->
                            let content = CSVParsing.gtElementsToCSV table interactionState
                            let downLoad fileName fileContent =
                                let anchor = Browser.Dom.document.createElement "a"
                                let encodedContent = fileContent |> sprintf "data:text/plain;charset=utf-8,%s" |> Fable.Core.JS.encodeURI
                                anchor.setAttribute("href",  encodedContent)
                                anchor.setAttribute("download", fileName)
                                anchor.click()
                            downLoad "GT-dataset.csv" content
                        )
                        prop.text "Download table"
                        if interactionState.IsEmpty then prop.disabled true; prop.className "button" 
                        else prop.className "button"
                        if table = [] then prop.style [style.visibility.hidden]
                    ]
                ]
            ]
        Html.div [
            prop.className "childstyle"
            prop.children [
                if table = [] then
                    Daisy.card [
                        prop.style [
                            style.marginTop 100
                            style.maxWidth 700
                            style.fontSize 20
                        ] 
                        prop.className "shadow-lg textCard"
                        prop.children [
                            Daisy.cardBody [
                                Daisy.cardTitle [prop.text "Hello there and welcome to my page! ✨"; prop.style [style.fontSize 27; style.marginBottom 30]]
                                Daisy.cardTitle "What is GroundTruth Helper about?"
                                Html.p "By using GroundTruth Helper you can create a ground truth from abstracts to 
                                find protein/gene partners and their type of interaction. You can also create other types of datasets, 
                                for example to train a large language model or just to simplify your research. You can then download your created data. 
                                Just click on each partner in the abstract after expanding it to assign it to partner 1 or 2." 
                                Html.p "Start editing your abstracts by uploading your file using this button!"
                                Daisy.cardActions [
                                    prop.className "card-actions justify-center"
                                    prop.style [style.marginTop 30]
                                    prop.children [
                                        upLoadButton
                                        
                                    ]
                                ]                             
                            ]
                        ]
                    ]
                    if isOnLoad = true then
                        Daisy.loading [
                            loading.spinner 
                            loading.lg
                        ]
                else
                    threeButtonElement
                    Daisy.table [
                        prop.tabIndex 0
                        prop.style [
                            style.maxWidth 1500
                            style.textAlign.center
                        ]
                        prop.children [
                            Helper.headerRow
                            Html.tbody [
                                for i in 0 .. (table.Length - 1)  do //für jedes Element in table wird folgendes gemacht:
                                    let element = List.item i table  
                                    let updateElement(index: int) (interList: Interaction list) =
                                        if interList = [] then  interactionState.Remove index
                                        else interactionState.Add (index, interList)
                                        |> fun t ->
                                            t |> setInteractionState
                                            t |> setLocalStorageInteraction "Interaction"
                                        log "safed Interactions"
                                        log interactionState
                                    GTtable.PaperElement(i, element, interactionState, updateElement)
                            ]
                        ]
                    ]
                    threeButtonElement
            ]
        ]
        

                   
                
        
        
            
            
        