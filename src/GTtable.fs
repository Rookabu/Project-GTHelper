
namespace Components

open Feliz
open Feliz.DaisyUI

type InteractionType =
    |ProteinProtein
    |ProteineGene
    |Other of string

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

    let updatePartner (index: int) (clickedWord: string) (stateActiveField:option<ActiveField>) (state: GTelement list) : GTelement list  =
        state
        |> List.mapi ( fun i a -> 
            if i = index then
                match stateActiveField with
                |Some Partner1 -> {a with Interactions = (
                                                    a.Interactions |> List.map (fun (b: Interaction) ->
                                                        {b with Partner1 = clickedWord}))} 
                |Some Partner2 -> {a with Interactions = (
                                                    a.Interactions |> List.map (fun (b: Interaction) ->
                                                        {b with Partner2 = clickedWord}))}
                |None -> 
                    if (a.Interactions |> List.map (fun (b:Interaction) -> b.Partner1)) = 
                        [""] then {a with Interactions = (a.Interactions |> List.map (fun (b: Interaction) ->{b with Partner1 = clickedWord}))} 
                    elif (a.Interactions |> List.map (fun (b:Interaction) -> b.Partner2)) = 
                        [""] then {a with Interactions = (a.Interactions |> List.map (fun (b: Interaction) ->{b with Partner2 = clickedWord}))}    
                    else a
            else 
                a
        ) 

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

    let minitable(element) =
        Daisy.table [
            Html.thead [Html.tr [Html.th "Partner 1"; Html.th "Partner 2"; Html.th "Interaction Type"]]
            Html.tbody [
                for interaction in element.Interactions do  
                    Html.tr [
                        Html.td [
                            prop.text (interaction.Partner1)
                        ] 
                        Html.td [
                            prop.text (interaction.Partner2)
                        ]
                        Html.td [
                            prop.text (
                                if interaction.InteractionType = ProteinProtein then "Protein-Protein"
                                elif interaction.InteractionType = ProteineGene then "Protein-Gene"
                                else "user input" //noch an input binden
                            )
                        ]
                        Html.td [
                            Daisy.button.button [
                                prop.text "X"
                                button.xs
                                prop.className "button"
                                prop.onClick (fun _ -> ()
                                    // List.removeAt index element.Interactions 
                                )
                            ]
                        ]
                    ] 
            ]
        ]    

    let clickableWords (text, stateActiveField) =
        prop.children [
            for word: string in text do
                Html.span [
                    prop.onMouseDown (fun _ -> 
                        ()
                        //settable (updatePartner index word stateActiveField table)
                    ) 
                    prop.text word
                    prop.className "hover:bg-orange-700"  
                    prop.style [style.cursor.pointer; style.userSelect.none] 
                ]
        ]

    let tableCellPaperContent (abst: string list, title: string list, stateActiveField) =
        Html.td [
            Daisy.collapse [
                prop.tabIndex 0
                collapse.arrow
                prop.children [
                    Html.input [
                        prop.type' "checkbox" 
                        // prop.onCheckedChange (fun (isChecked: bool) ->
                        //     table
                        //     |> List.mapi (fun i a ->
                        //         if i = index then
                        //             {a with Checked = isChecked}
                        //         else 
                        //             a
                        //     )
                        //     |>settable
                        // )                                              
                        // prop.isChecked (
                        //     element.Checked                                              
                        // )
                    ]
                    Daisy.collapseTitle [ 
                        prop.style [
                            style.fontSize 15
                            style.display.flex
                            style.gap (length.rem 0.5)
                            style.pointerEvents.unset
                        ]
                        clickableWords (title, stateActiveField)
                    ]
                    Daisy.collapseContent [
                        Daisy.cardBody [
                            prop.style [
                                style.flexDirection.row
                                style.flexWrap.wrap
                                style.fontSize 15
                            ]    
                            clickableWords (abst, stateActiveField)
                            ]
                        ]
                    ]
                ]
            ]

    let labelAndInputField (title: string, activeField: ActiveField, (partnerGetter: Interaction -> string), setField, element) =
        let textfeld = (element.Interactions |> List.map (partnerGetter))
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
                prop.onClick (fun _ ->
                    setField (Some activeField)
                ) 
                prop.className "tableElement" 
                prop.valueOrDefault textfeld.[0]
                prop.onBlur (fun _ -> setField None)
            ]
        ]

    let DropDownElement (title: string, interactionName, table, settable, index) =
        Html.li [
            Html.a [
                prop.text title
                prop.onClick (fun _ -> 
                table
                |> List.mapi (fun i a ->
                    if i = index then
                        {a with Interactions = (
                            a.Interactions |> List.map (fun (b: Interaction) ->
                                {b with InteractionType = interactionName}))}
                    else 
                        a
                )
                |>settable
                )
                prop.className "button"
            ]
        ]

    let form(table, settable, setField: option<ActiveField> -> unit, element, index: int) =
        Html.div [
            prop.className "flex gap-1 flex-col lg:flex-row"
            prop.children [
                labelAndInputField ("Partner1", Partner1, (fun (b:Interaction)-> b.Partner1), setField, element)
                labelAndInputField ("Partner2", Partner2, (fun (b:Interaction)-> b.Partner2), setField, element)
                Daisy.formControl [
                    Daisy.label [
                        prop.className "title" 
                        prop.text"InteractionType"
                        prop.style [style.fontSize 15]]
                    if (element.Interactions |> List.map (fun b -> b.InteractionType)) = [Other ""] then 
                        Daisy.input [
                            input.bordered 
                            input.sm 
                            prop.style [style.color.white; style.maxWidth 135]
                            prop.className "tableElement"
                            prop.placeholder "What is it?"
                    ] 
                    Daisy.dropdown [
                        Daisy.button.button [
                            button.sm
                            prop.style [
                                style.width 135
                                style.fontSize 15  
                            ]                                            
                            prop.text (
                                if (element.Interactions |> List.map (fun b -> b.InteractionType)) = [ProteineGene] then "Protein-Gene"
                                elif (element.Interactions |> List.map (fun b -> b.InteractionType)) = [ProteinProtein] then "Protein-Protein"
                                else "Choose Type"
                            )
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
                                DropDownElement ("Proteine-Gene", ProteineGene, table, settable, index)
                                DropDownElement ("Protein-Protein", ProteinProtein, table, settable, index)
                                DropDownElement ("Other", Other "", table, settable, index)
                            ]
                        ]
                    ]
                ]
            ]
        ]
    let tableCellInteractions (setField, element, settable: list<GTelement> -> unit, table, index, stateActiveField) =
        let (input1: string , setInput1) = React.useState ("")
        let (input2: string, setInput2) = React.useState ("")
        let (inputType: InteractionType, setInputType) = React.useState (Other "")
        let reset () = 
            setInput1 ""
            setInput2 ""
            setInputType (Other "")

        Html.td [
            prop.className "flex"
            prop.children [
                Daisy.collapse [
                    prop.style [style.overflow.visible]
                    prop.tabIndex 0
                    collapse.arrow
                    prop.children [
                        Html.input [
                            prop.type' "checkbox" 
                            // prop.onCheckedChange (fun (isChecked: bool) ->
                            // table
                            // |> List.mapi (fun i a ->
                            //     if i = index then
                            //         {a with Interactions = (
                            //             a.Interactions |> List.map (fun (b: Interaction) ->
                            //                 {b with Checked = isChecked}))} //auf alle updaten
                            //     else 
                            //         a
                            // )
                            // |>settable
                            // )                                              
                            // prop.isChecked (
                            //     if element.Interactions[0] then true
                            //     else false                           
                            // )        
                        ]
                        Daisy.collapseTitle [ 
                            prop.style [
                                style.fontSize 15
                            ]  
                            prop.text "Interactions"
                        ]
                        Daisy.collapseContent [
                            form(table, settable, setField, element, index)
                            Daisy.button.button [
                                button.sm
                                prop.text "add Interaction"
                                prop.className "button"
                                prop.style [
                                    style.marginTop 5
                                ]
                                prop.onClick (fun _ -> (
                                    let newInteraction = {Partner1 = input1; Partner2 = input2; InteractionType = inputType}
                                    let newInteractionAdd = newInteraction::element.Interactions 
                                    //{recordType with SubRecordTypeField1 = {recordType.SubRecordTypeField1 with AnotherField = newValue}}
                                    table
                                    |> List.mapi (fun i a ->
                                        if i = index then {element with Interactions = newInteractionAdd} 
                                        else a
                                        )
                                    |> settable
                                    reset ()
                                ))
                                    //Hänge neue Interaktion an Element.interactions an. //über "::"
                                    //Update bestehendes Element in table. //record type update
                                    //settable auf geupdateten table. 
                                    //Nach dem klick sind die Felder wieder leer //reset 
                            ]
                            minitable(element)
                        ]
                    ]
                ]
            ]
        ]

type GTtable =
    [<ReactComponent>]
    static member PaperElement (index: int, element: GTelement, settable, stateActiveField, setField) =
        Html.tr [
            Html.td (index + 1)
            Helper.tableCellPaperContent (element.Content, element.Title, stateActiveField) 
            Helper.tableCellInteractions (setField, element, settable, table, i, stateActiveField)
        ]
    [<ReactComponent>]
    static member Main() =
        let (stateActiveField: (ActiveField) option, setField) = React.useState (None)

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
            Interactions = [{
                Partner1 = ""
                Partner2 = ""
                InteractionType = InteractionType.Other "" 
            }]              
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
            Interactions = [{
                Partner1 = ""
                Partner2 = ""
                InteractionType = InteractionType.Other "" 
            }]
            }
        ]


        let (table, settable) = React.useState (exAbstract)
        
        

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
                            for i in [0 .. (table.Length - 1)]  do //für jedes Element in table wird folgendes gemacht:
                                let element = List.item i table
                                //         Html.td "RDO5"
                                //         Html.td "APUM9"
                                
                                
                            ]
                    ]
                ]
            ]
        ]

                   
                
        
        
            
            
        