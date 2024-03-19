
namespace Components

open Feliz
open Feliz.DaisyUI

type GTtable =

    /// <summary>
    /// A stateful React component that maintains a counter
    /// </summary>
    [<ReactComponent>]
    static member Main() =
        let (myInput, setinput) = React.useState("Proteine-Gene")
        let splitTextIntoWords (text: string) =
            text.Split([|' '; '\n'; '\t'; '\r'|], System.StringSplitOptions.RemoveEmptyEntries)
            |> Array.toList

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
                Html.h1 [
                    prop.text "What is GroundTruth Helper about?"  
                    prop.style [
                        style.fontSize 20
                        style.fontWeight.bold
                        style.textDecorationLine.underline
                    ] 
                ]
                Daisy.card [
                    prop.style [
                        style.fontSize 15
                        style.width 700
                        style.textAlign.justify
                    ] 
                    prop.children [
                        Daisy.cardBody [
                            Html.p "By using GroundTruth Helper you can create a ground truth using biology abstracts to 
                                find protein/gene partners and their type of interaction. You can also use this website to create other types of datasets, 
                                for example to train a large language model or just to simplify your research. You can then download your created data. 
                                Just click on each partner in the abstract after expanding the abstract to assign it to partner 1 or 2."                             
                        ]
                    ]
                ]
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
                Daisy.table [
                    table.zebra
                    prop.style [
                        style.fontSize 16
                        style.width 1500
                    ]
                    prop.children [
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
                        Html.tbody [
                            //         Html.td "RDO5"
                            //         Html.td "APUM9"
                            Html.tr [
                                Html.td "1"
                                Html.td [
                                    Daisy.collapse [
                                        prop.tabIndex 0
                                        collapse.arrow
                                        prop.children [
                                            Html.input [prop.type' "checkbox"]
                                            Daisy.collapseTitle [ 
                                                prop.style [
                                                    style.width 900
                                                    style.fontSize 15 
                                                ]  
                                                prop.text "Example: Reduced Dormancy5 encodes a protein phosphatase 2C that is required for seed dormancy in Arabidopsis"
                                            ]
                                            Daisy.collapseContent [
                                                Daisy.cardBody [
                                                    prop.style [
                                                        style.flexDirection.row
                                                        style.flexWrap.wrap
                                                        style.width 900
                                                        style.fontSize 15
                                                    ]    
                                                    prop.children [
                                                        for word in (splitTextIntoWords exAbstract) do
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
                                                prop.text "Interactions"
                                            ]
                                            Daisy.collapseContent [
                                                Daisy.cardBody [
                                                    prop.style [
                                                        style.width 135
                                                        style.fontSize 15
                                                        style.color.black
                                                        style.padding 1

                                                    ]
                                                    prop.children [ 
                                                        Daisy.formControl [
                                                            Daisy.label [Daisy.labelText "Partner 1"]
                                                            Daisy.input [input.bordered; input.sm]
                                                        ]
                                                        Daisy.formControl [
                                                            Daisy.label [Daisy.labelText "Partner 2"]
                                                            Daisy.input [input.bordered; input.sm]
                                                        ]
                                                        Daisy.formControl [
                                                            Daisy.label [Daisy.labelText "InteractionType"]
                                                            Daisy.dropdown [
                                                                Daisy.button.button [
                                                                    button.sm
                                                                    prop.style [
                                                                        style.width 135
                                                                        style.fontSize 15  
                                                                    ]                                            
                                                                    prop.text myInput
                                                                    prop.className "tableElement"
                                                                ]
                                                                Daisy.dropdownContent [
                                                                    prop.className "p-1 shadow menu sm-base-150"
                                                                    prop.style [
                                                                        style.width 135
                                                                        style.fontSize 15
                                                                        style.color.white
                                                                    ]
                                                                    prop.tabIndex 0
                                                                    prop.children [
                                                                        Html.li [Html.a [prop.text "Protein-Gene"; prop.onClick (fun _ -> setinput "Protein-Gene"); prop.style [style.backgroundColor.aqua]]]
                                                                        Html.li [Html.a [prop.text "Protein-Protein"; prop.onClick (fun _ -> setinput "Protein-Protein"); prop.style [style.backgroundColor.aqua]]]
                                                                        Html.li [Html.a [prop.text "Other"; prop.onClick (fun _ -> setinput "Other"); prop.style [style.backgroundColor.aqua]]]
                                                                    ]
                                                                ]
                                                            ]
                                                        ]
                                                    ]
                                                ]
                                            ]
                                        ]
                                    ]
                                ] 
                            ]
                        ]
                    ]
                ]
            ]
        ]
                   
                
        
        
            
            
        