
namespace Components

open Feliz
open Feliz.DaisyUI
open Fable.SimpleJson
open Fable.Core.JsInterop


type InteractionType =
    |ProteinProtein
    |ProteineGene
    |Other of string

    member this.ToStringRdb() =
        match this with
        | ProteineGene -> "Protein-Gene"
        | ProteinProtein -> "Protein-Protein"
        | Other s -> "Other" 


type Interaction = {
    Partner1: string
    Partner2: string
    InteractionType: InteractionType
}

type GTelement = {
    ///PaperTitle split by whitespace into single strings/words.
    Title: string list 
    ///PaperContent split by whitespace into single strings/words.
    Content: string list
    Interactions: Interaction list 
}

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
        |> Array.map (fun s -> s.Replace(",", "").Replace(".", ""))
        |> Array.toList 

    let log x = Browser.Dom.console.log x 

    let headerRow = 
        Html.thead [                   
            Html.tr [
                prop.className "title"
                prop.style [
                    style.fontSize 15
                ]
                prop.children [
                    Html.th "Nr."
                    Html.th "Title"
                    Html.th ""
                ]
            ]
        ]

    let minitable(interactions: Interaction list, removeInteraction: int -> unit) =
        // match interactions with
        // | [] -> Html.none
        // | _ ->
            Daisy.table [
                Html.thead [Html.tr [Html.th "Partner 1"; Html.th "Partner 2"; Html.th "Interaction Type"]]
                for i in 0 .. (interactions.Length - 1) do  
                    let interaction = List.item i interactions
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
                                        removeInteraction i
                                    )
                                    
                                ]
                            ]
                        ] 
                    ]
            ]    

    let clickableWords (text, setNewClickedWord, clickedItems, setClickedItems: string list -> unit, setLocalStorage: string ->list<string> -> unit) =

        let addingItems (word: string) (list: string list) =
            if list |> List.contains word then list //does nothing if the word is already in the list
            else word :: clickedItems   //adds the new word to the list 

        prop.children [
            for word: string in text do
                Html.span [
                    prop.onClick (fun _ -> 
                        setNewClickedWord word 
                        let nextItemlist = 
                            clickedItems 
                            |> addingItems word 
                        setClickedItems nextItemlist     
                        setLocalStorage "checkedWords" nextItemlist   
                    )
                    prop.text word
                    prop.className "hover:bg-orange-700"
                    if clickedItems |> List.contains word then prop.className "clickedText" //if word is in the List, then it changes its color
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

    let tableCellPaperContent (abst: string list, title: string list, setNewClickedWord: string -> unit, checkState: bool, clickedItems, setClickedItems, setLocalStorage) =
        Html.td [
            Daisy.collapse [
                // prop.isChecked (checkState)
                prop.className [if checkState then "collapse-open" else "collapse-close"]
                prop.children [
                    Daisy.collapseTitle [ 
                        Daisy.cardTitle [
                            prop.style [
                                style.fontSize 15
                                style.display.flex
                                style.gap (length.rem 0.5)
                                style.pointerEvents.unset
                            ]
                            if checkState then clickableWords (title, setNewClickedWord, clickedItems, setClickedItems, setLocalStorage)
                            else
                                prop.children [
                                    for word: string in title do
                                        Html.span [
                                            // prop.onMouseDown (fun _ -> setNewClickedWord word) // wo wird das wort an interaction gebunden?
                                            prop.text word
                                            // prop.className "hover:bg-orange-700"  
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
                                style.fontSize 15
                            ] 
                            clickableWords (abst, setNewClickedWord, clickedItems, setClickedItems, setLocalStorage)
                        ]
                    ]
                ]
            ]
        ]

    let addingWords (oldInput: string , newInput: string)=
        match oldInput with
        |"" -> newInput
        |_ -> newInput + " " + oldInput  
        
    let labelAndInputField (title: string, partnerStrValue: string, activeFieldOption: ActiveField option, activeField: ActiveField, setField, setInput) =
        Daisy.formControl [
            Daisy.label [
                prop.className "title" 
                prop.text title
                prop.style [style.fontSize 15]
            ]
            Daisy.input [
                input.bordered 
                input.sm 
                prop.style [style.color.white; style.maxWidth 150]
                prop.onMouseDown (fun _ ->
                    setField (Some activeField)
                ) 
                prop.onChange (fun (x:string) -> 
                    if activeFieldOption = Some activeField then setInput x
                )
                let oldInput = ["test"] //der bereits existierende text im input Feld

                prop.valueOrDefault (addingWords (oldInput, partnerStrValue))
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

    let form(setField: option<ActiveField> -> unit, input1: string, input2: string, inputType: InteractionType, setInputType, setInput1: string -> unit, setInput2: string -> unit, activeField: option<ActiveField> ) =
        Html.div [
            prop.className "flex gap-1 flex-col lg:flex-row"
            prop.children [
                labelAndInputField ("Partner 1", input1, activeField, Partner1, setField, setInput1)
                labelAndInputField ("Partner 2", input2, activeField, Partner2, setField, setInput2)
                Daisy.formControl [
                    Daisy.label [
                        prop.className "title" 
                        prop.text "InteractionType"
                        prop.style [style.fontSize 15]]
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
                                style.fontSize 15  
                            ]                                            
                            prop.text (inputType.ToStringRdb())
                            prop.className "tableElement"
                        ]
                        Daisy.dropdownContent [
                            prop.className "p-1 shadow menu sm-base-150"
                            prop.style [
                                style.width 140
                                style.fontSize 15
                                style.zIndex 2
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

    let tableCellInteractions (interactions: Interaction list, input1, input2, inputType: InteractionType, setInputType, setField, addInteraction, removeInteraction, checkState, setCheckState, setInput1, setInput2, activeField) =
        Html.td [
            prop.className "flex"
            prop.children [
                Daisy.collapse [
                    prop.style [style.overflow.visible]
                    collapse.arrow
                    prop.children [
                        checkHandle (checkState, setCheckState)
                        Daisy.collapseTitle [ 
                            prop.style [
                                style.fontSize 15
                            ]  
                            prop.text "Interactions"
                        ]
                        Daisy.collapseContent [
                            form(setField, input1, input2, inputType, setInputType, setInput1, setInput2, activeField)
                            Daisy.button.button [
                                button.sm
                                prop.text "add Interaction"
                                if input1 = "" || input2 = "" || inputType = Other "" then prop.disabled true; prop.className "button"
                                else prop.className "button"
                                prop.style [
                                    style.marginTop 5
                                ]
                                prop.onClick (fun _ ->
                                    addInteraction()
                                )
                            ]
                            minitable(interactions, removeInteraction)
                        ]
                    ]
                ]
            ]
        ]

type GTtable =

    [<ReactComponent>]
    static member PaperElement (index: int, element: GTelement, updateElement, isLocalStorageClear, setLocalStorage) =
        let (input1: string , setInput1) = React.useState ("")
        let (input2: string, setInput2) = React.useState ("")
        let (inputType: InteractionType, setInputType) = React.useState (ProteinProtein)
        let (checkState: bool, setCheckState) = React.useState (if index = 0 then true else false)
        let (activeField: ActiveField option, setActiveField) = React.useState (Some Partner1)

        let initialCheckedWords (key: string) =
            if isLocalStorageClear key () = true then []
            else Json.parseAs<string list> (Browser.WebStorage.localStorage.getItem key)

        let (clickedItems: string list, setClickedItems) = React.useState (initialCheckedWords "checkedWords")
        //let inputRef:IRefValue<Browser.Types.HTMLElement option> = React.useRef(None)

        let reset () = 
            setInput1 ""
            setInput2 ""
            setActiveField (Some Partner1)
            setInputType (ProteinProtein)
            
        let setNewClickedWord (word: string) =
            match activeField with
            |Some Partner1 -> setInput1 word; setActiveField (if input2 = "" then Some Partner2 else None)
            |Some Partner2 -> setInput2 word; setActiveField (if input1 = "" then Some Partner1 else None)
            |None -> 
                if input1 = "" then setInput1 word
                elif input2 = "" then setInput2 word
                else () // do nothing
                
           
            

        let addInteraction () : unit =
            let newInteraction = {Partner1 = input1; Partner2 = input2; InteractionType = inputType}
            let newElement = {element with Interactions = newInteraction::element.Interactions}
            updateElement newElement
            reset()
            
   
        let removeInteraction (index): unit =
            let newInteractions = element.Interactions |> List.removeAt index
            let newElement = {element with Interactions = newInteractions}
            updateElement newElement 
      

        Html.tr [
            prop.children [
                Html.td (index + 1)
                Helper.tableCellPaperContent (element.Content, element.Title, setNewClickedWord, checkState, clickedItems, setClickedItems, setLocalStorage) 
                Helper.tableCellInteractions (element.Interactions, input1, input2, inputType, setInputType, setActiveField, addInteraction, removeInteraction, checkState, setCheckState, setInput1, setInput2, activeField)
                
            ]
            prop.key index
        ]
        

    [<ReactComponent>]
    static member Main() =
        

        let exAbstract = [
            {
                Title = Helper.splitTextIntoWords "Example: Reduced Dormancy5 encodes a protein phosphatase 2C that is required for seed dormancy in Arabidopsis" 
                Content = Helper.splitTextIntoWords "Seed dormancy determines germination timing and contributes to crop production and the adaptation of natural populations to their environment. 
                    Our knowledge about its regulation is limited. In a mutagenesis screen of a highly dormant Arabidopsis thaliana line, the reduced dormancy5 (rdo5) mutant was isolated based on its strongly reduced seed dormancy. 
                    Cloning of RDO5 showed that it encodes a PP2C phosphatase. Several PP2C phosphatases belonging to clade A are involved in abscisic acid signaling and control seed dormancy. However, RDO5 does not cluster with clade A phosphatases, 
                    and abscisic acid levels and sensitivity are unaltered in the rdo5 mutant. RDO5 transcript could only be detected in seeds and was most abundant in dry seeds. RDO5 was found in cells throughout the embryo and is located in the nucleus. 
                    A transcriptome analysis revealed that several genes belonging to the conserved PUF family of RNA binding proteins, in particular Arabidopsis PUMILIO9 (APUM9) and APUM11, showed strongly enhanced transcript levels in rdo5 during seed imbibition. 
                    Further transgenic analyses indicated that APUM9 reduces seed dormancy. Interestingly, reduction of APUM transcripts by RNA interference complemented the reduced dormancy phenotype of rdo5, 
                    indicating that RDO5 functions by suppressing APUM transcript levels."
                Interactions = [] 
            }
            {
                Title = Helper.splitTextIntoWords "Distinct Clades of Protein Phosphatase 2A Regulatory B'/B56 Subunits Engage in Different Physiological Processes" 
                Content = Helper.splitTextIntoWords "Protein phosphatase 2A (PP2A) is a strongly conserved and major protein phosphatase in all eukaryotes. 
                    The canonical PP2A complex consists of a catalytic (C), scaffolding (A), and regulatory (B) subunit. Plants have three groups of evolutionary 
                    distinct B subunits: B55, B' (B56), and B''. Here, the Arabidopsis B' group is reviewed and compared with other eukaryotes. Members of the B'α/B'β clade are especially important for chromatid cohesion, 
                    and dephosphorylation of transcription factors that mediate brassinosteroid (BR) signaling in the nucleus. Other B' subunits interact with proteins at the cell membrane to dampen BR signaling or harness immune responses. 
                    The transition from vegetative to reproductive phase is influenced differentially by distinct B' subunits; B'α and B'β being of little importance, whereas others (B'γ, B'ζ, B'η, B'θ, B'κ) 
                    promote transition to flowering. Interestingly, the latter B' subunits have three motifs in a conserved manner, i.e., two docking sites for protein phosphatase 1 (PP1), and a POLO consensus phosphorylation site between these motifs. 
                    This supports the view that a conserved PP1-PP2A dephosphorelay is important in a variety of signaling contexts throughout eukaryotes. A profound understanding of these regulators may help in designing future crops and understand environmental issues."
                Interactions = [] 
            }
        ]


        let isLocalStorageClear (key:string) () =
            match (Browser.WebStorage.localStorage.getItem key) with
            | null -> true // Local storage is clear if the item doesn't exist
            | _ -> false //if false then something exists and the else case gets started

        let initialTable (key: string) =
            if isLocalStorageClear key () = true then exAbstract
            else Json.parseAs<GTelement list> (Browser.WebStorage.localStorage.getItem key)  

        let (table, setTable) = React.useState (initialTable "GTlist")

        let setLocalStorage (key: string)(nextTable: 'a list) =
            let JSONString = Json.stringify nextTable //tabelle wird zu einem string convertiert
            //Browser.Dom.console.log (JSONString) 
            Browser.WebStorage.localStorage.setItem(key,JSONString) //local storage wird gesettet
            
        Html.div [
            prop.className "childstyle"
            prop.children [
                Daisy.card [
                    prop.style [
                        style.fontSize 15
                        style.maxWidth 700
                        style.textAlign.justify
                    ] 
                    prop.children [
                        Daisy.cardBody [
                            Daisy.cardTitle "What is GroundTruth Helper about?"
                            Html.p "By using GroundTruth Helper you can create a ground truth using biology abstracts to 
                                find protein/gene partners and their type of interaction. You can also use this website to create other types of datasets, 
                                for example to train a large language model or just to simplify your research. You can then download your created data. 
                                Just click on each partner in the abstract after expanding the abstract to assign it to partner 1 or 2."                             
                        ]
                    ]
                ]
                Html.div [
                  prop.className "flex size-full justify-between"
                  prop.style [
                    style.paddingLeft 100
                    style.paddingRight 100
                  ]
                  prop.children [
                    Daisy.button.button [
                        button.md
                        prop.className "button"
                        prop.onClick (fun _ ->())
                        prop.text "Upload abstracts"
                    ]
                    
                    Daisy.button.button [
                        button.md
                        prop.className "button"
                        //prop.className 
                        prop.onClick (fun _ ->())
                        prop.text "Download table"
                    ]
                  ]
                ]
                Daisy.table [
                    prop.style [
                        style.fontSize 16
                        style.maxWidth 1500
                    ]
                    prop.children [
                        Helper.headerRow
                        Html.tbody [
                            for i in 0 .. (table.Length - 1)  do //für jedes Element in table wird folgendes gemacht:
                                let element = List.item i table
                                let updateElement (element': GTelement) =
                                    table
                                    |> List.mapi (fun indx a ->
                                        if indx = i then 
                                            element'
                                        else 
                                            a
                                    )
                                    |> fun t ->
                                        t |> setTable
                                        t |> setLocalStorage "GTlist"
                                    log "safed" 
                                GTtable.PaperElement(i, element, updateElement, isLocalStorageClear, setLocalStorage)
                            ]
                    ]
                ]
            ]
        ]

                   
                
        
        
            
            
        