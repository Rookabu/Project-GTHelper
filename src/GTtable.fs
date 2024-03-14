
namespace Components

open Feliz
open Feliz.DaisyUI

type GTtable =

    /// <summary>
    /// A stateful React component that maintains a counter
    /// </summary>
    [<ReactComponent>]
    static member Main() =
        Html.div [
            Daisy.button.button [
                button.lg
                prop.className "button"
                //prop.className 
                prop.onClick (fun _ ->())
                prop.text "Upload abstract"
            ]
            Html.div [
                prop.text "By using GroundTruth Helper you can create a ground truth using biology abstracts to find protein/gene partners and their type of interaction. You can also use this website to create other types of datasets, for example to train a large language model or just to simplify your research. You can then download your created data.Just click on each partner in the abstract after expanding the abstract to assign it to partner 1 or 2."
            ]
            Daisy.button.button [
                button.lg
                prop.className "button"
                //prop.className 
                prop.onClick (fun _ ->())
                prop.text "Download table"
            ]
            Daisy.table [
                Html.thead [                   
                    Html.tr [prop.className "text"; prop.children [Html.th "Nr."; Html.th "Title"; Html.th "Partner 1"; Html.th "Partner 2"; Html.th "Interactiontype"]
                    ]
                ]
                Html.tbody [
                    Html.tr [ Html.td "1"; prop.children [ prop.className "hover:bg-orange-500"; Html.td "Example: Reduced Dormancy5 encodes a protein phosphatase 2C that is required for seed dormancy in Arabidopsis"]; Html.td "RDO5"; Html.td "APUM9"; Html.td "Protein-Gene"]]
                ]
            ]
                   
                
        
        
            
            
        