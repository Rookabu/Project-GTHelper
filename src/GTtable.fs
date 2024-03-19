
namespace Components

open Feliz
open Feliz.DaisyUI


module private Helper =

    let splitTextIntoWords (text: string) =
        text.Split([|' '; '\n'; '\t'; '\r'|], System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList

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
                    // Html.th "Partner 1" 
                    // Html.th "Partner 2" 
                    // Html.th "Interactiontype"
                    Html.th ""
                ]
            ]
        ]

    let tableCellPaperContent (abst: string) =
        Html.td [
            Daisy.collapse [
                prop.tabIndex 0
                collapse.arrow
                prop.children [
                    Html.input [prop.type' "checkbox"]
                    Daisy.collapseTitle [ 
                        prop.style [
                            style.fontSize 15 
                        ]  
                        prop.text "Example: Reduced Dormancy5 encodes a protein phosphatase 2C that is required for seed dormancy in Arabidopsis"
                    ]
                    Daisy.collapseContent [
                        Daisy.cardBody [
                            prop.style [
                                style.flexDirection.row
                                style.flexWrap.wrap
                                style.fontSize 15
                            ]    
                            prop.children [
                                for word in (splitTextIntoWords abst) do
                                    Html.span [
                                        //prop.className 
                                        prop.onClick (fun _ ->())
                                        prop.text word
                                        prop.className "hover:bg-sky-700"  
                                        prop.style [style.cursor.pointer; style.userSelect.none] 
                                    ]
                            ]
                        ]
                    ]
                ]
            ]
        ]

    let form(inp: string, setInput) = 
        Html.div [
            prop.className "flex gap-1 flex-col lg:flex-row"
            prop.children [
                Daisy.formControl [
                    Daisy.label [prop.className "title"; prop.text "Partner 1"; prop.style [style.fontSize 15]]
                    Daisy.input [input.bordered; input.sm; prop.style [style.color.black; style.maxWidth 150]]
                ]
                Daisy.formControl [
                    Daisy.label [prop.className "title"; prop.text"Partner 2"; prop.style [style.fontSize 15]]
                    Daisy.input [input.bordered; input.sm; prop.style [style.color.black; style.maxWidth 150]]
                ]
                Daisy.formControl [
                    Daisy.label [prop.className "title"; prop.text"InteractionType"; prop.style [style.fontSize 15]]
                    Daisy.dropdown [
                        Daisy.button.button [
                            button.sm
                            prop.style [
                                style.width 135
                                style.fontSize 15  
                            ]                                            
                            prop.text inp
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
                                Html.li [Html.a [prop.text "Protein-Gene"; prop.onClick (fun _ -> setInput "Protein-Gene"); prop.className "dropDownElement"]]
                                Html.li [Html.a [prop.text "Protein-Protein"; prop.onClick (fun _ -> setInput "Protein-Protein"); prop.className "dropDownElement"]]
                                Html.li [Html.a [prop.text "Other"; prop.onClick (fun _ -> setInput "Other"); prop.className "dropDownElement"]]
                            ]
                        ]
                    ]
                ]
            ]
        ]

    let tableCellFormInput (inp: string, setInput) =
        Html.td [
            prop.className "flex"
            prop.children [
                Daisy.collapse [
                    prop.style [style.overflow.visible]
                    prop.tabIndex 0
                    collapse.arrow
                    prop.children [
                        Html.input [prop.type' "checkbox"]
                        Daisy.collapseTitle [ 
                            prop.style [
                                style.fontSize 15
                            ]  
                            prop.text "Interactions"
                        ]
                        Daisy.collapseContent [
                            form(inp, setInput)
                        ]
                    ]
                ]
            ]
        ]

type GTtable =

    /// <summary>
    /// A stateful React component that maintains a counter
    /// </summary>
    [<ReactComponent>]
    static member Main() =
        let (myInput, setinput) = React.useState("Proteine-Gene")


        let exAbstract =
            "Seed dormancy determines germination timing and contributes to crop production and the adaptation of natural populations to their environment. 
            Our knowledge about its regulation is limited. In a mutagenesis screen of a highly dormant Arabidopsis thaliana line, the reduced dormancy5 (rdo5) mutant was isolated based on its strongly reduced seed dormancy. 
            Cloning of RDO5 showed that it encodes a PP2C phosphatase. Several PP2C phosphatases belonging to clade A are involved in abscisic acid signaling and control seed dormancy. However, RDO5 does not cluster with clade A phosphatases, 
            and abscisic acid levels and sensitivity are unaltered in the rdo5 mutant. RDO5 transcript could only be detected in seeds and was most abundant in dry seeds. RDO5 was found in cells throughout the embryo and is located in the nucleus. 
            A transcriptome analysis revealed that several genes belonging to the conserved PUF family of RNA binding proteins, in particular Arabidopsis PUMILIO9 (APUM9) and APUM11, showed strongly enhanced transcript levels in rdo5 during seed imbibition. 
            Further transgenic analyses indicated that APUM9 reduces seed dormancy. Interestingly, reduction of APUM transcripts by RNA interference complemented the reduced dormancy phenotype of rdo5, 
            indicating that RDO5 functions by suppressing APUM transcript levels."

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
                        prop.text "Upload abstract"
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
                            //         Html.td "RDO5"
                            //         Html.td "APUM9"
                            Html.tr [
                                prop.children [
                                    Html.td "1"
                                    Helper.tableCellPaperContent exAbstract 
                                    Helper.tableCellFormInput(myInput,setinput)
                                ]
                            ]
                            Html.tr [
                                prop.children [
                                    Html.td "2"
                                    Helper.tableCellPaperContent exAbstract 
                                    Helper.tableCellFormInput(myInput,setinput)
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]

//Todo 
                   
                
        
        
            
            
        